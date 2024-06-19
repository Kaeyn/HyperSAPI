using KendoNET.DynamicLinq;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APP.Bus.Repository.Mathmathics
{

    public static class StaticFunc
    {
        public static float CalculatePriceAfterDiscount(int price, int? discount)
        {
            if (discount.HasValue && discount.Value > 0)
            {
                return price - (price * discount.Value / 100);
            }
            return price;
        }
        public static List<Sort> GetSortDescriptor(string field, string dir)
        {
            List<Sort> sorts = new List<Sort>();
            Sort param = new Sort();
            param.Dir = dir;
            param.Field = field;
            sorts.Add(param);
            return sorts; 
        }

        public static DataSourceRequest FormatFilter(DataSourceRequest options)
        {
            if (options.Filter != null && options.Filter.Filters.Count() > 0)
            {
                ConvertFilter(options.Filter.Filters);
            }
            else
            {
                options.Filter = null;
            }

            return options;
        }

        public static void ConvertFilter(IEnumerable<Filter> filters)
        {
            foreach (var filter in filters)
            {
                if(filter.Filters != null)
                {
                    ConvertFilter(filter.Filters);
                }
                if (filter.Value is JsonElement jsonElement)
                {
                    switch (jsonElement.ValueKind)
                    {
                        case JsonValueKind.Number:
                            filter.Value = jsonElement.GetInt32();
                            break;
                        case JsonValueKind.String:
                            filter.Value = jsonElement.GetString();
                            break;
                        case JsonValueKind.True:
                        case JsonValueKind.False:
                            filter.Value = jsonElement.GetBoolean();
                            break;
                            // Add other cases as needed
                    }
                }
            }
        }

        public static string ConvertStatusToStr(int status) {
            switch (status)
            {
                case 0: return "Normal";
                case 1: return "Blocked";
                default: return "null";
            }
        }

        public static string ConvertPermissionToStr(int permission)
        {
            switch (permission)
            {
                case 0: return "Customer";
                case 1: return "Admin";
                case 2: return "Staff";
                default: return "null";
            }
        }
    }
}
