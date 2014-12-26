using UnityEngine;
using System.Collections;

public class SaveState : Singleton<SaveState> {

	private string letterWord = "letterWord_{0}";

	public int wordListPage {
		get {
			if (PlayerPrefs.GetInt("wordListPage", -1) < 0 && WordList.Instance.Length > 0)
				wordListPage = 0;

			return PlayerPrefs.GetInt("wordListPage", -1);
		}

		set {
			if (value < 0 && WordList.Instance.Length > 0)
				value = 0;
			else if (value < -1)
				value = -1;
			PlayerPrefs.SetInt("wordListPage", value);
		}
	}

	public void ClearSaveSate() {
		wordListPage = -1;
	}

	public void SetSolvedLetterWord(string word) {
		PlayerPrefs.SetInt(string.Format(letterWord, word), 1);
	}

	public bool GetSolvedLetterWord(string word) {
		return PlayerPrefs.GetInt(string.Format(letterWord, word, 0)) == 1;
	}
}
