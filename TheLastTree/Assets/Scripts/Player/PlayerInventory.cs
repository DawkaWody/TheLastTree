using System.Collections.Generic;
using UnityEngine;
public enum ItemType
{
    None,
    LeafPad,
    Water,
    TreeSap,
    Other
}
public class PlayerInventory : MonoBehaviour
{
    private Dictionary<ItemType, MonoBehaviour> heldItems = new();

    private LeafPad heldLeafPad;

    private MonoBehaviour heldItemObject;
    public bool CanPickUp(ItemType itemType)
    {
        return !heldItems.ContainsKey(itemType);
    }

    public void PickUpItem(ItemType itemType, MonoBehaviour itemInstance)
    {
        if (CanPickUp(itemType))
        {
            heldItems[itemType] = itemInstance;
        }
    }

    public T GetHeldItem<T>(ItemType itemType) where T : MonoBehaviour
    {
        if (heldItems.TryGetValue(itemType, out MonoBehaviour item))
        {
            return item as T;
        }
        return null;
    }

    public void ClearHeldItem(ItemType itemType)
    {
        if (heldItems.ContainsKey(itemType))
        {
            heldItems.Remove(itemType);
        }
    }

    public bool HasItem(ItemType itemType)
    {
        return heldItems.ContainsKey(itemType);
    }
}
