using System;
using System.Collections.Generic;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.PaymentService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class PaymentAPI
    {
        public static PaymentResponse ProcessCreditCard(CreditCardRequest request)
        {
            using (PaymentServiceClient client = new PaymentServiceClient())
            {
                client.Open();
                return client.ProcessCreditCard(request);
            }
        }
    }
}
