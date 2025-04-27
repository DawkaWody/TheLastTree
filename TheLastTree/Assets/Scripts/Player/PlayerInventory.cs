using UnityEngine;
public enum ItemType
{
    None,
    LeafPad,
    Other
}
public class PlayerInventory : MonoBehaviour
{
    public ItemType heldItem;

    private LeafPad heldLeafPad;

    private MonoBehaviour heldItemObject;
    public bool CanPickUp(ItemType itemType)
    {
        return heldItem == ItemType.None || heldItem != itemType;
    }

    public void PickUpItem(ItemType itemType, MonoBehaviour itemInstance)
    {
        if (CanPickUp(itemType))
        {
            heldItem = itemType;
            heldItemObject = itemInstance;
        }
    }

    public T GetHeldItem<T>() where T : MonoBehaviour
    {
        return heldItemObject as T;
    }

    public void ClearHeldItem()
    {
        heldItem = ItemType.None;
        heldItemObject = null;
    }
    /*public void DropItem()
    {
        heldItem = ItemType.None;
    }*/
}
