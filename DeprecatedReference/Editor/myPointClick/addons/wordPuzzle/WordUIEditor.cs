using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick.Addons.WordPuzzle;

[CustomEditor(typeof(WordUI))]
public class WordUIEditor : Editor {

	public override void OnInspectorGUI ()
	{
		WordUI myTarget = (WordUI) target;
		
		base.OnInspectorGUI ();
		EditorGUILayout.LabelField("Status:");
		EditorGUI.indentLevel += 1;
		if (myTarget.knownSprite == null || myTarget.unknownSprite == null) {
			if (myTarget.word != "") {
				EditorGUILayout.HelpBox(string.Format(
					"\"{0}\" incorrectly setup, lacking sprite(s)", myTarget.word), MessageType.Error);
			} else {
				EditorGUILayout.HelpBox("Inactive word", MessageType.Warning);
			}
		} else
			EditorGUILayout.HelpBox(string.Format(
				"Word:\t\t{0}\nKnown Sprite:\t{1}\nUnknown Sprite:\t{2}", 
				myTarget.word, myTarget.knownSprite.name, myTarget.unknownSprite.name), MessageType.Info);
		EditorGUI.indentLevel -= 1;
	}
}
