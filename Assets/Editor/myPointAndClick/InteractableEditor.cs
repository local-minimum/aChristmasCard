using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using PointClick;

[CustomEditor(typeof(Interactable), true)]
public class InteractableEditor : Editor {

	public override void OnInspectorGUI ()
	{
		Interactable myTarget = (Interactable) target;

		base.OnInspectorGUI ();

		EditorGUILayout.Space();	
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Mass in inventory", GUILayout.Width(200));
		if (myTarget.physicsMass) {
			if (GUILayout.Button("Enable non-physics mass", GUILayout.Width(200)))
				myTarget.mass = Mathf.Abs(myTarget.mass);
		} else {
			if (GUILayout.Button("Use physics mass", GUILayout.Width(200)))
				myTarget.mass = -1;
		}
		EditorGUILayout.EndHorizontal();

		EditorGUI.indentLevel += 1;
		if (myTarget.physicsMass) {
			EditorGUILayout.LabelField("Mass", myTarget.mass.ToString()); 
			if (!myTarget.rigidbody && !myTarget.rigidbody2D) {
				EditorGUILayout.HelpBox("Add a Rigidbody or Rigidbody2D to use physics", MessageType.Info);
				EditorGUILayout.BeginHorizontal();
				EditorGUILayout.Space();
				if (GUILayout.Button("Rigidbody", GUILayout.Width(100)) && !myTarget.rigidbody)
					myTarget.gameObject.AddComponent<Rigidbody>();
				if (GUILayout.Button("Rigidbody2D", GUILayout.Width(100)) && !myTarget.rigidbody2D)
					myTarget.gameObject.AddComponent<Rigidbody2D>();
				EditorGUILayout.Space();
				EditorGUILayout.EndHorizontal();

			}
		} else {
			float mass = EditorGUILayout.FloatField("Mass", myTarget.mass);
			if (mass < 0)
				mass = 0f;
			myTarget.mass = mass;
		}
		EditorGUI.indentLevel -= 1;

		EditorGUILayout.Space();
		EditorGUILayout.Space();
		EditorGUILayout.BeginHorizontal();
		EditorGUILayout.LabelField("Shape in inventory", GUILayout.Width(200));
		if (myTarget.pocketable) {
			if (GUILayout.Button("Non-Pocketable", GUILayout.Width(100)))
				myTarget.inventoryShape = new int[2] {-1, -1};
		} else {
			if (GUILayout.Button("Pocketable", GUILayout.Width(100)))
				myTarget.inventoryShape = new int[2] {1, 1};
		}
		EditorGUILayout.EndHorizontal();

		EditorGUI.indentLevel += 1;
		string[] dimentionNames = new string[4] {"Width", "Height", "Depth", "Duration"};

		if (myTarget.pocketable) {

			myTarget.inventorySprite = (Sprite) EditorGUILayout.ObjectField("Sprite", (Object) myTarget.inventorySprite, typeof(Sprite), false);
			int[] newShape = myTarget.inventoryShape;
			for (int i=0; i<newShape.Length; i++) {
				newShape[i] = EditorGUILayout.IntSlider(dimentionNames[i], newShape[i], 1, 10);
				if (newShape[i] < 1)
					newShape[i] = 1;
			}
			myTarget.inventoryShape = newShape;

		}
		EditorGUI.indentLevel -= 1;
	}
}
