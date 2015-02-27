using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;
using PointClick;

[CustomPropertyDrawer(typeof(InventoryTypeRestriction))]
public class InventoryTypeEditor : PropertyDrawer {

	public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
	{
		EditorGUI.BeginProperty(position, label, property);

		position = EditorGUI.PrefixLabel(position, GUIUtility.GetControlID(FocusType.Passive), label);

		int indent = EditorGUI.indentLevel;
		EditorGUI.indentLevel = 0;
		SerializedProperty flags = property.FindPropertyRelative("flags");
		flags.intValue = EditorGUI.MaskField(position, flags.intValue, InventoryTypes.NamesEnumerator.ToArray(), EditorStyles.popup);
		EditorGUI.indentLevel = indent;
		EditorGUI.EndProperty();
	}
}
