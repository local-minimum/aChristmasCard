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
		InterestPointEditor.BasicStatus(myTarget, "");
	}

	public static void BasicStatus (InterestPoint myTarget, string details){

		EditorGUILayout.LabelField("Status:");
		EditorGUI.indentLevel += 1;
		EditorGUILayout.HelpBox(string.Format(
			"Object in room:\t{0}\nTotal slots:\t\t{1}\nPocketable:\t{2}\n{3}", 
			myTarget.room, myTarget.dropPositions.Count, myTarget.pocketable != null, details), MessageType.Info);
		EditorGUI.indentLevel -= 1;
	}
}
