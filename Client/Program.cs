using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.Windows.Forms;

namespace Client
{
    class Program
    {
        public static IReport report;
        public static ICustomer customer;
        public static IOrder order;
        public static ISubscribe sub;
        public static Form1 form;
        static void Main(string[] args)
        {

            ChannelFactory<IReport> channel = new ChannelFactory<IReport>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8001/Service"));
            report = channel.CreateChannel();
            ChannelFactory<ICustomer> channel1 = new ChannelFactory<ICustomer>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8002/CO"));
            customer = channel1.CreateChannel();
            ChannelFactory<IOrder> channel2 = new ChannelFactory<IOrder>(new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8002/CO"));
            order = channel2.CreateChannel();
            DuplexChannelFactory<ISubscribe> ch_sub = new DuplexChannelFactory<ISubscribe>(new CallBack(), "NetTcpBinding_ISubscribe");// new NetTcpBinding(), new EndpointAddress("net.tcp://localhost:8002/CO"));
            sub = ch_sub.CreateChannel();

            form = new Form1();
            Application.Run(form);
            
        }
    }
    public class CallBack : ISubscribeCallback
    {
        public void OnInsertUpdateDelete(string message)
        {
            Program.form.showMessage(message);
        }
    }

}
