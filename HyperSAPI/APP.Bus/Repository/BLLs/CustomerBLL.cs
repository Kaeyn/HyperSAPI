using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Customer;
using APP.Bus.Repository.DTOs.Product;
using APP.Bus.Repository.DTOs.ShippingAddress;
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

        public async Task<DTOResponse> GetCustomer(string reqPhoneNumber)
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest();
            dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
            var respond = new DTOResponse();
            try
            {
                var result = DB.Customers.AsQueryable()
                           .Include(c => c.CodeUserNavigation).Where(c => c.CodeUserNavigation.PhoneNumber == reqPhoneNumber)
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
                           }).ToList();

                respond.ObjectReturn = result.AsQueryable().ToDataSourceResult(dataSourceRequest);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public async Task<DTOResponse> GetListCustomer(dynamic requestParam)
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

        public async Task<DTOResponse> UpdateCustomer(dynamic requestParam)
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

        /*public DTOResponse GetListShippingAddress(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                int CustomerCode = param.CustomerCode;
                string Address = param.Address;
                string PhoneNumber = param.PhoneNumber;
                string ReceiverName = param.ReceiverName;
                bool IsDefaultAddress = param.IsDefaultAddress;

                if (Code == 0)
                {
                    ShippingAddress newShippingAddress = new ShippingAddress
                    {
                        CustomerCode = CustomerCode,
                        Address = Address,
                        PhoneNumber = PhoneNumber,
                        ReceiverName = ReceiverName,
                        IsDefaultAddress = IsDefaultAddress == true ? (sbyte)1 : (sbyte)0,
                    };
                    DB.ShippingAddresses.Add(newShippingAddress);
                }
                else
                {
                    var existedAddress = DB.ShippingAddresses.FirstOrDefault(sa => sa.Code == Code && sa.CustomerCode == CustomerCode);
                    if (existedAddress != null)
                    {
                        existedAddress.Address = Address;
                        existedAddress.PhoneNumber = PhoneNumber;
                        existedAddress.ReceiverName = ReceiverName;
                        existedAddress.IsDefaultAddress = IsDefaultAddress == true ? (sbyte)1 : (sbyte)0;
                    }
                }

                DB.SaveChanges();
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }
*/
        public async Task<DTOResponse> UpdateShippingAddress(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOShippingAddress>(requestParam.ToString());
                int Code = param.Code;
                int CustomerCode = param.CustomerCode;
                string Address = param.Address;
                string PhoneNumber = param.PhoneNumber;
                string ReceiverName = param.ReceiverName;
                bool IsDefaultAddress = param.IsDefaultAddress;

                if (Code == 0)
                {
                    ShippingAddress newShippingAddress = new ShippingAddress
                    {
                        CustomerCode = CustomerCode,
                        Address = Address,
                        PhoneNumber = PhoneNumber,
                        ReceiverName = ReceiverName,
                        IsDefaultAddress = IsDefaultAddress == true ? (sbyte)1 : (sbyte)0,
                    };
                    DB.ShippingAddresses.Add(newShippingAddress);
                }
                else
                {
                    var existedAddress = DB.ShippingAddresses.FirstOrDefault(sa => sa.Code == Code && sa.CustomerCode == CustomerCode);
                    if (existedAddress != null)
                    {
                        existedAddress.Address = Address;
                        existedAddress.PhoneNumber = PhoneNumber;
                        existedAddress.ReceiverName = ReceiverName;
                        existedAddress.IsDefaultAddress = IsDefaultAddress == true ? (sbyte)1 : (sbyte)0;
                    }
                }

                DB.SaveChanges();
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
