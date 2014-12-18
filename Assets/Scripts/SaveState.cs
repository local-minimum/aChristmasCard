using UnityEngine;
using System.Collections;

public class SaveState : Singleton<SaveState> {


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
}
