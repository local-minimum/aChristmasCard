﻿using UnityEngine;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	[Range(0, 4)]
	public float arriveIntermediate = 1f;

	[Range(0, 2)]
	public float arriveDestination = 0.1f;

	public bool moveable = true;
	private bool inTransition = false;

	public float force = 500;
	public float maxVelocity = 1f;

	public Room room;

	private List<InterestPoint> walkPath = new List<InterestPoint>();

	private float nextDistance = 0f;

	private Vector3 offset {
		get {
			return Vector3.up * (renderer.bounds.extents.y);
		}
	}


	private InterestPoint target {
		get {
			if (walkPath.Count == 0)
				return null;
			
			return walkPath[walkPath.Count - 1];
		}

		set {
			if (inTransition)
				walkPath.RemoveRange(2, walkPath.Count - 2);
			else
				walkPath.RemoveRange(1, walkPath.Count - 1);
			walkPath.AddRange(value.FindPathTo(walkPath[walkPath.Count - 1]));
			inTransition = true;
		}
	}

	private InterestPoint location {
		get {
			if (walkPath.Count == 0)
				return null;

			return walkPath[0];
		}

		set {
			while (walkPath[0] != value)
				walkPath.RemoveAt(0);

			if (walkPath.Count == 0)
				walkPath.Add(value);
		}
	}
	
	private Vector3 aim {
		get {
			if (walkPath.Count > 1)
				return (walkPath[1].transform.position - (transform.position - offset)).normalized;
			return Vector3.zero;
		}
	}

	// Use this for initialization
	void Start () {
		room = gameObject.GetComponentInParent<Room>();
		walkPath.Add(room.GetWalkingPointClosestTo(transform.position));
		ArrivedAt(location);
		transform.position = location.transform.position + offset;
	}
	
	// Update is called once per frame
	void Update () {
		if (CheckProximity())
			return;
	
		rigidbody.AddForce(aim * force * Time.deltaTime, ForceMode.Force);
		rigidbody.velocity = Vector3.ClampMagnitude(rigidbody.velocity, maxVelocity);

	}

	bool CheckProximity() {

		if (!inTransition)
			return true;

		nextDistance = Vector3.Distance(walkPath[1].transform.position, transform.position - offset);
		bool arrived = false;
//		Debug.Log(nextDistance);
		if (walkPath.Count == 2) {
			if (nextDistance < arriveDestination)
				arrived = true;
		} else if (nextDistance < arriveIntermediate)
			arrived = true;

		if (arrived)
			ArrivedAt(walkPath[1]);
		return arrived;
	}

	void ArrivedAt(InterestPoint pt) {
		gameObject.layer = pt.gameObject.layer;
		room = pt.room;
		location = pt;
		inTransition = target != null && pt != target;
		if (!inTransition)
			rigidbody.velocity = Vector3.zero;
	}

	public void SetInterest(InterestPoint pt) {
		if (target == pt)
			return;

		target = pt;
	}

}