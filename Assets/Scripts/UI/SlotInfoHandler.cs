using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotInfoHandler : MonoBehaviour
{
    #region Singleton
    public static SlotInfoHandler instance;
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
    #endregion


    // Prefab
    public GameObject slotInfoPrefab;

    // Offset
    public Vector3 infoOffset = new Vector3(0f, -100f, 0f);


    // Object
    private GameObject slotInfoObject;
    private SlotInfo slotInfo;

    /// <summary>
    /// Create a slot info from prefab.
    /// </summary>
    /// <param name="item">Item to get info.</param>
    public void CreateInfo(Item item, Transform slotTransform, Transform parent)
    {
        // If ther is an slot info object, destroy it
        if(slotInfoObject != null)
        {
            DestroyInfo();
        }

        Vector3 position = (slotTransform.position.y < 300f) ?
            slotTransform.position + new Vector3(infoOffset.x, -1 * infoOffset.y, infoOffset.z)
            :
            slotTransform.position + infoOffset;

        // Create slot info object
        slotInfoObject = Instantiate(slotInfoPrefab, slotTransform.position, Quaternion.identity);
        slotInfoObject.transform.SetParent(parent);
        slotInfoObject.GetComponent<RectTransform>().position = position;
        slotInfoObject.GetComponent<RectTransform>().localScale = new Vector3(1f, 1f, 1f);
        slotInfo = slotInfoObject.GetComponent<SlotInfo>();
        slotInfo.SetInfo(item);
    }

    public void DestroyInfo()
    {
        if(slotInfoObject == null)
        {
            return;
        }

        slotInfo.SetInfo();
        slotInfoObject = null;
        slotInfo = null;
    }
}
