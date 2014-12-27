using UnityEngine;
using System.Collections;

public class UIitem : MonoBehaviour {

	public void OnMouseEnter() {
		LevelManager.Instance.SetUIFocus(gameObject);
	}

	public void OnMouseExit() {
		LevelManager.Instance.RemoveUIFocus(gameObject);
	}

}
