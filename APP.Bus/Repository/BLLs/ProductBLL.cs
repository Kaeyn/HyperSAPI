using APP.DAL.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.BLLs
{
    public class ProductBLL
    {
        private DAL.Repository.DALs.ProductDAL _DAL;

        public ProductBLL() {
            _DAL = new DAL.Repository.DALs.ProductDAL();
        }

        public List<Product> GetAllProducts()
        {
            return _DAL.GetAllProducts();
        }
    }
}
