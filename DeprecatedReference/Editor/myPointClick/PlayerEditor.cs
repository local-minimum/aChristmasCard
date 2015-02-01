using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick;

[CustomEditor(typeof(PlayerController))]
public class PlayerEditor : Editor {

	public override void OnInspectorGUI ()
	{
		PlayerController myTarget = (PlayerController) target;
		
		base.OnInspectorGUI ();
		EditorGUILayout.LabelField("Status:");
		EditorGUI.indentLevel += 1;
		EditorGUILayout.TextArea(string.Format(
			"Room:\t\t{0}\nLocation:\t\t{1}\nTarget:\t\t{2}\nCan move:\t{3}\nLocked:\t\t{4}\nHas light:\t\t{5}\nUsing:\t\t{6}", 
			myTarget.room, myTarget.location, myTarget.target, myTarget.moveable, myTarget.playerLocked,
			 myTarget.hasLight, myTarget.cursor));
		EditorGUI.indentLevel -= 1;
	}
}
