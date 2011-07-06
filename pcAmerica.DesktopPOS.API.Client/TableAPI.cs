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
        public static List<TableInfo> GetTables(Context context)
        {
            using (TableServiceClient client = new TableServiceClient())
            {
                client.Open();
                return new List<TableInfo>(client.GetTables(context));
            }
        }
    }
}
