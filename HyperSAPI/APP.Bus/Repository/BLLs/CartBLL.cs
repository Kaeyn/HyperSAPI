using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Bill;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.DTOs.Coupon;
using APP.Bus.Repository.DTOs.Product;
using APP.DAL.Repository.Entities;
using KendoNET.DynamicLinq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using static APP.Bus.Repository.Mathmathics.StaticFunc;

namespace APP.Bus.Repository.BLLs
{
    public class CartBLL
    {
        private AppDBContext DB;
        private BillBLL _billBLL;

        public CartBLL()
        {
            DB = new AppDBContext();
            _billBLL = new BillBLL();
        }

        public DTOResponse ProceedToPayment(dynamic requestParam, DTOProceedToPayment? dTOProceedToPayment, bool isCountDown)
        {
            dynamic request = null;
            if(dTOProceedToPayment != null)
            {
                request = dTOProceedToPayment;
            }
            else
            {
                request = JsonConvert.DeserializeObject<DTOProceedToPayment>(requestParam.ToString());
            }
            var respond = new DTOResponse();
            try
            {
                string ordererPhoneNumber = request.OrdererPhoneNumber;
                string reqCusName = request.CustomerName;
                string reqPhoneNumber = request.PhoneNumber;
                string reqShippingAddress = request.ShippingAddress;
                int reqPaymentMethod = request.PaymentMethod;
                int reqTotalBill = request.TotalBill;
                string reqCouponApplied = request.CouponApplied;
                bool reqIsGuess = request.IsGuess;
                isCountDown = reqPaymentMethod == 2 || reqPaymentMethod == 1 ? true : false;
                List<string> errorList= new List<string>();
                           
                List<DTOProductInCart> reqListProduct = JsonConvert.DeserializeObject<List<DTOProductInCart>>(request.ListProduct.ToString());
                int totalBeforeDiscount = 0;
                foreach(var product in reqListProduct)
                {
                    var stock = DB.ProductSizes.Include(ps => ps.CodeProductNavigation).FirstOrDefault(p => p.CodeSize == product.SizeSelected.Code && p.CodeProduct == product.Product.Code);
                    if(stock.Stock < product.Quantity)
                    {
                        var errorString = "Sản phẩm: " + stock.CodeProductNavigation.Name + " hiện tại còn " + stock.Stock + " sản phẩm trong kho.";
                        errorList.Add(errorString);
                    }
                    totalBeforeDiscount += stock.CodeProductNavigation.Price;
                }

                Coupon avaiableCoupon = null;

                if (reqCouponApplied == "" || reqCouponApplied == null)
                {
                    DTOApplyCouponRequest requestApply = new DTOApplyCouponRequest
                    {
                        IdCoupon = reqCouponApplied,
                        TotalBill = reqTotalBill,
                        IsGuess = reqIsGuess
                    };

                    DTOResponse applyResult = _billBLL.ApplyCoupon(requestApply, false);
                    if (applyResult.ErrorString != "")
                    {
                        errorList.Add(applyResult.ErrorString);
                    }
                    else
                    {
                        avaiableCoupon = DB.Coupons.FirstOrDefault(c => c.IdCoupon == reqCouponApplied);
                    }
                }
                

                if (errorList.Count == 0)
                {
                        TimeZoneInfo vietnamTimeZone = TimeZoneInfo.FindSystemTimeZoneById("SE Asia Standard Time");
                        DateTime vietnamTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, vietnamTimeZone);
                        Bill newBill = new Bill
                        {
                            CustomerName = reqCusName,
                            OrdererPhoneNumber = ordererPhoneNumber,
                            PhoneNumber = reqPhoneNumber,
                            ShippingAddress = reqShippingAddress,
                            CreateAt = vietnamTime,
                            PaymentMethod = reqPaymentMethod,
                            TotalBeforeDiscount = totalBeforeDiscount,
                            TotalBill = reqTotalBill,
                            CouponApplied = reqCouponApplied ?? null,
                            Status = reqPaymentMethod == 2 || reqPaymentMethod == 1 ? 17 : 1
                        };

                        DB.Bills.Add(newBill);
                        DB.SaveChanges();

                        int SumBill = 0;
                        foreach (var product in reqListProduct)
                        {
                            var productInDB = DB.Products.FirstOrDefault(p => p.Code == product.Product.Code);
                            var stockOfProduct = DB.ProductSizes.FirstOrDefault(p => p.CodeSize == product.SizeSelected.Code && p.CodeProduct == product.Product.Code);
                            if (productInDB != null)
                            {
                                BillInfo newBI = new BillInfo
                                {
                                    CodeBill = newBill.Code,
                                    CodeProduct = product.Product.Code,
                                    SelectedSize = product.SizeSelected.Code,
                                    Quantity = product.Quantity,
                                    Price = productInDB.Price,
                                    Discount = productInDB.Discount,
                                    TotalPriceBeforeDiscount = productInDB.Price * product.Quantity,
                                    TotalPrice = (int)(CalculatePriceAfterDiscount(productInDB.Price, productInDB.Discount) * product.Quantity),
                                    Status = reqPaymentMethod == 2 || reqPaymentMethod == 1 ? 17 : 1
                                };
                                DB.BillInfos.Add(newBI);
                                stockOfProduct.Stock -= product.Quantity;
                                stockOfProduct.Sold += product.Quantity;
                                DB.SaveChanges();
                                SumBill += newBI.TotalPrice;

                            var cusCode = DB.Customers.Include(c => c.CodeUserNavigation).FirstOrDefault(c => c.CodeUserNavigation.PhoneNumber == ordererPhoneNumber);
                                if (!reqIsGuess && cusCode != null)
                                {
                                    var cartItem = DB.Carts.Include(c => c.CodeCustomerNavigation).ThenInclude(c => c.CodeUserNavigation).FirstOrDefault(c =>
                                    c.CodeCustomer == cusCode.Code && c.CodeProduct == product.Product.Code && c.SelectedSize == product.SizeSelected.Code);
                                    if (cartItem != null)
                                    {
                                        DB.Carts.Remove(cartItem);
                                        DB.SaveChanges();
                                    }
                                }
                            }
                        }

                        newBill.TotalBill = SumBill;

                        if (avaiableCoupon != null)
                        {
                            int discountValue = CalculateCouponDiscount(SumBill, avaiableCoupon);
                            newBill.TotalBeforeDiscount = SumBill;
                            newBill.CouponDiscount = discountValue;
                            newBill.TotalBill = SumBill - discountValue;
                            
                        }
                        
                        DB.SaveChanges();

                        if (isCountDown)
                        {
                            int codeCoupon = 0;
                            if(avaiableCoupon != null)
                            {
                                codeCoupon = avaiableCoupon.Code;
                            }   
                            string eventName = $"DeleteUnPaidBill_{newBill.Code}";
                            string sqlStatement = $@"
                            CREATE EVENT {eventName}
                            ON SCHEDULE AT CURRENT_TIMESTAMP + INTERVAL 16 MINUTE
                            DO
                            CALL DeleteBill('{newBill.Code}','{codeCoupon}');";

                            DB.Database.ExecuteSqlRaw(sqlStatement);

                            dynamic objReturn = new System.Dynamic.ExpandoObject();
                            objReturn.Total = reqTotalBill;
                            objReturn.Code = newBill.Code;
                            objReturn.PaymentMethod = reqPaymentMethod;
                            respond.ErrorString = "Payment";
                            respond.ObjectReturn = objReturn;

                        }
                    }
                
                else
                {
                    respond.ErrorString = "Error";
                    respond.ObjectReturn = new { ErrorList = errorList };
                }
                
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }

        public DTOResponse GetListCartProduct(dynamic requestParam)
        {
            var request = JsonConvert.DeserializeObject<DTOGetListCartRequest>(requestParam.ToString());
            var respond = new DTOResponse();
            DTOCart resultCart = new DTOCart
            {
                CodeCustomer = null,
                ListProductInCart = new List<DTOProductInCart>()
            };
            try
            {
                var productsQuery = new List<Product>();
                

                if (request.CodeCustomer != null)
                {
                    resultCart.CodeCustomer = request.CodeCustomer;
                    int? codeCustomer = request.CodeCustomer;
                    /*productsQuery = from cart in DB.Carts
                                   join product in DB.Products on cart.CodeProduct equals product.Code
                                   where cart.CodeCustomer == codeCustomer
                                   select product;*/

                    productsQuery = DB.Carts.Join(DB.Products, cart => cart.CodeProduct, prod => prod.Code, (cart, product) => new { cart, product })
                                            .Where(cp => cp.cart.CodeCustomer == codeCustomer)
                                            .Select(cp => cp.product).Distinct().ToList();
                    var cartItems = DB.Carts.Where(c => c.CodeCustomer == codeCustomer).ToList();

                    foreach (var cartItem in cartItems)
                    {
                        var product = productsQuery.FirstOrDefault(p => p.Code == cartItem.CodeProduct);
                        if(product != null)
                        {
                            resultCart.ListProductInCart.Add(new DTOProductInCart
                            {
                                Product = MapToDTOProduct(product),
                                Quantity = cartItem.Quantity,
                                SizeSelected = new DTOProductSize
                                {
                                    Code = cartItem.SelectedSize,
                                    Size = DB.Sizes.FirstOrDefault(s => s.Code == cartItem.SelectedSize).Size1
                                },
                                TotalPriceOfProduct = CalculatePriceAfterDiscount(product.Price, product.Discount) * cartItem.Quantity
                            });
                        }
                    }
                }

                else if (request.ListGuessCartProduct != null)
                {
                    var productCodeQuantities = ((IEnumerable<dynamic>)request.ListGuessCartProduct).Select(x => new DTOGuessCartProduct
                    {
                        Code = x.Code,
                        SelectedSize = x.SelectedSize,
                        Quantity = x.Quantity
                    }).ToList();

                    var productCodes = productCodeQuantities.Select(pc => pc.Code).ToList();

                    productsQuery = DB.Products.Include(p => p.CodeProductTypeNavigation)
                                                .Include(p => p.CodeBrandNavigation)
                                                .Where(p => productCodes.Contains(p.Code))
                                                .ToList();

                    foreach (var pcq in productCodeQuantities)
                    {
                        var product = productsQuery.FirstOrDefault(p => p.Code == pcq.Code);
                        if (product != null)
                        {
                            resultCart.ListProductInCart.Add(new DTOProductInCart
                            {
                                Product = MapToDTOProduct(product),
                                Quantity = pcq.Quantity,
                                SizeSelected = new DTOProductSize
                                {
                                    Code = pcq.SelectedSize,
                                    Size = DB.Sizes.FirstOrDefault(s => s.Code == pcq.SelectedSize).Size1
                                },
                                TotalPriceOfProduct = CalculatePriceAfterDiscount(product.Price, product.Discount) * pcq.Quantity
                            });
                        }
                    }
                }

               
                respond.ObjectReturn = resultCart;
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }   
            return respond;
        }

        public DTOResponse GetCountInCart(dynamic requestParam)
        {
            var request = JsonConvert.DeserializeObject<DTOGetListCartRequest>(requestParam.ToString());
            int codeCustomer = request.CodeCustomer;
            var respond = new DTOResponse();
            if (codeCustomer != null)
            {
                var count = DB.Carts.Where(c => c.CodeCustomer == codeCustomer).Sum(c => c.Quantity);
                respond.ObjectReturn = new { Total = count };
            }
            return respond;
        }

        private DTOProduct MapToDTOProduct(Product product)
        {
            DTOProduct result = DB.Products.AsQueryable().Include(p => p.CodeProductTypeNavigation).Include(p => p.CodeBrandNavigation).Where(p => p.Code == product.Code)
            .Select(p => new DTOProduct
            {
                Code = p.Code,
                IdProduct = p.IdProduct,
                Name = p.Name,
                Price = p.Price,
                Description = p.Description,
                ListOfSize = DB.ProductSizes.AsQueryable().Where(i => i.CodeProduct == p.Code).Select(s => new DTOProductSize
                {
                    Code = s.CodeSize,
                    Size = s.CodeSizeNavigation.Size1
                }).ToList(),
                ListOfImage = p.ProductImages.Select(i => new DTOImage
                {
                    Code = i.Code,
                    ImgUrl = i.Img,
                    IsThumbnail = i.IsThumbnail == 1
                }).ToList(),
                Discount = p.Discount,
                PriceAfterDiscount = CalculatePriceAfterDiscount(p.Price, p.Discount),
                CodeProductType = p.CodeProductType,
                ProductType = p.CodeProductTypeNavigation.Name,
                CodeBrand = p.CodeBrand,
                BrandName = p.CodeBrandNavigation.BrandName ?? "",
                Gender = p.Gender,
                Color = p.Color,
                Stock = p.ProductSizes.Sum(p => p.Stock),
                Sold = p.ProductSizes.Sum(p => p.Sold),
                Status = p.Status
            }).FirstOrDefault();
            return result;

        }


        private int CalculateCouponDiscount(int TotalBill, Coupon coupon)
        {
            if(coupon.CouponType == 0)
            {
                int percentDiscount = (int)coupon.PercentDiscount;
                int discountValue = (TotalBill * percentDiscount / 100);
                if(discountValue > coupon.MaxBillDiscount)
                {
                    discountValue = (int)coupon.MaxBillDiscount;
                }
                coupon.RemainingQuantity -= 1;
                DB.SaveChanges();
                return discountValue;
            }
            else
            {
                int discountValue = (int)coupon.DirectDiscount;
                coupon.RemainingQuantity -= 1;
                DB.SaveChanges();
                return discountValue;
            }
        }
    }
}
