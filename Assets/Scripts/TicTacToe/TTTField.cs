using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TicTacToe;

namespace TicTacToe {
public class TTTField : MonoBehaviour {

	public int row;
	public int column;
	public Sprite crossImage;
	public Sprite circleImage;

	TTTGame tttGame;
	Image image;
	Button button;

	void Start() {
		tttGame = GetComponentInParent<TTTGame>();
		image = GetComponent<Image>();
		button = GetComponent<Button>();
	}

	public void Select() {
		button.interactable = false;
		if(tttGame.player1IsActive) {
			image.sprite = crossImage;
		} else {
			image.sprite = circleImage;
		}
		tttGame.ClickedButton(row, column);
	}
}
}