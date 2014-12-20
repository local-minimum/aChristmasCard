using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(InterestPointRestricted))]
public class InterestPointRestricedEditor : Editor {
	
	
	public override void OnInspectorGUI ()
	{
		
		base.OnInspectorGUI ();
		InterestPointEditor.BasicStatus((InterestPoint) target);
	}
}
