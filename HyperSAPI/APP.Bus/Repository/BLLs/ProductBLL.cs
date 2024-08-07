﻿using APP.Bus.Repository.DTOs;
using APP.DAL.Repository.Entities;
using KendoNET.DynamicLinq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web.Http;
using APP.Bus.Repository.Mathmathics;
using static APP.Bus.Repository.Mathmathics.StaticFunc;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.DTOs.Product;
using System.Diagnostics;
using APP.Bus.Repository.DTOs.Staff;
namespace APP.Bus.Repository.BLLs
{
    public class ProductBLL
    {
        private AppDBContext DB;

        public ProductBLL() {
            DB = new AppDBContext();
        }

        public async Task<DTOResponse> GetProduct(DTOProduct dtoRequest)
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest();
            dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
            var respond = new DTOResponse();
            try
            {

                var products = DB.Products.AsQueryable()
                    .Include(p => p.CodeProductTypeNavigation)
                    .Include(p => p.CodeBrandNavigation).Where(p => p.Code == dtoRequest.Code)
                    .Select(p => new
                    {
                        Code = p.Code,
                        IdProduct = p.IdProduct,
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        ListOfSize = DB.ProductSizes.AsQueryable().Where(i => i.CodeProduct == p.Code).Select(s => new DTOProductSize
                        {
                            Code = s.CodeSize,
                            Size = s.CodeSizeNavigation.Size1,
                            Stock = s.Stock ?? 0,
                            Sold = s.Sold ?? 0,
                        }).ToList(),
                        ListOfImage = p.ProductImages.Select(i => new DTOImage
                        {
                            Code = i.Code,
                            ImgUrl = i.Img,
                            IsThumbnail = i.IsThumbnail == 1 ? true : false,
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
                        ThumbnailImg = p.ProductImages.FirstOrDefault(i => i.IsThumbnail == 1).Img,
                        Status = p.Status
                    }).ToList();

                respond.ObjectReturn = products.AsQueryable().ToDataSourceResult(dataSourceRequest);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }
        public async Task<DTOResponse> GetProductByID(string ID)
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest();
            dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
            var respond = new DTOResponse();
            try
            {

                var products = DB.Products.AsQueryable()
                    .Include(p => p.CodeProductTypeNavigation)
                    .Include(p => p.CodeBrandNavigation).Where(p => p.IdProduct == ID)
                    .Select(p => new
                    {
                        Code = p.Code,
                        IdProduct = p.IdProduct,
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        ListOfSize = DB.ProductSizes.AsQueryable().Where(i => i.CodeProduct == p.Code).Select(s => new DTOProductSize
                        {
                            Code = s.CodeSize,
                            Size = s.CodeSizeNavigation.Size1,
                            Stock = s.Stock ?? 0,
                            Sold = s.Sold ?? 0,
                        }).ToList(),
                        ListOfImage = p.ProductImages.Select(i => new DTOImage
                        {
                            Code = i.Code,
                            ImgUrl = i.Img,
                            IsThumbnail = i.IsThumbnail == 1 ? true : false,
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
                        ThumbnailImg = p.ProductImages.FirstOrDefault(i => i.IsThumbnail == 1).Img,
                        Status = p.Status
                    }).ToList();

                respond.ObjectReturn = products.AsQueryable().ToDataSourceResult(dataSourceRequest);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public async Task<DTOResponse> GetListProduct(dynamic options)
        {
            var respond = new DTOResponse();           
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(options.ToString());
                /*options = StaticFunc.FormatFilter(options);*/
                
                var products = DB.Products.AsQueryable().Include(p => p.CodeProductTypeNavigation).Include(p => p.CodeBrandNavigation)
                    .Select(p => new 
                    {
                        Code = p.Code,
                        IdProduct = p.IdProduct,
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        ListOfSize = DB.ProductSizes.AsQueryable().Where(i => i.CodeProduct == p.Code).Select(s => new DTOProductSize
                        {
                            Code = s.CodeSize,
                            Size = s.CodeSizeNavigation.Size1,
                            Stock = s.Stock?? 0,
                            Sold = s.Sold?? 0,
                            
                        }).ToList(),
                        ListOfImage = p.ProductImages.Select(i => new DTOImage
                        {
                            Code = i.Code,
                            ImgUrl = i.Img,
                            IsThumbnail = i.IsThumbnail == 1 ? true : false,
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
                        ThumbnailImg = p.ProductImages.FirstOrDefault(i => i.IsThumbnail == 1).Img,
                        Status = p.Status
                    }).ToList();

                respond.ObjectReturn = products.AsQueryable().ToDataSourceResult((DataSourceRequest)param);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public async Task<DTOResponse> GetListProductSale(dynamic options)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(options.ToString());
                /*options = StaticFunc.FormatFilter(options);*/

                var products = DB.Products.AsQueryable().Include(p => p.CodeProductTypeNavigation).Include(p => p.CodeBrandNavigation).Where(p => p.Discount.HasValue)
                    .Select(p => new 
                    {
                        Code = p.Code,
                        IdProduct = p.IdProduct,
                        Name = p.Name,
                        Price = p.Price,
                        Description = p.Description,
                        ListOfSize = DB.ProductSizes.AsQueryable().Where(i => i.CodeProduct == p.Code).Select(s => new DTOProductSize
                        {
                            Code = s.CodeSize,
                            Size = s.CodeSizeNavigation.Size1,
                            Stock = s.Stock ?? 0,
                            Sold = s.Sold ?? 0,
                        }).ToList(),
                        ListOfImage = p.ProductImages.Select(i => new DTOImage
                        {
                            Code = i.Code,
                            ImgUrl = i.Img,
                            IsThumbnail = i.IsThumbnail == 1 ? true : false,
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
                        ThumbnailImg = p.ProductImages.FirstOrDefault(i => i.IsThumbnail == 1).Img,
                        Status = p.Status
                    }).ToList();

                respond.ObjectReturn = products.AsQueryable().ToDataSourceResult((DataSourceRequest)param);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }

        public async Task<DTOResponse> GetListProductType()
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest();
            dataSourceRequest.Sort = GetSortDescriptor("Code", "asc");
            var respond = new DTOResponse();
            try
            {
                var products = DB.ProductTypes.AsQueryable()
                    .Select(p => new
                    {
                        Code = p.Code,
                        IdProductType = p.IdProductType,
                        Name = p.Name                   
                    }).ToList();

                respond.ObjectReturn = products.AsQueryable().ToDataSourceResult(dataSourceRequest);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public async Task<DTOResponse> AddProductToCart(DTOAddToCart request)
        {
                
            var respond = new DTOResponse();
            try
            {
                var stock = DB.ProductSizes.Include(ps=> ps.CodeProductNavigation).FirstOrDefault(p => p.CodeSize == request.SelectedSize && p.CodeProduct == request.CodeProduct);
                if (request.CodeCustomer == -1)
                {
                    if(stock.Stock <= request.Quantity)
                    {
                        var errorString = "Sản phẩm: " + stock.CodeProductNavigation.Name + " hiện tại còn " + stock.Stock + " sản phẩm trong kho.";
                        respond.ErrorString = errorString;
                    }
                }
                else
                {
                    var existedCartItem = DB.Carts.FirstOrDefault(ci => ci.CodeCustomer == request.CodeCustomer && ci.CodeProduct == request.CodeProduct && request.SelectedSize == ci.SelectedSize);

                    if (request.Type.Equals("Add"))
                    {
                        if (existedCartItem != null)
                        {
                            if (request.Quantity == 1 && request.SelectedSize == existedCartItem?.SelectedSize)
                            {
                                if (existedCartItem.Quantity < 10)
                                {
                                    if (stock.Stock > existedCartItem.Quantity)
                                    {
                                        existedCartItem.Quantity += 1;
                                    }
                                    else
                                    {
                                        var errorString = "Sản phẩm: " + stock.CodeProductNavigation.Name + " hiện tại còn " + stock.Stock + " sản phẩm trong kho.";
                                        respond.ErrorString = errorString;
                                    }
                                }
                            }
                        }
                        else
                        {
                            var newCartItem = new Cart
                            {
                                CodeProduct = request.CodeProduct,
                                CodeCustomer = request.CodeCustomer,
                                SelectedSize = request.SelectedSize,
                                Quantity = request.Quantity
                            };
                            DB.Carts.Add(newCartItem);
                        }

                    }
                    else if (request.Type.Equals("Update"))
                    {

                        if (existedCartItem != null)
                        {
                            existedCartItem.Quantity = request.Quantity;
                        }
                    }
                    else if (request.Type.Equals("Delete"))
                    {
                        if (existedCartItem != null)
                        {
                            DB.Carts.Remove(existedCartItem);
                        }
                    }
                    DB.SaveChanges();
                    respond.ObjectReturn = new { };
                }
                
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public async Task<DTOResponse> UpdateProduct(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {   
                var param = JsonConvert.DeserializeObject<DTOUpdateProductRequest>(requestParam.ToString());
                DTOProduct reqProd = param.Product;
                var changedProperties = param.Properties;
                var existedCheck = DB.Products.Any(p=> p.IdProduct == reqProd.IdProduct);

                if (reqProd.Code == 0)
                {
                    if (existedCheck)
                    {
                        respond.ErrorString = "Trùng mã sản phẩm trong hệ thống!";
                    }
                    else
                    {
                        var newProd = new Product
                        {
                            IdProduct = reqProd.IdProduct,
                            Name = reqProd.Name,
                            CodeProductType = reqProd.CodeProductType,
                            CodeBrand = reqProd.CodeBrand,
                            Price = reqProd.Price,
                            Description = reqProd.Description,
                            Color = reqProd.Color,
                            Gender = reqProd.Gender,
                            Discount = reqProd.Discount,
                            DiscountDescription = reqProd.DiscountDescription,
                            IsNew = reqProd.Status,
                            Status = reqProd.Status
                        };

                        await DB.Products.AddAsync(newProd);
                        DB.SaveChanges();

                        var reqListOfImage = reqProd.ListOfImage;

                        foreach (var img in reqListOfImage)
                        {
                            var newImg = new ProductImage
                            {
                                IdImage = img.IdImage,
                                Img = img.ImgUrl,
                                ProductCode = newProd.Code,
                                IsThumbnail = img.IsThumbnail == true ? (sbyte)1 : (sbyte)0,
                            };

                            await DB.ProductImages.AddAsync(newImg);
                            DB.SaveChanges();
                        }
                        var reqListOfSize = reqProd.ListOfSize;

                        foreach (var size in reqListOfSize)
                        {
                            var newSize = new ProductSize
                            {
                                CodeProduct = newProd.Code,
                                CodeSize = size.Code,
                                Stock = size.Stock,
                                Sold = size.Sold
                            };

                            await DB.ProductSizes.AddAsync(newSize);
                            DB.SaveChanges();
                        }

                        DB.SaveChanges();

                        respond.ObjectReturn = new { };
                    }
                           
                }
                else
                {
                    var existingProd = DB.Products.Include(p => p.CodeBrandNavigation)
                                                    .Include(p => p.CodeProductTypeNavigation)
                                                    .Include(p => p.ProductSizes)
                                                    .Include(p => p.ProductImages)
                                                    .FirstOrDefault(p => p.Code == reqProd.Code);
                    if (existingProd != null)
                    {
                        foreach (var property in changedProperties)
                        {
                            var prodProperty = typeof(DTOProduct).GetProperty(property);
                            if (prodProperty != null)
                            {
                                var newValue = prodProperty.GetValue(reqProd);
                                var existingProdProperty = typeof(Product).GetProperty(property);
                                if (existingProdProperty != null)
                                {
                                    existingProdProperty.SetValue(existingProd, newValue, null);
                                    DB.SaveChanges();

                                }
                            }
                        }

                        DB.ProductImages.RemoveRange(existingProd.ProductImages);
                        foreach (var image in reqProd.ListOfImage)
                        {
                            existingProd.ProductImages.Add(new ProductImage
                            {
                                Img = image.ImgUrl,
                                ProductCode = existingProd.Code,
                                IsThumbnail = image.IsThumbnail ? (sbyte)1 : (sbyte)0
                            });
                        }

                        DB.ProductSizes.RemoveRange(existingProd.ProductSizes);
                        foreach (var size in reqProd.ListOfSize)
                        {
                            existingProd.ProductSizes.Add(new ProductSize
                            {
                                CodeSize = size.Code,
                                CodeProduct = existingProd.Code,
                                Stock = size.Stock,
                                Sold = size.Sold
                            });
                        }



                        DB.SaveChanges();
                    }
                    else
                    {
                        respond.StatusCode = 404;
                        respond.ErrorString = "Product not found.";
                    }
                }
                           
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }

        public async Task<DTOResponse> UpdateProductType(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOUpdateProductType>(requestParam.ToString());
                DTOProductType reqProductType = param.ProductType;
                var changedProperties = param.Properties;
                bool existedProductType = DB.ProductTypes.Any(pt => pt.IdProductType == reqProductType.IdProductType);
                if (reqProductType.Code == 0)
                {
                    if (existedProductType)
                    {
                        respond.ErrorString = "Trùng mã loại sản phẩm trong hệ thống !";
                    }
                    else
                    {
                        var newProductType = new ProductType
                        {
                            IdProductType = reqProductType.IdProductType,
                            Name = reqProductType.Name,
                        };

                        DB.ProductTypes.Add(newProductType);
                        DB.SaveChanges();
                    }
                    
                }
                else
                {
                    var existingProductType = DB.ProductTypes.FirstOrDefault(b => b.Code == reqProductType.Code);
                    if (existingProductType != null)
                    {
                        foreach (var property in changedProperties)
                        {
                            var productTypeProperty = typeof(DTOProductType).GetProperty(property);
                            if (productTypeProperty != null)
                            {
                                var newValue = productTypeProperty.GetValue(reqProductType);
                                var existingProductTypeProperty = typeof(ProductType).GetProperty(property);
                                if (existingProductTypeProperty != null)
                                {                                   
                                    existingProductTypeProperty.SetValue(existingProductType, newValue, null);
                                    DB.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        respond.StatusCode = 404;
                        respond.ErrorString = "ProductType not found.";
                    }
                }

            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }
    }
}
