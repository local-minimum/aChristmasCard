using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using FunOps;

namespace PointClick {

	public struct Path {

		public Point[] points;
		private float _length;

		public float getLength() {
			return _length;
		}

		public Path(Point[] path) {
			this.points = path;
			this._length = GetPathLength(path);
		}

		private static float GetPathLength(Point[] path) {
			
			float length = 0f;
			for (int i=0, l=path.Length-1; i<l; i++)
				length += Vector3.Distance(path[i].transform.position, path[i + 1].transform.position);

			return length;
		}
	}

	public class PathFinder : MonoBehaviour {

		private Room _room;

		[SerializeThis]
		public float closeWalkingPointProximityThreshold = 3f;

		public Room room {
			get {
				if (!_room)
					_room = GetComponent<Room>();
				return _room;
			}
		}

		public WalkingPoint GetWalkingPointClosestTo(Point point) {
			return room.walkingPoints.Where(p => p != point).OrderBy(wp => Vector3.Distance(wp.transform.position, point.transform.position)).FirstOrDefault();
			
		}
		
		public WalkingPoint GetWalkingPointClosestTo(Vector3 position) {
			return room.walkingPoints.OrderBy(wp => Vector3.Distance(wp.transform.position, position)).FirstOrDefault();
		}
		
		public IEnumerable<WalkingPoint> GetWalkingPointsCloseTo(Point point) {
			return room.walkingPoints.Where(p => p != point && Vector3.Distance(p.transform.position, point.transform.position) < closeWalkingPointProximityThreshold);
		}

		public Point[] ClosestPathBetween(Point source, Point target) {

			if (source.connections.Contains(target))
			    return new Point[] {target};

			List<Path> paths = GetPossiblePathsFromPoint(source);

			int idCurPath = 0;
			while (idCurPath < paths.Count()) {
				paths.AddRange(GetNovelRelevantPaths(paths, paths[idCurPath].points));
				idCurPath ++;
			}

			return paths.Where(p => p.points.Last() == target).OrderBy(p => p.getLength()).First().points;
		}

		private IEnumerable<Path> GetNovelRelevantPaths(List<Path> paths, Point[] path) {

			foreach (Path candidatePath in GetPossiblePathExtensions(path)) {
				Point candidateEnd = candidatePath.points.Last();

				if (!(paths.Any(p => p.points.Last() == candidateEnd && 
				              p.getLength() < candidatePath.getLength()))) {

					yield return candidatePath;
				}
			}	                               
		}


		private List<Path> GetPossiblePathsFromPoint(Point point) {

			List<Path> paths = new List<Path>();
			foreach (Point connectedPoint in point.connections)
				paths.Add(new Path(new Point[] {connectedPoint}));
			return paths;
		}

		private List<Path> GetPossiblePathExtensions(Point[] path) {
			List<Path> pathExtensions = new List<Path>();
			Point point = path.Last();

			foreach (Point connectedPoint in point.connections) {
				if (!path.Contains(connectedPoint))
					pathExtensions.Add(new Path(path.Conj(connectedPoint)));
			}
			return pathExtensions;
		}

	}
}
