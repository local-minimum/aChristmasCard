using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomPropertyDrawer(typeof(WordPage))]
public class WordPageEditor : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{

		position.height = 16f;


		label.text = AggregateWords(property.FindPropertyRelative("words"));
		label = EditorGUI.BeginProperty(position, label, property);
		property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label, true);
		EditorGUI.EndProperty();

		if (property.isExpanded) {
			EditorGUI.indentLevel += 1;
			WordPageEditor.Show(property.FindPropertyRelative("words"));
			EditorGUI.indentLevel -= 1;
		}
	}

	private string AggregateWords(SerializedProperty list) {
		if (list.arraySize == 0)
			return "-{EMPTY}-";

		string ret = "";
		for (int i=0; i<list.arraySize; i++) {
			if (ret.Length > 0)
				ret += " | ";
			ret += list.GetArrayElementAtIndex(i).FindPropertyRelative("word").stringValue;
		}
		return ret;
	}

	public override float GetPropertyHeight (SerializedProperty property, GUIContent label)
	{
		return base.GetPropertyHeight (property, label);
	}

	public static void Show (SerializedProperty list) {
		EditorGUILayout.PropertyField(list.FindPropertyRelative("Array.size"));
		for (int i = 0; i < list.arraySize; i++) {
			EditorGUILayout.PropertyField(list.GetArrayElementAtIndex(i));
		}

	}
}
