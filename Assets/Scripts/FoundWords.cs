using UnityEngine;
using System.Collections.Generic;

public class FoundWords : MonoBehaviour {

	public UnityEngine.UI.Text text;

	private Animator anim;

	private string triggerStr = "WordDiscovered";
	public string readyState = "playerWorldUIhidden";

	private List<string> wordQueue = new List<string>();
	private float showTime= 0f;
	public float minBetweenT = 2f;

	void Start () {
		anim = GetComponent<Animator>();

	}

	void Update() {
		if (anim.GetCurrentAnimatorStateInfo(0).IsName(readyState) && 
		    LevelManager.Instance.plaayTime - showTime > minBetweenT)

			NextWord();
	}

	void NextWord() {
		if (wordQueue.Count == 0)
			return;

		showTime = LevelManager.Instance.plaayTime;
		text.text = wordQueue[0];
		wordQueue.RemoveAt(0);
		anim.SetTrigger(triggerStr);
	}

	public void FoundWord(string word) {
		wordQueue.Add (word);
	}
}
