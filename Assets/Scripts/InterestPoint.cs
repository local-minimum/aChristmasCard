﻿using UnityEngine;
using System.Collections.Generic;

public class InterestPoint : MonoBehaviour {

	public bool editMode = false;

	public bool walkingPoint = true;

	public InterestPoint viewedFrom;

	public List<InterestPoint> connections = new List<InterestPoint>();

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	[ExecuteInEditMode]
	void Update () {
		if (editMode) {
			foreach (InterestPoint ip in connections)
				Debug.DrawLine(transform.position, ip.transform.position, Color.blue);
		}
	}
}
