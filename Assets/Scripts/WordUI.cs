using UnityEngine;
using System.Collections;

public class WordUI : UIitem {

	private Word currentWord;

	public string word {
		get {
			return "";
		}
	}

	public Sprite knownSprite {
		get {
			return currentWord.knownVersion;
		}
	}

	public Sprite unknownSprite {
		get {
			return currentWord.unknownVersion;
		}
	}

	public void SetWord(Word word) {
		currentWord = word;
	}

	public void DragMe() {
		Debug.Log(this);
		LetterWriter.Instance.currentWord = this;
	}
}
