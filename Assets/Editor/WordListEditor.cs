using UnityEngine;
using System.Collections;
using UnityEditor;


[CustomEditor(typeof(WordList))]
public class WordListEditor : Editor {

	public override void OnInspectorGUI ()
	{
		/*

		WordList myTarget = (WordList) target;
		int listSize = EditorGUILayout.IntField("Words", myTarget.words.Count);
		if (listSize != myTarget.words.Count) {
			AddEmptyWords(myTarget, listSize - myTarget.words.Count);
			TrimWords(myTarget, listSize);
		}
		*/
		serializedObject.Update();
		WordListEditor.Show(serializedObject.FindProperty("words"));
		serializedObject.ApplyModifiedProperties();
	}

	private void AddEmptyWords(WordList myTarget, int n) {
		while (n > 0) {
			myTarget.words.Add(new Word());
			n--;
		}
	}

	private void TrimWords(WordList myTarget, int expectedLength) {
		while (myTarget.words.Count > expectedLength)
			myTarget.words.RemoveAt(myTarget.words.Count - 1);
	}

	public static void Show (SerializedProperty list) {
		EditorGUILayout.PropertyField(list);
		EditorGUI.indentLevel += 1;
		if (list.isExpanded) {
			EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
			for (int i = 0; i < list.arraySize; i++) {
				EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
			}
			EditorGUI.indentLevel -= 1;
		}
	}
}
