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
        public InventoryItem GetItem(string itemNumber)
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return client.GetItem(itemNumber);
            }
        }
        public List<InventoryItem> GetItemListExtended()
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return new List<InventoryItem>(client.GetItemListExtended());
            }
        }
        public List<ModifierGroup> GetModiferGroupsForItem(string itemNumber)
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return new List<ModifierGroup>(client.GetModifierGroupsForItem(itemNumber));
            }
        }
        public List<ModifierItem> GetModifierItemsForItem(string itemNumber)
        {
            using (InventoryServiceClient client = new InventoryServiceClient())
            {
                client.Open();
                return new List<ModifierItem>(client.GetModifierItemsForItem(itemNumber));
            }
        }
    }
}
