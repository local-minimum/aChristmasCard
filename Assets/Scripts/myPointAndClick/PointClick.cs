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

	public struct ActionMessage {
		public Player player;
		public bool invokedInPlace;

		public ActionMessage(Player player) {
			this.player = player;
			this.invokedInPlace = true;
		}

		public ActionMessage(Player player, bool invokedInPlace) {
			this.player = player;
			this.invokedInPlace = invokedInPlace;
		}
	}
}
