using pcAmerica.DesktopPOS.API.Client.MenuService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class MenuAPI
    {
        /// <summary>
        /// Returns the structure of the currently active menu. If the current menu never expires, the expiration will be DateTime.MinValue.
        /// The menu hierarchy begins with departments. Items are nested within departments. Both departments and items are represented as buttons.
        /// </summary>
        /// <param name="context">The store id, station id, and cashier id the information should be restricted to.</param>
        /// <returns>The menu structure.</returns>
        public Menu GetCurrentMenu(Context context)
        {
            using (var client = new MenuServiceClient())
            {
                client.Open();
                return client.GetCurrentMenu(context);
            }
        }
    }
}