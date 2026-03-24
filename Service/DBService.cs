using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TRPO_KI_15pr_ElectronicShop.Models;

namespace TRPO_KI_15pr_ElectronicShop.Service
{
    public class DBService
    {
        private ElectricalShopKirichenkoContext context;
        public ElectricalShopKirichenkoContext Context => context;

        private static DBService? instance;
        public static DBService Instance
        {
            get
            {
                if (instance == null)
                    instance = new DBService();
                return instance;
            }
        }

        private DBService()
        {
            context = new ElectricalShopKirichenkoContext();
        }
    }
}
