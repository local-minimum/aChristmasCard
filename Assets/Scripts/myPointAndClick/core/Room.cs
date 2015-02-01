using UnityEngine;
using System.Collections.Generic;


namespace PointClick {

	public class Room : MonoBehaviour {

		private List<Point> _points = new List<Point>();
		private List<Point> _walkingPoints = new List<Point>();

		private PathFinder _paths;

		public List<Point> walkingPoints {
			get {
				return _walkingPoints;
			}

		}

		public List<Point> points {
			get {
				return _points;
			}
		}

		public PathFinder paths {
			get {
				return _paths;
			}
		}

		void Awake () {
			SetupPathFinder();
		}

		void SetupPathFinder() {
			_paths = GetComponent<PathFinder>();
			if (!_paths) {
				_paths = (PathFinder) gameObject.AddComponent<PathFinder>();
			}
		}

		public void AddPoint(Point point)
		{
			_points.Add(point);
			if (point.isType<WalkingPoint>())
				_walkingPoints.Add(point);

		}


	}

}
