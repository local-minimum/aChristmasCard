using UnityEngine;
using System.Collections;

namespace PointClick {

	public abstract class GameEntity : MonoBehaviour {

		[SerializeThis]
		private Room _room;

		private bool _updatingRoom = false;

		public Room room { 
			get  {
				if (!_updatingRoom && !_room)
					SetRoomFromParent();

				return _room;
			}
			
			set {
				if (value) {
					_updatingRoom = true;
					value.Add(this);
					_room = value;
					_updatingRoom = false;
				} else {
					Debug.LogError(string.Format("{0} is void of room, this is not allowed", name));
				}
			}
		}
		
		protected void SetRoomFromParent() {
			room = gameObject.GetComponentInParent<Room>();
		}

		public bool isType<T>() {
			return this.GetType() == typeof(T);
		}

		public bool isTypeOrSubclass<T>()
		{
			return GetType().IsSubclassOf(typeof(T)) || isType<T>();
		}
	}

}
