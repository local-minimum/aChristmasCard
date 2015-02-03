using UnityEngine;
using System.Collections;

namespace PointClick {
	public class ActionReactor : GameEntity {

		private Point myPoint;

		public bool debugPlayerMessages = false;

		void Awake() {
			myPoint =  gameObject.GetComponentInParent<Point>();
		}

		protected virtual void OnPlayerArrive(WalkingMessage msg) {
			if (debugPlayerMessages) {
				Debug.Log(string.Format(
					"{0} arrived at {1}, final destination? {2}", msg.player, name, msg.destination));
			}
		}

		protected virtual void OnPlayerApply(ActionMessage msg) {
			if (debugPlayerMessages) {
				Debug.Log(string.Format(
					"{0} applied at {1}, am target? {3}, invoked in place? {2}", msg.player, name, msg.invokedInPlace, msg.target == myPoint));
			}

		}

		protected virtual void OnPlayerUse(ActionMessage msg) {
			if (debugPlayerMessages) {
				Debug.Log(string.Format(
					"{0} used something at {1}, am target? {3}, invoked in place? {2}", msg.player, name, msg.invokedInPlace, msg.target == myPoint));
			}
			
		}
	}
}