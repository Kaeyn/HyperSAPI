using APP.DAL.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.BLLs
{
    public class BrandBLL
    {
        private DAL.Repository.DALs.BrandDAL _DAL;

        public BrandBLL()
        {
            _DAL = new DAL.Repository.DALs.BrandDAL();
        }

        public List<Brand> GetAllBrands()
        {
            return _DAL.GetALlBrands();
        }
    }
}

