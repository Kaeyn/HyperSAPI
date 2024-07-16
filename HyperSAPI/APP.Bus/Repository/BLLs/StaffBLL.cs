using APP.Bus.Repository.DTOs.Customer;
using APP.Bus.Repository.DTOs;
using APP.DAL.Repository.Entities;
using KendoNET.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using APP.Bus.Repository.DTOs.Staff;
using static APP.Bus.Repository.Mathmathics.StaticFunc;

namespace APP.Bus.Repository.BLLs
{
    public class StaffBLL
    {
        private AppDBContext DB;

        public StaffBLL()
        {
            DB = new AppDBContext();
        }

        public async Task<DTOResponse> GetCurrentStaffInfo(string phoneNumber)
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest();
            dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
            var respond = new DTOResponse();
            try
            {
                var result = DB.Staff.AsQueryable()
                    .Include(s => s.CodeUserNavigation).Where(s => s.CodeUserNavigation.PhoneNumber == phoneNumber)
                    .Select(s => new
                    {
                        Code = s.Code,
                        IdStaff = s.IdStaff,
                        Name = s.Name,
                        ImageUrl = s.ImageUrl,
                        Gender = s.Gender,
                        Birthday = s.Birthday,
                        PhoneNumber = s.CodeUserNavigation.PhoneNumber,
                        Email = s.CodeUserNavigation.Email,
                        Address = s.Address,
                        Identication = s.Identication,
                        CodeAccount = s.CodeUser,
                        Status = s.CodeUserNavigation.Status,
                        StatusAccountStr = ConvertStatusToStr(s.CodeUserNavigation.Status),
                        Permission = s.CodeUserNavigation.Permission
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

        public async Task<DTOResponse> GetStaff(DTOStaff requestStaff)
        {
            DataSourceRequest dataSourceRequest = new DataSourceRequest();
            dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
            var respond = new DTOResponse();
            try
            {
                var result = DB.Staff.AsQueryable()
                    .Include(s => s.CodeUserNavigation).Where(s => s.Code == requestStaff.Code)
                    .Select(s => new
                    {
                        Code = s.Code,
                        IdStaff = s.IdStaff,
                        Name = s.Name,
                        ImageUrl = s.ImageUrl,
                        Gender = s.Gender,
                        Birthday = s.Birthday,
                        PhoneNumber = s.CodeUserNavigation.PhoneNumber,
                        Email = s.CodeUserNavigation.Email,
                        Address = s.Address,
                        Identication = s.Identication,
                        CodeAccount = s.CodeUser,
                        Status = s.CodeUserNavigation.Status,
                        StatusAccountStr = ConvertStatusToStr(s.CodeUserNavigation.Status),
                        Permission = s.CodeUserNavigation.Permission
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

        public async Task<DTOResponse> GetListStaff(dynamic requestParam)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(requestParam.ToString());

                var result = DB.Staff.AsQueryable()
                    .Include(s => s.CodeUserNavigation)
                    .Select(s => new
                    {
                        Code = s.Code,
                        IdStaff = s.IdStaff,
                        Name = s.Name,
                        ImageUrl = s.ImageUrl,
                        Gender = s.Gender,
                        Birthday = s.Birthday,
                        PhoneNumber = s.CodeUserNavigation.PhoneNumber,
                        Email = s.CodeUserNavigation.Email,
                        Address = s.Address,
                        Identication = s.Identication,
                        CodeAccount = s.CodeUser,
                        Status = s.CodeUserNavigation.Status,
                        StatusAccountStr = ConvertStatusToStr(s.CodeUserNavigation.Status),
                        Permission = s.CodeUserNavigation.Permission
                    }).ToList();
                            
                respond.ObjectReturn = result.AsQueryable().ToDataSourceResult((DataSourceRequest)param);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }

        /*public DTOResponse UpdateStaff(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOUpdateStaffRequest>(requestParam.ToString());
                DTOStaff staffData = param.Staff;
                var changedProperties = param.Properties;
                if (staffData.Code == 0)
                {
                    var newUser = new User
                    {
                        PhoneNumber = staffData.PhoneNumber,
                        Email = staffData.Email,
                        Status = 0,
                        Permission = staffData.Permission
                    };

                    DB.Add(newUser);
                    DB.SaveChanges();

                    var newUserCode = newUser.Code;

                    var newStaff = new Staff
                    {
                        Name = staffData.Name,
                        Birthday = staffData.Birth,
                        Address = staffData.Address,
                        Position = staffData.Position,
                        CodeUser = newUserCode,
                        ImageUrl = staffData.ImageURL,
                        Gender = staffData.Gender,
                        Identication = staffData.Identication,
                        Idstaff = staffData.IDStaff
                    };

                    DB.Staff.Add(newStaff);
                    DB.SaveChanges();

                    respond.ObjectReturn = new { };
                }
                else
                {
                    var existingStaff = DB.Staff.Include(s => s.CodeUserNavigation)
                                                .Include(s => s.PositionNavigation)
                                                .FirstOrDefault(s => s.Code == staffData.Code);
                    if (existingStaff != null)
                    {
                        foreach (var property in changedProperties)
                        {
                            var staffProperty = typeof(DTOStaff).GetProperty(property);
                            if (staffProperty != null)
                            {
                                var newValue = staffProperty.GetValue(staffData);
                                var existingStaffProperty = typeof(Staff).GetProperty(property);
                                if (existingStaffProperty != null)
                                {
                                    existingStaffProperty.SetValue(existingStaff, Convert.ChangeType(newValue, existingStaffProperty.PropertyType), null);
                                }
                                else
                                {
                                    var existingUserProperty = typeof(User).GetProperty(property);
                                    if (existingUserProperty != null)
                                    {
                                        existingUserProperty.SetValue(existingStaff, Convert.ChangeType(newValue, existingUserProperty.PropertyType), null);
                                    }
                                    else
                                    {
                                        respond.StatusCode = 400;
                                        respond.ErrorString = $"Property {property} not found.";
                                        respond.ObjectReturn = new { };
                                        return respond;
                                    }
                                }
                            }
                        }
                    }
                    else
                    {
                        respond.StatusCode = 404;
                        respond.ErrorString = "Staff not found.";
                    }
                }
                DB.SaveChanges();
                respond.ObjectReturn = new { };
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }
            return respond;
        }
*/
    }
}
