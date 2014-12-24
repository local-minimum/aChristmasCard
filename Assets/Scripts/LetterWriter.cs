using UnityEngine;
using System.Collections.Generic;

public class LetterWriter : Singleton<LetterWriter> {

	public Animator wordbookAnimator;
	public float delayBeforePlay = 1f;

	public void ShowLetterView() {
		wordbookAnimator.SetTrigger("Show");
		LevelManager.Instance.SetUIFocus(gameObject);
		WorldListUI.Instance.DisplayCurrentPage();
	}

	public void Hide() {
		wordbookAnimator.SetTrigger("Hide");
		StartCoroutine(DelayEnablePlay());
	}

	IEnumerator<WaitForSeconds> DelayEnablePlay() {
		yield return new WaitForSeconds(delayBeforePlay);
		LevelManager.Instance.RemoveUIFocus(gameObject);
	}
}
