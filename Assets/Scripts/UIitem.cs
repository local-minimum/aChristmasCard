using UnityEngine;
using System.Collections;

public class UIitem : MonoBehaviour {

	void OnMouseEnter() {
		LevelManager.Instance.SetUIFocus(gameObject);
	}

	void OnMouseExit() {
		LevelManager.Instance.RemoveUIFocus(gameObject);
	}
}
