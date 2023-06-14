using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using UnityEngine.Localization.Tables;


public class StringLocalizer : MonoBehaviour
{
    #region Singleton
    public static StringLocalizer instance;

    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }

        
    }
    #endregion

    // Language options
    public List<string> options;


    /// <summary>
    /// Localize a string, based on table name and key.
    /// It also updates the text UI.
    /// </summary>
    /// <param name="tableName">Table to search the localization.</param>
    /// <param name="key">Key of localization string.</param>
    /// <param name="text">UI text to be updated.</param>
    public void Localize(string tableName, string key, TMPro.TextMeshProUGUI text)
    {
        StartCoroutine(GetLocalizedString(tableName, key, text));
    }

    /// <summary>
    /// Get the localized string and update the text UI.
    /// </summary>
    /// <param name="tableName">Table to search the localization.</param>
    /// <param name="key">Key of localization string.</param>
    /// <param name="text">UI text to be updated.</param>
    private IEnumerator GetLocalizedString(string tableName, string key, TMPro.TextMeshProUGUI text)
    {
        // Get the string table with name tableName
        LocalizedStringTable _stringTable = new LocalizedStringTable { TableReference = tableName };
        var tableLoading = _stringTable.GetTable();
        yield return tableLoading;

        // When table is loaded, get the localized value
        StringTable currentStringTable = tableLoading;
        text.text = currentStringTable[key].LocalizedValue;
    }

    /// <summary>
    /// Get all avalilable locales for the game.
    /// </summary>
    public void GetLocales()
    {
        options = new List<string>();
        StartCoroutine(StartGettingLocales());
    }

    /// <summary>
    /// Wait localization settings initialization and get all available locales.
    /// </summary>
    private IEnumerator StartGettingLocales()
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;

        // Generate list of available Locales
        options = new List<string>();
        for(int i = 0; i < LocalizationSettings.AvailableLocales.Locales.Count; ++i)
        {
            Locale locale = LocalizationSettings.AvailableLocales.Locales[i];
            options.Add(locale.Identifier.CultureInfo.NativeName);
        }

    }

    /// <summary>
    /// Change the game language.
    /// </summary>
    /// <param name="index">Locale index.</param>
    public void ChangeLanguage(int index)
    {
        StartCoroutine(ChangeLanguageAsync(index));
    }

    /// <summary>
    /// Wait localization settings initialization and change language.
    /// </summary>
    /// <param name="index">Locale index.</param>
    private IEnumerator ChangeLanguageAsync(int index)
    {
        // Wait for the localization system to initialize
        yield return LocalizationSettings.InitializationOperation;
        LocalizationSettings.SelectedLocale = LocalizationSettings.AvailableLocales.Locales[index];
    }
}
