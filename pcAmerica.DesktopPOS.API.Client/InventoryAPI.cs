using System.Collections.Generic;
using pcAmerica.DesktopPOS.API.Client.InventoryService;

namespace pcAmerica.DesktopPOS.API.Client
{
    public class InventoryAPI
    {
        /// <summary>
        /// Retrieves a list of all inventory items
        /// </summary>
        /// <returns>A list of inventory items</returns>
        public List<InventoryItem> GetItemList()
        {
            using (var client = new InventoryServiceClient())
            {
                client.Open();
                return new List<InventoryItem>(client.GetItemList());
            }
        }

        /// <summary>
        /// Retrieves information about a specific item, including nested modifier groups and modifiers
        /// </summary>
        /// <param name="context">The store id, station id, and cashier id the information should be restricted to.</param>
        /// <param name="itemNumber">The item number to retrieve info for</param>
        /// <returns>An inventory item</returns>
        public InventoryItem GetItem(Context context, string itemNumber)
        {
            using (var client = new InventoryServiceClient())
            {
                client.Open();
                return client.GetItem(context, itemNumber);
            }
        }

        /// <summary>
        /// Retrieves more properties of an item than GetItemList()
        /// </summary>
        /// <returns>A list of inventory items</returns>
        public List<InventoryItem> GetItemListExtended(Context context)
        {
            using (var client = new InventoryServiceClient())
            {
                client.Open();
                return new List<InventoryItem>(client.GetItemListExtended(context));
            }
        }

        /// <summary>
        /// Retrieves the modifier groups associated with the specified item
        /// </summary>
        /// <param name="context">The store id, station id, and cashier id the information should be restricted to.</param>
        /// <param name="itemNumber">The item number to retrieve info for</param>
        /// <returns>A list of ModifierGroups for the item number</returns>
        public List<ModifierGroup> GetModiferGroupsForItem(Context context, string itemNumber)
        {
            using (var client = new InventoryServiceClient())
            {
                client.Open();
                return new List<ModifierGroup>(client.GetModifierGroupsForItem(context, itemNumber));
            }
        }

        /// <summary>
        /// Retrieves the individual modifiers associated with the specified item (modifiers that are not in groups)
        /// </summary>
        /// <param name="context">The store id, station id, and cashier id the information should be restricted to.</param>
        /// <param name="itemNumber">The item number to retrieve info for</param>
        /// <returns>A list of ModifierItems for the item number</returns>
        public List<ModifierItem> GetIndividualModifiers(Context context, string itemNumber)
        {
            using (var client = new InventoryServiceClient())
            {
                client.Open();
                return new List<ModifierItem>(client.GetIndividualModifiers(context, itemNumber));
            }
        }

        /// <summary>
        /// Retrieves the individual modifiers associated with the specified Modifier Group
        /// </summary>
        /// <param name="context">The store id, station id, and cashier id the information should be restricted to.</param>
        /// <param name="itemNumber">The item number to retrieve info for</param>
        /// <returns>A list of ModifierItems for the item number</returns>
        public List<ModifierItem> GetModifierItemsForModifierGroup(Context context, string itemNumber)
        {
            using (var client = new InventoryServiceClient())
            {
                client.Open();
                return new List<ModifierItem>(client.GetModifierItemsForModifierGroup(context, itemNumber));
            }
        }
    }
}