using APP.DAL.Repository.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace APP.Bus.Repository.BLLs
{
    public class BillBLL
    {
        private AppDBContext DB;

        public BillBLL()
        {
            DB = new AppDBContext();
        }


    }
}
