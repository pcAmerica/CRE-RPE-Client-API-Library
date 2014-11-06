

using System;
using System.Collections.Generic;
using System.Text;
using pcAmerica.DesktopPOS.API.Client;
using pcAmerica.DesktopPOS.API.Client.PaymentService;
using pcAmerica.DesktopPOS.API.Client.CustomerService;
using pcAmerica.DesktopPOS.API.Client.CompanyInformationService;
using pcAmerica.DesktopPOS.API.Client.EmployeeService;
using pcAmerica.DesktopPOS.API.Client.InventoryService;
using pcAmerica.DesktopPOS.API.Client.SalesService;
using pcAmerica.DesktopPOS.API.Client.MenuService;
using pcAmerica.DesktopPOS.API.Client.TableService;


namespace APITester
{
    
    class Program
    {
        Random random = new Random();
        static void Main(string[] args)
        {
            //TestCreditCard();
            //TestCustomers();
            //TestEmployee();
            //TestInventory();
            //TestSales();
            //TestMenus();
            //TestTables();
            //TestBurgerExpress();
            //deleteItemsTest();
            //TestVoidInvoice();
            //TestSectionsAndTables();
            //TestSplits();
            //TestPreAuthInvoice();
            //TestGetStoreIDsAndGetStationIDs();
            //AddItemsOutOfOrderTest();
            //TestDBInfo();
            //testSendToKitchen();
            TestSaleWithCreditCardPayment();
        }

        static void NewTest()
        {
            try
            {
                SalesAPI api = new SalesAPI();

                pcAmerica.DesktopPOS.API.Client.SalesService.Context salesContext = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                salesContext.CashierID = "100101";
                salesContext.StoreID = "1001";
                salesContext.StationID = "07";

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context =
                    new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "07";

                InventoryAPI InvApi = new InventoryAPI();

                pcAmerica.DesktopPOS.API.Client.InventoryService.Context InvContext = new pcAmerica.DesktopPOS.API.Client.InventoryService.Context();
                InvContext.CashierID = "100101";
                InvContext.StationID = "07";
                InvContext.StoreID = "1001";

                // StartNewInvoice - this also automatically locks an invoice so it can't be opened by a terminal
                Invoice inv = api.StartNewInvoice(context, "Luigi" + DateTime.Now.Second.ToString(), "XXOPEN TABS");
                Console.WriteLine(String.Format("Started new invoice with #: {0}", inv.InvoiceNumber));

                // getting invoice to show locked status should be locked by this station(7)
                inv = api.GetInvoice(context, inv.InvoiceNumber);
                if (inv.Locked == true)
                {
                    Console.WriteLine("Invoice #{0} is locked by Station: {1}", inv.InvoiceNumber.ToString(), inv.LockedByStation);
                }
                else
                {
                    Console.WriteLine("Invoice #{0} is unlocked", inv.InvoiceNumber.ToString());
                }
                //setting party size
                api.SetPartySizeForInvoice(context, inv.InvoiceNumber, 3);


                InventoryItem itemToAdd = InvApi.GetItem(InvContext, "SALAD1");
                Guid itemToAddID = Guid.NewGuid();
                LineItem LineItemToAdd = new LineItem() { Id = itemToAddID, ItemName = itemToAdd.ItemName, ItemNumber = itemToAdd.ItemNumber, Price = itemToAdd.Price, Quantity = 1, State = EntityState.Added, Guest = "42" };
                inv.LineItems.Add(LineItemToAdd);

                itemToAdd.ModifierGroups = InvApi.GetModiferGroupsForItem(InvContext, itemToAdd.ItemNumber);
                foreach (ModifierGroup ModGroup in itemToAdd.ModifierGroups)
                {
                    Console.WriteLine("ModifierGroup:{0}", ModGroup.ItemName);
                    Console.WriteLine("{0}", ModGroup.Prompt);
                    int i = 1;
                    if (ModGroup.Forced == false)
                    {
                        Console.WriteLine("{0} - NONE", i);
                        i++;
                    }
                    //NOTE THIS HAS CHANGED Modifier Items for Groups now are retrieved by calling GetModiferItemsForModiferGroups
                    ModGroup.ModifierItems = InvApi.GetModifierItemsForModifierGroup(InvContext, ModGroup.ItemNumber);
                    foreach (ModifierItem ModItem in ModGroup.ModifierItems)
                    {
                        Console.WriteLine("{0} - {1} : {2}", i, ModItem.ItemNumber, ModItem.ItemName);
                        i++;
                    }
                    string answer = Console.ReadLine();
                    if (answer.Length > 1)
                    {
                        Console.WriteLine("Invalid answer i Choose option 1 is chosen by default");
                        answer = "1";
                    }
                    else if (char.IsDigit(answer[0]) == false)
                    {
                        Console.WriteLine("Invalid answer i Choose option 1 chosen by defualt");
                        answer = "1";
                    }
                    InventoryItem dressing = InvApi.GetItem(InvContext, ModGroup.ModifierItems[Convert.ToInt32(answer) - 1].ItemNumber);
                    decimal Price = 0;
                    if (ModGroup.Charged == true) { Price = dressing.Price; }
                    inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = itemToAdd.ItemName, ItemNumber = dressing.ItemNumber, Price = dressing.Price, Quantity = 1, State = EntityState.Added, Guest = "42" });
                }

                // I created this item with the prompt description status set.
                itemToAdd = InvApi.GetItem(InvContext, "MiscItem");
                itemToAddID = Guid.NewGuid();
                LineItemToAdd = new LineItem() { Id = itemToAddID, ItemName = "Keychain", ItemNumber = itemToAdd.ItemNumber, Price = itemToAdd.Price, Quantity = 1, State = EntityState.Added, Guest = "11" };
                inv.LineItems.Add(LineItemToAdd);

                api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                api.UnLockInvoice(context, inv.InvoiceNumber);
                inv = api.GetInvoice(context, inv.InvoiceNumber);
                Console.WriteLine("The invoice has {0} guests", inv.PartySize);
                foreach (LineItem individualItem in inv.LineItems)
                {
                    Console.WriteLine("Guest {0} has ordered: {1}-{2}", individualItem.Guest, individualItem.ItemNumber, individualItem.ItemName);
                }

                //should show this invoice as locked by this station
                api.LockInvoice(context, inv.InvoiceNumber);
                List<OnHoldInfo> onHoldInfos = api.GetAllOnHoldInvoices(context);
                Console.WriteLine(String.Format("Retrieved {0} OnHoldInfo from GetAllOnHoldInvoices",
                                                    onHoldInfos.Count));
                foreach (OnHoldInfo onHoldInfo in onHoldInfos)
                {

                    foreach (OnHoldInfo OHI in onHoldInfos)
                    {
                        if (OHI.Locked == true)
                        {
                            Console.WriteLine(String.Format("Invoice {0} is locked by Station {1}",
                                                            OHI.InvoiceNumber, OHI.LockedByStation));
                        }
                    }
                }

                //checking locked status should be locked by this station
                inv = api.GetInvoice(context, inv.InvoiceNumber);
                if (inv.Locked == true)
                {
                    Console.WriteLine("Invoice #{0} is locked by Station: {1}", inv.InvoiceNumber.ToString(), inv.LockedByStation);
                }
                else
                {
                    Console.WriteLine("Invoice #{0} is unlocked", inv.InvoiceNumber.ToString());
                }

                api.UnLockInvoice(context, inv.InvoiceNumber);
                onHoldInfos.Clear();
                onHoldInfos = api.GetAllOnHoldInvoices(context);
                //should show this invoice as unlocked.
                Console.WriteLine(String.Format("Retrieved {0} OnHoldInfo from GetAllOnHoldInvoices",
                                                    onHoldInfos.Count));
                foreach (OnHoldInfo onHoldInfo in onHoldInfos)
                {

                    foreach (OnHoldInfo OHI in onHoldInfos)
                    {
                        if (OHI.Locked == true)
                        {
                            Console.WriteLine(String.Format("Invoice {0} is locked by Station {1}",
                                                            OHI.InvoiceNumber, OHI.LockedByStation));
                        }
                    }
                }
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }
        static void TestPreAuthInvoice()
        {
            try
            {
                SalesAPI salesAPI = new SalesAPI();

                pcAmerica.DesktopPOS.API.Client.SalesService.Context salesContext = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                salesContext.CashierID = "100101";
                salesContext.StoreID = "1001";
                salesContext.StationID = "01";

                PaymentAPI paymentAPI = new PaymentAPI();

                // first create the credit card object
                pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest creditCard1 = new pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest();
                creditCard1.Amount = 5.00M;
                creditCard1.CardNumber = "5454545454545454";
                creditCard1.ExpirationMonth = 05;
                creditCard1.ExpirationYear = 13;
                creditCard1.BarTab = true;// set this for a pre authed bar tab
                creditCard1.ProcessingType = pcAmerica.DesktopPOS.API.Client.PaymentService.ProcessingType.PreAuth;
                pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardPaymentProcessingResponse ProcessingResponse1 = paymentAPI.ProcessCreditCard(creditCard1);

                // create an invoice to apply your payment to
                Invoice inv = salesAPI.StartNewInvoice(salesContext, "Jane", "XXOPEN TABS");

                // use paymentIndex of -1 since the card isn't attached to the invoice yet
                AppliedPaymentResponse paymentResponse1 = salesAPI.ApplyCardPayment(salesContext, inv.InvoiceNumber, -1, new pcAmerica.DesktopPOS.API.Client.SalesService.CreditCardPaymentProcessingResponse() { Amount = ProcessingResponse1.Amount, ApprovalCode = ProcessingResponse1.ApprovalCode, CardNumber = ProcessingResponse1.CardNumber, ExpirationMonth = ProcessingResponse1.ExpirationMonth, ExpirationYear = ProcessingResponse1.ExpirationYear, ExtensionData = ProcessingResponse1.ExtensionData, IsPrePaidCard = ProcessingResponse1.IsPrePaidCard, PostAuthReferenceNumber = ProcessingResponse1.PostAuthReferenceNumber, ProcessType = pcAmerica.DesktopPOS.API.Client.SalesService.ProcessingType.PreAuth, ReferenceNumber = ProcessingResponse1.ReferenceNumber, Result = ProcessingResponse1.Result, TipAmount = ProcessingResponse1.TipAmount, TransactionNumber = ProcessingResponse1.TransactionNumber }, -1);

                // some time passes and items are added to the invoice
                InventoryAPI invAPI = new InventoryAPI();
                pcAmerica.DesktopPOS.API.Client.InventoryService.Context invContext = new pcAmerica.DesktopPOS.API.Client.InventoryService.Context();
                invContext.CashierID = "100101";
                invContext.StationID = "01";
                invContext.StoreID = "1001";

                salesAPI.LockInvoice(salesContext, inv.InvoiceNumber);
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "1" });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "2" });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "3" });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "4" });
                inv = salesAPI.ModifyItems(salesContext, inv.InvoiceNumber, inv.LineItems);


                //to complete the earlier pre auth card you must call CompletePreAuth and pass it the PaymentIndex you got back from ApplyCardPayment earlier
                creditCard1.Amount = inv.GrandTotal - 7.98M;
                creditCard1.ProcessingType = pcAmerica.DesktopPOS.API.Client.PaymentService.ProcessingType.PostAuth;
                creditCard1.PostAuthReferenceNumber = ProcessingResponse1.PostAuthReferenceNumber;
                creditCard1.ReferenceNumber = ProcessingResponse1.ReferenceNumber;
                creditCard1.PaymentIndex = paymentResponse1.PaymentIndex;

                ProcessingResponse1 = paymentAPI.CompletePreAuth(creditCard1, inv.InvoiceNumber);
                paymentResponse1 = salesAPI.ApplyCardPayment(salesContext, inv.InvoiceNumber, -1, new pcAmerica.DesktopPOS.API.Client.SalesService.CreditCardPaymentProcessingResponse() { Amount = ProcessingResponse1.Amount, ApprovalCode = ProcessingResponse1.ApprovalCode, CardNumber = ProcessingResponse1.CardNumber, ExpirationMonth = ProcessingResponse1.ExpirationMonth, ExpirationYear = ProcessingResponse1.ExpirationYear, ExtensionData = ProcessingResponse1.ExtensionData, IsPrePaidCard = ProcessingResponse1.IsPrePaidCard, PostAuthReferenceNumber = ProcessingResponse1.PostAuthReferenceNumber, ProcessType = pcAmerica.DesktopPOS.API.Client.SalesService.ProcessingType.PostAuth, ReferenceNumber = ProcessingResponse1.ReferenceNumber, Result = ProcessingResponse1.Result, TipAmount = ProcessingResponse1.TipAmount, TransactionNumber = ProcessingResponse1.TransactionNumber }, paymentResponse1.PaymentIndex);

                // create a second credit card object
                pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest creditCard2 = new pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest();
                creditCard2.Amount = 7.98M;
                creditCard2.CardNumber = "4545454545454545";
                creditCard2.ExpirationMonth = 05;
                creditCard2.ExpirationYear = 13;
                creditCard2.BarTab = false;// this card is not a bar tab
                creditCard2.ProcessingType = pcAmerica.DesktopPOS.API.Client.PaymentService.ProcessingType.PreAuth;
                //process and apply the 2nd card
                pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardPaymentProcessingResponse ProcessingResponse2 = paymentAPI.ProcessCreditCard(creditCard2);
                AppliedPaymentResponse paymentResponse2 = salesAPI.ApplyCardPayment(salesContext, inv.InvoiceNumber, -1, new pcAmerica.DesktopPOS.API.Client.SalesService.CreditCardPaymentProcessingResponse() { Amount = ProcessingResponse2.Amount, ApprovalCode = ProcessingResponse2.ApprovalCode, CardNumber = ProcessingResponse2.CardNumber, ExpirationMonth = ProcessingResponse2.ExpirationMonth, ExpirationYear = ProcessingResponse2.ExpirationYear, ExtensionData = ProcessingResponse2.ExtensionData, IsPrePaidCard = ProcessingResponse2.IsPrePaidCard, PostAuthReferenceNumber = ProcessingResponse2.PostAuthReferenceNumber, ProcessType = pcAmerica.DesktopPOS.API.Client.SalesService.ProcessingType.PreAuth, ReferenceNumber = ProcessingResponse2.ReferenceNumber, Result = ProcessingResponse2.Result, TipAmount = ProcessingResponse2.TipAmount, TransactionNumber = ProcessingResponse2.TransactionNumber }, -1);

                //update the invoice object and end the transaction
                inv = salesAPI.GetInvoice(salesContext, inv.InvoiceNumber);
                salesAPI.EndInvoice(salesContext, inv.InvoiceNumber);

                //salesAPI.PrintReceipt(salesContext, inv.InvoiceNumber,-1);

                //add a tips to the invoice
                creditCard1.TipAmount = 1.5M;
                creditCard1.BarTab = false;
                ProcessingResponse1 = paymentAPI.CompletePreAuth(creditCard1, inv.InvoiceNumber);
                salesAPI.ApplyCardPayment(salesContext, inv.InvoiceNumber, -1, new pcAmerica.DesktopPOS.API.Client.SalesService.CreditCardPaymentProcessingResponse() { Amount = ProcessingResponse1.Amount, ApprovalCode = ProcessingResponse1.ApprovalCode, CardNumber = ProcessingResponse1.CardNumber, ExpirationMonth = ProcessingResponse1.ExpirationMonth, ExpirationYear = ProcessingResponse1.ExpirationYear, ExtensionData = ProcessingResponse1.ExtensionData, IsPrePaidCard = ProcessingResponse1.IsPrePaidCard, PostAuthReferenceNumber = ProcessingResponse1.PostAuthReferenceNumber, ProcessType = pcAmerica.DesktopPOS.API.Client.SalesService.ProcessingType.PostAuth, ReferenceNumber = ProcessingResponse1.ReferenceNumber, Result = ProcessingResponse1.Result, TipAmount = ProcessingResponse1.TipAmount, TransactionNumber = ProcessingResponse1.TransactionNumber }, paymentResponse1.PaymentIndex);


                //to complete the earlier pre auth card you must call CompletePreAuth and pass it the PaymentIndex you got back from ApplyCardPayment earlier
                creditCard2.Amount = 7.98M;
                creditCard2.TipAmount = 1.77M;
                creditCard2.ProcessingType = pcAmerica.DesktopPOS.API.Client.PaymentService.ProcessingType.PostAuth;
                creditCard2.PostAuthReferenceNumber = ProcessingResponse2.PostAuthReferenceNumber;
                creditCard2.ReferenceNumber = ProcessingResponse2.ReferenceNumber;
                creditCard2.PaymentIndex = paymentResponse2.PaymentIndex;
                ProcessingResponse2 = paymentAPI.CompletePreAuth(creditCard2, inv.InvoiceNumber);
                salesAPI.ApplyCardPayment(salesContext, inv.InvoiceNumber, -1, new pcAmerica.DesktopPOS.API.Client.SalesService.CreditCardPaymentProcessingResponse() { Amount = ProcessingResponse2.Amount, ApprovalCode = ProcessingResponse2.ApprovalCode, CardNumber = ProcessingResponse2.CardNumber, ExpirationMonth = ProcessingResponse2.ExpirationMonth, ExpirationYear = ProcessingResponse2.ExpirationYear, ExtensionData = ProcessingResponse2.ExtensionData, IsPrePaidCard = ProcessingResponse2.IsPrePaidCard, PostAuthReferenceNumber = ProcessingResponse2.PostAuthReferenceNumber, ProcessType = pcAmerica.DesktopPOS.API.Client.SalesService.ProcessingType.PostAuth, ReferenceNumber = ProcessingResponse2.ReferenceNumber, Result = ProcessingResponse2.Result, TipAmount = ProcessingResponse2.TipAmount, TransactionNumber = ProcessingResponse2.TransactionNumber }, paymentResponse2.PaymentIndex);

                salesAPI.PrintReceipt(salesContext, inv.InvoiceNumber, -1);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        
        static void DoItemsNeedToBeSentToKitchen(pcAmerica.DesktopPOS.API.Client.SalesService.Context context, long invoiceNumber)
        {
            SalesAPI api = new SalesAPI();
            Invoice inv = api.GetInvoice(context, invoiceNumber);
            int i = 0, count = 0;
            for (i = 0; i <= inv.LineItems.Count - 1; i++)
            {
                if (inv.LineItems[i].SentToKitchen == false)
                {
                    Console.WriteLine(String.Format("Line#{0} needs to be sent to the Kitchen", i + 1));
                    count++;
                }
            }
            if (count == 0) { Console.WriteLine("No items need to be sent to the Kitchen"); }
        }

        static void TestSplits()
        {
            try
            {
                SalesAPI api = new SalesAPI();

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "01";

                Invoice inv = api.StartNewInvoice(context, "Rich", "XXOPEN TABS");
                api.LockInvoice(context, inv.InvoiceNumber);
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 3, State = EntityState.Added, Guest = "1" });
                api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                api.UnLockInvoice(context, inv.InvoiceNumber);

                inv = api.SplitInvoice(context, inv.InvoiceNumber, 3);

                for (int i = 0; i <= inv.SplitInfo.NumberOfSplitChecks - 1; i++)
                {
                    Console.WriteLine(String.Format("Rich - Guest #{0}: ${1}", i + 1, inv.SplitInfo.GrandTotalForSplit[i]));
                }

                inv = api.StartNewInvoice(context, "Steve", "XXOPEN TABS");
                api.SetPartySizeForInvoice(context, inv.InvoiceNumber, 3);
                api.LockInvoice(context, inv.InvoiceNumber);
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "1" });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "2" });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "3" });
                api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                api.UnLockInvoice(context, inv.InvoiceNumber);

                inv = api.SplitInvoice(context, inv.InvoiceNumber, 3);

                api.ApplyCashPayment(context, inv.InvoiceNumber, 1, 2.00M);
                api.ApplyCashPayment(context, inv.InvoiceNumber, 2, 50.00M);
                //updates the split information so it has the payment info
                inv = api.GetInvoiceHeader(context, inv.InvoiceNumber);
                // shows split info grand total and if it is completly paid
                // NOTE: Even if you have fully paid a sub check it won't be marked as paid until you run EndSubCheck on it
                for (int i = 0; i <= inv.SplitInfo.NumberOfSplitChecks - 1; i++)
                {
                    Console.WriteLine(String.Format("Steve -  Grand Total SPLIT #{0}: ${1}", i + 1, inv.SplitInfo.GrandTotalForSplit[i]));
                    Console.WriteLine(String.Format("Steve -  Paid SPLIT #{0}: {1}", i + 1, inv.SplitInfo.IsSplitPaid[i]));
                }
                api.EndSubCheck(context, inv.InvoiceNumber, 2);
                inv = api.GetInvoiceHeader(context, inv.InvoiceNumber);
                for (int i = 0; i <= inv.SplitInfo.NumberOfSplitChecks - 1; i++)
                {
                    Console.WriteLine(String.Format("Steve -  Grand Total SPLIT #{0}: ${1}", i + 1, inv.SplitInfo.GrandTotalForSplit[i]));
                    Console.WriteLine(String.Format("Steve -  Paid SPLIT #{0}: {1}", i + 1, inv.SplitInfo.IsSplitPaid[i]));
                }
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void TestSectionsAndTables()
        {
            try
            {
                SalesAPI salesAPI = new SalesAPI();
                TableAPI tableAPI = new TableAPI();

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "01";

                pcAmerica.DesktopPOS.API.Client.TableService.Context tableContext = new pcAmerica.DesktopPOS.API.Client.TableService.Context();
                tableContext.CashierID = "100101";
                tableContext.StationID = "01";
                tableContext.StoreID = "1001";
                Invoice inv;
                List<TableInfo> tables = tableAPI.GetAllTablesAndOpenInvoices(tableContext);
                if (tables.Count > 0)
                {
                    int i = 0;
                    for (i = 0; i < tables.Count - 1; i++)
                    {
                        if (tables[i].SectionID.StartsWith("XX") && !tables[i].Occupied) { continue; }
                        inv = salesAPI.StartNewInvoice(context, tables[i].TableNumber, tables[i].SectionID);
                        salesAPI.LockInvoice(context, inv.InvoiceNumber);
                        inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "1" });
                        salesAPI.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                        salesAPI.UnLockInvoice(context, inv.InvoiceNumber);
                        break;
                    }
                }

                inv = salesAPI.StartNewInvoice(context, "Dave", "XXTAKEOUT");
                salesAPI.LockInvoice(context, inv.InvoiceNumber);
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "1" });
                salesAPI.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                salesAPI.UnLockInvoice(context, inv.InvoiceNumber);

                inv = salesAPI.StartNewInvoice(context, "Jay", "XXOPEN TABS");
                salesAPI.LockInvoice(context, inv.InvoiceNumber);
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "1" });
                salesAPI.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                salesAPI.UnLockInvoice(context, inv.InvoiceNumber);


                //NOTE: Delivery invoices will be put into the delivery tab section however the will not be put
                //into Delivery Tracking as there is curently no way to provide customer numbers or a time promised
                inv = salesAPI.StartNewInvoice(context, "Sara", "XXDELIVERY");
                salesAPI.LockInvoice(context, inv.InvoiceNumber);
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "1" });
                salesAPI.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                salesAPI.UnLockInvoice(context, inv.InvoiceNumber);

                Console.WriteLine("There should now be open invoices on the first empty table in the list as well as in the Delivery, Takeout and  Open Tabs sections.");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }

        }

        static void TestVoidInvoice()
        {
            try
            {
                SalesAPI api = new SalesAPI();

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "01";

                Console.WriteLine("Enter an invoice number to void: ");
                string answer = Console.ReadLine();
                Console.WriteLine("Send voided invoice to the kitchen printer?" + Environment.NewLine + "1 for YES" + Environment.NewLine + "0 for NO");
                string answer2 = Console.ReadLine();
                
                api.VoidInvoice(context, Convert.ToInt64(answer), answer2 == "1" ? true : false );
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }
        static void deleteItemsTest()
        {
            try
            {
                SalesAPI api = new SalesAPI();

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "01";

                // StartNewInvoice - this also automatically locks an invoice so it can't be opened by a terminal
                Invoice inv = api.StartNewInvoice(context, "ROB" + DateTime.Now.Ticks.ToString(), "XXOPEN TABS");
                Console.WriteLine(String.Format("Started new invoice with #: {0}", inv.InvoiceNumber));

                // Unlock Invoice
                /*if (api.UnLockInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine(String.Format("Unlocked invoice # {0}", inv.InvoiceNumber));
                else
                    Console.WriteLine(String.Format("Failed to unlock invoice # {0}", inv.InvoiceNumber));*/

                // Lock Invoice
                if (api.LockInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine(String.Format("Locked invoice # {0}", inv.InvoiceNumber));
                else
                    Console.WriteLine(String.Format("Failed to lock invoice # {0}", inv.InvoiceNumber));

                // GetInvoiceHeader
                inv = api.GetInvoiceHeader(context, inv.InvoiceNumber);
                Console.WriteLine(String.Format("GetInvoiceHeader with #: {0}", inv.InvoiceNumber));

                // GetInvoice
                inv = api.GetInvoice(context, inv.InvoiceNumber);
                Console.WriteLine(String.Format("GetInvoice with #: {0}", inv.InvoiceNumber));

                InventoryAPI InvApi = new InventoryAPI();

                pcAmerica.DesktopPOS.API.Client.InventoryService.Context InvContext = new pcAmerica.DesktopPOS.API.Client.InventoryService.Context();
                InvContext.CashierID = "100101";
                InvContext.StationID = "01";
                InvContext.StoreID = "1001";

                Guid parentGuid = new Guid();
                // ModifyItems
                inv.LineItems.Add(new LineItem() { Id = parentGuid, ItemName = "BURGER", ItemNumber = "SAND1", Price = 1.99M, Quantity = 2, State = EntityState.Added });

                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Add Tomato", ItemNumber = "SANDMod3", Price = 0.10M, Quantity = 1, State = EntityState.Added, ParentId = parentGuid });

                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "BURGER", ItemNumber = "SAND1", Price = 1.99M, Quantity = 1, State = EntityState.Added });

                api.UnLockInvoice(context, inv.InvoiceNumber);
                api.LockInvoice(context, inv.InvoiceNumber);

                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);

                Invoice inv2 = api.GetInvoice(context, inv.InvoiceNumber);

                Console.WriteLine(String.Format("ModifyItems new invoice value: {0}", inv2.GrandTotal));
                //inv2.LineItems[0].State = EntityState.Deleted;
                //inv2.LineItems[1].State = EntityState.Deleted;
                inv2 = api.ModifyItems(context, inv2.InvoiceNumber, inv2.LineItems);
                Console.WriteLine(String.Format("ModifyItems new invoice value after deleting burger with tomato: {0}", inv2.GrandTotal));

                api.LockInvoice(context, inv2.InvoiceNumber);


                // ApplyCashPayment - applying grand total minus 1 dollar
                AppliedPaymentResponse payResponse = api.ApplyCashPayment(context, inv2.InvoiceNumber, -1, inv2.GrandTotal);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied cash payment, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply payment");

                // EndInvoice
                if (api.EndInvoice(context, inv2.InvoiceNumber))
                    Console.WriteLine("Ended invoice successfully");
                else
                    Console.WriteLine("***ERROR*** Could not end invoice");

                // PrintReceipt - providing -1 for the split check # when there are no split checks
                if (api.PrintReceipt(context, inv2.InvoiceNumber, -1))
                    Console.WriteLine("Receipt was printed");
                else
                    Console.WriteLine("***ERROR*** Receive was NOT printed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }

        }

        // more examples to work specificly with the sample database Burger Express that comes bundled with Resturant Pro Express
        // shows getting list of all inventory items, adding items two different ways, selecting a modifier for an item and splitting a check by guest
        static void TestBurgerExpress()
        {
            try
            {
                SalesAPI api = new SalesAPI();

                DateTime startDateTime = DateTime.Parse("1/1/2010");
                DateTime endDateTime = DateTime.Parse("12/31/2010");

                SalesTotals totals = api.GetTotals(startDateTime, endDateTime);
                Console.WriteLine(String.Format("Sales totals between {0}-{1} -- NetSales:{2} TotalTax:{3} GrandTotal:{4}", startDateTime, endDateTime, totals.NetSales, totals.TotalTax, totals.GrandTotal));

                List<ItemSale> sales = api.GetItemsSold(startDateTime, endDateTime);
                Console.WriteLine(String.Format("Between {0}-{1}, there are {2} records of items being sold", startDateTime, endDateTime, sales.Count));

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "01";

                // StartNewInvoice - this also automatically locks an invoice so it can't be opened by a terminal
                Invoice inv = api.StartNewInvoice(context, "Dan" + DateTime.Now.Second.ToString(), "XXOPEN TABS");
                Console.WriteLine(String.Format("Started new invoice with #: {0}", inv.InvoiceNumber));

                // Unlock Invoice
                if (api.UnLockInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine(String.Format("Unlocked invoice # {0}", inv.InvoiceNumber));
                else
                    Console.WriteLine(String.Format("Failed to unlock invoice # {0}", inv.InvoiceNumber));

                // Lock Invoice
                if (api.LockInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine(String.Format("Locked invoice # {0}", inv.InvoiceNumber));
                else
                    Console.WriteLine(String.Format("Failed to lock invoice # {0}", inv.InvoiceNumber));

                // GetInvoiceHeader
                inv = api.GetInvoiceHeader(context, inv.InvoiceNumber);
                Console.WriteLine(String.Format("GetInvoiceHeader with #: {0}", inv.InvoiceNumber));

                // GetInvoice
                inv = api.GetInvoice(context, inv.InvoiceNumber);
                Console.WriteLine(String.Format("GetInvoice with #: {0}", inv.InvoiceNumber));

                InventoryAPI InvApi = new InventoryAPI();

                pcAmerica.DesktopPOS.API.Client.InventoryService.Context InvContext = new pcAmerica.DesktopPOS.API.Client.InventoryService.Context();
                InvContext.CashierID = "100101";
                InvContext.StationID = "01";
                InvContext.StoreID = "1001";

                api.SetPartySizeForInvoice(context, inv.InvoiceNumber, 2);

                List<InventoryItem> items = InvApi.GetItemListExtended(InvContext);

                Console.WriteLine("*******************All Inventory Items************************");
                foreach (InventoryItem singleItem in items)
                {
                    Console.WriteLine(String.Format("Item#: {0} ItemName:{1}", singleItem.ItemNumber, singleItem.ItemName));
                }
                Console.WriteLine("***************************************************************");

                // ModifyItems
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "1" });

                InventoryItem itemToAdd = InvApi.GetItem(InvContext, "SALAD3");
                Guid itemToAddID = Guid.NewGuid();
                LineItem LineItemToAdd = new LineItem() { Id = itemToAddID, ItemName = itemToAdd.ItemName, ItemNumber = itemToAdd.ItemNumber, Price = itemToAdd.Price, Quantity = 1, State = EntityState.Added, Guest = "2" };
                inv.LineItems.Add(LineItemToAdd);

                itemToAdd.ModifierGroups = InvApi.GetModiferGroupsForItem(InvContext, itemToAdd.ItemNumber);
                foreach (ModifierGroup ModGroup in itemToAdd.ModifierGroups)
                {
                    Console.WriteLine("ModifierGroup:{0}", ModGroup.ItemName);
                    Console.WriteLine("{0}", ModGroup.Prompt);
                    int i = 1;
                    if (ModGroup.Forced == false)
                    {
                        Console.WriteLine("{0} - NONE", i);
                        i++;
                    }
                    //NOTE THIS HAS CHANGED Modifier Items for Groups now are retrieved by calling GetModiferItemsForModiferGroups
                    ModGroup.ModifierItems = InvApi.GetModifierItemsForModifierGroup(InvContext, ModGroup.ItemNumber);
                    foreach (ModifierItem ModItem in ModGroup.ModifierItems)
                    {
                        Console.WriteLine("{0} - {1} : {2}", i, ModItem.ItemNumber, ModItem.ItemName);
                        i++;
                    }
                    string answer = Console.ReadLine();
                    if (answer.Length > 1)
                    {
                        Console.WriteLine("Invalid answer i Choose option 1 is chosen by default");
                        answer = "1";
                    }
                    else if (char.IsDigit(answer[0]) == false)
                    {
                        Console.WriteLine("Invalid answer i Choose option 1 chosen by defualt");
                        answer = "1";
                    }
                    InventoryItem dressing = InvApi.GetItem(InvContext, ModGroup.ModifierItems[Convert.ToInt32(answer) - 1].ItemNumber);
                    decimal Price = 0;
                    if (ModGroup.Charged == true) { Price = dressing.Price; }
                    inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = dressing.ItemName, ItemNumber = dressing.ItemNumber, Price = dressing.Price, Quantity = 1, State = EntityState.Added, Guest = "2" });
                }


                itemToAdd = InvApi.GetItem(InvContext, "DaveBurger");
                itemToAddID = Guid.NewGuid();
                LineItemToAdd = new LineItem() { Id = itemToAddID, ItemName = itemToAdd.ItemName, ItemNumber = itemToAdd.ItemNumber, Price = itemToAdd.Price, Quantity = 1, State = EntityState.Added, Guest = "2" };
                inv.LineItems.Add(LineItemToAdd);
                itemToAdd.ModifierItems = InvApi.GetIndividualModifiers(InvContext, itemToAdd.ItemNumber);
                if (itemToAdd.ModifierItems.Count > 0)
                    inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = itemToAdd.ModifierItems[0].ItemName, ItemNumber = itemToAdd.ModifierItems[0].ItemNumber, Price = 0.00M, Quantity = 1, State = EntityState.Added, Guest = "1" });


                // This is a sample Kit Item I made that i added some items to you just have to ring up the base item(test) and the kit items will add along with it automaticly
                itemToAdd = InvApi.GetItem(InvContext, "test");
                itemToAddID = Guid.NewGuid();
                LineItemToAdd = new LineItem() { Id = itemToAddID, ItemName = itemToAdd.ItemName, ItemNumber = itemToAdd.ItemNumber, Price = itemToAdd.Price, Quantity = 1, State = EntityState.Added, Guest = "2" };
                inv.LineItems.Add(LineItemToAdd);

                itemToAdd = InvApi.GetItem(InvContext, "test");
                itemToAddID = Guid.NewGuid();
                LineItemToAdd = new LineItem() { Id = itemToAddID, ItemName = itemToAdd.ItemName, ItemNumber = itemToAdd.ItemNumber, Price = itemToAdd.Price, Quantity = 1, State = EntityState.Added, Guest = "2" };
                inv.LineItems.Add(LineItemToAdd);

                itemToAdd = InvApi.GetItem(InvContext, "DRINKCHOICE");
                int j = 1;
                Console.WriteLine(itemToAdd.ItemName2);
                InventoryItem Choice;
                foreach (String choiceItemNumber in itemToAdd.ChoiceItems)
                {
                    Choice = InvApi.GetItem(InvContext, choiceItemNumber);
                    Console.WriteLine(string.Format("{0}: ", Choice.ItemName));
                    j++;
                }
                int choiceItemSelection = Convert.ToInt32(Console.ReadLine()) - 1;
                itemToAdd = InvApi.GetItem(InvContext, itemToAdd.ChoiceItems[choiceItemSelection]);
                itemToAddID = Guid.NewGuid();
                LineItemToAdd = new LineItem() { Id = itemToAddID, ItemName = itemToAdd.ItemName, ItemNumber = itemToAdd.ItemNumber, Price = itemToAdd.Price, Quantity = 1, State = EntityState.Added, Guest = "1" };
                inv.LineItems.Add(LineItemToAdd);

                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems new invoice value: {0}", inv.GrandTotal));

                inv.LineItems[0].Quantity = 2;
                inv.LineItems[0].State = EntityState.Modified;
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems CHANGED 1st item QUANTITY, new invoice value: {0}", inv.GrandTotal));

                // SendToKitchen
                if (api.SendToKitchen(context, inv.InvoiceNumber))
                    Console.WriteLine("Invoice was printed in kitchen");
                else
                    Console.WriteLine("***ERROR*** Invoice was NOT printed in kitchen");

                // Splitcheck
                inv = api.SplitInvoiceByGuest(context, inv.InvoiceNumber);
                if (inv.SplitInfo.NumberOfSplitChecks == 2)
                    Console.WriteLine("Split invoice by guest");
                else
                    Console.WriteLine("***ERROR*** Invoice could be split");



                // ApplyCashPayment - applying grand total minus 1 dollar (NOTE SPLITS Starts counting at 0 not 1)
                AppliedPaymentResponse payResponse = api.ApplyCashPayment(context, inv.InvoiceNumber, 0, inv.SplitInfo.GrandTotalForSplit[0] - 1);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied cash payment to split 1, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply payment");

                // ApplyCardPayment - applying remaining 1 dollar as a credit card
                payResponse = api.ApplyCardPayment(context,
                    inv.InvoiceNumber,
                    0,
                    new pcAmerica.DesktopPOS.API.Client.SalesService.CreditCardPaymentProcessingResponse()
                    {
                        Amount = 1,
                        CardNumber = "4***********1",
                        ReferenceNumber = "123456",
                        Result = true,
                        TipAmount = 1,
                        TransactionNumber = 1234
                    }, -1);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied card payment to split 1, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply card payment");

                payResponse = api.ApplyCashPayment(context, inv.InvoiceNumber, 1, inv.SplitInfo.GrandTotalForSplit[1] + 13);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied cash payment to split 2, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply payment");

                // EndInvoice
                if (api.EndInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine("Ended invoice successfully");
                else
                    Console.WriteLine("***ERROR*** Could not end invoice");

                // looping and printing receipt for all splits
                for (int i = 0; i < inv.SplitInfo.NumberOfSplitChecks; i++)
                {
                    if (api.PrintReceipt(context, inv.InvoiceNumber, i))
                        Console.WriteLine("Receipt was printed");
                    else
                        Console.WriteLine("***ERROR*** Receive was NOT printed");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }

        }


        static void TestCreditCard()
        {
            try
            {
                pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest request = new pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardRequest();
                request.Amount = 1.00M;
                request.CardNumber = "4012888888881";
                request.ExpirationMonth = 12;
                request.ExpirationYear = 12;

                PaymentAPI api = new PaymentAPI();
                pcAmerica.DesktopPOS.API.Client.PaymentService.CreditCardPaymentProcessingResponse response = api.ProcessCreditCard(request);
                Console.WriteLine(String.Format("Response: Result={0}, CardNumber={1}, Amount={2}, Reference={3}, TransactionNumber={4}", response.Result, response.CardNumber, response.Amount, response.ReferenceNumber, response.TransactionNumber));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void TestCustomers()
        {
            CustomerAPI api = new CustomerAPI();

            try
            {
                string customerNumber = "abcd1234";
                bool result = false;

                Customer customer = api.FindRecord(customerNumber);
                Console.WriteLine(String.Format("Customer {0} found = {1}", customerNumber, customer != null));

                if (customer == null)
                {
                    customer = new Customer();
                    customer.CustomerNumber = customerNumber;
                    customer.FirstName = "Test";
                    customer.LastName = "Tester";
                    result = api.UpdateRecord(MessageAction.CreateOrUpdate, customer);
                    Console.WriteLine(String.Format("Add/Update customer {0} result = {1}", customerNumber, result));

                    customer = api.FindRecord(customerNumber);
                    Console.WriteLine(String.Format("Customer {0} found = {1}", customerNumber, customer != null));
                }

                result = api.UpdateRecord(MessageAction.Delete, customer);
                Console.WriteLine(String.Format("Delete customer {0} result = {1}", customer.CustomerNumber, result));

                customer = api.FindRecord(customerNumber);
                Console.WriteLine(String.Format("Customer {0} found = {1}", customerNumber, customer != null));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void TestEmployee()
        {
            EmployeeAPI api = new EmployeeAPI();

            try
            {
                Employee employee = api.GetCurrentUser();
                if (employee == null)
                    Console.WriteLine("No employee is currently logged into the POS application.");
                else
                    Console.WriteLine(String.Format("Employee ID:{0} FirstName:{1} LastName:{2} AccessLevel:{3}", employee.CashierID, employee.FirstName, employee.LastName, employee.AccessLevel));

                employee = api.AuthenticateEmployee("100101", "cashier");
                if (employee == null)
                    Console.WriteLine("***ERROR*** Invalid username/password");
                else
                    Console.WriteLine(String.Format("Authenticated employee: Employee ID:{0} FirstName:{1} LastName:{2} AccessLevel:{3}", employee.CashierID, employee.FirstName, employee.LastName, employee.AccessLevel));
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void TestInventory()
        {
            InventoryAPI api = new InventoryAPI();

            try
            {
                List<InventoryItem> items = api.GetItemList();
                Console.WriteLine(String.Format("There are currently {0} items in the POS database.", items.Count));

                pcAmerica.DesktopPOS.API.Client.InventoryService.Context context = new pcAmerica.DesktopPOS.API.Client.InventoryService.Context();
                context.StoreID = "1001";
                context.StationID = "01";
                context.CashierID = "100101";

                InventoryItem item = api.GetItem(context, "Non_Inventory");
                if (item == null)
                    Console.WriteLine("***ERROR*** Could not retrieve Non_Inventory item");
                else
                    Console.WriteLine("Retrieved Non_Inventory item");

                Console.WriteLine(string.Format("The Non_Inventory item has Item Type of {0}", item.ItemType));

                List<ModifierGroup> modGroups = api.GetModiferGroupsForItem(context, "Non_Inventory");
                if (modGroups == null || modGroups.Count == 0)
                    Console.WriteLine("No modifier groups exist for the Non_Inventory item!");
                else
                    Console.WriteLine(String.Format("Found {0} modifier groups for the Non_Inventory item!", modGroups.Count));

                List<ModifierItem> modifiers = api.GetIndividualModifiers(context, "Non_Inventory");
                if (modifiers == null || modifiers.Count == 0)
                    Console.WriteLine("No modifiers exist for the Non_Inventory item!");
                else
                    Console.WriteLine(String.Format("Found {0} modifiers for the Non_Inventory item!", modifiers.Count));

                if (item.KitItems == null || item.KitItems.Count == 0)
                    Console.WriteLine("The Non_Inventory has no Kit Items!");
                else
                    Console.WriteLine(String.Format("Found {0} Kit Item(s) for the Non_Inventory item!", item.KitItems.Count));

                InventoryItem kitTest = api.GetItem(context, "kit1");
                if (kitTest.KitItems == null || kitTest.KitItems.Count == 0)
                    Console.WriteLine("kit1 has no Kit Items!");
                else
                    Console.WriteLine(String.Format("Found {0} Kit Item(s) for kit1!", kitTest.KitItems.Count));

                if (item.ChoiceItems == null || item.ChoiceItems.Count == 0)
                    Console.WriteLine("The Non_Inventory has no Choice Items!");
                else
                    Console.WriteLine(String.Format("Found {0} Choice Item(s) for the Non_Inventory item!", item.ChoiceItems.Count));

                InventoryItem choiceTest = api.GetItem(context, "Choice Item One");
                if (choiceTest.ChoiceItems == null || choiceTest.ChoiceItems.Count == 0)
                    Console.WriteLine("Choice Item One has no Choice Items!");
                else
                    Console.WriteLine(String.Format("Found {0} Choice Item(s) for Choice Item One!", choiceTest.ChoiceItems.Count));

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void TestSales()
        {
            try
            {
                SalesAPI api = new SalesAPI();

                DateTime startDateTime = DateTime.Parse("1/1/2010");
                DateTime endDateTime = DateTime.Parse("12/31/2010");

                SalesTotals totals = api.GetTotals(startDateTime, endDateTime);
                Console.WriteLine(
                    String.Format("Sales totals between {0}-{1} -- NetSales:{2} TotalTax:{3} GrandTotal:{4}",
                                  startDateTime, endDateTime, totals.NetSales, totals.TotalTax, totals.GrandTotal));

                List<ItemSale> sales = api.GetItemsSold(startDateTime, endDateTime);
                Console.WriteLine(String.Format("Between {0}-{1}, there are {2} records of items being sold",
                                                startDateTime, endDateTime, sales.Count));

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context =
                    new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "01";

                // StartNewInvoice - this also automatically locks an invoice so it can't be opened by a terminal
                Invoice inv = api.StartNewInvoice(context, "ROB" + DateTime.Now.Second.ToString(), "XXOPEN TABS");
                Console.WriteLine(String.Format("Started new invoice with #: {0}", inv.InvoiceNumber));

                // Unlock Invoice
                if (api.UnLockInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine(String.Format("Unlocked invoice # {0}", inv.InvoiceNumber));
                else
                    Console.WriteLine(String.Format("Failed to unlock invoice # {0}", inv.InvoiceNumber));

                // Lock Invoice
                if (api.LockInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine(String.Format("Locked invoice # {0}", inv.InvoiceNumber));
                else
                    Console.WriteLine(String.Format("Failed to lock invoice # {0}", inv.InvoiceNumber));

                // GetInvoiceHeader
                inv = api.GetInvoiceHeader(context, inv.InvoiceNumber);
                Console.WriteLine(String.Format("GetInvoiceHeader with #: {0}", inv.InvoiceNumber));

                // GetInvoice
                inv = api.GetInvoice(context, inv.InvoiceNumber);
                Console.WriteLine(String.Format("GetInvoice with #: {0}", inv.InvoiceNumber));

                // ModifyItems
                inv.LineItems.Add(new LineItem()
                {
                    Id = Guid.NewGuid(),
                    ItemName = "Non Inventory",
                    ItemNumber = "Non_Inventory",
                    Price = 1,
                    Quantity = 1,
                    State = EntityState.Added
                });
                inv.LineItems.Add(new LineItem()
                {
                    Id = Guid.NewGuid(),
                    ItemName = "Non Inventory",
                    ItemNumber = "Non_Inventory",
                    Price = 2,
                    Quantity = 1,
                    State = EntityState.Added
                });
                inv.LineItems.Add(new LineItem()
                {
                    Id = Guid.NewGuid(),
                    ItemName = "Non Inventory",
                    ItemNumber = "Non_Inventory",
                    Price = 3,
                    Quantity = 1,
                    State = EntityState.Added,
                    ParentId = inv.LineItems[1].Id
                });
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems new invoice value: {0}", inv.GrandTotal));
                inv.LineItems[0].State = EntityState.Deleted;
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems DELETED 1st item, new invoice value: {0}", inv.GrandTotal));
                inv.LineItems[0].Quantity = 2;
                inv.LineItems[0].State = EntityState.Modified;
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems CHANGED 1st item QUANTITY, new invoice value: {0}",
                                                inv.GrandTotal));
                inv.LineItems.Add(new LineItem()
                {
                    ItemNumber = "Non_Inventory",
                    ItemName = "Hot dog",
                    Price = 1,
                    Quantity = 1,
                    State = EntityState.Added
                });
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems ADDED item # 1, new invoice value: {0}", inv.GrandTotal));

                // SendToKitchen
                if (api.SendToKitchen(context, inv.InvoiceNumber))
                    Console.WriteLine("Invoice was printed in kitchen");
                else
                    Console.WriteLine("***ERROR*** Invoice was NOT printed in kitchen");

                if (api.SendToKitchen(context, inv.InvoiceNumber))
                    Console.WriteLine(
                        "Invoice was printed in kitchen, it should not have printed anything out the 2nd time");
                else
                    Console.WriteLine("***ERROR*** Invoice was NOT printed in kitchen");

                // Splitcheck
                inv = api.SplitInvoice(context, inv.InvoiceNumber, 2);
                if (inv.SplitInfo.NumberOfSplitChecks == 2)
                    Console.WriteLine("Split invoice 2 ways");
                else
                    Console.WriteLine("***ERROR*** Invoice could be split");

                // CombineSplits
                inv = api.CombineSplits(context, inv.InvoiceNumber);
                if (inv.SplitInfo.NumberOfSplitChecks == 0)
                    Console.WriteLine("Combined split checks");
                else
                    Console.WriteLine("***ERROR*** Invoice could be split");

                // GetAllOnHoldInvoices
                List<OnHoldInfo> onHoldInfos = api.GetAllOnHoldInvoices(context);
                if (onHoldInfos == null)
                    Console.WriteLine("***ERROR*** Could not retrieve GetAllOnHoldInvoices");
                else
                {
                    Console.WriteLine(String.Format("Retrieved {0} OnHoldInfo from GetAllOnHoldInvoices",
                                                    onHoldInfos.Count));
                    foreach (OnHoldInfo onHoldInfo in onHoldInfos)
                    {
                        if (onHoldInfo.Locked == true)
                        {
                            Console.WriteLine(String.Format("Invoice {0} is locked by Station {1}",
                                                            onHoldInfo.InvoiceNumber, onHoldInfo.LockedByStation));
                        }
                    }
                }

                // GetOnHoldInvoicesForCashier
                onHoldInfos = api.GetOnHoldInvoicesForCashier(context);
                if (onHoldInfos == null)
                    Console.WriteLine("***ERROR*** Could not retrieve GetOnHoldInvoicesForCashier");
                else
                {
                    Console.WriteLine(String.Format("Retrieved {0} OnHoldInfo from GetOnHoldInvoicesForCashier",
                                                    onHoldInfos.Count));
                    foreach (OnHoldInfo onHoldInfo in onHoldInfos)
                    {
                        if (onHoldInfo.Locked == true)
                        {
                            Console.WriteLine(String.Format("Invoice {0} is locked by Station {1}", onHoldInfo.InvoiceNumber, onHoldInfo.LockedByStation));
                        }
                    }
                }


                // ApplyCashPayment - applying grand total minus 1 dollar
                AppliedPaymentResponse payResponse = api.ApplyCashPayment(context, inv.InvoiceNumber, -1, inv.GrandTotal - 1);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied cash payment, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply payment");

                // ApplyCardPayment - applying remaining 1 dollar as a credit card
                payResponse = api.ApplyCardPayment(context,
                    inv.InvoiceNumber,
                    -1,
                    new pcAmerica.DesktopPOS.API.Client.SalesService.CreditCardPaymentProcessingResponse()
                    {
                        Amount = 1,
                        CardNumber = "4***********1",
                        ReferenceNumber = "123456",
                        Result = true,
                        TipAmount = 1,
                        TransactionNumber = 1234
                    }, -1);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied card payment, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply card payment");

                // EndInvoice
                if (api.EndInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine("Ended invoice successfully");
                else
                    Console.WriteLine("***ERROR*** Could not end invoice");

                // PrintReceipt - providing -1 for the split check # when there are no split checks
                if (api.PrintReceipt(context, inv.InvoiceNumber, -1))
                    Console.WriteLine("Receipt was printed");
                else
                    Console.WriteLine("***ERROR*** Receive was NOT printed");

                // EmailReceipt - providing -1 for the split check # when there are no split checks
                if (api.EmailReceipt(context, inv.InvoiceNumber, -1, "dtomasheski@pcamerica.com"))
                    Console.WriteLine("Receipt was emailed");
                else
                    Console.WriteLine("***ERROR*** Receipt was NOT emailed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void TestMenus()
        {
            try
            {
                pcAmerica.DesktopPOS.API.Client.MenuService.Context context = new pcAmerica.DesktopPOS.API.Client.MenuService.Context();
                context.CashierID = "100101";
                context.StationID = "01";
                context.StoreID = "1001";

                MenuAPI api = new MenuAPI();
                Menu menu = api.GetCurrentMenu(context);
                if (menu == null)
                    Console.WriteLine("***ERROR*** No menu was returned");
                else
                {
                    Console.WriteLine(String.Format("Menu contains {0} departments", menu.Departments.Count));
                    foreach (Button dep in menu.Departments)
                    {
                        Console.WriteLine(String.Format("Department {0} contains {1} item buttons", dep.ID, dep.ChildButtons.Count));
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void TestTables()
        {
            try
            {
                pcAmerica.DesktopPOS.API.Client.TableService.Context context = new pcAmerica.DesktopPOS.API.Client.TableService.Context();
                context.CashierID = "100101";
                context.StationID = "01";
                context.StoreID = "1001";

                TableAPI api = new TableAPI();

                List<TableInfo> tables = api.GetAllTablesAndOpenInvoices(context);
                int takeoutOrders = 0;
                int openTabOrders = 0;
                int deliveryOrders = 0;
                int occupiedTables = 0;
                int emptyTables = 0;
                if (tables == null)
                    Console.WriteLine("***ERROR*** No tables or invoices were returned");
                else

                    foreach (TableInfo table in tables)
                    {
                        if (table.SectionID == "XXTAKEOUT")
                        {
                            takeoutOrders++;
                        }
                        else if (table.SectionID == "XXOPEN TABS")
                        {
                            openTabOrders++;
                        }
                        else if (table.SectionID == "XXDELIVERY")
                        {
                            deliveryOrders++;
                        }
                        else if (!string.IsNullOrEmpty(table.OnHoldID))
                        {
                            occupiedTables++;
                        }
                        else
                        {
                            emptyTables++;
                        }
                    }
                Console.WriteLine("Takeout Order Count: {0}", takeoutOrders);
                Console.WriteLine("Open Tabs Order Count: {0}", openTabOrders);
                Console.WriteLine("Delivery Order Count: {0}", deliveryOrders);
                Console.WriteLine("Occupied Table Count: {0}", occupiedTables);
                Console.WriteLine("Empty Table Count: {0}", emptyTables);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }
        static void TestGetStoreIDsAndGetStationIDs()
        {
            try
            {
                CompanyInformationAPI api = new CompanyInformationAPI();
                List<String> StoreIDs = api.GetStoreIDs();
                List<String> StationIDs;
                foreach (String StoreID in StoreIDs)
                {
                    StationIDs = api.GetStationIDs(StoreID);
                    Console.WriteLine("Store {0} has the following Stations:", StoreID);
                    foreach (String StationID in StationIDs)
                    {
                        Console.WriteLine("  {0}", StationID);
                    }
                    Console.WriteLine("");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void AddItemsOutOfOrderTest()
        {
            try
            {
                SalesAPI api = new SalesAPI();

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "01";
                context.StoreID = "1001";
                context.StationID = "02";

                // StartNewInvoice - this also automatically locks an invoice so it can't be opened by a terminal
                Invoice inv = api.StartNewInvoice(context, Guid.NewGuid().ToString().Substring(0, 8), "XXOPEN TABS");
                Console.WriteLine(String.Format("Started new invoice with #: {0}", inv.InvoiceNumber));

                // GetInvoiceHeader
                inv = api.GetInvoiceHeader(context, inv.InvoiceNumber);
                Console.WriteLine(String.Format("GetInvoiceHeader with #: {0}", inv.InvoiceNumber));

                // GetInvoice
                inv = api.GetInvoice(context, inv.InvoiceNumber);
                Console.WriteLine(String.Format("GetInvoice with #: {0}", inv.InvoiceNumber));


                Guid parent1 = Guid.NewGuid();
                Guid parent2 = Guid.NewGuid();

                // ModifyItems
                inv.LineItems.Add(new LineItem() { Id = parent1, ItemName = "All American Meatloaf", ItemNumber = "All American Meat", Price = 6.00M, Quantity = 1, State = EntityState.Added });
                inv.LineItems.Add(new LineItem() { Id = parent2, ItemName = "Green St. Greek Salad", ItemNumber = "Greek Salad", Price = 6.50M, Quantity = 2, State = EntityState.Added });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Lobster Roll", ItemNumber = "Lobster", Price = 13.50M, Quantity = 2, State = EntityState.Added });

                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Avocado", ItemNumber = "Avocado FREE", ParentId = parent2, State = EntityState.Added });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Add", ItemNumber = "ADD", ParentId = parent2, State = EntityState.Added });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "No Blue Cheese", ItemNumber = "NO Blue Cheese", ParentId = parent2, State = EntityState.Added });

                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Bulkie", ItemNumber = "Bulkie", ParentId = parent1, State = EntityState.Added });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Blue Cheese", ItemNumber = "BLUE CHEESE", ParentId = parent1, State = EntityState.Added });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Avocado", ItemNumber = "Avocado FREE", ParentId = parent1, State = EntityState.Added });

                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);

                Invoice inv2 = api.GetInvoice(context, inv.InvoiceNumber);


                //We see all the modifier items now have the parentID set to the Lobster item which is incorrect:
                foreach (LineItem item in inv2.LineItems)
                {
                    Console.WriteLine("item name:{0}, id:{1}, parentId:{2}", item.ItemName, item.Id, item.ParentId);
                }
                //Also in addition sometimes there is exception "Can't add item "some item" to the invoice"

                api.UnLockInvoice(context, inv2.InvoiceNumber);

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }

        }

        static void TestDBInfo()
        {
            try
            {
                string DBName = "";
                string InstanceName = "";
                CompanyInformationAPI api = new CompanyInformationAPI();
                api.GetDBInfo(ref DBName,ref InstanceName);
                Console.WriteLine("Database name:{0}", DBName);
                Console.WriteLine("Instance name:{0}", InstanceName);

            }
            catch (Exception ex)
            {
                
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }

        }

        static void testSendToKitchen()
        {
            try
            {
                SalesAPI api = new SalesAPI();

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "01";

                Random random = new Random();
                Invoice inv = api.StartNewInvoice(context, "Audry" + random.Next(0,10000) , "XXOPEN TABS");
                api.LockInvoice(context, inv.InvoiceNumber);
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 3, State = EntityState.Added, Guest = "1" });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 3, State = EntityState.Added, Guest = "1" });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 3, State = EntityState.Added, Guest = "1" });
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine("Three Tripple Cheese Burgers added to invoice");

                Console.WriteLine("Needs to be sent to kitchen: {0}", inv.NeedsToBeSentToKitchen);
                DoItemsNeedToBeSentToKitchen(context, inv.InvoiceNumber);
                
                if (api.SendToKitchen(context, inv.InvoiceNumber))// should output items sent to kitchen
                {
                    Console.WriteLine("Items Sent To Kitchen");
                }
                else
                {
                    Console.WriteLine("Failed sending items to kitchen");
                }
                Console.WriteLine("");

                inv.LineItems[1].State = EntityState.Deleted;
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine("Tripple Cheese Burger in position 1 deletd");
                Console.WriteLine("Needs to be sent to kitchen: {0}",inv.NeedsToBeSentToKitchen);
                DoItemsNeedToBeSentToKitchen(context, inv.InvoiceNumber);
                if (api.SendToKitchen(context, inv.InvoiceNumber))
                {
                    Console.WriteLine("Items Sent To Kitchen");
                }
                else
                {
                    Console.WriteLine("Failed sending items to kitchen");
                }
                Console.WriteLine("");

                Console.WriteLine("Did nothing checking for false");
                inv = api.GetInvoice(context, inv.InvoiceNumber);
                Console.WriteLine("Needs to be sent to kitchen: {0}", inv.NeedsToBeSentToKitchen);
                DoItemsNeedToBeSentToKitchen(context, inv.InvoiceNumber);
                if (api.SendToKitchen(context, inv.InvoiceNumber))
                {
                    Console.WriteLine("Items Sent To Kitchen");
                }
                else
                {
                    Console.WriteLine("Failed sending items to kitchen");
                }


                api.VoidInvoice(context, inv.InvoiceNumber, true);

            }
            catch (Exception ex)
            {

                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }

        static void TestSaleWithCreditCardPayment()
        {
            try
            {
                var api = new SalesAPI();

                var context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context
                    {
                        CashierID = "100101",
                        StoreID = "1001",
                        StationID = "01"
                    };

                // StartNewInvoice - this also automatically locks an invoice so it can't be opened by a terminal
                var inv = api.StartNewInvoice(context, "ROB" + DateTime.Now.Second.ToString(), "XXOPEN TABS");
                Console.WriteLine(String.Format("Started new invoice with #: {0}", inv.InvoiceNumber));

                inv.LineItems.Add(new LineItem()
                {
                    ItemNumber = "Non_Inventory",
                    ItemName = "Hot dog",
                    Price = 1,
                    Quantity = 1,
                    State = EntityState.Added
                });
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems ADDED item # 1, new invoice value: {0}", inv.GrandTotal));

                // SendToKitchen
                if (api.SendToKitchen(context, inv.InvoiceNumber))
                    Console.WriteLine("Invoice was printed in kitchen");
                else
                    Console.WriteLine("***ERROR*** Invoice was NOT printed in kitchen");

                if (api.SendToKitchen(context, inv.InvoiceNumber))
                    Console.WriteLine(
                        "Invoice was printed in kitchen, it should not have printed anything out the 2nd time");
                else
                    Console.WriteLine("***ERROR*** Invoice was NOT printed in kitchen");

                // ApplyCardPayment - applying remaining 1 dollar as a credit card
                var payResponse = api.ApplyCardPayment(context,
                    inv.InvoiceNumber,
                    -1,
                    new pcAmerica.DesktopPOS.API.Client.SalesService.CreditCardPaymentProcessingResponse()
                    {
                        Amount = inv.GrandTotal,
                        CardNumber = "4***********1",
                        ReferenceNumber = "123456",
                        Result = true,
                        TipAmount = 1,
                        TransactionNumber = 1234
                    }, -1);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied card payment, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply card payment");

                // EndInvoice
                if (api.EndInvoice(context, inv.InvoiceNumber))
                    Console.WriteLine("Ended invoice successfully");
                else
                    Console.WriteLine("***ERROR*** Could not end invoice");

                // PrintReceipt - providing -1 for the split check # when there are no split checks
                if (api.PrintReceipt(context, inv.InvoiceNumber, -1))
                    Console.WriteLine("Receipt was printed");
                else
                    Console.WriteLine("***ERROR*** Receive was NOT printed");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                Console.WriteLine("PRESS ENTER TO CONTINUE...");
                Console.ReadLine();
            }
        }
    }
}