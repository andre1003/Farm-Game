using System.Collections;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Tables;
using TMPro;

public class SeasonsLocalizedStrings : MonoBehaviour {
    public string tableName;
    public string season;
    public TextMeshProUGUI seasonText;


    // Reference to the localized string table
    [SerializeField] private LocalizedStringTable _localizedStringTable;

    private StringTable _currentStringTable;

    void Update() {
        UpdateSeason();
    }

    public void UpdateSeason() {
        StartCoroutine(GetSeasonTranslation());
    }

    private IEnumerator GetSeasonTranslation() {
        /* See https://phrase.com/blog/posts/localizing-unity-games-official-localization-package/
         * for more details about localized strings via C# with Unity
         */

        // Wait for the table to load asynchronously
        var tableLoading = _localizedStringTable.GetTable();
        yield return tableLoading;

        _currentStringTable = tableLoading;

        // At this point _currentStringTable can be used to
        // access our strings

        // Retrieve the localized string     
        season = _currentStringTable[tableName].LocalizedValue;

        seasonText.text = season;
    }
}