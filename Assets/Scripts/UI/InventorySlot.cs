using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class InventorySlot : MonoBehaviour
{
    // Images
    public Image icon;
    public Image amountImage;

    // Text
    public TextMeshProUGUI text;


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
        // Get plantation zone
        GameObject plantationZone = InGameSaves.GetPlantationZone();
        if(item != null && plantationZone != null && InventoryUI.instance.GetTab() == 0)
        {
            InventoryUI.instance.SetUI(false);
            HUDManager.instance.SetHUD(true);

            // Set temporary plant on player movement controller and get plantation controller
            MovementController.instance.SetTempPlant((Plant)item);
            PlantationController controller = plantationZone.GetComponent<PlantationController>();

            // Move player to front spot to plant
            MovementController.instance.SendTo(controller.front.position, controller.gameObject.transform);
        }

        // Stop hovering slot UI
        StopHoverSlot();
    }

    /// <summary>
    /// Hover slot.
    /// </summary>
    public void HoverSlot()
    {
        // Change mouse cursor
        ObjectsManager.instance.HoverUI();
        SlotInfoHandler.instance.CreateInfo(item, transform, transform.parent.parent);
    }

    /// <summary>
    /// Stop hover slot.
    /// </summary>
    public void StopHoverSlot()
    {
        ObjectsManager.instance.StopHoverUI();
        SlotInfoHandler.instance.DestroyInfo();
    }
}
