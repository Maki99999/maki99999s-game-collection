using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class LocalizationScript : MonoBehaviour {

	void Start() {
		if(!FalseTruth.Localization.currentLanguageHasBeenSet) {
            FalseTruth.Localization.currentLanguageHasBeenSet = true;

            SetCurrentLanguage(FalseTruth.Localization.PlayerLanguage);
         }
	}

	public static string GetLocalizedString(string key) {
		if (FalseTruth.Localization.CurrentLanguageStrings.ContainsKey(key))
			return FalseTruth.Localization.CurrentLanguageStrings[key];
		else
			return string.Empty;
	}

	public static string Get(string key) {
		return GetLocalizedString(key);
	}

	/*public static string[] GetMultiple(string key) {
		List<string> stringList = new List<string>();
		for(int i = 0;; i++) {
			string localString = GetLocalizedString(key + i);
			if(localString != string.Empty) {
				stringList.Add(localString);
			} else {
				break;
			}
		}
		return stringList.ToArray();
	}*/

	public static void SetCurrentLanguage(SystemLanguage language) {
		FalseTruth.Localization.CurrentLanguage = language.ToString();
		FalseTruth.Localization.PlayerLanguage = language;
		FalseTruth.LocalizationText[] allTexts = GameObject.FindObjectsOfType<FalseTruth.LocalizationText>();
		foreach(FalseTruth.LocalizationText aText in allTexts)
			aText.UpdateLocale();
	}
}
}