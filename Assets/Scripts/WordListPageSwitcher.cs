using UnityEngine;
using System.Collections;

public class WordListPageSwitcher : Singleton<WordListPageSwitcher> {

	public string NextAnimation;
	public string PrevAnimation;
	public string NextAnimationHover;
	public string PrevAnimationHover;
	public string NoHover;

	private Animator anim;

	void Start() {
		anim = GetComponentInParent<Animator>();
	}

	public void NextPage() {
		SaveState.WordListPage++;
		anim.SetTrigger(NextAnimation);
	}

	public void PrevPage() {
		SaveState.WordListPage--;
		anim.SetTrigger(PrevAnimation);
	}

	public void HoverNext() {
		if (WordList.Instance.HasNextPage())
			anim.SetTrigger(NextAnimationHover);
	}

	public void HoverPrev() {
		if (WordList.Instance.HasPrevPage())
			anim.SetTrigger(PrevAnimationHover);
	}

	public void RemoveHoverEffect() {
		anim.SetTrigger(NoHover);
	}
}
