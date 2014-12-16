using UnityEngine;
using System.Collections.Generic;

public class LevelManager : Singleton<LevelManager> {

	public MouseHover hoverCanvas;

	[Range(0,1)]
	public float cameraSmoothness = 0.5f;

	private PlayerController _player;
	public Camera mainCamera;

	public PlayerController player {
		get {
			if (_player == null)
				_player = FindObjectOfType<PlayerController>();
			return _player;
		}
	}

	// Use this for initialization
	void Start () {
		if (!mainCamera)
			mainCamera = Camera.main;
	}
	
	// Update is called once per frame
	void Update () {
		mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.room.cameraPosition, cameraSmoothness);

	}
}
