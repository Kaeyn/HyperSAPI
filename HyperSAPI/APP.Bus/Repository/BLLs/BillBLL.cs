using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Bill;
using APP.Bus.Repository.DTOs.Product;
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
    public class BillBLL
    {
        private AppDBContext DB;

        public BillBLL()
        {
            DB = new AppDBContext();
        }

        public DTOResponse GetBill(int reqCode)
        {
            var respond = new DTOResponse();
            try
            {
                DataSourceRequest dataSourceRequest = new DataSourceRequest();
                dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
                var bills = DB.Bills.AsQueryable().Include(b => b.BillInfos).Where(b => b.Code == reqCode)
                    .Select(b => new
                    {
                        Code = b.Code,
                        CustomerName = b.CustomerName,
                        PhoneNumber = b.PhoneNumber,
                        ShippingAddress = b.ShippingAddress,
                        CreateAt = b.CreateAt,
                        PaymentMethod = b.PaymentMethod,
                        ListBillInfo = b.BillInfos.Select(bi => new
                        {
                            Code = bi.Code,
                            IDProduct = bi.CodeProductNavigation.IdProduct,
                            Name = bi.CodeProductNavigation.ProductName,
                            ImageURL = bi.CodeProductNavigation.ProductImages.FirstOrDefault(pi => pi.IsThumbnail == 1).Img,
                            Size = bi.SelectedSize,
                            Price = bi.Price,
                            Quantity = bi.Quantity,
                            TotalPrice = bi.TotalPrice
                        }),
                        Voucher = "",
                        Discount = 0,
                        TotalBill = b.BillInfos.Sum(bi => bi.TotalPrice),
                        Status = b.Status
                    }).ToList();

                respond.ObjectReturn = bills.AsQueryable().ToDataSourceResult(dataSourceRequest);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public DTOResponse GetListCustomerBill(string reqPhoneNumber)
        {
            var respond = new DTOResponse();
            try
            {
                DataSourceRequest dataSourceRequest = new DataSourceRequest();
                dataSourceRequest.Sort = GetSortDescriptor("CreateAt", "desc");
                var bills = DB.Bills.AsQueryable().Include(b => b.BillInfos).Where(b => b.PhoneNumber == reqPhoneNumber)
                    .Select(b => new
                    {
                        Code = b.Code,
                        CustomerName = b.CustomerName,
                        PhoneNumber = b.PhoneNumber,
                        ShippingAddress = b.ShippingAddress,
                        CreateAt = b.CreateAt,
                        PaymentMethod = b.PaymentMethod,
                        ListBillInfo = b.BillInfos.Select(bi => new
                        {
                            Code = bi.Code,
                            IDProduct = bi.CodeProductNavigation.IdProduct,
                            Name = bi.CodeProductNavigation.ProductName,
                            ImageURL = bi.CodeProductNavigation.ProductImages.FirstOrDefault(pi => pi.IsThumbnail == 1).Img,
                            Size = bi.SelectedSize,
                            Price = bi.Price,
                            Quantity = bi.Quantity,
                            TotalPrice = bi.TotalPrice
                        }),
                        Voucher = "",
                        Discount = 0,
                        TotalBill = b.BillInfos.Sum(bi => bi.TotalPrice),
                        Status = b.Status
                    }).ToList();

                respond.ObjectReturn = bills.AsQueryable().ToDataSourceResult(dataSourceRequest);
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }

        public DTOResponse GetListBill(dynamic requestParam)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DataSourceRequest>(requestParam.ToString());
                /*options = StaticFunc.FormatFilter(options);*/

                var bills = DB.Bills.AsQueryable().Include(b => b.BillInfos)
                    .Select(b => new
                    {
                        Code = b.Code,
                        CustomerName = b.CustomerName,
                        PhoneNumber = b.PhoneNumber,
                        ShippingAddress = b.ShippingAddress,
                        CreateAt = b.CreateAt,
                        PaymentMethod = b.PaymentMethod,
                        ListBillInfo = b.BillInfos.Select(bi => new
                        {
                            Code = bi.Code,
                            IDProduct = bi.CodeProductNavigation.IdProduct,
                            Name = bi.CodeProductNavigation.ProductName,
                            ImageURL = bi.CodeProductNavigation.ProductImages.FirstOrDefault(pi => pi.IsThumbnail == 1).Img,
                            Size = bi.SelectedSize,
                            Price = bi.Price,
                            Quantity = bi.Quantity,
                            TotalPrice = bi.TotalPrice
                        }),
                        Voucher = "",
                        Discount = 0,
                        TotalBill = b.BillInfos.Sum(bi => bi.TotalPrice),
                        Status = b.Status
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

        public DTOResponse UpdateBill(dynamic requestParam)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOUpdateBillRequest>(requestParam.ToString());
                /*options = StaticFunc.FormatFilter(options);*/
                int reqCodeBill = param.CodeBill;
                int reqStatus = param.Status;
                string reqNote = param.Note ?? "";

                var existedBill = DB.Bills.FirstOrDefault(b => b.Code == reqCodeBill);
                if(existedBill != null)
                {
                    existedBill.Status = reqStatus;
                    existedBill.Note = reqNote;
                    DB.SaveChanges();
                }
                else
                {
                    respond.ErrorString = "Bill not found";
                }
                
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
