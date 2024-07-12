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

        public CartBLL()
        {
            DB = new AppDBContext();         
        }

        public async Task<DTOResponse> GetListCartProduct(dynamic requestParam)
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

        public async Task<DTOResponse> GetCountInCart(dynamic requestParam)
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

      
    }
}
