using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class LetterWriter : Singleton<LetterWriter> {

	public static bool InLetterView {
		get {
			return Instance.letterView;
		}
	}

	public static Transform Cursor {
		get {
			return Instance.cursor;
		}
	}

	public Animator wordbookAnimator;
	public float delayBeforePlay = 1f;
	private bool letterView = false;
	private bool placingWord = false;

	private Vector3 cursorOrigin;
	private Quaternion cursorRotation;
	private Transform _cursor;
	public Transform cursor {
		get {
			return _cursor;
		}

		set {

			if (value == null && _cursor != null) {
				cursor.position = cursorOrigin;
				cursor.rotation = cursorRotation;
				_cursor.gameObject.GetComponent<UnityEngine.UI.Button>().enabled = true;
				_cursor = null;
			} if (_cursor == null && value != null) {
				cursorOrigin = value.position;
				cursorRotation = value.rotation;
				value.gameObject.GetComponent<UnityEngine.UI.Button>().enabled = false;
				_cursor = value;
				_cursor.rotation = Quaternion.identity;
			} else if (_cursor != null) {
				Debug.Log(string.Format("Letter writer cursor {0} can't be activated while using {1}", value.name, _cursor.name));
			}
		}
	}

	private WordUI _currentWord;

	public WordUI currentWord {
		set {
			_currentWord = value;
			placingWord = true;
			cursor = value.transform;
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
			cursor = null;
			placingWord = false;
		} else {

		}
	}
}
