namespace pcAmerica.DesktopPOS.API.Client
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using pcAmerica.DesktopPOS.API.Client.TableService;

    /// <summary>
    /// TODO: Update summary.
    /// </summary>
    public class TableAPI
    {
        /// <summary>
        /// Returns a list of all tables and on hold invoices in the store/restaurant 
        /// delivery invoices will have a sectionID of XXDELIVERY
        /// takeout invoices will have a sectionID of XXTAKEOUT
        /// tabs not associated with tables will have a section ID of XXOPEN TABS
        /// invoices that are at table will have the section ID of the resturant section that the table diagram screen lists them in(i.e. Bar, Dining Room, ext)
        /// </summary>
        /// <param name="context">The store id, station id, and cashier id the information should be restricted to.</param>
        /// <returns>A list of information about the open invoices, occupied status, cashier assigned, dollar value, etc.</returns>
        public List<TableInfo> GetAllTablesAndOpenInvoices(Context context)
        {
            using (TableServiceClient client = new TableServiceClient())
            {
                client.Open();
                return new List<TableInfo>(client.GetAllTablesAndOpenInvoices(context));
            }
        }
    }
}
