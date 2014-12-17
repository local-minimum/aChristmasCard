using UnityEngine;
using System.Collections;

public class SaveState : Singleton<SaveState> {


	public float wordListPage {
		get {
			return PlayerPrefs.GetFloat("wordListPage", -1);
		}

		set {
			PlayerPrefs.SetFloat("wordListPage", value);
		}
	}

	public void ClearSaveSate() {
		wordListPage = -1;
	}
}
