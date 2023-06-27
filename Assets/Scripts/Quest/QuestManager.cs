using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class QuestManager : MonoBehaviour
{
    #region Singleton
    public static QuestManager instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion


    // Transition
    public float transitionLength = 0.25f;


    // Questlines
    [Header("Questlines")]
    public List<Questline> questlines = new List<Questline>();
    public List<Questline> completedQuestlines = new List<Questline>();
    public int currentIndex = -1;

    // UI
    [Header("UI")]
    [SerializeField] private GameObject questCanvas;
    [SerializeField] private GameObject questlineCompletedCanvas;
    [SerializeField] private TextMeshProUGUI titleText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI shortDescriptionText;
    [SerializeField] private TextMeshProUGUI questlineNameText;
    private CanvasGroup canvasGroup;
    private bool changeUI = false;

    // Transition
    private float initialAlpha;
    private float targetAlpha;
    private float elapsedTime = 0f;


    // Start is called before the first frame update
    void Start()
    {
        // Disable quest canvas
        questCanvas.SetActive(false);

        // Get canvas group
        canvasGroup = questlineCompletedCanvas.GetComponent<CanvasGroup>();

        // Get questline list length
        int length = questlines.Count;

        // If there is no questline, exit
        if(length <= 0)
        {
            return;
        }

        // Star all questlines
        for(int i = 0; i < length; i++)
        {
            questlines[i].StartQuestline(i);
        }

        FocusOnQuestline("initial");
    }

    // Update is called once per frame
    void Update()
    {
        // Get questline list length
        int length = questlines.Count;

        // If there is no questline, exit
        if(length <= 0)
        {
            return;
        }

        // Update all queslines
        for(int i = 0; i < length; i++)
        {
            questlines[i].UpdateQuestline();
        }

        // If doesn't need to change HUD visibility, exit
        if(!changeUI)
        {
            return;
        }

        // Change UI opacity over transition length seconds
        canvasGroup.alpha = Mathf.Lerp(initialAlpha, targetAlpha, (elapsedTime / transitionLength));
        elapsedTime += Time.deltaTime;

        // If UI opacity is the target opacity, reset elapsed time and set change UI controller to false
        if(canvasGroup.alpha == targetAlpha)
        {
            elapsedTime = 0f;
            changeUI = false;

            // If target opacity is 0
            if(targetAlpha == 0f)
            {
                questlineCompletedCanvas.SetActive(false);
            }
        }
    }

    /// <summary>
    /// Update quest UI.
    /// </summary>
    /// <param name="index"></param>
    /// <param name="title">Quest title.</param>
    /// <param name="description">Quest description.</param>
    /// <param name="shortDescription">Quest short description.</param>
    /// <param name="isActive">Quest canvas active status.</param>
    public void UpdateUI(int index, string title = "", string description = "", string shortDescription = "", bool isActive = false)
    {

        // If the current index is not equal updated questline index, exit
        if(currentIndex != index)
        {
            Debug.Log("(Current index) " + currentIndex + " != " + index + " (Index)");
            questCanvas.SetActive(false);
            return;
        }

        questCanvas.SetActive(isActive);
        titleText.text = title;
        descriptionText.text = description;
        shortDescriptionText.text = shortDescription;
    }

    /// <summary>
    /// Show questline completed UI.
    /// </summary>
    /// <param name="questline">Questline name.</param>
    public void QuestlineCompleted(string questline)
    {
        questlineNameText.text = questline;
        questlineCompletedCanvas.SetActive(true);

        initialAlpha = canvasGroup.alpha;
        targetAlpha = 1f;
        changeUI = true;

        StartCoroutine(DeactivateCanvasDelay(5f));
    }

    /// <summary>
    /// Deactivate canvas after seconds.
    /// </summary>
    /// <param name="seconds">Seconds to wait.</param>
    /// <param name="canvas">Canvas to deactivate.</param>
    private IEnumerator DeactivateCanvasDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        initialAlpha = canvasGroup.alpha;
        targetAlpha = 0f;
        changeUI = true;
    }



    /// <summary>
    /// Set the focus questline to the desired questline.
    /// </summary>
    /// <param name="questlineId">Questline ID.</param>
    public void FocusOnQuestline(string questlineId)
    {
        // Try to get questline index and if it is invalid, exti
        int index = GetQuestlineIndex(questlineId);
        if(index == -1)
        {
            return;
        }

        // Set current index to the requested questline
        currentIndex = index;
        questlines[currentIndex].GetInfo();
    }

    /// <summary>
    /// Try to get questline index, based on questline ID.
    /// </summary>
    /// <param name="questlineId">Questline ID.</param>
    /// <returns>Index of questline. It returns -1 if questline not found.</returns>
    private int GetQuestlineIndex(string questlineId)
    {
        // Set questline ID to lower
        questlineId = questlineId.ToLower();

        // Loop all questlines, searching for questline ID
        for(int i = 0; i < questlines.Count; i++)
        {
            // If questline found, return it's index
            if(questlines[i].id.ToLower() == questlineId)
            {
                return i;
            }
        }

        // If questline NOT found, return -1
        return -1;
    }


    /// <summary>
    /// Add a new questline to questline manager.
    /// </summary>
    /// <param name="questline">New questline.</param>
    public void AddQuestline(Questline questline)
    {
        questlines.Add(questline);
    }

    /// <summary>
    /// Remove a questline from list.
    /// </summary>
    /// <param name="index">Index of questline to remove.</param>
    public void RemoveQuestline(int index)
    {
        // If index is invalid, exit
        if(index < 0 || index >= questlines.Count)
        {
            return;
        }

        // Add questline to completed questline list
        completedQuestlines.Add(questlines[index]);

        // Update current index and UI
        currentIndex = -1;
        UpdateUI(currentIndex);

        // Remove questline and update indexes
        questlines.RemoveAt(index);
        UpdateIndexes();
    }

    /// <summary>
    /// Update questlines index.
    /// </summary>
    private void UpdateIndexes()
    {
        for(int i = 0; i < questlines.Count; i++)
        {
            questlines[i].SetIndex(i);
        }
    }
}
