using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * Lungu D. Catalin Iliuta
 */

namespace ReportService
{
    public class DB
    {
        #region costumer
        public static CUSTOMER selectCUSTOMER(Int16 id)
        {
            using (var testEntities = new TestEntities())
            {
                var cost = from c in testEntities.CUSTOMER where c.CUSTOMERID == id select c;
                if (cost.Count() > 0)
                    return cost.First();
                else
                    return null;
            }
        }
        public static List<CUSTOMER> selectAllCustomers()
        {
            List<CUSTOMER> lista = new List<CUSTOMER>();
            using (var testEntities = new TestEntities())
            {
                var customers = from c in testEntities.CUSTOMER select c;
                foreach (var cu in customers)
                {
                    lista.Add(cu);
                }
            }
            return lista;
        }

        public static List<ORDER> selectOrdersC(Int16 cId)
        {
            List<ORDER> lista = new List<ORDER>();
            using (var testEntities = new TestEntities())
            {
                var orders = from o in testEntities.ORDER where o.CUSTOMERID == cId select o;
                foreach (var or in orders)
                    lista.Add(or);
            }
            return lista;
        }
        #endregion
    } 
}
