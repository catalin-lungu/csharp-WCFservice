using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace CustomerOrder
{
    public class CustomerOrder : ICustomer, IOrder , ISubscribe
    {
        private static readonly List<IReportSuccessOrError> subscribers = new List<IReportSuccessOrError>();
        #region subscribe
        public void Subscribe()
        {
            IReportSuccessOrError callback = OperationContext.Current.GetCallbackChannel<IReportSuccessOrError>();
            if (!subscribers.Contains(callback))
                subscribers.Add(callback);
            //
        }
        public void Unsubscribe()
        {
            IReportSuccessOrError callback = OperationContext.Current.GetCallbackChannel<IReportSuccessOrError>();
            if (subscribers.Contains(callback))
                subscribers.Remove(callback);
        }
        #endregion

        #region ICustomer
        public Customer GetCustomerById(int _id)
        {
            Int16 id = Convert.ToInt16(_id);
            CUSTOMER C = DB.selectCUSTOMER(id);
            Customer c = new Customer();
            c.id = C.CUSTOMERID;
            c.name = C.NAME;
            c.adresa = C.ADRESA;
            return c;
        }
        public void InsertCustomer(string name, string address)
        {
            bool ok = DB.addCostumer(name, address);
            if (ok)
            {
                subscribers.ForEach(delegate(IReportSuccessOrError callback)
                {
                    if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                    {
                        callback.OnInsertUpdateDelete("client inserat cu succes!");
                    }
                    else
                    {
                        subscribers.Remove(callback);
                    }
                }); 
            }
        }
        public void UpdateCustomer(Customer customer)
        {
            bool ok = DB.updateCostumer(Convert.ToInt16(customer.id), customer.name, customer.adresa);
            if (ok)
            {
                subscribers.ForEach(delegate(IReportSuccessOrError callback)
                {
                    if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                    {
                        callback.OnInsertUpdateDelete("client update cu succes!");
                    }
                    else
                    {
                        subscribers.Remove(callback);
                    }
                });
            }
        }
        public void DeleteCustomer(Customer customer)
        {
            bool ok = DB.deleteCostumer(Convert.ToInt16(customer.id));
            if (ok)
            {
                subscribers.ForEach(delegate(IReportSuccessOrError callback)
                {
                    if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                    {
                        callback.OnInsertUpdateDelete("client sters cu succes!");
                    }
                    else
                    {
                        subscribers.Remove(callback);
                    }
                });
            }
        }
        // 
        public List<Order> GetAllOrders(int customerId)
        {
            List<Order> lista = new List<Order>();
            ChannelFactory<IReport> channel = new ChannelFactory<IReport>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8001/Service"));
            IReport report = channel.CreateChannel();
            
            //using (ReportClient rc = new ReportClient())
            //{
                ReportService.Order[] list = report.GetAllOrdersForCustomer(customerId);
                foreach (var v in list)
                {
                    Order o = new Order();
                    o.id=v.id;
                    o.data=v.data;
                    o.idC=v.idC;
                    o.valoare=v.valoare;
                    lista.Add(o);
                }
            //}
            return lista;
        }
        #endregion

        #region IOrder
        public Order GetOrder(int orderId)
        {
            ORDER OR = DB.selectOrder(Convert.ToInt16(orderId));
            Order or = new Order();
            or.id = OR.ORDERID;
            or.data = Convert.ToString(OR.DATA);
            or.idC = OR.CUSTOMERID;
            try{
                or.valoare = Convert.ToDecimal(OR.VALOARE);
            }
            catch{}
            return or;
        }
        public void InsertOrder(int customerId, DateTime dt, decimal value)
        {
            bool ok = DB.addOrder(dt, Convert.ToInt16(customerId), value);
            if (ok)
            {
                subscribers.ForEach(delegate(IReportSuccessOrError callback)
                {
                    if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                    {
                        callback.OnInsertUpdateDelete("comanda inserata cu succes!");
                    }
                    else
                    {
                        subscribers.Remove(callback);
                    }
                });
            }
        }
        public void UpdateOrder(Order order)
        {
            decimal d; bool ok=false;
            if (order.valoare != null) 
            {
                try
                {
                    d = Convert.ToDecimal(order.valoare);
                    ok = DB.updateOrder(Convert.ToInt16(order.id), Convert.ToDateTime(order.data), Convert.ToInt16(order.idC), d);
                }
                catch { }
            }
            else
            {
                ok = DB.updateOrder(Convert.ToInt16(order.id), Convert.ToDateTime(order.data), Convert.ToInt16(order.idC));
            }
            if (ok)
            {
                subscribers.ForEach(delegate(IReportSuccessOrError callback)
                {
                    if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                    {
                        callback.OnInsertUpdateDelete("update comanda cu succes!");
                    }
                    else
                    {
                        subscribers.Remove(callback);
                    }
                });
            }

        }
        public void DeleteOrder(Order order)
        {
            bool ok = DB.deleteOrder(Convert.ToInt16(order.id));
            if (ok)
            {
                subscribers.ForEach(delegate(IReportSuccessOrError callback)
                {
                    if (((ICommunicationObject)callback).State == CommunicationState.Opened)
                    {
                        callback.OnInsertUpdateDelete("comada stearsa cu succes!");
                    }
                    else
                    {
                        subscribers.Remove(callback);
                    }
                });
            }
        }
        #endregion
    }

    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]//, CallbackContract = typeof(IReportSuccessOrError)
    public interface ICustomer
    {
        [OperationContract]
        Customer GetCustomerById(int _id);
        [OperationContract]
        void InsertCustomer(string _name, string _address);
        [OperationContract]
        void UpdateCustomer(Customer customer);
        [OperationContract]
        void DeleteCustomer(Customer customer);
        [OperationContract]
        List<Order> GetAllOrders(int _customerId);
    }

    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IOrder
    {
        [OperationContract]
        Order GetOrder(int _orderId);
        [OperationContract]
        void InsertOrder(int _customerId, DateTime dt, decimal _value);
        [OperationContract]
        void UpdateOrder(Order order);
        [OperationContract]
        void DeleteOrder(Order order);
    }

    public interface IReportSuccessOrError
    {
        [OperationContract(IsOneWay=true)]//1
        void OnInsertUpdateDelete(string message);
    }
    // needed in client to subscribe
    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples", CallbackContract = typeof(IReportSuccessOrError))]
    public interface ISubscribe
    {
        [OperationContract]
        void Subscribe();
        [OperationContract]
        void Unsubscribe();
    }

    [Serializable]
    public class Order
    {
        public int id;
        public string data;
        public int idC;
        public decimal valoare;
    }
    [Serializable]
    public class Customer
    {
        public int id;
        public string name;
        public string adresa;
    }
}
