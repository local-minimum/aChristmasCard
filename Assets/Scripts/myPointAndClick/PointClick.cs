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
		public Point target;
		public bool invokedInPlace;

		public ActionMessage(Player player, Point target) {
			this.player = player;
			this.target = target;
			this.invokedInPlace = true;
		}

		public ActionMessage(Player player, Point target, bool invokedInPlace) {
			this.player = player;
			this.target = target;
			this.invokedInPlace = invokedInPlace;
		}
	}
}
