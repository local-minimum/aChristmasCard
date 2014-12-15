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

	// Use this for initialization
	void Start () {
		interactions.AddRange(gameObject.GetComponentsInChildren<InterestPoint>());
		walkingPoints.AddRange(interactions.Where(ip => ip.walkingPoint));
		if (!mainCamera)
			mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update() {
		IdentifyInteraction();
	}

	void IdentifyInteraction () {
		if (playerInRoom) {
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
}
