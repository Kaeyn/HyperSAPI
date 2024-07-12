using APP.Bus.Repository.DTOs.Bill;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.DTOs;
using APP.DAL.Repository.Entities;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APP.Bus.Repository.Mathmathics.StaticFunc;
using KendoNET.DynamicLinq;

namespace APP.Bus.Repository.BLLs
{
    public class CartBillBLL
    {
        private AppDBContext DB;


        public CartBillBLL()
        {
            DB = new AppDBContext();
        }

        public async Task<DTOResponse> ProceedToPayment(dynamic requestParam, DTOProceedToPayment? dTOProceedToPayment, bool isCountDown)
        {
            dynamic request = null;
            if (dTOProceedToPayment != null)
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
                List<string> errorList = new List<string>();

                List<DTOProductInCart> reqListProduct = JsonConvert.DeserializeObject<List<DTOProductInCart>>(request.ListProduct.ToString());
                int totalBeforeDiscount = 0;
                foreach (var product in reqListProduct)
                {
                    var stock = DB.ProductSizes.Include(ps => ps.CodeProductNavigation).FirstOrDefault(p => p.CodeSize == product.SizeSelected.Code && p.CodeProduct == product.Product.Code);
                    if (stock.Stock < product.Quantity)
                    {
                        var errorString = "Sản phẩm: " + stock.CodeProductNavigation.Name + " hiện tại còn " + stock.Stock + " sản phẩm trong kho.";
                        errorList.Add(errorString);
                    }
                    totalBeforeDiscount += stock.CodeProductNavigation.Price;
                }

                Coupon avaiableCoupon = null;

                if (reqCouponApplied != "" || reqCouponApplied != null)
                {
                    DTOApplyCouponRequest requestApply = new DTOApplyCouponRequest
                    {
                        IdCoupon = reqCouponApplied,
                        TotalBill = reqTotalBill,
                        IsGuess = reqIsGuess
                    };

                    DTOResponse applyResult = await ApplyCoupon(requestApply, false);
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
                        if (avaiableCoupon != null)
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

        public async Task<DTOResponse> ApplyCoupon(DTOApplyCouponRequest couponRequest, bool getDB)
        {
            var respond = new DTOResponse();
            try
            {

                DataSourceRequest dataSourceRequest = new DataSourceRequest();
                dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
                var coupons = DB.Coupons.FirstOrDefault(b => b.IdCoupon == couponRequest.IdCoupon);
                if (coupons != null)
                {
                    if (coupons.Stage == 2 && coupons.Status == 1)
                    {
                        int toIntGuessorCus = couponRequest.IsGuess ? 0 : 1;
                        if (coupons.ApplyTo == toIntGuessorCus)
                        {
                            if (couponRequest.TotalBill >= coupons.MinBillPrice)
                            {
                                if (coupons.RemainingQuantity > 0)
                                {
                                    if (getDB == true)
                                    {
                                        var avaibleCoupons = DB.Coupons.AsQueryable().Where(b => b.IdCoupon == couponRequest.IdCoupon).Select(c => new
                                        {
                                            IdCoupon = c.IdCoupon,
                                            MaxBillDiscount = c.MaxBillDiscount,
                                            CouponType = c.CouponType,
                                            DirectDiscount = c.DirectDiscount,
                                            PercentDiscount = c.PercentDiscount
                                        }).ToList();
                                        respond.ObjectReturn = avaibleCoupons.AsQueryable().ToDataSourceResult(dataSourceRequest);
                                    }
                                }
                                else
                                {
                                    respond.ErrorString = "Phiếu giảm giá đã hết lượt sử dụng !";
                                }
                            }
                            else
                            {
                                respond.ErrorString = "Đơn hàng chưa đạt giá trị tối thiếu là: " + coupons.MinBillPrice;
                            }
                        }
                        else
                        {
                            respond.ErrorString = "Bạn không đủ điều kiện dùng phiếu giảm giá này";
                        }

                    }
                    else
                    {
                        respond.ErrorString = "Phiếu giảm giá không còn hiệu lực !";
                    }
                }
                else
                {
                    respond.ErrorString = "Phiếu giảm giá không tồn tại !";
                }

            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public void UpdatePaymentString(int codeBill, string url)
        {
            var bill = DB.Bills.Include(b => b.BillInfos).FirstOrDefault(b => b.Code == codeBill);
            bill.PaymentUrl = url;
            DB.SaveChanges();
        }

        public void SuccessPaymentUpdate(int codeBill)
        {
            var bill = DB.Bills.Include(b => b.BillInfos).FirstOrDefault(b => b.Code == codeBill);
            bill.Status = 1;
            DB.SaveChanges();
            foreach (var item in bill.BillInfos)
            {
                item.Status = 1;
                DB.SaveChanges();
            }
        }

        public int CalculateCouponDiscount(int TotalBill, Coupon coupon)
        {
            if (coupon.CouponType == 0)
            {
                int percentDiscount = (int)coupon.PercentDiscount;
                int discountValue = (TotalBill * percentDiscount / 100);
                if (discountValue > coupon.MaxBillDiscount)
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
