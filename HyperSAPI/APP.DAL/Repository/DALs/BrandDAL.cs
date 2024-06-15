using APP.DAL.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.DAL.Repository.DALs
{
    public class BrandDAL
    {
        private AppDBContext _dBContext;

        public BrandDAL()
        {
            _dBContext = new AppDBContext();
        }

        public List<Brand> GetALlBrands()
        {
            return _dBContext.Brands.ToList();
        }
    }
}
