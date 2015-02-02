using UnityEngine;
using System.Collections;

namespace PointClick {

	public class Player : GameEntity {

		private PlayerActions _playerActions;

		public PlayerActions actions {
			get {
				if (!_playerActions) {
					_playerActions = GetComponent<PlayerActions>();
					if (!_playerActions)
						_playerActions = gameObject.AddComponent<PlayerActions>();
				}
				return _playerActions;
			}
		}

		public PlayerMovement _playerMovement;

		public PlayerMovement movement {
			get {
				if (!_playerMovement) {
					_playerMovement = GetComponent<PlayerMovement>();
					if (!_playerMovement)
						_playerMovement = gameObject.AddComponent<PlayerMovement>();
				}
				return _playerMovement;
			}
		}
	}

}
