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
		string learnDetails = "";
		if (myTarget.word != "")
			learnDetails = string.Format("\nWord learned:\t{0}\nWord solved:\t{1}", SaveState.Instance.GetLearnedLetterWord(myTarget.word), SaveState.Instance.GetSolvedLetterWord(myTarget.word));

		EditorGUILayout.HelpBox(string.Format(
			"Object in room:\t{0}\nTotal slots:\t\t{1}\nPocketable:\t{2}{3}{4}", 
			myTarget.room, myTarget.dropPositions.Count, myTarget.pocketable != null, learnDetails, details), MessageType.Info);
		EditorGUI.indentLevel -= 1;
	}
}
