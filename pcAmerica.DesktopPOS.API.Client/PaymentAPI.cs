using pcAmerica.DesktopPOS.API.Client.PaymentService;
using pcAmerica.Utilities.ExternalEncryption;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class PaymentAPI
    {
        public CreditCardPaymentProcessingResponse ProcessCreditCard(CreditCardRequest request)
        {
            string cardNumber = request.CardNumber;
            string swipe = request.CardSwipe;
            var encryptor = new CreditCardEncryption();
            request.CardNumber = encryptor.Encrypt(cardNumber);
            request.CardSwipe = encryptor.Encrypt(swipe);
            var returnValue = new CreditCardPaymentProcessingResponse();
            try
            {
                using (var client = new PaymentServiceClient())
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
            string cardNumber = request.CardNumber;
            string swipe = request.CardSwipe;
            var encryptor = new CreditCardEncryption();
            request.CardNumber = encryptor.Encrypt(cardNumber);
            request.CardSwipe = encryptor.Encrypt(swipe);
            var returnValue = new CreditCardPaymentProcessingResponse();
            try
            {
                using (var client = new PaymentServiceClient())
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