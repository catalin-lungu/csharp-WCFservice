using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;
using System.ServiceModel.Description;
using System.Net;

namespace Microsoft.ServiceModel.Samples
{
    class Program
    {
        static void Main(string[] args)
        {
            ServiceHost host = new ServiceHost(typeof(ReportService.ReportServiceCO)  , new Uri("net.tcp://localhost:8001/Service"));
            host.Open();
            ServiceHost host2 = new ServiceHost(typeof(CustomerOrder.CustomerOrder), new Uri("net.tcp://localhost:8002/CO"));
            host2.Open();
            Console.WriteLine("Server stated..");
            Console.ReadLine();
            host.Close();
            host2.Close();
            Console.WriteLine("Stoped!");

        }

    }
  
    

    [ServiceContract]
    public interface ITestServ
    {
        [OperationContract]
        int daNr();
    }
    public class TestServ : ITestServ
    {
        public int daNr()
        {
            return 3;
        }
    }

}
