using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {

	public override void OnInspectorGUI ()
	{
		LevelManager myTarget = (LevelManager) target;
		
		base.OnInspectorGUI ();
		EditorGUILayout.LabelField("Status:");
		EditorGUI.indentLevel += 1;
		EditorGUILayout.TextArea(string.Format(
			"Play Time:\t\t{0}\nIn UI mode:\t{1}", 
			myTarget.playTime, myTarget.uiView));
		EditorGUI.indentLevel -= 1;
	}
}
