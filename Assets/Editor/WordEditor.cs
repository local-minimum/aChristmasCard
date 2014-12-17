﻿using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(Word))]
public class WordEditor : PropertyDrawer {

	SerializedProperty curProp;

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{

		curProp = property;
		string name = property.FindPropertyRelative("word").stringValue;
		if (name != "")
			label.text = name;
		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
		if (property.isExpanded) {
			int indentLvl =	EditorGUI.indentLevel;
			Rect contentPosition = EditorGUI.IndentedRect(position);
			EditorGUI.indentLevel = 0;
			contentPosition.y += base.GetPropertyHeight(property, label);
			contentPosition.height /= 4;
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("word"));
			contentPosition.y += base.GetPropertyHeight(property, label);
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("knownVersion"));
			contentPosition.y += base.GetPropertyHeight(property, label);
			EditorGUI.PropertyField(contentPosition, property.FindPropertyRelative("unknownVersion"));
			EditorGUI.indentLevel = indentLvl;

		}

	}

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label) {
		if (curProp == property)
			return base.GetPropertyHeight(property, label);
		return base.GetPropertyHeight(property, label) * (property.isExpanded ? 4 : 1);
	}
}