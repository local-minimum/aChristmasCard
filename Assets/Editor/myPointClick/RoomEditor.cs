using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick;

[CustomEditor(typeof(RoomManager))]
public class RoomEditor : Editor {

	public override void OnInspectorGUI ()
	{
		RoomManager myTarget = (RoomManager) target;

		base.OnInspectorGUI ();
		EditorGUILayout.LabelField("Status:");
		EditorGUI.indentLevel += 1;
		EditorGUILayout.TextArea(string.Format(
			"Player in room:\t{0}\nZoom position:\t{1}\nCamera:\t\t{2}", 
			myTarget.playerInRoom, myTarget.zoomPosition, myTarget.cameraPosition));
		EditorGUI.indentLevel -= 1;
	}
}
