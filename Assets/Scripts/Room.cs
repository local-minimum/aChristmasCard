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
	void Start () {
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

	void IdentifyInteraction () {
		Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;

		if (!Physics.Raycast(ray, out hit, 30f, interestPointLayers))
			return;

		if (Input.GetMouseButtonDown(0)) {
			//dosomething
		} else {
			//hover
			InterestPoint pt = interactions.OrderBy(ip => Vector3.Distance(hit.point, ip.transform.position)).First();
			if (pt && !pt.walkingPoint)
				pt = pt.viewedFrom;

			hoverCanvas.SetHoverPoint(pt);
		}

	}
}
