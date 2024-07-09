using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Coupon;
using APP.DAL.Repository.Entities;
using KendoNET.DynamicLinq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.BLLs
{
    public class BannerBLL
    {
        private AppDBContext DB;

        public BannerBLL() 
        {
            DB = new AppDBContext();
        }

        public DTOResponse GetListBanner(dynamic request)
        {
            var response = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(request.ToString());
                /*options = StaticFunc.FormatFilter(options);*/

                var banners = DB.Banners.Select(b => new
                {
                    Code = b.Code,
                    Title = b.Title,
                    BannerType = b.BannerType,
                    BannerUrl = b.BannerUrl,
                    Position = b.Position,
                    Page = b.Page,
                    Status = b.Status
                });

                response.ObjectReturn = banners.AsQueryable().ToDataSourceResult((DataSourceRequest)param);
            }
            catch (Exception ex)
            {
                response.StatusCode = 500;
                response.ErrorString = ex.Message;
            }
            return response;
        }

        public DTOResponse UpdateBanner(dynamic requestParam)
        {
            DTOResponse respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOUpdateBannerRequest>(requestParam.ToString());
                DTOBanner reqBanner = param.Banner;
                var changedProperties = param.Properties;
                var existedBannerInPos = DB.Banners.Where(b => b.Page == reqBanner.Page && b.Position == reqBanner.Position && b.Status == 0).ToList();
                if (reqBanner.Code == 0)
                {
                    foreach (var item in existedBannerInPos)
                    {
                        item.Status = 1;
                        DB.SaveChanges();
                    }

                    var newBanner = new Banner
                    {
                        Title = reqBanner.Title,
                        BannerType = reqBanner.BannerType,
                        BannerUrl = reqBanner.BannerUrl,
                        Position = reqBanner.Position,
                        Page = reqBanner.Page,
                        Status = 0
                    };

                    DB.Banners.Add(newBanner);
                    DB.SaveChanges();
                }
                else
                {
                    var existingBanner = DB.Banners.FirstOrDefault(b => b.Code == reqBanner.Code);
                    if (existingBanner != null)
                    {
                        foreach (var property in changedProperties)
                        {
                            var bannerProperty = typeof(DTOBanner).GetProperty(property);
                            if (bannerProperty != null)
                            {
                                var newValue = bannerProperty.GetValue(reqBanner);
                                var existingBannerProperty = typeof(Banner).GetProperty(property);
                                if (existingBannerProperty != null)
                                {
                                    if(property == "Status" && newValue == 0)
                                    {
                                        foreach (var item in existedBannerInPos)
                                        {
                                            item.Status = 1;
                                            DB.SaveChanges();
                                        }
                                    }
                                    existingBannerProperty.SetValue(existingBanner, newValue, null);
                                    DB.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        respond.StatusCode = 404;
                        respond.ErrorString = "Banner not found.";
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
