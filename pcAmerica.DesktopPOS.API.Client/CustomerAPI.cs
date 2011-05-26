using System;
using System.Collections.Generic;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.CustomerService;
using System.Runtime.InteropServices;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class CustomerAPI
    {
        public Customer FindRecord(string customerNumber)
        {
            using (CustomerServiceClient client = new CustomerServiceClient())
            {
                client.Open();
                return client.FindRecord(customerNumber);
            }
        }
        public bool UpdateRecord(MessageAction action, Customer customer)
        {
            using (CustomerServiceClient client = new CustomerServiceClient())
            {
                client.Open();
                return client.UpdateRecord(action, customer);
            }
        }
    }
}
