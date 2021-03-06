﻿using UnityEngine;
using System.Collections;
using PointClick.Addons.WordPuzzle;

public class SaveState : Singleton<SaveState> {

	private string currentWordListPage = "wordListPage";
	private string letterWord = "letterWord_{0}";
	private string solvedLetterWord = "letterWordSolved_{0}";
	private string wordPageListToIndex = "wordPageListToIndex_{0}";
	private string wordPageListHighestListIndex = "wordPageListHighestIndex";
	private string eventStr = "event_{0}";
	private string player = "player_{0}";
	public static Vector3 nullVector = Vector3.down * 100f;
	
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

	public bool GetFoodInOven() {
		return PlayerPrefs.GetInt(string.Format(eventStr, "foodInOven"), 0) == 1;
	}

	public void SetFoodInOven(bool val) {
		PlayerPrefs.SetInt(string.Format(eventStr, "foodInOven"), val ? 1 : 0);
	}

	public bool GetFoodCooking() {
		return PlayerPrefs.GetInt(string.Format(eventStr, "cooking"), 0) == 1;
	}

	public void SetFoodCooking(bool val) {
		PlayerPrefs.SetInt(string.Format(eventStr, "cooking"), val ? 1 : 0);
	}

	public bool GetEventFire() {
		return PlayerPrefs.GetInt(string.Format(eventStr, "fire"), 0) == 1;
	}

	public void	 SetEventFire(bool val) {
		PlayerPrefs.SetInt(string.Format(eventStr, "fire"), val ? 1 : 0);
	}

	public Vector3 GetPlayerPosition() {
		if (PlayerPrefs.HasKey(string.Format(player, "x")) && PlayerPrefs.HasKey(string.Format(player, "y")) && PlayerPrefs.HasKey(string.Format(player, "z")))
			return new Vector3(PlayerPrefs.GetFloat(string.Format(player, "x")),
			                   PlayerPrefs.GetFloat(string.Format(player, "y")),
			                   PlayerPrefs.GetFloat(string.Format(player, "z")));

		return nullVector;
	}

	public void SetPlayerPosition(Vector3 pos) {
		if (pos == nullVector) {
			PlayerPrefs.DeleteKey(string.Format(player, "x"));
			PlayerPrefs.DeleteKey(string.Format(player, "y"));
			PlayerPrefs.DeleteKey(string.Format(player, "z"));
		} else {
			PlayerPrefs.SetFloat(string.Format(player, "x"), pos.x);
			PlayerPrefs.SetFloat(string.Format(player, "y"), pos.y);
			PlayerPrefs.SetFloat(string.Format(player, "z"), pos.z);
		}
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
		SetPlayerPosition(nullVector);

		//TODO; Player inventory

		//TODO: Event statuses
		SetEventFire(false);

	}
}
