﻿using UnityEngine;
using System.Collections.Generic;

public class DropPositionWord : MonoBehaviour {

	private UnityEngine.UI.Image img;
	public string word;

	void Start() {
		img = GetComponent<UnityEngine.UI.Image>();
		Word wrd = WordList.Instance.GetWord(word);
		if (wrd != null) {
			if (SaveState.Instance.GetSolvedLetterWord(word))
				img.sprite = wrd.knownVersion;
			else
				img.sprite = wrd.unknownVersion;
		} else
			Debug.LogError(string.Format("Word {0} not known to WordList",  word));
	}

	public bool CanTake(WordUI wordUI) {

		return wordUI.word == word;
	}

	public bool Apply(WordUI wordUI) {
		if (CanTake(wordUI)) {
			SaveState.Instance.SetSolvedLetterWord(wordUI.word);
			img.sprite = wordUI.knownSprite;
			return true;
		}

		return false;
	}



}
