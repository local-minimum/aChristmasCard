using UnityEngine;
using System.Collections;

public class SaveState : Singleton<SaveState> {

	private string letterWord = "letterWord_{0}";
	private string solvedLetterWord = "letterWordSolved_{0}";
	private string wordPageListToIndex = "wordPageListToIndex_{0}";

	public static int WordListPage {
		get {
			return Instance.wordListPage;
		}

		set {
			Instance.wordListPage = value;
		}
	}

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

	public void SetSolvedLetterWord(string word) {
		PlayerPrefs.SetInt(string.Format(solvedLetterWord, word), 1);
	}

	public bool GetSolvedLetterWord(string word) {
		return PlayerPrefs.GetInt(string.Format(solvedLetterWord, word, 0)) == 1;
	}

	public void SetLearnedLetterWord(string word) {
		PlayerPrefs.SetInt(string.Format(letterWord, word), 1);
	}

	public bool GetLearnedLetterWord(string word) {
		return PlayerPrefs.GetInt(string.Format(letterWord, word), 0) == 1;
	}

	public int GetWordListIndex(int indexInList) {
		return PlayerPrefs.GetInt(string.Format(wordPageListToIndex, indexInList), -1);
	}

	public void SetWordListIndex(int indexInList, int index) {
		PlayerPrefs.SetInt(string.Format(wordPageListToIndex, indexInList), index);
	}

	public void ClearSaveSate() {

		//Resetting page in word list
		wordListPage = -1;

		//Knowing no words
		foreach (string word in WordList.Instance.AllWords) {
			PlayerPrefs.SetInt(string.Format(letterWord, word), 0);
			PlayerPrefs.SetInt(string.Format(solvedLetterWord, word), 0);
		}

		//TODO: Player position


		//TODO; Player inventory

		//TODO: Object statuses


	}
}
