using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.DTOs.Product;
using APP.DAL.Repository.Entities;
using KendoNET.DynamicLinq;
using Microsoft.EntityFrameworkCore;
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
    }
}

