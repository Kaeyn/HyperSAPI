using APP.Bus.Repository.DTOs;
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
namespace APP.Bus.Repository.BLLs
{
    public class ProductBLL
    {
        private AppDBContext DB;

        public ProductBLL() {
            DB = new AppDBContext();
        }

        public DTOResponse GetProduct(DTOProduct dtoRequest)
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest();
            dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
            var respond = new DTOResponse();
            try
            {

                var products = DB.Products.AsQueryable()
                    .Include(p => p.ProductTypeNavigation)
                    .Include(p => p.BrandNavigation).Where(p => p.Code == dtoRequest.Code)
                    .Select(p => new
                    {
                        Code = p.Code,
                        IdProduct = p.IdProduct,
                        Name = p.ProductName,
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
                            IsThumbnail = i.IsThumbnail == 1 ? true : false,
                        }).ToList(),
                        Discount = p.Discount,
                        PriceAfterDiscount = CalculatePriceAfterDiscount(p.Price, p.Discount),
                        CodeProductType = p.ProductType,
                        ProductType = p.ProductTypeNavigation.Name,
                        CodeBrand = p.Brand,
                        BrandName = p.BrandNavigation.BrandName ?? "",
                        Gender = p.Gender,
                        Color = p.Color,
                        Stock = p.Stock,
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

        public DTOResponse GetListProduct(dynamic options)
        {
            var respond = new DTOResponse();           
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(options.ToString());
                /*options = StaticFunc.FormatFilter(options);*/

                var products = DB.Products.AsQueryable().Include(p => p.ProductTypeNavigation).Include(p => p.BrandNavigation)
                    .Select(p => new 
                    {
                        Code = p.Code,
                        IdProduct = p.IdProduct,
                        Name = p.ProductName,
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
                            IsThumbnail = i.IsThumbnail == 1 ? true : false,
                        }).ToList(),
                        Discount = p.Discount,
                        PriceAfterDiscount = CalculatePriceAfterDiscount(p.Price, p.Discount),
                        CodeProductType = p.ProductType,
                        ProductType = p.ProductTypeNavigation.Name,
                        CodeBrand = p.Brand,
                        BrandName = p.BrandNavigation.BrandName ?? "",
                        Gender = p.Gender,
                        Color = p.Color,
                        Stock = p.Stock,
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

        public DTOResponse GetListProductSale(dynamic options)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(options.ToString());
                /*options = StaticFunc.FormatFilter(options);*/

                var products = DB.Products.AsQueryable().Include(p => p.ProductTypeNavigation).Include(p => p.BrandNavigation).Where(p => p.Discount.HasValue)
                    .Select(p => new 
                    {
                        Code = p.Code,
                        IdProduct = p.IdProduct,
                        Name = p.ProductName,
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
                            IsThumbnail = i.IsThumbnail == 1 ? true : false,
                        }).ToList(),
                        Discount = p.Discount,
                        PriceAfterDiscount = CalculatePriceAfterDiscount(p.Price, p.Discount),
                        CodeProductType = p.ProductType,
                        ProductType = p.ProductTypeNavigation.Name,
                        CodeBrand = p.Brand,
                        BrandName = p.BrandNavigation.BrandName ?? "",
                        Gender = p.Gender,
                        Color = p.Color,
                        Stock = p.Stock,
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

        public DTOResponse GetListProductType()
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

        private static decimal CalculatePriceAfterDiscount(decimal price, int? discount)
        {
            if (discount.HasValue && discount.Value > 0)
            {
                return price - (price * discount.Value / 100);
            }
            return price;
        }
    }
}
