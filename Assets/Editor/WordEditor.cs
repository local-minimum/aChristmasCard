using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(Word))]
public class WordEditor : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		string name = property.FindPropertyRelative("word").stringValue;
		if (name != "")
			label.text = name;
		EditorGUI.PropertyField(position, property.FindPropertyRelative("__inspectorExpanded"), GUIContent.none);
		position.x += 20f;
		EditorGUI.PrefixLabel(position, label);
		if (property.FindPropertyRelative("__inspectorExpanded").boolValue) {
			int indentLvl =	EditorGUI.indentLevel;
			Rect contentPosition = EditorGUI.IndentedRect(position);
			EditorGUI.indentLevel = 0;
			contentPosition.y += GetPropertyHeight(property, label);
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("word"));
			contentPosition.y += GetPropertyHeight(property, label);
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("knownVersion"));
			contentPosition.y += GetPropertyHeight(property, label);
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("unknownVersion"));
			EditorGUI.indentLevel = indentLvl;

		}
	}
}
