using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Brands;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.DTOs.Product;
using APP.DAL.Repository.Entities;
using KendoNET.DynamicLinq;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APP.Bus.Repository.Mathmathics.StaticFunc;

namespace APP.Bus.Repository.BLLs
{
    public class BrandBLL
    {
        private AppDBContext DB;

        public BrandBLL()
        {
            DB = new AppDBContext();
        }

        public DTOResponse GetListBrand()
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest();
            dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
            var respond = new DTOResponse();
            try
            {
                var brands = DB.Brands.AsQueryable().Select(b => new
                {
                    Code = b.Code,
                    IdBrand = b.IdBrand,
                    Name = b.BrandName,
                    ImageUrl = b.ImageUrl
                });
                respond.ObjectReturn = brands.AsQueryable().ToDataSourceResult(dataSourceRequest);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public DTOResponse UpdateBrand(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOUpdateBrandRequest>(requestParam.ToString());
                DTOBrand reqBrand = param.Brand;
                var changedProperties = param.Properties;
                bool existedBrand = DB.Brands.Any(pt => pt.IdBrand == reqBrand.IdBrand);

                if (reqBrand.Code == 0)
                {
                    if (existedBrand)
                    {
                        respond.ErrorString = "Trùng mã thương hiệu trong hệ thống !";
                    }
                    else
                    {
                        var newBrand = new Brand
                        {
                            IdBrand = reqBrand.IdBrand,
                            BrandName = reqBrand.BrandName,
                            ImageUrl = reqBrand.ImageUrl,
                        };

                        DB.Brands.Add(newBrand);
                        DB.SaveChanges();
                    }

                }
                else
                {
                    var existingBrand = DB.ProductTypes.FirstOrDefault(b => b.Code == reqBrand.Code);
                    if (existingBrand != null)
                    {
                        foreach (var property in changedProperties)
                        {
                            var brandProperty = typeof(DTOBrand).GetProperty(property);
                            if (brandProperty != null)
                            {
                                var newValue = brandProperty.GetValue(reqBrand);
                                var existingBrandProperty = typeof(Brand).GetProperty(property);
                                if (existingBrandProperty != null)
                                {
                                    existingBrandProperty.SetValue(existingBrand, newValue, null);
                                    DB.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        respond.StatusCode = 404;
                        respond.ErrorString = "Brand not found.";
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

