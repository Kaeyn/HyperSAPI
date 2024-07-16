using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Bill;
using APP.Bus.Repository.DTOs.Cart;
using APP.Bus.Repository.DTOs.Product;
using APP.DAL.Repository.Entities;
using KendoNET.DynamicLinq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.Json;
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
        private CartBillBLL cartBillBLL;

        public BillBLL()
        {
            DB = new AppDBContext();
            cartBillBLL = new CartBillBLL();
        }

        public async Task<DTOResponse> GetBill(int reqCode, string reqPhoneNumber)
        {
            var respond = new DTOResponse();
            try
            {
                DataSourceRequest dataSourceRequest = new DataSourceRequest();
                dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
                var bills = DB.Bills.AsQueryable().Include(b => b.BillInfos).Where(b => b.Code == reqCode && b.OrdererPhoneNumber == reqPhoneNumber)
                .Select(b => new
                {
                    Code = b.Code,
                    CustomerName = b.CustomerName,
                    OrdererPhoneNumber = b.OrdererPhoneNumber,
                    PhoneNumber = b.PhoneNumber,
                    ShippingAddress = b.ShippingAddress,
                    CreateAt = b.CreateAt,
                    PaymentMethod = b.PaymentMethod,
                    PaymentUrl = b.PaymentUrl,
                    ListBillInfo = b.BillInfos.Select(bi => new
                    {
                        Code = bi.Code,
                        IDProduct = bi.CodeProductNavigation.IdProduct,
                        Name = bi.CodeProductNavigation.Name,
                        ImageURL = bi.CodeProductNavigation.ProductImages.FirstOrDefault(pi => pi.IsThumbnail == 1).Img,
                        Size = DB.Sizes.FirstOrDefault(s => s.Code == bi.SelectedSize).Size1,
                        Quantity = bi.Quantity,
                        Price = bi.Price,
                        Discount = bi.Discount,
                        PriceAfterDiscount = CalculatePriceAfterDiscount(bi.Price, bi.Discount),
                        TotalPriceBeforeDiscount = bi.TotalPriceBeforeDiscount,
                        TotalPrice = bi.TotalPrice,
                        Status = bi.Status,
                        Note = bi.Note

                    }),
                    CouponApplied = b.CouponApplied,
                    CouponDiscount = b.CouponDiscount,
                    TotalBeforeDiscount = b.TotalBeforeDiscount,
                    TotalBill = b.TotalBill,
                    Status = b.Status,
                    Note = b.Note
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

        public async Task<DTOResponse> GetListCustomerBill(string reqPhoneNumber)
        {
            var respond = new DTOResponse();
            try
            {
                DataSourceRequest dataSourceRequest = new DataSourceRequest();
                dataSourceRequest.Sort = GetSortDescriptor("CreateAt", "desc");
                var bills = DB.Bills.AsQueryable().Include(b => b.BillInfos).Where(b => b.OrdererPhoneNumber == reqPhoneNumber)
                .Select(b => new
                {
                    Code = b.Code,
                    CustomerName = b.CustomerName,
                    OrdererPhoneNumber = b.OrdererPhoneNumber,
                    PhoneNumber = b.PhoneNumber,
                    ShippingAddress = b.ShippingAddress,
                    CreateAt = b.CreateAt,
                    PaymentMethod = b.PaymentMethod,
                    PaymentUrl = b.PaymentUrl,
                    ListBillInfo = b.BillInfos.Select(bi => new
                    {
                        Code = bi.Code,
                        IDProduct = bi.CodeProductNavigation.IdProduct,
                        Name = bi.CodeProductNavigation.Name,
                        ImageURL = bi.CodeProductNavigation.ProductImages.FirstOrDefault(pi => pi.IsThumbnail == 1).Img,
                        Size = DB.Sizes.FirstOrDefault(s => s.Code == bi.SelectedSize).Size1,
                        Quantity = bi.Quantity,
                        Price = bi.Price,
                        Discount = bi.Discount,
                        PriceAfterDiscount = CalculatePriceAfterDiscount(bi.Price, bi.Discount),
                        TotalPriceBeforeDiscount = bi.TotalPriceBeforeDiscount,
                        TotalPrice = bi.TotalPrice,
                        Status = bi.Status,
                        Note = bi.Note

                    }),
                    CouponApplied = b.CouponApplied,
                    CouponDiscount = b.CouponDiscount,
                    TotalBeforeDiscount = b.TotalBeforeDiscount,
                    TotalBill = b.TotalBill,
                    Status = b.Status,
                    Note = b.Note

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

        public async Task<DTOResponse> GetListBill(dynamic requestParam)
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
                        OrdererPhoneNumber = b.OrdererPhoneNumber,
                        PhoneNumber = b.PhoneNumber,
                        ShippingAddress = b.ShippingAddress,
                        CreateAt = b.CreateAt,
                        PaymentMethod = b.PaymentMethod,
                        PaymentUrl = b.PaymentUrl,
                        ListBillInfo = b.BillInfos.Select(bi => new
                        {
                            Code = bi.Code,
                            IDProduct = bi.CodeProductNavigation.IdProduct,
                            Name = bi.CodeProductNavigation.Name,
                            ImageURL = bi.CodeProductNavigation.ProductImages.FirstOrDefault(pi => pi.IsThumbnail == 1).Img,
                            Size = DB.Sizes.FirstOrDefault(s => s.Code == bi.SelectedSize).Size1,
                            Quantity = bi.Quantity,
                            Price = bi.Price,
                            Discount = bi.Discount,
                            PriceAfterDiscount = CalculatePriceAfterDiscount(bi.Price, bi.Discount),
                            TotalPriceBeforeDiscount = bi.TotalPriceBeforeDiscount,
                            TotalPrice = bi.TotalPrice,
                            Status = bi.Status,
                            Note = bi.Note

                        }),
                        CouponApplied = b.CouponApplied,
                        CouponDiscount = b.CouponDiscount,
                        TotalBeforeDiscount = b.TotalBeforeDiscount,
                        TotalBill = b.TotalBill,
                        Status = b.Status,
                        Note = b.Note

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
        public async Task<DTOResponse> UpdateBill(dynamic requestParam)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOUpdateBillRequest>(requestParam.ToString());
                /*options = StaticFunc.FormatFilter(options);*/
                DTOUpdateBill dTOUpdateBill = param.DTOUpdateBill;
                DTOProceedToPayment dTOProceedToPayment = param.DTOProceedToPayment;
                if (dTOProceedToPayment != null && dTOUpdateBill == null)
                {                  
                    var result = await cartBillBLL.ProceedToPayment(null, param.DTOProceedToPayment, false);
                    respond = result;
                }
                else if(dTOProceedToPayment == null && dTOUpdateBill != null)
                {
                    int reqCodeBill = dTOUpdateBill.CodeBill;
                    int reqStatus = dTOUpdateBill.Status;
                    List<DTOBillInfo> reqListOfBI = dTOUpdateBill.ListOfBillInfo;
                    string reqNote = dTOUpdateBill.Note ?? "";

                    var existedBill = DB.Bills.FirstOrDefault(b => b.Code == reqCodeBill);
                    if (existedBill != null)
                    {
                        existedBill.Status = reqStatus;
                        existedBill.Note = reqNote;
                        foreach (var bill in reqListOfBI)
                        {
                            var billInfoInDB = DB.BillInfos.FirstOrDefault(bi => bi.Code == bill.Code);
                            billInfoInDB.Status = bill.Status;
                            billInfoInDB.Note = bill.Note;
                        }
                        DB.SaveChanges();
                    }
                    else
                    {
                        respond.ErrorString = "Bill not found";
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
      
        public async Task<DTOResponse> GetBillAnalystic()
        {
            var respond = new DTOResponse();
            try
            {
                /*options = StaticFunc.FormatFilter(options);*/

                int totalBill = DB.Bills.Count();
                int totalCompleteBill = DB.Bills.Where(b => b.Status == 22).Count();
                int totalInCompleteBill = DB.Bills.Where(b => b.Status >= 4 && b.Status < 22).Count();
                int totalPendingBill = DB.Bills.Where(b => b.Status == 4).Count();

                respond.ObjectReturn = new {TotalBill = totalBill, TotalCompleteBill = totalCompleteBill, TotalInCompleteBill = totalInCompleteBill, TotalPendingBill = totalPendingBill };
            }
            catch (Exception ex)
            {
                respond.StatusCode = 500;
                respond.ErrorString = ex.Message;
            }

            return respond;
        }
        public async Task<DTOResponse> GetMonthYearAnalystic(dynamic requestParam)
        {
            var respond = new DTOResponse();
            try
            {
                var param = JsonConvert.DeserializeObject<DTOGetAnalysticRequest>(requestParam.ToString());
                /*options = StaticFunc.FormatFilter(options);*/
                int year = param.Year;
                int month = param.Month;
                bool isMonthSort = param.IsMonthSort;
                if (!isMonthSort)
                {
                    var bills = DB.Bills.AsQueryable()
                           .Where(b => b.CreateAt.Year == year).ToList()
                           .GroupBy(b => b.CreateAt.Month)
                           .Select(b => new
                           {
                               Month = b.Key,
                               TotalBill = b.Where(b => b.Status == 22).Count(),
                               TotalIncome = b.Where(b => b.Status == 22).Sum(b => b.TotalBill)
                           }).ToList();

                    var result = Enumerable.Range(1, 12).Select(month =>
                    {
                        var monthData = bills.FirstOrDefault(b => b.Month == month);
                        return new
                        {
                            Month = month,
                            TotalBill = monthData?.TotalBill ?? 0,
                            TotalIncome = monthData?.TotalIncome ?? 0
                        };
                    }).ToList();

                    respond.ObjectReturn = result;
                }
                else
                {
                    var bills = DB.Bills.AsQueryable()
                           .Where(b => b.CreateAt.Year == year && b.CreateAt.Month == month).ToList()
                           .GroupBy(b => (b.CreateAt.Day - 1) / 7 + 1)
                           .Select(b => new
                           {
                               Week = b.Key,
                               TotalBill = b.Where(b => b.Status == 22).Count(),
                               TotalIncome = b.Where(b => b.Status == 22).Sum(b => b.TotalBill)
                           }).ToList();

                    var result = Enumerable.Range(1, 4).Select(week =>
                    {
                        var weekData = bills.FirstOrDefault(b => b.Week == week);
                        return new
                        {
                            Week = week,
                            TotalBill = weekData?.TotalBill ?? 0,
                            TotalIncome = weekData?.TotalIncome ?? 0
                        };
                    }).ToList();

                    respond.ObjectReturn = result;
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
