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
        /// </summary>
        /// <param name="context">The store id, station id, and cashier id the information should be restricted to.</param>
        /// <returns>A list of information about the open invoices, occupied status, cashier assigned, dollar value, etc.</returns>
        public List<TableInfo> GetTables(Context context)
        {
            using (TableServiceClient client = new TableServiceClient())
            {
                client.Open();
                return new List<TableInfo>(client.GetTables(context));
            }
        }
    }
}
