using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Coupon;
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

namespace APP.Bus.Repository.BLLs
{
    public class CouponBLL
    {
        private AppDBContext DB;

        public CouponBLL() {
            DB = new AppDBContext();
        }

        public async Task<DTOResponse> GetListCoupon(dynamic requestParam)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(requestParam.ToString());
                /*options = StaticFunc.FormatFilter(options);*/

                var bills = DB.Coupons.AsQueryable().Select(c => new
                {
                    Code = c.Code,
                    IdCoupon = c.IdCoupon,
                    Description = c.Description,
                    StartDate = c.StartDate,
                    EndDate = c.EndDate,
                    Quantity = c.Quantity,
                    RemainingQuantity = c.RemainingQuantity,
                    MinBillPrice = c.MinBillPrice,
                    MaxBillDiscount = c.MaxBillDiscount,
                    Status = c.Status,
                    Stage = c.Stage,
                    CouponType = c.CouponType,
                    DirectDiscount = c.DirectDiscount,
                    PercentDiscount = c.PercentDiscount,
                    ApplyTo = c.ApplyTo
                }).ToList();

                respond.ObjectReturn = bills.AsQueryable().ToDataSourceResult((DataSourceRequest)param);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public async Task<DTOResponse> UpdateCoupon(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOUpdateCouponRequest>(requestParam.ToString());
                DTOCoupon reqCoupon = param.Coupon;
                var changedProperties = param.Properties;
                var existedCheck = DB.Coupons.Any(p => p.IdCoupon == reqCoupon.IdCoupon);

                if (reqCoupon.Code == 0)
                {
                    if (existedCheck)
                    {
                        respond.ErrorString = "Trùng mã sản phẩm trong hệ thống!";
                    }
                    else
                    {
                        var newCoupon = new Coupon
                        {
                            IdCoupon = reqCoupon.IdCoupon,
                            Description = reqCoupon.Description,
                            StartDate = reqCoupon.StartDate,
                            EndDate = reqCoupon.EndDate,
                            Quantity = reqCoupon.Quantity,
                            RemainingQuantity = reqCoupon.RemainingQuantity,
                            MinBillPrice = reqCoupon.MinBillPrice,
                            MaxBillDiscount = reqCoupon.MaxBillDiscount,
                            Status = reqCoupon.Status,
                            Stage = reqCoupon.Stage,
                            CouponType = reqCoupon.CouponType,
                            DirectDiscount = reqCoupon.DirectDiscount,
                            PercentDiscount = reqCoupon.PercentDiscount,
                            ApplyTo = reqCoupon.ApplyTo
                        };

                        DB.Coupons.Add(newCoupon);
                        DB.SaveChanges();
                    }

                }
                else
                {
                    var existingCoupon = DB.Coupons.FirstOrDefault(c => c.Code == reqCoupon.Code);
                    if (existingCoupon != null)
                    {
                        foreach (var property in changedProperties)
                        {
                            var couponProperty = typeof(DTOCoupon).GetProperty(property);
                            if (couponProperty != null)
                            {
                                var newValue = couponProperty.GetValue(reqCoupon);
                                var existingCouponProperty = typeof(Coupon).GetProperty(property);
                                if (existingCouponProperty != null)
                                {
                                    existingCouponProperty.SetValue(existingCoupon, newValue, null);
                                    DB.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        respond.StatusCode = 404;
                        respond.ErrorString = "Coupon not found.";
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
