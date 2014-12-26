using UnityEngine;
using System.Collections;

public class WorldListUI : Singleton<WorldListUI> {

	public UnityEngine.UI.Button[] wordButtons;
	public UnityEngine.UI.Button nextButton;
	public UnityEngine.UI.Button prevButton;

	public void NextPage() {
		SaveState.Instance.wordListPage += 1;
	}

	public void PrevPage() {
		SaveState.Instance.wordListPage -= 1;
	}

	public void DisplayCurrentPage() {
		WordPage wp = WordList.Instance.CurrentPage();

		if (wp == null) {
			BlankPage();
			nextButton.enabled = false;
			prevButton.enabled = false;
			return;
		}

		for (int i = 0; i<wordButtons.Length; i++) {
			if (i < wp.words.Length) {
				wordButtons[i].image.sprite = wp.words[i].image;
				TweakButton(wordButtons[i], 0.9f);
			} else {
				TweakButton(wordButtons[i], 0);
			}


		}
	}

	private void BlankPage() {
		for (int i=0; i< wordButtons.Length; i++) {
			TweakButton(wordButtons[i], 0);
		}
	}

	private void TweakButton(UnityEngine.UI.Button btn, float alpha) {
		Color c = btn.image.color;
		c.a = alpha;
		btn.image.color = c;
		btn.enabled = (alpha != 0);
	}


}
