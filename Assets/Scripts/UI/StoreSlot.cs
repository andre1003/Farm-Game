using TMPro;
using UnityEngine;
using UnityEngine.UI;


public class StoreSlot : MonoBehaviour
{
    // Slot icon
    public Image icon;

    // Slot add-ons
    public Image costImage;

    // Slot texts
    public TextMeshProUGUI costText;


    // Auxiliar variables
    private Item item;
    public int amount = -1;


    /// <summary>
    /// Add item to slot.
    /// </summary>
    /// <param name="item">Item reference.</param>
    public void AddItem(Item item)
    {
        // Plant setup
        this.item = item;
        icon.sprite = item.icon;

        // Enable add-ons
        costText.enabled = true;

        // Texts setup
        costText.text = item.buyValue.ToString("F2");

        // Enable icon, level and cost image
        icon.enabled = true;
        costImage.enabled = true;
    }

    /// <summary>
    /// Clear the slot.
    /// </summary>
    public void ClearSlot()
    {
        // Clear plant
        item = null;
        icon.sprite = null;

        // Disable images
        costImage.enabled = false;

        // Disable texts
        costText.enabled = false;

        // Disable icon
        icon.enabled = false;
    }

    /// <summary>
    /// Add an item to sell.
    /// </summary>
    /// <param name="item">Item reference.</param>
    /// <param name="amount">Amount.</param>
    public void AddItemToSell(Item item, int amount)
    {
        // Plant setup
        this.item = item;
        icon.sprite = item.icon;

        // Enable add-ons
        costText.enabled = true;

        // Display amount
        this.amount = amount;

        // Calculate and display sell value
        float cost;       
        if(item.GetType() == typeof(Plant))
        {
            cost = item.baseSellValue * (2f / (((Plant)item).seasons.IndexOf(TimeManager.instance.season) + 1));
        }
        else
        {
            cost = item.baseSellValue;
        }
        costText.text = cost.ToString("F2");

        // Enable all images
        icon.enabled = true;
        costImage.enabled = true;
    }

    /// <summary>
    /// Sell a certain amount of this plant.
    /// </summary>
    /// <param name="amount">Amount to sell.</param>
    public void SellAmount(int amount)
    {
        // Decrease amount
        this.amount -= amount;

        // If amount is less or equal to 0, clear the slot
        if(this.amount <= 0)
        {
            ClearSlot();
        }

        // Else, update the slot
        else
        {
            AddItemToSell(item, this.amount);
        }
    }

    /// <summary>
    /// Store handler.
    /// </summary>
    public void StoreActionHandler()
    {
        if(item != null)
        {
            Store.instance.SetSelectedItem(item);
            Store.instance.SetSelectedStoreSlot(this);
            StoreUI.instance.AmountSelectionMenu(true);
        }
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
