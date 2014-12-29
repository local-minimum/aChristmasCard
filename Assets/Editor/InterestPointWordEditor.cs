using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor(typeof(InterestPointWord))]
public class InterestPointWordEditor : Editor {

	public override void OnInspectorGUI ()
	{
		
		base.OnInspectorGUI ();
		InterestPointWord myTarget = (InterestPointWord) target;

		string details = string.Format("Word learned:\t{0}\nWord solved:\t{1}", SaveState.Instance.GetLearnedLetterWord(myTarget.word), SaveState.Instance.GetSolvedLetterWord(myTarget.word));
		InterestPointEditor.BasicStatus((InterestPoint) myTarget, details);
	}
}
