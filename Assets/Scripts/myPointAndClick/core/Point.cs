using UnityEngine;
using System.Collections.Generic;


namespace PointClick {

	public class Point : MonoBehaviour {

		[SerializeField]
		public static float gizmoSphereSize = 0.1f ;

		public List<Point> connections = new List<Point>();

		public bool autoUpdate = true;

		private Room _room;
		public Room room { 
			get  {
				return _room;
			}

			set {
				if (value) {
					_room = value;
					_room.AddPoint(this);
				} else {
					Debug.LogError(string.Format("Point {0} is void of room, this is not allowed", name));
				}
			}
		}

		void Awake () {
			SetRoomFromParent();
		}

		void Start() {
			if (connections.Count == 0)
				SetConnections();
		}
		
		void SetRoomFromParent() {
			room = gameObject.GetComponentInParent<Room>();
		}

		protected virtual void SetConnections() {
			connections.Clear();
			connections.Add(room.paths.GetWalkingPointClosestTo(this));

		}

		public bool isBaseType() {
			return isType<Point>();
		}

		public bool isType<T>() {
			return this.GetType() == typeof(T);
		}

		void OnDrawGizmos() {

#if UNITY_EDITOR
			if (autoUpdate) {
				SetRoomFromParent();
				SetConnections();
			}
#endif

			foreach (Point pt in connections) {
				if (pt) {
					Gizmos.color = isType<WalkingPoint>() && pt.isType<WalkingPoint>() ? Color.blue : Color.red;
					Gizmos.DrawLine(transform.position, pt.transform.position); 
				}
			}

			Gizmos.color = isType<WalkingPoint>() ? Color.blue : Color.red;
			Gizmos.DrawSphere(transform.position, gizmoSphereSize);
		}

	}

}
