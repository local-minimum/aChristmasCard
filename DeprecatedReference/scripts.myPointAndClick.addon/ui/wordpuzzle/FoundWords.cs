using UnityEngine;
using System.Collections.Generic;

namespace PointClick.Addons.WordPuzzle {

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
			    LevelManager.Instance.playTime - showTime > minBetweenT)

				NextWord();
		}

		void NextWord() {
			if (wordQueue.Count == 0)
				return;

			showTime = LevelManager.Instance.playTime;
			text.text = wordQueue[0];
			wordQueue.RemoveAt(0);
			anim.SetTrigger(triggerStr);
			WordList.Instance.LearnedWordEffect();
		}

		public void FoundWord(string word) {
			wordQueue.Add (Captialize(word));
		}

		public static string Captialize(string s) {
			char[] a = s.ToCharArray();
			a[0] = char.ToUpper(a[0]);
			return new string(a);
		}
	}

}