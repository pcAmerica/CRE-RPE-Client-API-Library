using System;
using System.Collections.Generic;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.PaymentService;


namespace pcAmerica.DesktopPOS.API.Client
{
    public class PaymentAPI
    {
        public CreditCardPaymentProcessingResponse ProcessCreditCard(CreditCardRequest request)
        {
            var cardNumber = request.CardNumber;
            var swipe = request.CardSwipe;
            var encryptor = new Utilities.ExternalEncryption.CreditCardEncryption();
            request.CardNumber = encryptor.Encrypt(cardNumber);
            request.CardSwipe = encryptor.Encrypt(swipe);
            CreditCardPaymentProcessingResponse returnValue = new CreditCardPaymentProcessingResponse();
            try
            {
                using (PaymentServiceClient client = new PaymentServiceClient())
                {
                    client.Open();
                    returnValue = client.ProcessCreditCard(request);
                    return returnValue;
                }
            }
            finally
            {
                request.CardNumber = cardNumber;
                request.CardSwipe = swipe;
                returnValue.CardNumber = cardNumber;
            }
        }
        public CreditCardPaymentProcessingResponse CompletePreAuth(CreditCardRequest request, long invoiceNumber)
        {
            var cardNumber = request.CardNumber;
            var swipe = request.CardSwipe;
            var encryptor = new Utilities.ExternalEncryption.CreditCardEncryption();
            request.CardNumber = encryptor.Encrypt(cardNumber);
            request.CardSwipe = encryptor.Encrypt(swipe);
            CreditCardPaymentProcessingResponse returnValue = new CreditCardPaymentProcessingResponse();
            try
            {
                using (PaymentServiceClient client = new PaymentServiceClient())
                {
                    client.Open();
                    returnValue = client.CompletePreAuth(request, invoiceNumber);
                    return returnValue;
                }
            }
            finally
            {
                request.CardNumber = cardNumber;
                request.CardSwipe = swipe;
                returnValue.CardNumber = cardNumber;
            }
        }

    }
}
