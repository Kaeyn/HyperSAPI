using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs.ShippingAddress
{
    public class DTOShippingAddress
    {
        public int Code { get; set; }

        public int CustomerCode { get; set; }

        public string? Address { get; set; }

        public string? PhoneNumber { get; set; }

        public string? ReceiverName { get; set; }

        /// <summary>
        /// 0: False
        /// 1: True
        /// </summary>
        public bool IsDefaultAddress { get; set; }
    }
}
