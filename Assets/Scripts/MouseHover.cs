using UnityEngine;
using System.Collections;

public class MouseHover : MonoBehaviour {
	
	InterestPoint nextPt;

	public UnityEngine.UI.Image uiImage;
	public Vector3 fullScale;
	public Vector3 miniScale;

	Canvas canvas;

	static string tweenName = "imageScales";

	bool showing = false;
	bool hiding = false;
	bool effecting = false;

	[Range(0, 5)]
	public float effectDuration = 2f;

	void Start() {
		uiImage.transform.localScale = miniScale;
		canvas = gameObject.GetComponent<Canvas>();
	}

	public void SetHoverPoint(InterestPoint pt) {
		if (pt == nextPt)
			return;

		nextPt = pt;

		if (!hiding && !showing && !effecting)
			OnHiddenDoShow();
		else if (!hiding && effecting) {
			iTween.StopByName(gameObject, tweenName);
			showing = true;
			effecting = false;
//			Debug.Log("QuitShowing");
		} 

		if (!effecting) {

			hiding = true;
			effecting = true;

			float duration = uiImage.transform.localScale.magnitude / fullScale.magnitude * effectDuration;
//			Debug.Log(duration);
			iTween.ScaleTo(uiImage.gameObject,
			                       iTween.Hash(
					"scale", miniScale,
					"duration", duration,
					"easetype", iTween.EaseType.easeInCubic,
					"oncomplete", "OnHiddenDoShow",
					"oncompletetarget", gameObject
					));
		}
	}

	void OnHiddenDoShow() {
		showing = false;
		hiding = false;
		if (nextPt) {
			canvas.transform.position = nextPt.transform.position;
			iTween.ScaleTo(uiImage.gameObject,
			                       iTween.Hash(
				"name", tweenName,
				"scale", fullScale,
				"duration", effectDuration,
				"easetype", iTween.EaseType.easeOutCubic,
				"oncomplete", "OnShown",
				"oncompletetarget", gameObject
				));
		} else {
			OnHidden();
		}
//		Debug.DrawLine(hit.point, pt.transform.position, Color.red);

	}

	void OnHidden() {
		effecting = false;
	}

	void OnShown() {
		showing = true;
		effecting = false;

	}
}
