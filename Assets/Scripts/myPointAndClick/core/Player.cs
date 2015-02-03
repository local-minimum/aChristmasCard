using UnityEngine;
using System.Collections;

namespace PointClick {

	public class Player : GameEntity {

		private PlayerActions _playerActions;
		
		public float gizmoSize = 0.1f;

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

		
		void OnDrawGizmos() {
			Gizmos.color = Color.green;
			if (movement.walking) {
				Gizmos.DrawSphere(transform.position, gizmoSize);
				Gizmos.DrawRay(transform.position, rigidbody.velocity);
			} else 
				Gizmos.DrawCube(transform.position, Vector3.one * gizmoSize);
			
		}
	}

}
