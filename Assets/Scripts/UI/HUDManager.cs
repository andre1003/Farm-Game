using UnityEngine;


public class HUDManager : MonoBehaviour
{
    #region Singleton
    public static HUDManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion

    // HUD
    public GameObject hudCanvas;

    // Transition
    public float transitionLength = 0.25f;


    // HUD
    private CanvasGroup canvasGroup;
    private bool changeHUD = false;

    // Transition
    private float initialAlpha;
    private float targetAlpha;
    private float elapsedTime = 0f;
    

    // Start method
    void Start()
    {
        canvasGroup = hudCanvas.GetComponent<CanvasGroup>();
    }

    // Update method
    void Update()
    {
        // If doesn't need to change HUD visibility, exit
        if(!changeHUD)
        {
            return;
        }

        // Change HUD opacity over transition length seconds
        canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, (elapsedTime / transitionLength));
        elapsedTime += Time.deltaTime;

        // If HUD opacity is the target opacity, reset elapsed time and set change HUD controller to false
        if(canvasGroup.alpha == targetAlpha)
        {
            elapsedTime = 0f;
            changeHUD = false;
        }
    }

    /// <summary>
    /// Hide or display HUD.
    /// </summary>
    /// <param name="isActive">New HUD visibility.</param>
    public void SetHUD(bool isActive)
    {
        // Set initial opacity
        initialAlpha = canvasGroup.alpha;

        // If the target is to display HUD, set target opacity to 1
        if(isActive)
        {
            targetAlpha = 1f;
        }

        // If the target is to hide HUD, set target opacity to 0
        else
        {
            targetAlpha = 0f;
        }

        // Set change HUD controller to true
        changeHUD = true;
    }
}
