using APP.Bus.Repository.DTOs;
using APP.Bus.Repository.DTOs.Bill;
using APP.Bus.Repository.DTOs.Cart;
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
        private CartBLL cartBLL;

        public BillBLL()
        {
            DB = new AppDBContext();
            cartBLL = new CartBLL();
        }

        public DTOResponse GetBill(int reqCode, string reqPhoneNumber)
        {
            var respond = new DTOResponse();
            try
            {
                DataSourceRequest dataSourceRequest = new DataSourceRequest();
                dataSourceRequest.Sort = GetSortDescriptor("Code", "desc");
                var bills = DB.Bills.AsQueryable().Include(b => b.BillInfos).Where(b => b.Code == reqCode && b.PhoneNumber == reqPhoneNumber)
                .Select(b => new
                {
                    Code = b.Code,
                    CustomerName = b.CustomerName,
                    OrdererPhoneNumber = b.OrdererPhoneNumber,
                    PhoneNumber = b.PhoneNumber,
                    ShippingAddress = b.ShippingAddress,
                    CreateAt = b.CreateAt,
                    PaymentMethod = b.PaymentMethod,
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
                    }),
                    Voucher = "",
                    Discount = 0,
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

        public DTOResponse GetListCustomerBill(string reqPhoneNumber)
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
                        Status = bi.Status

                    }),
                    Voucher = "",
                    Discount = 0,
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
                        OrdererPhoneNumber = b.OrdererPhoneNumber,
                        PhoneNumber = b.PhoneNumber,
                        ShippingAddress = b.ShippingAddress,
                        CreateAt = b.CreateAt,
                        PaymentMethod = b.PaymentMethod,
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
                            Status = bi.Status

                        }),
                        Voucher = "",
                        Discount = 0,
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
        public DTOResponse UpdateBill(dynamic requestParam)
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
                    var result = cartBLL.ProceedToPayment(null, param.DTOProceedToPayment, false);
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

        public void SuccessPaymentUpdate(int codeBill)
        {
            var bill = DB.Bills.Include(b => b.BillInfos).FirstOrDefault(b => b.Code == codeBill);
            bill.Status = 1;
            DB.SaveChanges();
            foreach (var item in bill.BillInfos)
            {
                item.Status = 1;
                DB.SaveChanges();
            }
        }
    }
}
