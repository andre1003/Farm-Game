using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    // Images
    public Image icon;
    public Image amountImage;

    // Text
    public Text text;


    // Plant
    private Item item;


    /// <summary>
    /// Add a certain type of plant to the inventory slot.
    /// </summary>
    /// <param name="item">Item to be added to inventory slot.</param>
    /// <param name="amount">Amount of plant to be added.</param>
    public void AddItem(Item item, int amount)
    {
        this.item = item;

        icon.sprite = item.icon;
        amountImage.enabled = true;
        icon.enabled = true;

        text.enabled = true;
        text.text = amount.ToString();
    }

    /// <summary>
    /// Clear the slot.
    /// </summary>
    public void ClearSlot()
    {
        item = null;

        icon.sprite = null;
        icon.enabled = false;
        amountImage.enabled = false;

        text.enabled = false;
    }

    /// <summary>
    /// Plant the selected item in inventory at the selected plantation zone.
    /// </summary>
    public void Plant()
    {
        GameObject plantationZone = InGameSaves.GetPlantationZone();
        if(item != null && plantationZone != null && InventoryUI.instance.GetTab() == 0)
        {
            InventoryUI.instance.SetUI(false);
            HUDManager.instance.SetHUD(true);
            plantationZone.GetComponent<PlantationController>().Plant((Plant)item);
        }
    }
}
