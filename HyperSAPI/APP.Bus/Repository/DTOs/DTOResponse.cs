using KendoNET.DynamicLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs
{
    public class DTOResponse
    {
        public int StatusCode {  get; set; }

        public string? RedirectUrl { get; set; } = string.Empty;

        public string ErrorString { get; set; } = string.Empty;

        public dynamic ObjectReturn { get; set; } = new {};

    }
}
