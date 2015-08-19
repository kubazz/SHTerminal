using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using SmartLocalization;
using InControl;

public class LocalizationManager {
    static private LocalizationManager instance;

    public static LocalizationManager Instance
    {
        get
        {
            if (instance == null)
                instance = new LocalizationManager();

            return instance; 
        }

    }
    private Dictionary<string, string> currentLanguageValues;
    private List<SmartCultureInfo> availableLanguages;
    private LanguageManager languageManager;

    public LocalizationManager()
    {
        languageManager = LanguageManager.Instance;

        SmartCultureInfo systemLanguage = languageManager.GetSupportedSystemLanguage();
        if (systemLanguage != null)
        {
            languageManager.ChangeLanguage(systemLanguage);
        }

        if (languageManager.NumberOfSupportedLanguages > 0)
        {
            currentLanguageValues = languageManager.RawTextDatabase;
            availableLanguages = languageManager.GetSupportedLanguages();
        }
        else
        {
            Debug.LogError("No languages are created!, Open the Smart Localization plugin at Window->Smart Localization and create your language!");
        }

        LanguageManager.Instance.OnChangeLanguage += OnLanguageChanged;
    }

    void OnLanguageChanged(LanguageManager languageManager)
    {
        currentLanguageValues = languageManager.RawTextDatabase;
    }

    public string GetLocalized(string key)
    {
        if (key.Contains("INPUT"))
        {
            if (InputManager.ActiveDevice.Name == "Keyboard/Mouse")
                key += "_KEYMOUSE";
            else
                key += "_GAMEPAD";
        }

        if (languageManager.HasKey(key))
            return languageManager.GetTextValue(key);
        else
            return key;
    }

    private Vector2 valuesScrollPosition = Vector2.zero;
    private Vector2 languagesScrollPosition = Vector2.zero;
    void OnGUI()
    {
        if (languageManager.NumberOfSupportedLanguages > 0)
        {
            if (languageManager.CurrentlyLoadedCulture != null)
            {
                GUILayout.Label("Current Language:" + languageManager.CurrentlyLoadedCulture.ToString());
            }

            GUILayout.BeginHorizontal();
            GUILayout.Label("Keys:", GUILayout.Width(460));
            GUILayout.Label("Values:", GUILayout.Width(460));
            GUILayout.EndHorizontal();

            valuesScrollPosition = GUILayout.BeginScrollView(valuesScrollPosition);
            foreach (KeyValuePair<string, string> languageValue in currentLanguageValues)
            {
                GUILayout.BeginHorizontal();
                GUILayout.Label(languageValue.Key, GUILayout.Width(460));
                GUILayout.Label(languageValue.Value, GUILayout.Width(460));
                GUILayout.EndHorizontal();
            }
            GUILayout.EndScrollView();

            languagesScrollPosition = GUILayout.BeginScrollView(languagesScrollPosition);
            foreach (SmartCultureInfo language in availableLanguages)
            {
                if (GUILayout.Button(language.nativeName, GUILayout.Width(960)))
                {
                    languageManager.ChangeLanguage(language);
                }
            }

            GUILayout.EndScrollView();
        }
    }
}
