using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LetterWriter : Singleton<LetterWriter> {

	public Animator wordbookAnimator;
	public float delayBeforePlay = 1f;
	private bool letterView = false;
	private bool placingWord = false;


	private WordUI _currentWord;

	public WordUI currentWord {
		set {
			_currentWord = value;
			placingWord = true;
			LevelManager.Instance.player.Using(value.gameObject);
		}
	}

	private DropPositionWord[] wordPositions;

	void Start() {
		wordPositions = GetComponentsInChildren<DropPositionWord>();
	}

	public void ShowLetterView() {
		wordbookAnimator.SetTrigger("Show");
		LevelManager.Instance.SetUIFocus(gameObject);
		WorldListUI.Instance.DisplayCurrentPage();
		letterView = true;
	}

	public void Hide() {
		letterView = false;
		wordbookAnimator.SetTrigger("Hide");
		StartCoroutine(DelayEnablePlay());
	}

	IEnumerator<WaitForSeconds> DelayEnablePlay() {
		yield return new WaitForSeconds(delayBeforePlay);
		LevelManager.Instance.RemoveUIFocus(gameObject);
	}

	void Update() {
		if (!letterView)
			return;

		if (Input.GetButtonDown("Fire1")) 
			WordAction();
	}

	void WordAction() {
		if (placingWord) {
			DropPositionWord dropPos = LevelManager.Instance.UIwithFocusByType<DropPositionWord>();
			if (dropPos) {
				if (dropPos.Apply(_currentWord)) {
					Debug.Log(string.Format("Player solved {0}", _currentWord.word));
					int otherPlaces = wordPositions.Where(wPos => !wPos.knownWord && wPos.Apply(_currentWord)).Count();
					Debug.Log(string.Format("Solved {0} other instances of {1}", otherPlaces, _currentWord.word));
				} else
					Debug.Log(string.Format("Player tried solving {0} with {1}", dropPos.word, _currentWord.word));
			}
			LevelManager.Instance.player.StopUsing();
			placingWord = false;
		} else {

		}
	}
}
