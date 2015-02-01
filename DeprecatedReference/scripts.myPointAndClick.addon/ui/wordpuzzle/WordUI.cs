using UnityEngine;
using System.Collections;
using UnityEngine.UI;

namespace PointClick.Addons.WordPuzzle {

	public class WordUI : PointClick.UI.UIitem {

		private Word currentWord;
		private Button btn;

		public string word {
			get {
				if (currentWord != null)
					return currentWord.word;
				else
					return "";
			}
		}

		public Sprite knownSprite {
			get {
				if (currentWord != null)
					return currentWord.knownVersion;
				else
					return null;
			}
		}

		public Sprite unknownSprite {
			get {
				if (currentWord != null)
					return currentWord.unknownVersion;
				else
					return null;
			}
		}

		void Start() {
			btn = GetComponent<Button>();
		}

		public void SetWord(Word word) {
			currentWord = word;
			if (word != null)
				btn.image.sprite = currentWord.image;
			TweakButton(word == null ? 0f : 0.9f);
		}

		public void DragMe() {
			Debug.Log(this);
			LetterWriter.Instance.currentWord = this;
			iTween.PunchScale(gameObject,
			                  iTween.Hash("x", 0.25f, "y", 0.25f, "z", 1, "looptype", iTween.LoopType.loop, "time", 2f));
		}

		public void UnDragMe() {
			iTween.Stop(gameObject);
		}

		private void TweakButton(float alpha) {
			Color c = btn.image.color;
			c.a = alpha;
			btn.image.color = c;
			btn.enabled = (alpha != 0);
		}

	}

}