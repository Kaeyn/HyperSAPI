using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.DTOs
{
    public class DTOBanner
    {
        public int Code { get; set; }

        public string? Title { get; set; }

        public int? BannerType { get; set; }

        public string? BannerUrl { get; set; }

        public int? Position { get; set; }

        public string? Page { get; set; }

        public int? Status { get; set; }
    }

    public class DTOUpdateBannerRequest
    {
        public DTOBanner Banner { get; set; }
        public List<string> Properties { get; set; }
    }
}
