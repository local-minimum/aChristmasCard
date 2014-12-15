using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Room : MonoBehaviour {

	public MouseHover hoverCanvas;

	public Camera mainCamera;

	public LayerMask interestPointLayers;

	private List<InterestPoint> interactions = new List<InterestPoint>();
	private List<InterestPoint> walkingPoints = new List<InterestPoint>();

	public bool playerInRoom = true;

	public PlayerController player;

	// Use this for initialization
	void Awake () {
		player = GameObject.FindObjectOfType<PlayerController>();
		interactions.AddRange(gameObject.GetComponentsInChildren<InterestPoint>());
		walkingPoints.AddRange(interactions.Where(ip => ip.walkingPoint));
		if (!mainCamera)
			mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update() {
		if (playerInRoom && player.moveable)
			IdentifyInteraction();
	}

	public InterestPoint GetPointClosestTo(Vector3 pos) {
		return interactions.OrderBy(ip => Vector3.Distance(pos, ip.transform.position)).First();
	}

	public InterestPoint GetWalkingPointClosestTo(Vector3 pos) {
		InterestPoint pt = GetPointClosestTo(pos);
		if (pt && !pt.walkingPoint)
			pt = pt.viewedFrom;
		return pt;
	}

	void IdentifyInteraction () {
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (!Physics.Raycast(ray, out hit, 30f, interestPointLayers))
			return;

		InterestPoint pt = GetWalkingPointClosestTo(hit.point);

		if (Input.GetMouseButtonDown(0)) {
			player.SetInterest(pt);
		} else {
			hoverCanvas.SetHoverPoint(pt);
		}

	}
}
