using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public class Word {

	[SerializeField]
	private bool __inspectorExpanded = true;
	public bool learned = false;
	public string word;
	public Sprite unknownVersion;
	public Sprite knownVersion;

}

public class WordList : Singleton<WordList> {


	public  List<Word> words = new List<Word>();

}
