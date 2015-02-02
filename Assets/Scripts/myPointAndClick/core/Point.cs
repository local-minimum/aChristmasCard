using UnityEngine;
using System.Collections.Generic;


namespace PointClick {

	public class Point : GameEntity {

		[SerializeField]
		public static float gizmoSphereSize = 0.1f ;

		public List<Point> connections = new List<Point>();

		public bool autoUpdate = true;

		void Start() {
			if (connections.Count == 0)
				SetConnections();
		}


		protected virtual void SetConnections() {
			connections.Clear();
			connections.Add(room.paths.GetWalkingPointClosestTo(this));

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
