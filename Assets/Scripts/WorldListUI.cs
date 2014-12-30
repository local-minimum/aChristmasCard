using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WorldListUI : Singleton<WorldListUI> {

	public WordUI[] wordButtons;
	public Button nextButton;
	public Button prevButton;

	public void DisplayCurrentPage() {
		WordPage wp = WordList.Instance.CurrentPage();

		if (wp == null) {
			BlankPage();
			nextButton.enabled = false;
			prevButton.enabled = false;
			return;
		}

		for (int i = 0; i<wordButtons.Length; i++)
			wordButtons[i].SetWord(i < wp.words.Length ? wp.words[i] : null);

		nextButton.enabled = WordList.Instance.HasNextPage();
		prevButton.enabled = WordList.Instance.HasPrevPage();
	}

	private void BlankPage() {
		for (int i=0; i< wordButtons.Length; i++)
			wordButtons[i].SetWord(null);
	}
	
}
