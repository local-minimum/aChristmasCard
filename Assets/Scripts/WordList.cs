using UnityEngine;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class Word {

	public bool learned = false;
	public string word;
	public Sprite unknownVersion;
	public Sprite knownVersion;

}

[System.Serializable]
public class WordPage {

	public Word[] words;

	public bool Contains(string word) {
		return words.Select(w => w.word == word).Any();
	}

	public bool[] Knows() {
		return words.Select(w => w.learned).ToArray();
	}
	
}

public class WordList : Singleton<WordList> {


	public  List<WordPage> wordPages = new List<WordPage>();

}
