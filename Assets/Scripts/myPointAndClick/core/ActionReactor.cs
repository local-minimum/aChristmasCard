using UnityEngine;
using System.Collections;

namespace PointClick {
	public class ActionReactor : GameEntity {

		public bool debugPlayerMessages = false;

		protected virtual void OnPlayerArrive(WalkingMessage msg) {
			if (debugPlayerMessages) {
				Debug.Log(string.Format(
					"{0} arrived at {1}, final destination? {2}", msg.player, name, msg.destination));
			}
		}

		protected virtual void OnPlayerApply(ActionMessage msg) {
			if (debugPlayerMessages) {
				Debug.Log(string.Format(
					"{0} applied at {1}, invoked in place? {2}", msg.player, name, msg.invokedInPlace));
			}

		}

		protected virtual void OnPlayerUse(ActionMessage msg) {
			if (debugPlayerMessages) {
				Debug.Log(string.Format(
					"{0} used something at {1}, invoked in place? {2}", msg.player, name, msg.invokedInPlace));
			}
			
		}
	}
}