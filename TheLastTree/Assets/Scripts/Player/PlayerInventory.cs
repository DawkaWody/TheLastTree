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
    private SpriteRenderer _spriteRenderer;
    void Start()
    {
        _spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public bool CanPickUp(ItemType itemType)
    {
        return heldItem == ItemType.None || heldItem != itemType;
    }

    public void PickUpItem(ItemType itemType)
    {
        if (CanPickUp(itemType))
        {
            heldItem = itemType;
            if (itemType == ItemType.LeafPad && _spriteRenderer != null) _spriteRenderer.sortingOrder = 1;
        }
    }
    /*public void DropItem()
    {
        heldItem = ItemType.None;
    }*/
}
