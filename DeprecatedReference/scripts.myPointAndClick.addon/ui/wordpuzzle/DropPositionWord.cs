using UnityEngine;
using System.Collections.Generic;

namespace PointClick.Addons.WordPuzzle {

	public class DropPositionWord : MonoBehaviour {

		private UnityEngine.UI.Image img;
		public string word;
		private Word _word;

		public bool solvedWord {
			get {
				return _word.solved;
			}
		}

		void Start() {
			img = GetComponent<UnityEngine.UI.Image>();
			_word = WordList.Instance.GetWord(word);
			if (_word != null) {
				img.sprite = _word.solutionImage;
			} else
				Debug.LogError(string.Format("Word {0} not known to WordList",  word));
		}

		public bool CanTake(WordUI wordUI) {

			return wordUI.word == word;
		}

		public bool Apply(WordUI wordUI) {
			if (CanTake(wordUI)) {
				SaveState.Instance.SetSolvedLetterWord(wordUI.word);
				_word.solved = true;
				img.sprite = _word.solutionImage;
				return true;
			}

			return false;
		}



	}

}