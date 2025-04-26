using UnityEngine;
public enum ItemType
{
    None,
    LeafPad,
    Other
}
public class PlayerInventory : MonoBehaviour
{
    public ItemType heldItem = ItemType.None;

    public bool CanPickUp(ItemType itemType)
    {
        return heldItem == ItemType.None || heldItem != itemType;
    }

    public void PickUpItem(ItemType itemType)
    {
        if (CanPickUp(itemType))
        {
            heldItem = itemType;
        }
    }
    /*public void DropItem()
    {
        heldItem = ItemType.None;
    }*/
}
