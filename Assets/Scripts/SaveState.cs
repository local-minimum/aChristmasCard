using UnityEngine;
using System.Collections;

public class SaveState : Singleton<SaveState> {

	private string currentWordListPage = "wordListPage";
	private string letterWord = "letterWord_{0}";
	private string solvedLetterWord = "letterWordSolved_{0}";
	private string wordPageListToIndex = "wordPageListToIndex_{0}";
	private string wordPageListHighestListIndex = "wordPageListHighestIndex";

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
			if (PlayerPrefs.GetInt(currentWordListPage, -1) < 0 && WordList.Instance.Length > 0)
				wordListPage = 0;

			return PlayerPrefs.GetInt(currentWordListPage, -1);
		}

		set {
			if (value < 0 && WordList.Instance.Length > 0)
				value = 0;
			else if (value < -1)
				value = -1;
			PlayerPrefs.SetInt(currentWordListPage, value);
		}
	}

	public void SetSolvedLetterWord(string word) {
		PlayerPrefs.SetInt(string.Format(solvedLetterWord, word.ToLower()), 1);
	}

	public bool GetSolvedLetterWord(string word) {
		return PlayerPrefs.GetInt(string.Format(solvedLetterWord, word.ToLower(), 0)) == 1;
	}

	public void SetLearnedLetterWord(string word) {
		PlayerPrefs.SetInt(string.Format(letterWord, word.ToLower()), 1);
	}

	public bool GetLearnedLetterWord(string word) {
		return PlayerPrefs.GetInt(string.Format(letterWord, word.ToLower()), 0) == 1;
	}

	public int GetWordListIndex(int indexInList) {
		return PlayerPrefs.GetInt(string.Format(wordPageListToIndex, indexInList), -1);
	}

	public void SetWordListIndex(int indexInList, int index) {
		if (index > PlayerPrefs.GetInt(wordPageListHighestListIndex, -1))
			PlayerPrefs.SetInt(wordPageListHighestListIndex, indexInList);
		PlayerPrefs.SetInt(string.Format(wordPageListToIndex, indexInList), index);
	}

	public void RestAll() {
		PlayerPrefs.DeleteAll();
	}

	public void NewGame() {

		//Resetting page in word list
		PlayerPrefs.SetInt(currentWordListPage, -1);

		//Knowing no words
		foreach (string word in WordList.Instance.AllWords) {
			PlayerPrefs.DeleteKey(string.Format(letterWord, word));
			PlayerPrefs.DeleteKey(string.Format(solvedLetterWord, word));
		}

		//Clearing word page index
		for (int i=0; i<=PlayerPrefs.GetInt(wordPageListHighestListIndex, 999); i++)
			PlayerPrefs.DeleteKey(string.Format(wordPageListToIndex, i));
		PlayerPrefs.SetInt(wordPageListHighestListIndex, -1);

		//TODO: Player position


		//TODO; Player inventory

		//TODO: Object statuses


	}
}
