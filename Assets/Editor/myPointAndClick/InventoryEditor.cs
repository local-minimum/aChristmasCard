using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using PointClick;

[CustomEditor(typeof(PlayerInventory))]
public class InventoryEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		PlayerInventory myTarget = (PlayerInventory) target;

		EditorGUILayout.LabelField("Inventories",
		                           myTarget.size.ToString());


		EditorGUI.indentLevel += 1;

		foreach (PlayerInventoryMap im in myTarget.Inventories)
			DrawInventoryPosition(myTarget, im);

		EditorGUI.indentLevel -= 1;

		if (GUILayout.Button("Add inventory...", GUILayout.Width(120))) {
			GUI.FocusControl(null);
			InventoryWizard.NewInventory(myTarget);

		}
	}

	void DrawInventoryPosition(PlayerInventory inventory, PlayerInventoryMap im) {
		if (im == null) {
			inventory.RemoveInventory(im);
			return;
		}
		EditorGUILayout.BeginHorizontal();
		string permissableTypes = string.Join(", ", im.permissableObjects.SelectedNames.ToArray());
		if (permissableTypes != "")
			permissableTypes = string.Format("Allows: {0}", permissableTypes);
		else
			permissableTypes = "Allows nothing to enter";

		im.name = EditorGUILayout.TextField(im.name, GUILayout.Width(150));
		GUIContent description = new GUIContent(string.Format("Size: {0}", im.sizeString), permissableTypes);
		EditorGUILayout.LabelField(description, GUILayout.Width(100));
		EditorGUILayout.Space();
		if (GUILayout.Button("View", GUILayout.Width(60))) {
			GUI.FocusControl(null);
			InventoryWizard.EditInventory(inventory, im);
		}

		if (GUILayout.Button("↑", GUILayout.Width(20))) {
			GUI.FocusControl(null);
			inventory.Promote(im);
		}
		if (GUILayout.Button("↓", GUILayout.Width(20))) {
			GUI.FocusControl(null);
			inventory.Demote(im);
		} 

		if (GUILayout.Button("Remove", GUILayout.Width(70))) {
			GUI.FocusControl(null);
			inventory.RemoveInventory(im);
		}

		EditorGUILayout.EndHorizontal();
	}
}
