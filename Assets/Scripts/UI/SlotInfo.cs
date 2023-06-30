using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class SlotInfo : MonoBehaviour
{
    // UI
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public CanvasGroup group;

    // Timer
    public float transitionTime = 0.25f;


    /// <summary>
    /// Set item information.
    /// </summary>
    /// <param name="item">Item reference.</param>
    public void SetInfo(Item item = null)
    {
        if(item != null)
        {
            // Set name and description texts
            nameText.text = StringLocalizer.instance.Localize(item.nameLocalization, item.name);
            descriptionText.text = StringLocalizer.instance.Localize(item.descriptionLocalization, item.name);
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
