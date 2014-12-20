using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(InterestPoint))]
public class InterestPointEditor : Editor {

	
	public override void OnInspectorGUI ()
	{
		
		base.OnInspectorGUI ();
		InterestPointEditor.BasicStatus((InterestPoint) target);
	}

	public static void BasicStatus (InterestPoint myTarget){

		EditorGUILayout.LabelField("Status:");
		EditorGUI.indentLevel += 1;
		EditorGUILayout.TextArea(string.Format(
			"Object in room:\t{0}\nTotal slots:\t\t{1}\nPocketable:\t{2}", 
			myTarget.room, myTarget.dropPositions.Count, myTarget.pocketable != null));
		EditorGUI.indentLevel -= 1;
	}
}
