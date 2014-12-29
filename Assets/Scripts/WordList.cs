using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Word {

	public string word;
	public Sprite unknownVersion;
	public Sprite knownVersion;

	public bool learned {
		get {
			return SaveState.Instance.GetLearnedLetterWord(word);
		}

		set {
			if (value)
				SaveState.Instance.SetLearnedLetterWord(word);
			else
				Debug.LogWarning("Can't unlearn words");
		}
	}

	public bool solved {
		get {
			return SaveState.Instance.GetSolvedLetterWord(word);
		}

		set {
			if (value)
				SaveState.Instance.SetSolvedLetterWord(word);
			else
				Debug.LogWarning("Can't unsolve words");
		}
	}

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

	private float wordListPage;

	public  List<WordPage> wordPages = new List<WordPage>();
	private Dictionary<int, WordPage> index = new Dictionary<int, WordPage>();

	public bool Learn(PlayerController player, string word) {
		foreach (WordPage wp in wordPages) {
			if (wp.Contains(word)) {
				return wp.Learn(player, word);
			}
		}
		return false;
	}

	void Start() {
		for (int i=0; i<wordPages.Count(); i++) {
			int idX = SaveState.Instance.GetWordListIndex(i);
			if (idX>=0)
				index.Add(idX, wordPages[i]);
		}
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

		SaveState.Instance.SetWordListIndex(wordPages.IndexOf(page),Length);
		index.Add(Length, page);

	}

	public WordPage CurrentPage() {
		if (index.ContainsKey(SaveState.WordListPage))
			return index[SaveState.WordListPage];
		return null;
	}

	public bool HasNextPage() {
		return index.ContainsKey(SaveState.WordListPage + 1);
	}

	public bool HasPrevPage() {
		return index.ContainsKey(SaveState.WordListPage - 1);
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
