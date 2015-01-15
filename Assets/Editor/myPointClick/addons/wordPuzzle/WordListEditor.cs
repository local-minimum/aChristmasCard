using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick.Addons.WordPuzzle;

[CustomEditor(typeof(WordList))]
public class WordListEditor : Editor {

	public override void OnInspectorGUI ()
	{

		serializedObject.Update();
		WordListEditor.Show(serializedObject.FindProperty("wordPages"));
		serializedObject.ApplyModifiedProperties();
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
