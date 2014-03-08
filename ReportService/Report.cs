using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace ReportService
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple)]//, InstanceContextMode = InstanceContextMode.PerSession)]
    public class ReportServiceCO : IReport
    {
        public ReportServiceCO() { }
        public List<Customer> GetAllCustomer()
        {
            List<CUSTOMER> lista = DB.selectAllCustomers();
            List<Customer> list = new List<Customer>();
            foreach (var v in lista)
            {
                Customer c = new Customer();
                c.id = v.CUSTOMERID;
                c.name = v.NAME;
                c.adresa = v.ADRESA;
                list.Add(c);
            }
            return list;
        }
        public List<Order> GetAllOrdersForCustomer(int customerId)
        {
            List<ORDER> lista = DB.selectOrdersC(Convert.ToInt16(customerId));
            List<Order> list = new List<Order>();
            foreach (var v in lista)
            {
                Order o = new Order();
                o.id = v.ORDERID;
                o.data = v.DATA.ToString();
                o.idC = v.CUSTOMERID;
                try
                {
                    o.valoare = Convert.ToDecimal(v.VALOARE);
                }
                catch { }
                list.Add(o);
            }
            return list;
        }
        
    }

    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
//    CallbackContract = typeof(IReportSuccessOrError) , SessionMode=SessionMode.Required)]
    public interface IReport
    {
        [OperationContract]
        List<Customer> GetAllCustomer();
        [OperationContract]
        List<Order> GetAllOrdersForCustomer(int _CUSTOMERId);
        //[OperationContract]
        //void Notify();
    }

    [Serializable]
    public class Order {
        public int id;
        public string data;
        public int idC;
        public decimal valoare;
    }
    [Serializable]
    public class Customer {
        public int id;
        public string name;
        public string adresa;
    }
}
