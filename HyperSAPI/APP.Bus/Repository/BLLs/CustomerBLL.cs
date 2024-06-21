using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Customer;
using APP.DAL.Repository.Entities;
using KendoNET.DynamicLinq;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static APP.Bus.Repository.Mathmathics.StaticFunc;

namespace APP.Bus.Repository.BLLs
{
    public class CustomerBLL
    {
        private AppDBContext DB;

        public CustomerBLL()
        {
            DB = new AppDBContext();
        }

        public DTOResponse GetListCustomer(dynamic requestParam)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(requestParam.ToString());

                var result = DB.Customers.AsQueryable()
                            .Include(c => c.CodeUserNavigation)
                            .Select(c => new DTOCustomer
                            {
                                Code = c.Code,
                                IDCustomer = c.Idcustomer,
                                Name = c.Name,
                                ImageURL = c.ImageUrl,
                                Gender = c.Gender,
                                Birth = c.Birthday,
                                PhoneNumber = c.CodeUserNavigation.PhoneNumber,
                                Email = c.CodeUserNavigation.Email,
                                CodeAccount = c.CodeUserNavigation.Code,
                                StatusAccount = c.CodeUserNavigation.Status,
                                StatusAccountStr = ConvertStatusToStr(c.CodeUserNavigation.Status),
                                Permission = c.CodeUserNavigation.Permission,
                                PermissionStr = ConvertPermissionToStr(c.CodeUserNavigation.Permission)
                            }).ToList().OrderBy(c => c.Name);
                respond.ObjectReturn = result.AsQueryable().ToDataSourceResult((DataSourceRequest)param);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }

        public DTOResponse UpdateCustomer(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOUpdateCustomerRequest>(requestParam.ToString());
                int codeAccount = param.CodeAccount;
                int status = param.CodeStatus;
                var result = DB.Users.FirstOrDefault(c => c.Code == codeAccount);
                if (result != null) {
                    result.Status = status;
                }
                else
                {
                    respond.StatusCode = 500;
                    respond.ErrorString = "User not exists !";
                }

                DB.SaveChanges();
                respond.ObjectReturn = new {};
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
