using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	public class Interactable : MonoBehaviour {

		[HideInInspector]
		[SerializeField]
		float _mass = -1;

		public float mass {
			get {
				if (_mass < 0f) {
					if (rigidbody)
						return rigidbody.mass;
					else if (rigidbody2D)
						return rigidbody2D.mass;
					else
						return 0f;

				} else
					return _mass;
			}

			set {
				_mass = value;
			}
		}

		public bool physicsMass {
			get {
				return _mass < 0f;
			}
		}

		public InventoryTypeRestriction inventoryType = new InventoryTypeRestriction();

		[HideInInspector]
		[SerializeField]
		int[] _inventorySize = new int[2] {-1, -1};

		[HideInInspector]
		[SerializeField]
		bool _pocketable = false;

		[HideInInspector]
		public Sprite inventorySprite;

		public int[] inventoryShape {
			get {
				if (!_pocketable)
					return null;
				else
					return _inventorySize;
			}

			set {
				if (value.Length != _inventorySize.Length)
					Debug.LogError(string.Format("{0} inventory size must be {1} dimensions, not {2}",
					                             name, _inventorySize.Length, value.Length));
				else {
					_inventorySize = value;
					SetPocketability();
				}
			}
		}

		public bool pocketable {
			get {
				return _pocketable;
			}
		}

		void SetPocketability() {
			_pocketable = _inventorySize.All(v => v > 0);
		}
	}
}
