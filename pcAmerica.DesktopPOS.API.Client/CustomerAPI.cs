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
            var encryptor = new Utilities.ExternalEncryption.CreditCardEncryption();
            Customer customer = new Customer();
            try
            {
                using (CustomerServiceClient client = new CustomerServiceClient())
                {
                    client.Open();
                    customer = client.FindRecord(customerNumber);
                    return customer;
                }
            }
            finally
            {
                customer.CreditCardNumber = encryptor.Decrypt(customer.CreditCardNumber);
            }
        }
        public bool UpdateRecord(MessageAction action, Customer customer)
        {
            var cardNumber = customer.CreditCardNumber;
            var encryptor = new Utilities.ExternalEncryption.CreditCardEncryption();
            customer.CreditCardNumber = encryptor.Encrypt(cardNumber);
            try
            {
                using (CustomerServiceClient client = new CustomerServiceClient())
                {
                    client.Open();
                    return client.UpdateRecord(action, customer);
                }
            }
            finally
            {
                customer.CreditCardNumber = cardNumber;
            }
        }
    }
}
