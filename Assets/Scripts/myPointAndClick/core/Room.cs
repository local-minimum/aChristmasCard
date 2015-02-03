using UnityEngine;
using System.Collections.Generic;


namespace PointClick {

	public class Room : MonoBehaviour {

		[SerializeThis]
		private HashSet<Point> _points = new HashSet<Point>();

		[SerializeThis]
		private HashSet<WalkingPoint> _walkingPoints = new HashSet<WalkingPoint>();

		private PathFinder _paths;

		[SerializeField]
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

		public HashSet<WalkingPoint> walkingPoints {
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
			if (_points.Contains(point))
				return;

			_points.Add(point);
			if (point.isType<WalkingPoint>())
				_walkingPoints.Add((WalkingPoint) point);

			point.transform.parent = pointsOrganizer;
			point.gameObject.layer = gameObject.layer;
		}

		public void AddPlayer(Player player) {
			player.transform.parent = transform;
		}

		public void Add(GameEntity entity) {

			if (entity.room == this)
				return;
			else if (entity.room)
				entity.room.Remove(entity);

			if (entity.isTypeOrSubclass<Point>())
				AddPoint((Point) entity);
			else if (entity.isType<Player>())
				AddPlayer((Player) entity);
			else if (entity.isType<PathFinder>()) {
				if (!_paths)
					_paths = (PathFinder) entity;
			} else
				Debug.LogError(string.Format("{0} ({1}) not added to {2}", entity.name, entity.GetType(), name));
		}

		protected void Remove(GameEntity entity) {
			if (entity.isType<Point>())
				points.Remove((Point) entity);
			else if (entity.isType<WalkingPoint>()) {
				points.Remove((Point) entity);
				walkingPoints.Remove((WalkingPoint) entity);
			} else if (entity.isType<Player>()) {

			} else
				Debug.LogError(string.Format("{0} ({1}) unknown remove from {2}", entity.name, entity.GetType(), name));
		}
	}

}
