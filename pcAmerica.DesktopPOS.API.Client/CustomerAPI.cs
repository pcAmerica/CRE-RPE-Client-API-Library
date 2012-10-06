using pcAmerica.DesktopPOS.API.Client.CustomerService;
using pcAmerica.Utilities.ExternalEncryption;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class CustomerAPI
    {
        public Customer FindRecord(string customerNumber)
        {
            var encryptor = new CreditCardEncryption();
            var customer = new Customer();
            try
            {
                using (var client = new CustomerServiceClient())
                {
                    client.Open();
                    customer = client.FindRecord(customerNumber);
                    return customer;
                }
            }
            finally
            {
                if (customer != null)
                    customer.CreditCardNumber = encryptor.Decrypt(customer.CreditCardNumber);
            }
        }

        public bool UpdateRecord(MessageAction action, Customer customer)
        {
            string cardNumber = customer.CreditCardNumber;
            var encryptor = new CreditCardEncryption();
            customer.CreditCardNumber = encryptor.Encrypt(cardNumber);
            try
            {
                using (var client = new CustomerServiceClient())
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