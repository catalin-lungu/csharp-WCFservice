using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ServiceModel;

namespace Microsoft.ServiceModel.Samples
{
    [ServiceBehavior(ConcurrencyMode = ConcurrencyMode.Multiple, InstanceContextMode = InstanceContextMode.PerSession)]
    public class ReportServiceCO : IReport
    {

        public List<CUSTOMER> GetAllCustomer()
        {
            List<CUSTOMER> lista = DB.selectAllCustomers();
            return lista;
        }
        public List<ORDER> GetAllOrdersForCustomer(int customerId)
        {
            List<ORDER> lista = DB.selectOrdersC(Convert.ToInt16(customerId));
            
            return lista;
        }

    }

    [ServiceContract(Namespace = "http://Microsoft.ServiceModel.Samples")]
    public interface IReport
    {
        [OperationContract]
        List<CUSTOMER> GetAllCustomer();
        [OperationContract]
        List<ORDER> GetAllOrdersForCustomer(int _CUSTOMERId);
    }
}
