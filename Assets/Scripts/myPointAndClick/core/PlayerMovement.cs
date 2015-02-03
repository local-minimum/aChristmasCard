using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	public class PlayerMovement : PlayerAspect {

		public bool snapToTargetLocation = true;

		public float arriveAtThreshold = 0.25f;

		private float lastDistance;

		[Range(0, 1)]
		public float aimingSpeed = 0.1f;

		public float force = 400f;

		public float maxVelocity = 40f;

		public float gizmoSize = 0.1f;

		public float stillThreshold = 0.001f;

		private Animator anim;

		public enum PlayerDirections {N, NW, W, SW, S, SE, E, NE, UP, DOWN, NONE};

		private Dictionary<PlayerDirections, string> directionTriggers = new Dictionary<PlayerDirections, string>();

		private PlayerDirections _previousDirection = PlayerDirections.NONE;

		public PlayerDirections direction {
			get {
				if (rigidbody.velocity.magnitude < stillThreshold) 
					return PlayerDirections.NONE;
				
				Vector3 normVel = rigidbody.velocity.normalized;

				if (normVel.y > stillThreshold)
					return PlayerDirections.UP;
				else if (normVel.y < -stillThreshold)
					return PlayerDirections.DOWN;

				float angle = Vector3.Angle(transform.right, normVel);
				int angleStep = Mathf.RoundToInt((angle-22.5f)/45f);
				if (angleStep == 0)
					return PlayerDirections.E;
				else if (angleStep == 1)
					return PlayerDirections.NE;
				else if (angleStep == 2)
					return PlayerDirections.N;
				else if (angleStep == 3)
					return PlayerDirections.NW;
				else if (angleStep == 4 || angleStep == -4)
					return PlayerDirections.W;
				else if (angleStep == 5 || angleStep == -3)
					return PlayerDirections.SW;
				else if (angleStep == 6 || angleStep == -2)
					return PlayerDirections.S;
				else if (angleStep == 7 || angleStep == -1)
					return PlayerDirections.SE;

				return PlayerDirections.NONE;
			}
		}

		[SerializeThis]
		private WalkingPoint _location;

		[SerializeThis]
		private List<Transform> _path = new List<Transform>();

		private bool walking = false;

		private Transform nextLocation;

		public WalkingPoint location {
			get {
				return _location;
			}
		}

		// Use this for initialization
		void Awake () {

			anim = GetComponent<Animator>();

			SetDirectionTriggers();
		}

		void SetDirectionTriggers() {
			directionTriggers[PlayerDirections.NONE] = "still";
			directionTriggers[PlayerDirections.DOWN] = "down";
			directionTriggers[PlayerDirections.E] = "east";
			directionTriggers[PlayerDirections.N] = "north";
			directionTriggers[PlayerDirections.NE] = "north east";
			directionTriggers[PlayerDirections.NW] = "north west";
			directionTriggers[PlayerDirections.S] = "south";
			directionTriggers[PlayerDirections.SE] = "south east";
			directionTriggers[PlayerDirections.SW] = "south west";
			directionTriggers[PlayerDirections.UP] = "up";
			directionTriggers[PlayerDirections.W] = "west";
		}

		void SetLocationByProximity() {
			_location = player.room.paths.GetWalkingPointClosestTo(transform.position);
			if (_location)
				transform.position = _location.transform.position;
			else
				Debug.LogWarning(string.Format("{0} has no home", name));
		}
		
		// Update is called once per frame
		void Update () {
			Walk();
		}

		void Walk() {

			if (!_location) {
				SetLocationByProximity();
				return;
			}

			if (!walking)
				return;

			if (HasArrived())
				ArrivedAtNextLocation();

			if (walking) {
				AddForce();
				UpdateWalkAnimation();
			} else
				rigidbody.velocity = Vector3.zero;
		}

		void SetWalkingState() {
			walking = _path.Count() > 0;
			if (!walking && snapToTargetLocation)
				transform.position = nextLocation.position;
			nextLocation = _path.FirstOrDefault();
			if (walking) {
				lastDistance = Vector3.Distance(transform.position, nextLocation.position);

			}
		}

		bool HasArrived() {

			float distanceToNext = Vector3.Distance(transform.position, nextLocation.position);
			return distanceToNext < arriveAtThreshold || distanceToNext > lastDistance;

		}

		void ArrivedAtNextLocation() {
			WalkingPoint walkingPoint = nextLocation.GetComponent<WalkingPoint>();

			nextLocation.BroadcastMessage("PlayerArrive", new WalkingMessage(player, _path.Count() == 1),
			                            SendMessageOptions.DontRequireReceiver);

			if (walkingPoint)
				_location = walkingPoint;

//			Debug.Log("---");
//			Debug.Log(nextTarget);
//			Debug.Log(_path.FirstOrDefault());
			_path.Remove(nextLocation);
//			Debug.Log(_path.FirstOrDefault());

			SetWalkingState();

		}

		void AddForce() {
			Vector3 aim = Vector3.Lerp(rigidbody.velocity.normalized,
			                           (_path.First().position - transform.position).normalized,
			                           aimingSpeed);

			rigidbody.AddForce(aim * force * Time.deltaTime, ForceMode.Acceleration);
			rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);

		}

		void UpdateWalkAnimation() {
			PlayerDirections currentDirection = direction;
			if (currentDirection != _previousDirection) {
				anim.SetTrigger(directionTriggers[direction]);
				_previousDirection = currentDirection;
			}
		}

		public void SetTarget(WalkingPoint target) {
			_path.Clear();
			_path.AddRange(
				player.room.paths.ClosestPathBetween((Point) _location, (Point) target).Select(wp => wp.transform));
			SetWalkingState();
		}

		public void ExtendPath(IEnumerable<Transform> pathExtension) {
			_path.AddRange(pathExtension);
			SetWalkingState();
		}

		void OnDrawGizmosSelected() {
			Gizmos.color = Color.green;
			if (walking) {
				Gizmos.DrawSphere(transform.position, gizmoSize);
				Gizmos.DrawRay(transform.position, rigidbody.velocity);
			} else 
				Gizmos.DrawCube(transform.position, Vector3.one * gizmoSize);

		}
	}
}
