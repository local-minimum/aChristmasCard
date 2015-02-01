using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick.Addons.WordPuzzle {

	[System.Serializable]
	public class Word {

		public string word;
		public Sprite unknownVersion;
		public Sprite knownVersion;

		public bool learned = false;
		public bool solved = false;

//		public bool learned {
//			get {
////				return SaveState.Instance.GetLearnedLetterWord(word);
//				return _learned;
//			}
//
//			set {
//				if (value)
//					_learned = value;
//				else
//					Debug.LogWarning("Can't unlearn words");
//			}
//		}
//
//		public bool solved {
//			get {
//				return _solved; //TODO: save
////				return SaveState.Instance.GetSolvedLetterWord(word);
//			}
//
//			set {
//				if (value)
//					SaveState.Instance.SetSolvedLetterWord(word);
//				else
//					Debug.LogWarning("Can't unsolve words");
//			}
//		}
//
		public Sprite image {
			get {
				return learned ? knownVersion : unknownVersion;
			}
		}

		public Sprite solutionImage {
			get {
				return solved ? knownVersion : unknownVersion;
			}
		}
	}

	[System.Serializable]
	public class WordPage {

		public Word[] words;

		public bool autoCompletes = false;
		public int autoCompleteThreshold = 3;

		public int learnedWords {
			get {
				return words.Where(w => w.learned).Count();
			}
		}

		public bool Contains(string word) {
			return words.Where(w => w.word == word).Any();
		}

		public Word GetWord(string word) {
			return words.Where(w => w.word == word).FirstOrDefault();
		}

		public bool[] Knows() {
			return words.Select(w => w.learned).ToArray();
		}
		
		public bool Learn(PlayerController player, string word) {
			Word w = words.Where(wrd => wrd.word == word).First();
			if (w.learned)
				return false;

			LearnWord(player, w);

			if (learnedWords >= autoCompleteThreshold)
				LearnRest(player);

			return true;
		}

		private void LearnWord(PlayerController player, Word w) {
			w.learned = true;
			TestFirstWord();
			player.gameObject.BroadcastMessage("FoundWord", w.word, SendMessageOptions.DontRequireReceiver);
		}

		private void LearnRest(PlayerController player) {
			foreach (Word w in words) {
				if (w.learned)
					continue;
				LearnWord(player, w);
			}
		}

		private void TestFirstWord() {
			if (learnedWords == 1)
				WordList.Instance.AddWordPageToIndex(this);
		}
		
	}

	public class WordList : Singleton<WordList> {

		public bool debug = false;
//		private float wordListPage;

		[SerializeThis]
		private int currentPage = -1;

		public  List<WordPage> wordPages = new List<WordPage>();

		[SerializeThis]
		private Dictionary<int, WordPage> index = new Dictionary<int, WordPage>();

		public bool Learn(PointClick.LearnedMessage msg) {
			foreach (WordPage wp in wordPages) {
				if (wp.Contains(msg.message)) {
					return wp.Learn(msg.player, msg.message);
				}
			}
			return false;
		}

		void Start() {
//			for (int i=0; i<wordPages.Count(); i++) {
//				int idX = SaveState.Instance.GetWordListIndex(i);
//				if (idX>=0)
//					index.Add(idX, wordPages[i]);
//			}
		}

		void OnGUI() {
			if (debug)
				PointClick.LevelManager.DebugText = string.Format(
					"<b>Word List index:</b>\n\tPages:\t{0}\n\tCurrent page:\t{1}\n\tHas Next:\t{2}\n\tHas Prev:\t{3}",
					Length, currentPage, HasNextPage(), HasPrevPage());
		}

		public void LearnedWordEffect() {

			iTween.PunchScale(
				gameObject,
				iTween.Hash(
				"amount", Vector3.one * 0.2f ,
				"delay", 1f,
				"duration", 2f,
				"space", Space.Self));

	//		iTween.PunchRotation(
	//			gameObject,
	//			iTween.Hash(
	//			"z", 10,
	//			"delay", 1f,
	//			"duration", 3f,
	//			"space", Space.Self
	//			));
		}

		public int Length {
			get {
				if (index.Count() == 0)
					return 0;
				else
					return index.Keys.Max() + 1;
			}
		}

		public void AddWordPageToIndex(WordPage page) {
			if (index.Values.Contains(page)) {
				Debug.LogError("Tried to add same page twice");
				return;
			}

//			SaveState.Instance.SetWordListIndex(wordPages.IndexOf(page),Length);
			index.Add(Length, page);

		}

		public WordPage CurrentPage() {
			if (index.ContainsKey(currentPage)) // SaveState.WordListPage))
				return index[currentPage]; // SaveState.WordListPage];
			return null;
		}

		public bool HasNextPage() {
			return index.ContainsKey(currentPage + 1); // SaveState.WordListPage + 1);
		}

		public bool HasPrevPage() {
			return index.ContainsKey(currentPage - 1); // SaveState.WordListPage - 1);
		}

		public Word GetWord(string word) {
			IEnumerable<WordPage> res = wordPages.Where(wp => wp.Contains(word));
			if (res.Any())
				return res.First().GetWord(word);
			return null;
		}

		public IEnumerable<string> AllWords {
			get {
				foreach (WordPage wp in wordPages) {
					foreach (Word w in wp.words)
						yield return w.word;

				}
			}
		}
	}

}