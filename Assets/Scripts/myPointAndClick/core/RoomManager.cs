using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	class CameraDistancePoints : Object {
		private Vector3 _pos;
		private float _dist;

		public Vector3 pos {
			get {
				return _pos;
			}
		}

		public float dist {
			get {
				return _dist;
			}
		}

		public CameraDistancePoints(Vector3 pos, float dist) {
			_pos = pos;
			_dist = dist;
		}

		public Vector3 Interpolate(CameraDistancePoints other) {
			float f = Vector3.Dot(LevelManager.Instance.player.transform.position, LevelManager.Instance.mainCamera.transform.right) - Vector3.Dot(pos, LevelManager.Instance.mainCamera.transform.right);
			f /= Vector3.Dot(other.pos - pos, LevelManager.Instance.mainCamera.transform.right);
			if (f < 0)
				return pos;
			return Vector3.Lerp(pos, other.pos, f);
		}
	}

	public class RoomManager : MonoBehaviour {

		public LayerMask interestPointLayers;

		private List<InterestPoint> interactions = new List<InterestPoint>();

		[HideInInspector]
		public List<InterestPoint> walkingPoints = new List<InterestPoint>();

		public bool playerInRoom {
			get {
				return LevelManager.Instance.player.room == this;
			}
		}

		public Transform[] cameraPositions;

		private Transform _zoomPosition;

		public GameObject ObjectsCollector;

		[Range(0f, 3f)]
		public float walkPointMaxDistConnector = 1f;

		public Vector3 zoomPosition {
			get {
				if (_zoomPosition)
					return _zoomPosition.position;
				return cameraPosition;
			}

		}

		public Vector3 cameraPosition {
			get {
				float pPos = Vector3.Dot(LevelManager.Instance.mainCamera.transform.right, LevelManager.Instance.player.transform.position - LevelManager.Instance.mainCamera.transform.position);
				float[] d = cameraPositions.Select(pt => Mathf.Abs(Vector3.Dot(LevelManager.Instance.mainCamera.transform.right, pt.position - LevelManager.Instance.mainCamera.transform.position) - pPos)).ToArray();

				CameraDistancePoints[] clostest = cameraPositions.Select((pt, i) => new CameraDistancePoints(pt.position, d[i])).OrderBy(e => e.dist).Take(2).ToArray();
				return clostest[0].Interpolate(clostest[1]);
			}
		}

		// Use this for initialization
		void Awake () {

			interactions.AddRange(gameObject.GetComponentsInChildren<InterestPoint>().Where(pt => !interactions.Contains(pt)));
			walkingPoints.AddRange(interactions.Where(ip => ip.walkingPoint));
			if (ObjectsCollector == null) {
				ObjectsCollector = new GameObject();
				ObjectsCollector.name = "Stuff";
				ObjectsCollector.transform.parent = gameObject.transform;
			}
		}
		
		// Update is called once per frame
		void Update() {
			if (playerInRoom && !LevelManager.Instance.uiView && !LevelManager.Instance.player.playerLocked)
				IdentifyInteraction();
		}

		public InterestPoint GetPointClosestTo(Vector3 pos) {
			return interactions.OrderBy(ip => Vector3.Distance(pos, ip.transform.position)).First();
		}

		public static RoomManager GetRoomOfPlayer(Vector3 pos) {
			return GameObject.FindObjectsOfType<RoomManager>()
				.Select(r => r.GetWalkingPointClosestTo(pos))
					.OrderBy(ip => Vector3.Distance(ip.transform.position, pos))
					.First()
					.room;
		}

		public InterestPoint GetWalkingPointClosestTo(Vector3 pos) {
			InterestPoint pt = GetPointClosestTo(pos);
			if (pt && !pt.walkingPoint)
				pt = pt.viewedFrom;
			return pt;
		}

		void IdentifyInteraction () {

			Ray ray = LevelManager.Instance.mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;

			if (!Physics.Raycast(ray, out hit, 30f, interestPointLayers))
				return;

			Debug.DrawLine(LevelManager.Instance.mainCamera.transform.position,
			               hit.point);
			if (Input.GetButtonDown("Fire1")) {
				LevelManager.Instance.player.SetInterest(GetPointClosestTo(hit.point));
			} else if (LevelManager.Instance.player.moveable) {
				LevelManager.Instance.hoverCanvas.SetHoverPoint(GetWalkingPointClosestTo(hit.point));
			}

		}

		public void SetZoomPosition(Transform t) {
			_zoomPosition = t;
		}

		public void AddInterest(InterestPoint pt) {
			if (!interactions.Contains(pt))
				interactions.Add(pt);
		}

		public void RemoveInterest(InterestPoint pt) {
			interactions.Remove(pt);
		}
	}
}