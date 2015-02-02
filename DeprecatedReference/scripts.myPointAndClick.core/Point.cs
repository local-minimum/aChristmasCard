using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FunOps;


namespace PointClick.Old {

	/// <summary>
	/// Common base for all places of interest.
	/// Its purpose to navigate between interests
	/// </summary>
	public class Point : MonoBehaviour {

		public bool editMode = false;

		public List<Point> connections { get; set; }

		private RoomManager _room;

		public RoomManager room {
			get {
				if (_room == null) {
					_room = gameObject.GetComponentInParent<RoomManager>();
					if (_room)
						gameObject.layer = _room.gameObject.layer;
				}
				return _room;
			}
			
			set {
				if (_room != value)
					_room.RemoveInterest(this);
				if (value != null) {
					value.AddInterest(this);
					if (!GetComponentsInParent<Transform>().Contains(value.ObjectsCollector.transform))
						transform.parent = value.ObjectsCollector.transform;
					gameObject.layer = value.ObjectsCollector.layer;
				}
				_room = value;
			}
		}

		void Awake() {
			if (connections.Count == 0) 
				SetConnections();
		}

		[ExecuteInEditMode]
		protected virtual void Update () {
			if (editMode) {
				foreach (Point pt in connections)
					Debug.DrawLine(transform.position, pt.transform.position, Color.blue);
			}
		}

		public Point[] FindPathTo(Point other) {
			if (connections.Contains(other))
				return new pob[] {this};
			if (viewedFrom == other)
				return new Point[][] {other, this};
			
			List<Point[]> paths = new List<Point[]>();
			foreach (Point pt in connections)
				paths.Add(new Point[] {pt});
			
			int pos = 0;
			bool foundTarget = false;
			int foundAtLength = 0;
			
			while (pos < paths.Count()) {
				Point[] query = paths[pos];
				if (foundTarget && foundAtLength < query.Length )
					break;
				if (query.Last().AddNovelRelevantPaths(other, query, ref paths) && !foundTarget) {			    
					foundAtLength = query.Length;	
					foundTarget = true;
				}
				pos ++;
			}
			
			//		Debug.Log(pos);
			if (!paths.Any(p => p.Contains(other)))
			return new Point[] {};
			return paths.Where(p => p.Contains(other)).OrderBy(p => Distance(p)).First().Reverse().Skip(1).ToArray().Conj<Point>(this);
		}
		
		private float Distance(Point[] path) {
			
			float f = 0f;
			for (int i=0, l=path.Length-1; i<l;i++)
				f += Vector3.Distance(path[i].transform.position, path[i + 1].transform.position);
			return f;
		}

		public bool AddNovelRelevantPaths(Point target, Point[] query, ref List<Point[]> paths) {
			if (connections.Contains(target)) {
				paths.Add(query.Conj<InterestPoint>(target));
				return true;
			} 
			
			foreach (InterestPoint pt in connections) {
				if (!paths.Any(p => p.Contains(pt)) || paths.Where(p => p.Contains(pt)).Max(p => System.Array.IndexOf((InterestPoint[]) p, pt)) >= query.Length + 1) {
					Point[] newPath = query.Conj<Point>(pt);
					if (!newPath.Window().Any(pt1, pt2 => isBaseType(pt1) && !isBaseType(pt2)))
					    paths.Add(query.Conj<InterestPoint>(pt));

				}
			}
			
			return false;
		}
					                     
		private bool isBaseType(Point pt) {
			return pt.GetType() == typeof(Point);
		}

		public void SetConnections() {
			connections.Clear();
			connections.Add(room.walkingPoints.Where(p => p != this).OrderBy(wp => Vector3.Distance(wp.transform.position, transform.position)).First());
		}
	}

}