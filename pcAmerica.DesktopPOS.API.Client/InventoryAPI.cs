using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using pcAmerica.DesktopPOS.API.Client.InventoryService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class InventoryAPI
    {
        public List<InventoryItem> GetItemList()
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return new List<InventoryItem>(client.GetItemList());
            }
        }
        public InventoryItem GetItem(Context context, string itemNumber)
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return client.GetItem(context, itemNumber);
            }
        }
        public List<InventoryItem> GetItemListExtended(Context context)
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return new List<InventoryItem>(client.GetItemListExtended(context));
            }
        }
        public List<ModifierGroup> GetModiferGroupsForItem(Context context, string itemNumber)
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return new List<ModifierGroup>(client.GetModifierGroupsForItem(context, itemNumber));
            }
        }
        public List<ModifierItem> GetModifierItemsForItem(Context context, string itemNumber)
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return new List<ModifierItem>(client.GetModifierItemsForItem(context, itemNumber));
            }
        }
    }
}
