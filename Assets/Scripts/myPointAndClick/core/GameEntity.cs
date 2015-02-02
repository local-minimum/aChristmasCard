using UnityEngine;
using System.Collections;

namespace PointClick {

	public abstract class GameEntity : MonoBehaviour {

		private Room _room;
		public Room room { 
			get  {
				if (_room)
					SetRoomFromParent();
				return _room;
			}
			
			set {
				if (value) {
					_room = value;
					_room.Add(this);
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
