using UnityEngine;
using System.Collections;

namespace PointClick {
	public struct LearnedMessage {
		public PlayerController player;
		public string message;

		public LearnedMessage(PlayerController player, string message) {
			this.player = player;
			this.message = message;
		}
	}
}
