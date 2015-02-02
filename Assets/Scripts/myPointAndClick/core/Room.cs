using UnityEngine;
using System.Collections.Generic;


namespace PointClick {

	public class Room : MonoBehaviour {

		private HashSet<Point> _points = new HashSet<Point>();
		private HashSet<Point> _walkingPoints = new HashSet<Point>();

		private PathFinder _paths;

		[SerializeThis]
		private Transform _pointsOrganizer;

		public Transform pointsOrganizer {
			get {
				if (!_pointsOrganizer) {
					_pointsOrganizer = new GameObject().transform;
					_pointsOrganizer.transform.parent = transform;
					_pointsOrganizer.transform.localPosition = Vector3.zero;
					_pointsOrganizer.name = "Points Organizer";
				}
				return _pointsOrganizer;
			}
			set {
				_pointsOrganizer = value;
			}
		}

		public HashSet<Point> walkingPoints {
			get {
				return _walkingPoints;
			}

		}

		public HashSet<Point> points {
			get {
				return _points;
			}
		}

		public PathFinder paths {
			get {
				if (!_paths)
					SetupPathFinder();

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

			point.transform.parent = pointsOrganizer;
			point.gameObject.layer = gameObject.layer;
		}

		public void Add(GameEntity entity) {
			if (entity.isTypeOrSubclass<Point>())
				AddPoint((Point) entity);
		}


	}

}
