using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.SalesService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class SalesAPI
    {
        public static SalesTotals GetTotals(DateTime startDateTime, DateTime endDateTime)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.GetTotals(startDateTime, endDateTime);
            }
        }

        public static List<ItemSale> GetItemsSold(DateTime startDateTime, DateTime endDateTime)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return new List<ItemSale>(client.GetItemsSold(startDateTime, endDateTime));
            }
        }

        public static List<OnHoldInfo> GetOnHoldInvoicesForCashier(Context context)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return new List<OnHoldInfo>(client.GetOnHoldInvoicesForCashier(context));
            }
        }

        public static List<OnHoldInfo> GetAllOnHoldInvoices(Context context)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return new List<OnHoldInfo>(client.GetAllOnHoldInvoices(context));
            }
        }

        public static Invoice StartNewInvoice(Context context, String onHoldID)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.StartNewInvoice(context, onHoldID);
            }
        }

        public static Invoice GetInvoice(Context context, long invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.GetInvoice(context, invoiceNumber);
            }
        }

        public static Invoice GetInvoiceHeader(Context context, long invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.GetInvoiceHeader(context, invoiceNumber);
            }
        }

        public static bool LockInvoice(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.LockInvoice(context, invoiceNumber);
            }
        }

        public static bool UnLockInvoice(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.UnLockInvoice(context, invoiceNumber);
            }
        }

        public static Invoice ModifyItems(Context context, Int64 invoiceNumber, List<LineItem> items)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.ModifyItems(context, invoiceNumber, items.ToArray());
            }
        }

        public static bool PrintReceipt(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.PrintReceipt(context, invoiceNumber);
            }
        }

        public static bool SendToKitchen(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.SendToKitchen(context, invoiceNumber);
            }
        }

        public static bool ApplyCardPayment(Context context, Int64 invoiceNumber, CreditCardRequest request)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.ApplyCardPayment(context, invoiceNumber, request);
            }
        }

        public static bool SplitInvoice(Context context, Int64 invoiceNumber, int numberOfWays)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.SplitInvoice(context, invoiceNumber, numberOfWays);
            }
        }

        public static bool SplitInvoiceByGuest(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.SplitInvoiceByGuest(context, invoiceNumber);
            }
        }

        public static bool CombineSplits(Context context, Int64 invoiceNumber)
        {
            using (SalesServiceClient client = new SalesServiceClient())
            {
                client.Open();
                return client.CombineSplits(context, invoiceNumber);
            }
        }
    }
}
