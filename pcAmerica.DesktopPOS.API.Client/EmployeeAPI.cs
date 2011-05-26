using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.EmployeeService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class EmployeeAPI
    {
        public Employee GetCurrentUser()
        {
            using (EmployeeServiceClient client = new EmployeeServiceClient())
            {
                client.Open();
                return client.GetCurrentUser();
            }
        }
        public Employee AuthenticateEmployee(string userName, string password)
        {
            using (EmployeeServiceClient client = new EmployeeServiceClient())
            {
                client.Open();
                return client.AuthenticateEmployee(userName, password);
            }
        }
    }
}
