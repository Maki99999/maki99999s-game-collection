using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace FalseTruth {
[RequireComponent(typeof(Text))]
public class LocalizationText : MonoBehaviour {

	private Text text;

    public string localizationKey;

	void Start() {
		text = GetComponent<Text>();
		UpdateLocale();
	}
	
	public void UpdateLocale() {
		if (!text) return; // catching race condition
		if (!System.String.IsNullOrEmpty(localizationKey) && FalseTruth.Localization.CurrentLanguageStrings.ContainsKey(localizationKey))
			text.text = FalseTruth.Localization.CurrentLanguageStrings[localizationKey].Replace(@"\n", "" + '\n'); ;
	}
}
}