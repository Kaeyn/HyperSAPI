using APP.DAL.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.DAL.Repository.DALs
{
    public class ProductDAL
    {
        private AppDBContext _dBContext;

        public ProductDAL()
        {
            _dBContext = new AppDBContext();
        }

        public List<Product> GetAllProducts()
        {
            return _dBContext.Products.ToList();
        }
    }
}
