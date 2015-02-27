using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick;

[CustomEditor(typeof(InventoryTypes))]
public class InventoryTypesManagerEditor : Editor {


	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		EditorGUILayout.LabelField("Names");
		EditorGUI.indentLevel += 1;
		for (int position=0; position<InventoryTypes.Size; position++)
			InventoryTypes.SetName(position, EditorGUILayout.TextField(InventoryTypes.GetName(position), GUILayout.Width(200)));

		EditorGUI.indentLevel -= 1;
	}
}
