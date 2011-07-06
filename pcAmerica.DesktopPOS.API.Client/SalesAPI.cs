using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.SalesService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class SalesAPI
    {
        public SalesTotals GetTotals(DateTime startDateTime, DateTime endDateTime)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.GetTotals(startDateTime, endDateTime);
            }
        }

        public List<ItemSale> GetItemsSold(DateTime startDateTime, DateTime endDateTime)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return new List<ItemSale>(client.GetItemsSold(startDateTime, endDateTime));
            }
        }

        public List<OnHoldInfo> GetOnHoldInvoicesForCashier(Context context)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return new List<OnHoldInfo>(client.GetOnHoldInvoicesForCashier(context));
            }
        }

        public List<OnHoldInfo> GetAllOnHoldInvoices(Context context)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return new List<OnHoldInfo>(client.GetAllOnHoldInvoices(context));
            }
        }

        public Invoice StartNewInvoice(Context context, String onHoldID)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.StartNewInvoice(context, onHoldID);
            }
        }

        public Invoice GetInvoice(Context context, long invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.GetInvoice(context, invoiceNumber);
            }
        }

        public Invoice GetInvoiceHeader(Context context, long invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.GetInvoiceHeader(context, invoiceNumber);
            }
        }

        public bool LockInvoice(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.LockInvoice(context, invoiceNumber);
            }
        }

        public bool UnLockInvoice(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.UnLockInvoice(context, invoiceNumber);
            }
        }

        public Invoice ModifyItems(Context context, Int64 invoiceNumber, List<LineItem> items)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.ModifyItems(context, invoiceNumber, items);
            }
        }

        public bool PrintReceipt(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.PrintReceipt(context, invoiceNumber);
            }
        }

        public bool PrintReceipt(Context context, Int64 invoiceNumber, string emailAddress)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.EmailReceipt(context, invoiceNumber, emailAddress);
            }
        }

        public bool SendToKitchen(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.SendToKitchen(context, invoiceNumber);
            }
        }

        public bool ApplyCardPayment(Context context, Int64 invoiceNumber, CreditCardRequest request)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.ApplyCardPayment(context, invoiceNumber, request);
            }
        }

        public bool SplitInvoice(Context context, Int64 invoiceNumber, int numberOfWays)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.SplitInvoice(context, invoiceNumber, numberOfWays);
            }
        }

        public bool SplitInvoiceByGuest(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.SplitInvoiceByGuest(context, invoiceNumber);
            }
        }

        public bool CombineSplits(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.CombineSplits(context, invoiceNumber);
            }
        }
    }
}
