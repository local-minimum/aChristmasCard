using UnityEngine;
using System.Collections;

namespace PointClick.UI {

	public class UIitem : MonoBehaviour {

		public void OnMouseEnter() {
			LevelManager.Instance.SetUIFocus(gameObject);
		}

		public void OnMouseExit() {
			LevelManager.Instance.RemoveUIFocus(gameObject);
		}

	}

}