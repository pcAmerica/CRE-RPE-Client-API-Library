using System;
using System.Collections.Generic;
using System.Text;
using pcAmerica.DesktopPOS.API.Client;
using pcAmerica.DesktopPOS.API.Client.PaymentService;
using pcAmerica.DesktopPOS.API.Client.CustomerService;
using pcAmerica.DesktopPOS.API.Client.EmployeeService;
using pcAmerica.DesktopPOS.API.Client.InventoryService;
using pcAmerica.DesktopPOS.API.Client.SalesService;

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

                PaymentResponse response = PaymentAPI.ProcessCreditCard(request);
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
                DateTime startDateTime = DateTime.Parse("1/1/2010");
                DateTime endDateTime = DateTime.Parse("12/31/2010");

                SalesTotals totals = SalesAPI.GetTotals(startDateTime, endDateTime);
                Console.WriteLine(String.Format("Sales totals between {0}-{1} -- NetSales:{2} TotalTax:{3} GrandTotal:{4}", startDateTime, endDateTime, totals.NetSales, totals.TotalTax, totals.GrandTotal));

                List<ItemSale> sales = SalesAPI.GetItemsSold(startDateTime, endDateTime);
                Console.WriteLine(String.Format("Between {0}-{1}, there are {2} records of items being sold", startDateTime, endDateTime, sales.Count));
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
