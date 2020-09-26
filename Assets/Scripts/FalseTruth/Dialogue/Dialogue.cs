using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FalseTruth {
public class Dialogue {

	public TextElement firstText;
	public TextElement lastText;

	bool hasChoice = false;

	public Dialogue() {
		firstText = new TextElement();
	}

	public Dialogue(TextElement textElement) {
		firstText = lastText = textElement;
	}

	public Dialogue(string text) {
		firstText = lastText = new TextElement(FalseTruth.LocalizationScript.Get("You"), text);
	}

	public void Add(TextElement textElement) {
		if(hasChoice) {
			Debug.LogError("Tried to add Text to Choice.");
			return;
		}
		if(firstText.name == null) {
			firstText = lastText = textElement;
		} else {
			lastText.nextTexts.Add(textElement);
			lastText = textElement;
		}
	}

	public void Add(string name, string text) {
		Add(new TextElement(name, text));
	}

	public void Add(string text) {
		Add(new TextElement(FalseTruth.LocalizationScript.Get("You"), text));
	}

	public void Add(string text, bool italic) {
		TextElement newElement = new TextElement(FalseTruth.LocalizationScript.Get("You"), text);
		newElement.italic = italic;
		Add(newElement);
	}

	public void Add(string[] texts) {
		foreach(string text in texts) {
			Add(new TextElement(FalseTruth.LocalizationScript.Get("You"), text));
		}
	}

	public void AddChoices(List<FalseTruth.Dialogue> dialogues, List<string> choiceTexts) {
		hasChoice = true;
		foreach(FalseTruth.Dialogue dialogue in dialogues) {
			lastText.nextTexts.Add(dialogue.firstText);
		}
		lastText.choiceTexts = choiceTexts;
	}
}

public class TextElement {

	public string name;
	public string text;

	public List<TextElement> nextTexts;
	public List<string> choiceTexts;

	public bool italic = false;

	public TextElement() {
		nextTexts = new List<TextElement>();
		choiceTexts = new List<string>();
	}

	public TextElement(string name, string text) {
		this.name = name;
		this.text = text;

		nextTexts = new List<TextElement>();
		choiceTexts = new List<string>();
	}

	public TextElement(string[] names, string[] texts) {
		TextElement curr = this;
		curr.name = names[0];
		curr.text = texts[0];

		for(int i = 1; i < texts.Length; i++) {
			TextElement newElement = new TextElement();
			curr.nextTexts.Add(newElement);
			curr = newElement;
			
			curr.name = names[i];
			curr.text = texts[i];
		}
	}
}
}