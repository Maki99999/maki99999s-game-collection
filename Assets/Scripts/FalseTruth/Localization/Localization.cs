using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public static class Localization {

        const string STR_LOCALIZATION_PREFIX = "Languages/";
        static string currentLanguage;
        public static bool currentLanguageHasBeenSet = false;
        public static Dictionary<string, string> CurrentLanguageStrings = new Dictionary<string, string>();
        static TextAsset currentLocalizationText;

        public static Dictionary<int, SystemLanguage> LangIntToSysLang = new Dictionary<int, SystemLanguage>() {{0, SystemLanguage.English}, {1, SystemLanguage.German}};

        public static string CurrentLanguage
        {
            get { return currentLanguage; }
            set
            {
                if (value != null && value.Trim() != string.Empty)
                {
                    currentLanguage = value;
                    currentLocalizationText = Resources.Load(STR_LOCALIZATION_PREFIX + currentLanguage, typeof(TextAsset)) as TextAsset;
                    if (currentLocalizationText == null)
                    {
                        Debug.LogWarningFormat("Missing locale '{0}', loading English.", currentLanguage);
                        currentLanguage = SystemLanguage.English.ToString();
                        currentLocalizationText = Resources.Load(STR_LOCALIZATION_PREFIX + currentLanguage, typeof(TextAsset)) as TextAsset;
                    }
                    if (currentLocalizationText != null)
                    {
                        // We split on newlines to retrieve the key pairss
                        string[] lines = currentLocalizationText.text.Split(new string[] { "\r\n", "\n\r", "\n" }, System.StringSplitOptions.RemoveEmptyEntries);
                        CurrentLanguageStrings.Clear();
                        for (int i = 0; i < lines.Length; i++)
                        {
                            string[] pairs = lines[i].Split(new char[] { '\t', '=' }, 2);
                            if (pairs.Length == 2)
                            {
                                CurrentLanguageStrings.Add(pairs[0].Trim(), pairs[1].Trim());

                                currentLanguageHasBeenSet = true;

                            } else {
                        		Debug.LogErrorFormat("Locale language file '{0}' has wrong format!", currentLanguage);

                                currentLanguageHasBeenSet = false;
							}
                        }
                    }
                    else
                    {
                        Debug.LogErrorFormat("Locale language '{0}' not found!", currentLanguage);

                        currentLanguageHasBeenSet = false;
                    }
                }
            }
        }

        public static SystemLanguage PlayerLanguage
        {
            get
            {
                int sl = PlayerPrefs.GetInt("language", -1);
                if(sl <= 0) {
                    return Application.systemLanguage;
                }
                return LangIntToSysLang[sl];
            }
            set
            {
                PlayerPrefs.SetInt("language", (int)value);
                PlayerPrefs.Save();
            }
        }
    }
}