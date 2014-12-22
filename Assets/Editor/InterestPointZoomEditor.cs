using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(InterestPointZoom))]
public class InterestPointZoomEditor : Editor {

	public override void OnInspectorGUI ()
	{
		
		base.OnInspectorGUI ();
		InterestPointZoom myTarget = (InterestPointZoom) target;
		string details = "\nChildren:\t\t" + string.Join("\n\t\t", myTarget.children.Select(ip => ip.name).ToArray());
		InterestPointEditor.BasicStatus(myTarget, details);
	}
}
