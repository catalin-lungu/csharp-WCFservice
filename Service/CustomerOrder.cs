using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Microsoft.ServiceModel.Samples
{
    public class CustomerOrder : ICustomer, IOrder
    {
        Int16 maxidc, maxido;
        public DB db = new DB();
        public CustomerOrder()
        {
            db = new DB();
            maxidc = db.getMaxIdC();
            maxido = db.getMaxIdO();
        }

        #region ICustomer
        public CUSTOMER GetCustomerById(int _id)
        {
            Int16 id = Convert.ToInt16(_id);
            CUSTOMER C = db.selectCUSTOMER(id);

            bool b = typeof(CUSTOMER).IsSerializable;
            return C;
        }
        public void InsertCustomer(string name, string address)
        {
            maxidc++;
            bool ok = db.addCostumer(maxidc, name, address);
        }
        public void UpdateCustomer(CUSTOMER customer)
        {
            bool ok = db.updateCostumer(Convert.ToInt16(customer.CUSTOMERID), customer.NAME, customer.ADRESA);
        }
        public void DeleteCustomer(CUSTOMER customer)
        {
            bool ok = db.deleteCostumer(Convert.ToInt16(customer.CUSTOMERID));
        }
        // ??
        public List<ORDER> GetAllOrders(int customerId)
        {
            ReportServiceCO rsco;
            using ((rsco = new ReportServiceCO()) as IDisposable)
            {
                List<ORDER> lista = rsco.GetAllOrdersForCustomer(customerId);
                return lista;

            }
        }
        #endregion

        #region IOrder
        public ORDER GetOrder(int orderId)
        {
            ORDER OR = db.selectOrder(Convert.ToInt16(orderId));
          
            return OR;
        }
        public void InsertOrder(int customerId, DateTime dt, decimal value)
        {
            maxido++;
            bool ok = db.addOrder(maxido, dt, Convert.ToInt16(customerId), value);
        }
        public void UpdateOrder(ORDER order)
        {
            decimal d; bool ok;
            if (order.VALOARE != null) // always??
            {
                try
                {
                    d = Convert.ToDecimal(order.VALOARE);
                    ok = db.updateOrder(Convert.ToInt16(order.ORDERID), order.DATA, Convert.ToInt16(order.CUSTOMERID), d);
                }
                catch { }
            }
            else
            {
                ok = db.updateOrder(Convert.ToInt16(order.ORDERID), order.DATA, Convert.ToInt16(order.CUSTOMERID));
            }

        }
        public void DeleteOrder(ORDER order)
        {
            bool ok = db.deleteOrder(Convert.ToInt16(order.ORDERID));
        }
        #endregion
    }

    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface ICustomer
    {
        [OperationContract]
        CUSTOMER GetCustomerById(int _id);
        [OperationContract]
        void InsertCustomer(string _name, string _address);
        [OperationContract]
        void UpdateCustomer(CUSTOMER customer);
        [OperationContract]
        void DeleteCustomer(CUSTOMER customer);
        [OperationContract]
        List<ORDER> GetAllOrders(int _customerId);
    }

    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IOrder
    {
        [OperationContract]
        ORDER GetOrder(int _orderId);
        [OperationContract]
        void InsertOrder(int _customerId, DateTime dt, decimal _value);
        [OperationContract]
        void UpdateOrder(ORDER order);
        [OperationContract]
        void DeleteOrder(ORDER order);
    }
}
