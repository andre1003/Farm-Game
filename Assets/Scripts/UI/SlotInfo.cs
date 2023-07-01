using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine.Localization;
using UnityEngine;
using UnityEngine.Localization.Components;

public class SlotInfo : MonoBehaviour
{
    // UI
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public LocalizeStringEvent descriptionLocalizer;
    public CanvasGroup group;

    // Timer
    public float transitionTime = 0.25f;

    // Item
    public Item item;


    /// <summary>
    /// Set item information.
    /// </summary>
    /// <param name="item">Item reference.</param>
    public void SetInfo(Item item = null)
    {
        if(item != null)
        {
            // Set item reference
            this.item = item;

            // Set name and description texts
            nameText.text = StringLocalizer.instance.Localize(item.nameLocalization, item.name);

            // Set table entry and refresh string
            string entry = (item.GetType() == typeof(Plant)) ? "Plant Description" : "Item Description";
            descriptionLocalizer.SetEntry(entry);
            descriptionLocalizer.RefreshString();
        }

        // Update canvas opacity
        StartCoroutine(UpdateCanvasOpacity());
    }

    /// <summary>
    /// Update canvas opacity.
    /// </summary>
    private IEnumerator UpdateCanvasOpacity()
    {
        // Define elapsed time and set it to 0
        float elapsedTime = 0f;

        // Define initial and target alpha, based on canvas opacity
        float initialAlpha = group.alpha;
        float targetAlpha = (initialAlpha == 0f) ? 1f : 0f;

        // Update group alpha
        while (elapsedTime < transitionTime)
        {
            elapsedTime += Time.deltaTime;
            group.alpha = Mathf.Lerp(initialAlpha, targetAlpha, (elapsedTime / transitionTime));
            yield return null;
        }

        // Make sure group alpha is equal target alpha
        group.alpha = targetAlpha;

        // If target alpha is 0, destroy this object
        if(targetAlpha == 0f)
        {
            Destroy(gameObject);
        }
    }
}
