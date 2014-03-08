using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
/*
 * Lungu D. Catalin Iliuta
 */

namespace CustomerOrder
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


        public static bool addCostumer(string name, string adress = "")
        {
            using (var testEntities = new TestEntities())
            {
                CUSTOMER c = new CUSTOMER();
                c.NAME = name;
                c.ADRESA = adress;
                testEntities.AddToCUSTOMER(c);
                testEntities.SaveChanges();
            }
            return true; // clientul a fost adauga
        }
        public static bool updateCostumer(Int16 id, string name, string adr)
        {
            bool updateit = false;
            using (var testEntities = new TestEntities())
            {
                CUSTOMER c = new CUSTOMER();
                c.CUSTOMERID = id;
                //selectam clientul din bd
                var costumer = testEntities.CUSTOMER.First(cost => cost.CUSTOMERID == c.CUSTOMERID);
                if ((CUSTOMER)costumer != null)
                {
                    ((CUSTOMER)costumer).NAME = name;
                    ((CUSTOMER)costumer).ADRESA = adr;
                    testEntities.SaveChanges();
                    updateit = true;
                }
            }
            return updateit;
        }
        public static bool deleteCostumer(Int16 id, string name = "")
        {
            bool del = false;
            if (id != -1)// sterg cliantul cu acest id
            {
                using (var testEntities = new TestEntities())
                {
                    // selectam clientul
                    var costumer = testEntities.CUSTOMER.First(cost => cost.CUSTOMERID == id);
                    // selectam toate comanzile
                    var orders = from o in testEntities.ORDER where o.CUSTOMERID == costumer.CUSTOMERID select o;
                    foreach (var order in orders) // toate comenzile clientului
                    {
                        var ordersDetails = from od in testEntities.ORDERDETAILS where od.ORDERID == order.ORDERID select od;
                        foreach (var orderD in ordersDetails) // toate detaliile tuturor comenzilor
                        {
                            testEntities.ORDERDETAILS.DeleteObject(orderD);
                        }
                        testEntities.ORDER.DeleteObject(order);
                    }
                    testEntities.CUSTOMER.DeleteObject(costumer);
                    testEntities.SaveChanges();
                    del = true;
                }

            }
            else // sterg toti clientii care au numele name
            {
                using (var testEntities = new TestEntities())
                {
                    // toti clientii
                    var costumers = from c in testEntities.CUSTOMER where c.NAME == name select c;
                    foreach (var costumer in costumers)
                    {
                        //toate comenzile clientului
                        var orders = from o in testEntities.ORDER where o.CUSTOMERID == costumer.CUSTOMERID select o;
                        foreach (var order in orders) // toate comenzile clientului
                        {
                            var ordersDetails = from od in testEntities.ORDERDETAILS where od.ORDERID == order.ORDERID select od;
                            foreach (var orderD in ordersDetails) // toate detaliile tuturor comenzilor
                            {
                                testEntities.ORDERDETAILS.DeleteObject(orderD);
                            }
                            testEntities.ORDER.DeleteObject(order);
                        }
                        testEntities.CUSTOMER.DeleteObject(costumer);
                    }
                    testEntities.SaveChanges();
                }
            }
            return del;
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

        #region order
        public static ORDER selectOrder(Int16 id)
        {
            using (var testEntities = new TestEntities())
            {
                var order = testEntities.ORDER.First(ord => ord.ORDERID == id);
                return order;
            }
        }
        public static string selectOrder(bool orderdetails, Int16 orderId, DateTime date, Int16 costumerId, Decimal valoare)
        {
            string rez = "OrderId\t Date\t CostumerId\t Valoare" + Environment.NewLine;
            if (orderId == -1 && date == new DateTime(1970, 1, 1) && costumerId == -1 && valoare == -1)
            {
                using (var testEntities = new TestEntities())
                {
                    var orders = from o in testEntities.ORDER select o;
                    foreach (var ord in orders)
                    {
                        rez += ord.ORDERID + "\t" + ord.DATA + "\t" + ord.CUSTOMERID + "\t" + ord.VALOARE + Environment.NewLine;
                        if (orderdetails)
                        {
                            rez += "\t OderID \t Produs \t Valoare \t Serial";
                            var ordersDetails = from od in testEntities.ORDERDETAILS where ord.ORDERID == od.ORDERID select od;
                            foreach (var o in ordersDetails)
                            {
                                rez += "\t" + o.ORDERID + "\t" + o.PRODUS + "\t" + o.VALOARE + "\t" + o.SERIAL + Environment.NewLine;
                            }
                        }
                    }
                }
            }
            else
            {
                using (var testEntities = new TestEntities())
                {
                    if (orderId != -1)
                    {
                        var orders = from o in testEntities.ORDER where o.ORDERID == orderId select o;
                        foreach (var ord in orders)
                        {
                            rez += ord.ORDERID + "\t" + ord.DATA + "\t" + ord.CUSTOMERID + "\t" + ord.VALOARE + Environment.NewLine;
                            if (orderdetails)
                            {
                                rez += "\t OderID \t Produs \t Valoare \t Serial";
                                var ordersDetails = from od in testEntities.ORDERDETAILS where ord.ORDERID == od.ORDERID select od;
                                foreach (var o in ordersDetails)
                                {
                                    rez += "\t" + o.ORDERID + "\t" + o.PRODUS + "\t" + o.VALOARE + "\t" + o.SERIAL + Environment.NewLine;
                                }
                            }
                        }
                    }
                    else if (date != new DateTime(1970, 1, 1))
                    {
                        var orders = from o in testEntities.ORDER where o.DATA == date select o;
                        foreach (var ord in orders)
                        {
                            rez += ord.ORDERID + "\t" + ord.DATA + "\t" + ord.CUSTOMERID + "\t" + ord.VALOARE + Environment.NewLine;
                            if (orderdetails)
                            {
                                rez += "\t OderID \t Produs \t Valoare \t Serial";
                                var ordersDetails = from od in testEntities.ORDERDETAILS where ord.ORDERID == od.ORDERID select od;
                                foreach (var o in ordersDetails)
                                {
                                    rez += "\t" + o.ORDERID + "\t" + o.PRODUS + "\t" + o.VALOARE + "\t" + o.SERIAL + Environment.NewLine;
                                }
                            }
                        }
                    }
                    else if (costumerId != -1)
                    {
                        var orders = from o in testEntities.ORDER where o.CUSTOMERID == costumerId select o;
                        foreach (var ord in orders)
                        {
                            rez += ord.ORDERID + "\t" + ord.DATA + "\t" + ord.CUSTOMERID + "\t" + ord.VALOARE + Environment.NewLine;
                            if (orderdetails)
                            {
                                rez += "\t OderID \t Produs \t Valoare \t Serial";
                                var ordersDetails = from od in testEntities.ORDERDETAILS where ord.ORDERID == od.ORDERID select od;
                                foreach (var o in ordersDetails)
                                {
                                    rez += "\t" + o.ORDERID + "\t" + o.PRODUS + "\t" + o.VALOARE + "\t" + o.SERIAL + Environment.NewLine;
                                }
                            }
                        }
                    }
                    else if (valoare != -1)
                    {
                        var orders = from o in testEntities.ORDER where o.VALOARE == valoare select o;
                        foreach (var ord in orders)
                        {
                            rez += ord.ORDERID + "\t" + ord.DATA + "\t" + ord.CUSTOMERID + "\t" + ord.VALOARE + Environment.NewLine;
                            if (orderdetails)
                            {
                                rez += "\t OderID \t Produs \t Valoare \t Serial";
                                var ordersDetails = from od in testEntities.ORDERDETAILS where ord.ORDERID == od.ORDERID select od;
                                foreach (var o in ordersDetails)
                                {
                                    rez += "\t" + o.ORDERID + "\t" + o.PRODUS + "\t" + o.VALOARE + "\t" + o.SERIAL + Environment.NewLine;
                                }
                            }
                        }
                    }
                }
            }
            return rez;
        }
        public static bool addOrder(DateTime data, Int16 costumerId, Decimal valoare = -1)
        {
            bool added = false;
            using (var testEntities = new TestEntities())
            {
                ORDER order = new ORDER();
                
                    order.DATA = data;
                    order.CUSTOMERID = costumerId;
                    if (valoare != -1)
                        order.VALOARE = valoare;
                    testEntities.AddToORDER(order);
                    testEntities.SaveChanges();
                    added = true;
               
            }
            return added;
        }
        public static bool updateOrder(Int16 id, DateTime data, Int16 costumerId, Decimal valoare = -1)
        {
            bool update = false;
            using (var testEntities = new TestEntities())
            {
                var order = testEntities.ORDER.First(o => o.ORDERID == id);
                if ((ORDER)order != null)
                {
                    ((ORDER)order).DATA = data;
                    ((ORDER)order).CUSTOMERID = costumerId;
                    if (valoare != -1)
                        ((ORDER)order).VALOARE = valoare;
                    testEntities.SaveChanges();
                    update = true;
                }

            }
            return update;
        }
        public static bool deleteOrder(Int16 id, DateTime data = new DateTime())
        {
            bool del = false;
            if (id != -1)
            {
                using (var testEntities = new TestEntities())
                {
                    var order = testEntities.ORDER.First(o => o.ORDERID == id);
                    //selectam toate detaliile acetei comenzi
                    var orderDetails = from od in testEntities.ORDERDETAILS where od.ORDERID == order.ORDERID select od;
                    foreach (var orderD in orderDetails)
                    {
                        testEntities.ORDERDETAILS.DeleteObject(orderD);
                    }
                    // stergem si comanda
                    testEntities.ORDER.DeleteObject(order);
                    testEntities.SaveChanges();
                    del = true;
                }
            }
            else // stergem toate comenzile efectuate pe data date
            {
                using (var testEntities = new TestEntities())
                {
                    var orders = from o in testEntities.ORDER where o.DATA == data select o;
                    foreach (var order in orders)
                    {
                        var orderDetails = from od in testEntities.ORDERDETAILS where od.ORDERID == order.ORDERID select od;
                        foreach (var orderD in orderDetails)
                        {
                            testEntities.ORDERDETAILS.DeleteObject(orderD);
                        }
                        testEntities.ORDER.DeleteObject(order);
                    }
                    testEntities.SaveChanges();
                    del = true;
                }
            }
            return del;
        }
        #endregion

        #region order details
        public string selectOrderDetails(Int16 orderId, string produs, Decimal valoare, Int16 serial)
        {
            string rez = "OrderId\t Produs\t Value\t Serial" + Environment.NewLine;
            if (orderId == -1 && produs == "" && valoare == -1 && serial == -1)
            {
                using (var testEntities = new TestEntities())
                {
                    var ordersDetails = from od in testEntities.ORDERDETAILS select od;
                    foreach (var ordD in ordersDetails)
                    {
                        rez += ordD.ORDERID + "\t" + ordD.PRODUS + "\t" + ordD.VALOARE + "\t" + ordD.SERIAL + Environment.NewLine;
                    }
                }
            }
            else
            {
                using (var testEntities = new TestEntities())
                {
                    //intersectie de selectii
                    List<ORDERDETAILS> lista = new List<ORDERDETAILS>();
                    bool alocat = false;
                    if (orderId != -1)
                    {
                        var orderDetails1 = from od in testEntities.ORDERDETAILS where od.ORDERID == orderId select od;
                        foreach (var od in orderDetails1)
                        {
                            lista.Add(od);
                        }
                        alocat = true;
                    }
                    if (produs != "")
                    {
                        var orderDetails2 = from od in testEntities.ORDERDETAILS where od.PRODUS == produs select od;
                        if (alocat)
                        {
                            foreach (var l in lista)
                            {
                                if (!orderDetails2.Contains(l))
                                {
                                    lista.Remove(l);
                                }
                            }
                        }
                        else
                        {
                            foreach (var od in orderDetails2)
                                lista.Add(od);
                            alocat = true;
                        }
                    }
                    if (valoare != -1)
                    {
                        var orderDetails3 = from od in testEntities.ORDERDETAILS where od.VALOARE == valoare select od;
                        if (alocat)
                        {
                            // daca l orderDetails3 nu contine l din lista atunci scot din lista l
                        }
                        else
                        {
                            foreach (var od in orderDetails3)
                                lista.Add(od);
                            alocat = true;
                        }
                    }
                    if (serial != -1)
                    {
                        var orderDetails4 = from od in testEntities.ORDERDETAILS where od.SERIAL == serial select od;
                        if (alocat)
                        {
                            foreach (var l in lista)
                            {
                                if (!orderDetails4.Contains(l))
                                {
                                    lista.Remove(l);
                                }
                            }
                        }
                        else
                        {
                            foreach (var od in orderDetails4)
                                lista.Add(od);
                            alocat = true;
                        }
                    }
                    // constructie rez;
                    foreach (var l in lista)
                        rez += l.ORDERID + "\t" + l.PRODUS + "\t" + l.VALOARE + "\t" + l.SERIAL + Environment.NewLine;
                }
            }
            return rez;
        }
        public bool addOrderDetails(Int16 orderId, string produs, Decimal valoare, Int16 serial)
        {
            using (var testEntities = new TestEntities())
            {
                ORDERDETAILS orderD = new ORDERDETAILS();
                orderD.ORDERID = orderId;
                orderD.SERIAL = serial;
                if (testEntities.ORDERDETAILS.Where(x => (x.ORDERID == orderD.ORDERID || x.SERIAL == orderD.SERIAL)).Count() <= 0)
                {
                    orderD.PRODUS = produs;
                    orderD.VALOARE = valoare;
                    testEntities.AddToORDERDETAILS(orderD);
                    testEntities.SaveChanges();
                }
                else
                {
                    return false;
                }
            }
            return true;
        }
        public bool updateOrderDetails(Int16 orderId, string produs, Decimal valoare, Int16 serial)
        {
            bool update = false;
            using (var testEntities = new TestEntities())
            {
                ORDERDETAILS orderD = new ORDERDETAILS();
                orderD.ORDERID = orderId;
                orderD.SERIAL = serial;
                var order2 = testEntities.ORDERDETAILS.First(o => (o.ORDERID == orderD.ORDERID && o.SERIAL == orderD.SERIAL));
                order2.PRODUS = produs;
                order2.VALOARE = valoare;
                testEntities.SaveChanges();
                update = true;
            }
            return update;
        }
        public bool deleteOrderDetails(Int16 orderId, string produs, Int16 serial)
        {
            bool delete = false;
            if (orderId != -1)
            {
                using (var testEntities = new TestEntities())
                {
                    var orderD = testEntities.ORDERDETAILS.First(od => od.ORDERID == orderId);
                    testEntities.ORDERDETAILS.DeleteObject(orderD);
                    testEntities.SaveChanges();
                    delete = true;
                }
            }
            else if (serial != -1)
            {
                using (var testEntities = new TestEntities())
                {
                    var orderD = testEntities.ORDERDETAILS.First(od => od.SERIAL == serial);
                    testEntities.ORDERDETAILS.DeleteObject(orderD);
                    testEntities.SaveChanges();
                    delete = true;
                }
            }
            else
            {
                using (var testEntities = new TestEntities())
                {
                    var orderDs = from od in testEntities.ORDERDETAILS.Where(od => od.PRODUS == produs)
                                  select od;
                    foreach (var orderd in orderDs)
                    {
                        testEntities.ORDERDETAILS.DeleteObject(orderd);
                    }
                    testEntities.SaveChanges();
                    delete = true;
                }
            }
            return delete;
        }
        #endregion

        
        private bool contine(IQueryable aux, List<ORDERDETAILS> lista)
        {
            foreach (var v in aux)
            {
                foreach (var l in lista)
                {
                    if (v == l)
                        return true;
                }
            }
            return false;
        }
    }
}
