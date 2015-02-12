﻿using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	public class Player : GameEntity {

		private PlayerActions _playerActions;
		
		public float gizmoSize = 0.1f;

		public PlayerActions actions {
			get {
				if (!_playerActions) 
					_playerActions = (PlayerActions) getHookUp<PlayerActions>();
				return _playerActions;
			}
		}

		private PlayerMovement _playerMovement;

		public PlayerMovement movement {
			get {
				if (!_playerMovement)
					_playerMovement = (PlayerMovement) getHookUp<PlayerMovement>();
				return _playerMovement;
			}
		}

		private List<PlayerInventory> _playerInventories = new List<PlayerInventory>();

		public PlayerInventory inventory {
			get {
				if (_playerInventories.Count() == 0) 
					_playerInventories.Add((PlayerInventory) getHookUp<PlayerInventory>());
				return _playerInventories.First();

			}
		}

		private MonoBehaviour getHookUp<T>() where T : MonoBehaviour {
			T hook = GetComponent<T>();
			if (!hook)
				hook = gameObject.AddComponent<T>();
			return hook;
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
