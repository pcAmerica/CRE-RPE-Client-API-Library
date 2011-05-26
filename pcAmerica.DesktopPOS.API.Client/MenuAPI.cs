using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.MenuService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class MenuAPI
    {
        public Menu GetCurrentMenu(Context context)
        {
            using (MenuServiceClient client = new MenuServiceClient())
            {
                client.Open();
                return client.GetCurrentMenu(context);
            }
        }
    }
}
