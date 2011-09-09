using System;
using System.Collections.Generic;
using System.Text;
using pcAmerica.DesktopPOS.API.Client;
using pcAmerica.DesktopPOS.API.Client.PaymentService;
using pcAmerica.DesktopPOS.API.Client.CustomerService;
using pcAmerica.DesktopPOS.API.Client.EmployeeService;
using pcAmerica.DesktopPOS.API.Client.InventoryService;
using pcAmerica.DesktopPOS.API.Client.SalesService;
using pcAmerica.DesktopPOS.API.Client.MenuService;
using pcAmerica.DesktopPOS.API.Client.TableService;

namespace APITester
{
    class Program
    {
        static void Main(string[] args)
        {
            //TestCreditCard();
            //TestCustomers();
            //TestEmployee();
            //TestInventory();
            TestSales();
            //TestMenus();
            //TestTables();
            //TestBurgerExpress();
            //deleteItemsTest();
            //TestVoidInvoice();

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
                api.VoidInvoice(context, Convert.ToInt64(answer));
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
                Invoice inv = api.StartNewInvoice(context, "ROB" + DateTime.Now.Ticks.ToString());
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
                Invoice inv = api.StartNewInvoice(context, "Jeremy" + DateTime.Now.Ticks.ToString());
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
                foreach(InventoryItem singleItem in items)
                {
                    Console.WriteLine(String.Format("Item#: {0} ItemName:{1}",singleItem.ItemNumber,singleItem.ItemName));
                }
                Console.WriteLine("***************************************************************");

                // ModifyItems
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "TRIPPLE CHEESE BURGER", ItemNumber = "SAND4", Price = 3.99M, Quantity = 1, State = EntityState.Added, Guest = "1"});

                InventoryItem itemToAdd = InvApi.GetItem(InvContext, "SALAD3");
                Guid itemToAddID = Guid.NewGuid();
                LineItem LineItemToAdd = new LineItem(){Id = itemToAddID, ItemName = itemToAdd.ItemName, ItemNumber = itemToAdd.ItemNumber, Price = itemToAdd.Price,Quantity = 1, State = EntityState.Added, Guest = "2"};
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
                    ModGroup.ModifierItems = InvApi.GetModifierItemsForItem(InvContext, ModGroup.ItemNumber);
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
                    InventoryItem dressing = InvApi.GetItem(InvContext, ModGroup.ModifierItems[Convert.ToInt32(answer)-1].ItemNumber);
                    decimal Price = 0;
                    if (ModGroup.Charged == true) { Price = dressing.Price; }
                    inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = dressing.ItemName, ItemNumber = dressing.ItemNumber, Price = dressing.Price, Quantity = 1, State = EntityState.Added, Guest = "2" });
                }
                
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
                inv = api.SplitInvoiceByGuest(context,inv.InvoiceNumber);
                if (inv.NumberOfSplitChecks == 2)
                    Console.WriteLine("Split invoice by guest");
                else
                    Console.WriteLine("***ERROR*** Invoice could be split");



                // ApplyCashPayment - applying grand total minus 1 dollar (NOTE SPLITS Starts counting at 0 not 1)
                AppliedPaymentResponse payResponse = api.ApplyCashPayment(context, inv.InvoiceNumber, 0, inv.GrandTotalForSplit[0] -1);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied cash payment to split 1, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply payment");
                
                // ApplyCardPayment - applying remaining 1 dollar as a credit card
                payResponse = api.ApplyCardPayment(context, 
                    inv.InvoiceNumber, 
                    0,
                    new pcAmerica.DesktopPOS.API.Client.SalesService.PaymentResponse() 
                    { Amount = 1,
                        CardNumber = "4***********1",
                        ReferenceNumber = 123456, 
                        Result = true, 
                        TipAmount = 1, 
                        TransactionNumber = 1234 });
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied card payment to split 1, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply card payment");

                payResponse = api.ApplyCashPayment(context, inv.InvoiceNumber, 1, inv.GrandTotalForSplit[1] + 13);
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
                for (int i = 0; i < inv.NumberOfSplitChecks; i++)
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
                pcAmerica.DesktopPOS.API.Client.PaymentService.PaymentResponse response = api.ProcessCreditCard(request);
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

                Console.WriteLine(string.Format("The Non_Inventory item has Item Type of {0}",item.ItemType));

                List<ModifierGroup> modGroups = api.GetModiferGroupsForItem(context, "Non_Inventory");
                if (modGroups == null || modGroups.Count == 0)
                    Console.WriteLine("No modifier groups exist for the Non_Inventory item!");
                else
                    Console.WriteLine(String.Format("Found {0} modifier groups for the Non_Inventory item!", modGroups.Count));

                List<ModifierItem> modifiers = api.GetModifierItemsForItem(context, "Non_Inventory");
                if (modifiers == null || modifiers.Count == 0)
                    Console.WriteLine("No modifiers exist for the Non_Inventory item!");
                else
                    Console.WriteLine(String.Format("Found {0} modifiers for the Non_Inventory item!", modifiers.Count));

                if (item.KitItems == null || item.KitItems.Count == 0)
                    Console.WriteLine("The Non_Inventory has no Kit Items!");
                else
                    Console.WriteLine(String.Format("Found {0} Kit Item(s) for the Non_Inventory item!", item.KitItems.Count));

                InventoryItem kitTest = api.GetItem(context, "kit1");
                if(kitTest.KitItems == null || kitTest.KitItems.Count ==0)
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
                Console.WriteLine(String.Format("Sales totals between {0}-{1} -- NetSales:{2} TotalTax:{3} GrandTotal:{4}", startDateTime, endDateTime, totals.NetSales, totals.TotalTax, totals.GrandTotal));

                List<ItemSale> sales = api.GetItemsSold(startDateTime, endDateTime);
                Console.WriteLine(String.Format("Between {0}-{1}, there are {2} records of items being sold", startDateTime, endDateTime, sales.Count));

                pcAmerica.DesktopPOS.API.Client.SalesService.Context context = new pcAmerica.DesktopPOS.API.Client.SalesService.Context();
                context.CashierID = "100101";
                context.StoreID = "1001";
                context.StationID = "01";

                // StartNewInvoice - this also automatically locks an invoice so it can't be opened by a terminal
                Invoice inv = api.StartNewInvoice(context, "ROB" + DateTime.Now.Ticks.ToString());
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
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Non Inventory", ItemNumber = "Non_Inventory", Price = 1, Quantity = 1, State = EntityState.Added });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Non Inventory", ItemNumber = "Non_Inventory", Price = 2, Quantity = 1, State = EntityState.Added });
                inv.LineItems.Add(new LineItem() { Id = Guid.NewGuid(), ItemName = "Non Inventory", ItemNumber = "Non_Inventory", Price = 3, Quantity = 1, State = EntityState.Added, ParentId = inv.LineItems[1].Id });
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems new invoice value: {0}", inv.GrandTotal));
                inv.LineItems[0].State = EntityState.Deleted;
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems DELETED 1st item, new invoice value: {0}", inv.GrandTotal));
                inv.LineItems[0].Quantity = 2;
                inv.LineItems[0].State = EntityState.Modified;
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems CHANGED 1st item QUANTITY, new invoice value: {0}", inv.GrandTotal));
                inv.LineItems.Add(new LineItem() { ItemNumber = "Non_Inventory", ItemName = "Hot dog", Price = 1, Quantity = 1, State = EntityState.Added });
                inv = api.ModifyItems(context, inv.InvoiceNumber, inv.LineItems);
                Console.WriteLine(String.Format("ModifyItems ADDED item # 1, new invoice value: {0}", inv.GrandTotal));

                // SendToKitchen
                if (api.SendToKitchen(context, inv.InvoiceNumber))
                    Console.WriteLine("Invoice was printed in kitchen");
                else
                    Console.WriteLine("***ERROR*** Invoice was NOT printed in kitchen");

                if (api.SendToKitchen(context, inv.InvoiceNumber))
                    Console.WriteLine("Invoice was printed in kitchen, it should not have printed anything out the 2nd time");
                else
                    Console.WriteLine("***ERROR*** Invoice was NOT printed in kitchen");

                // Splitcheck
                inv = api.SplitInvoice(context, inv.InvoiceNumber, 2);
                if (inv.NumberOfSplitChecks == 2)
                    Console.WriteLine("Split invoice 2 ways");
                else
                    Console.WriteLine("***ERROR*** Invoice could be split");

                // CombineSplits
                inv = api.CombineSplits(context, inv.InvoiceNumber);
                if (inv.NumberOfSplitChecks == 0)
                    Console.WriteLine("Combined split checks");
                else
                    Console.WriteLine("***ERROR*** Invoice could be split");

                // GetAllOnHoldInvoices
                List<OnHoldInfo> onHoldInfos = api.GetAllOnHoldInvoices(context);
                if (onHoldInfos == null)
                    Console.WriteLine("***ERROR*** Could not retrieve GetAllOnHoldInvoices");
                else
                    Console.WriteLine(String.Format("Retrieved {0} OnHoldInfo from GetAllOnHoldInvoices", onHoldInfos.Count));

                // GetOnHoldInvoicesForCashier
                onHoldInfos = api.GetOnHoldInvoicesForCashier(context);
                if (onHoldInfos == null)
                    Console.WriteLine("***ERROR*** Could not retrieve GetOnHoldInvoicesForCashier");
                else
                    Console.WriteLine(String.Format("Retrieved {0} OnHoldInfo from GetOnHoldInvoicesForCashier", onHoldInfos.Count));

                // ApplyCashPayment - applying grand total minus 1 dollar
                AppliedPaymentResponse payResponse = api.ApplyCashPayment(context, inv.InvoiceNumber, -1, inv.GrandTotal -1);
                if (payResponse.Success)
                    Console.WriteLine(String.Format("Applied cash payment, change due {0}", payResponse.ChangeAmount));
                else
                    Console.WriteLine("***ERROR*** Could not apply payment");

                // ApplyCardPayment - applying remaining 1 dollar as a credit card
                payResponse = api.ApplyCardPayment(context, 
                    inv.InvoiceNumber, 
                    -1,
                    new pcAmerica.DesktopPOS.API.Client.SalesService.PaymentResponse() 
                    { Amount = 1,
                        CardNumber = "4***********1",
                        ReferenceNumber = 123456, 
                        Result = true, 
                        TipAmount = 1, 
                        TransactionNumber = 1234 });
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
                if (api.EmailReceipt(context, inv.InvoiceNumber, -1, "asdsadsad"))
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

                List<TableInfo> tables = api.GetTables(context);
                if (tables == null)
                    Console.WriteLine("***ERROR*** No tables were returned");
                else
                    Console.WriteLine(String.Format("There were {0} open invoices returned", tables.Count));
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
