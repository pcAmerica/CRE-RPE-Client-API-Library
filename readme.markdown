pcAmerica DesktopPOS API Client
===============================
Basic workflow
--------------
###Authenticate an employee
Verify the employee's username/password using the EmployeeAPI.AuthenticateEmployee
###Display the list of on hold invoices for the current employee
Populate a Context object with the configured Store ID, Station ID, and Cashier ID (Employee ID)
Call the SalesAPI.GetOnHoldInvoicesForCashier to get a list of invoices for this employee
Decide to open an existing invoice, or to create a new one.
###Open an existing invoice
Call the SalesAPI.GetInvoice method with the Context, and invoice number.
###Start a new invoice
Call the SalesAPI.StartNewInvoice method with the Context and an OnHoldID. 
The OnHoldID can be any 12 character unique identifier, usually the customer name, or table number
This will create a new invoice, put it on hold in the database, and will "lock" it so no other device/computer can modify it.
###Display the menu
Call the MenuAPI.GetCurrentMenu to retrieve the currently configured menu for this device. The menu will contain all of the departments and items,
along with position, color, and visibility of each one.
The ID of an item button will be the Item Number that needs to be sent to be added to an invoice
###Modify an invoice
Either build a list of line items, or modify the existing list to add items to the invoice. Parent items should be added, followed immediately 
after by their child (modifier) items.
Batches of item modifications can be sent together. It's not necessary to send 1 add item request at a time.

Use the LineItem.EntityState to direct how the server should handle the modifications.

+ *Added* = new line item that should be added to the invoice
+ *Deleted* = line item should be removed from the invoice
+ *Existing* = line item will not be modified
+ *Modified* = the line item's price or quantity will be changed

###Send the ordered items to the kitchen printer
Call the SalesAPI.SendToKitchen method to send the new/modified items to the kitchen.
If you're finished working with this invoice, call the SalesAPI.UnLockInvoice method to make it available for use on other devices.
###Adding payments
Call the SalesAPI.ApplyCardPayment or SalesAPI.ApplyCashPayment to add enough money to the invoice to match the grand total.
Cash payments can return a change due, overpaying with a credit card will record the overpayment amount as a tip.
###Completing an invoice
When an invoice is fully paid, call the SalesAPI.EndInvoice method to record the invoice as being completed (and can no longer be modified).
Completing an invoice will also send it to the kitchen one final time in case there are items remaining to be prepared.
