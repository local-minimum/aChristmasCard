using UnityEngine;
using System.Collections;

namespace PointClick {
	public struct WalkingMessage {
		public Player player;
		public bool destination;

		public WalkingMessage(Player player, bool destination) {
			this.player = player;
			this.destination = destination;
		}
	}
}
