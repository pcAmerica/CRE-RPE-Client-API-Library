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
            TestCreditCard();
            TestCustomers();
            TestEmployee();
            TestInventory();
            TestSales();
            TestMenus();
            TestTables();
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

                pcAmerica.DesktopPOS.API.Client.PaymentService.PaymentResponse response = PaymentAPI.ProcessCreditCard(request);
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
                inv.LineItems.Add(new LineItem() { ItemNumber = "1", ItemName = "Hot dog", Price = 1, Quantity = 1, State = EntityState.Added });
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
                if (api.SplitInvoice(context, inv.InvoiceNumber, 2))
                    Console.WriteLine("Split invoice 2 ways");
                else
                    Console.WriteLine("***ERROR*** Invoice could be split");

                // CombineSplits
                if (api.CombineSplits(context, inv.InvoiceNumber))
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

                //TODO: ApplyCardPayment

                //TODO: CompleteTransaction

                // PrintReceipt
                if (api.PrintReceipt(context, inv.InvoiceNumber))
                    Console.WriteLine("Receipt was printed");
                else
                    Console.WriteLine("***ERROR*** Receive was NOT printed");

                // PrintReceipt - providing -1 for a split check prints the main check
                if (api.PrintReceiptForSplitCheck(context, inv.InvoiceNumber, -1))
                    Console.WriteLine("Receipt was printed");
                else
                    Console.WriteLine("***ERROR*** Receive was NOT printed");

                // EmailReceipt
                if (api.EmailReceipt(context, inv.InvoiceNumber, "asdsadsa"))
                    Console.WriteLine("Receipt was emailed");
                else
                    Console.WriteLine("***ERROR*** Receipt was NOT emailed");

                // EmailReceipt - providing -1 for a split check prints the main check
                if (api.EmailReceiptForSplitCheck(context, inv.InvoiceNumber, -1, "asdsadsa"))
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
                    Console.WriteLine(String.Format("Menu contains {0} departments", menu.Departments.Count));
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
