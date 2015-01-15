using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.UI;
using PointClick;

namespace PointClick {

	public class LevelManager : Singleton<LevelManager> {

		public PointClick.UI.MouseHover hoverCanvas;
		public Canvas screenCanvas;
		private Text debugText;
		private Text debugText2;
		public int debugFontSize;
		public Color debugFontColor = Color.red;
		public Color debugBgColor = Color.gray;

		[Range(0,1)]
		public float cameraSmoothness = 0.5f;

		private PlayerController _player;
		public Camera mainCamera;
		public bool debug = false;

		public static PlayerController Player {
			get {
				return instance.player;
			}
		}

		public static Camera MainCamera {
			get {
				return instance.mainCamera;
			}
		}

	//	public Texture2D mouseCursor;

		public PlayerController player {
			get {
				if (_player == null)
					_player = FindObjectOfType<PlayerController>();
				return _player;
			}
		}

		public float playTime {
			get {
				//TODO; add pauesefeature
				return Time.timeSinceLevelLoad;
			}
		}
		
		private HashSet<GameObject> _uiView = new HashSet<GameObject>();
		
		public bool uiView {
			get {
				return _uiView.Any();
			}

		}
		// Use this for initialization
		void Start () {
			if (!mainCamera)
				mainCamera = Camera.main;
			if (debug) {
				debugText = AddTextUI(new Vector2(150, 100), new Vector2(10, 10), Vector2.zero, Vector2.zero, "DebugTextUI");
				debugText2 = AddTextUI(new Vector2(150, 100), new Vector2(10, 120), Vector2.zero, Vector2.zero, "DebugWordList");
			}
	//		Cursor.SetCursor(mouseCursor, Vector2.zero, CursorMode.ForceSoftware);
		}

		Text AddTextUI(Vector2 size, Vector2 offset, string name) {
			return AddTextUI(size, offset, Vector2.up, Vector2.up, name);
		}

		Text AddTextUI(Vector2 size, Vector2 offset, Vector2 anchor, Vector2 pivot, string name) {
			GameObject gO = new GameObject();
			gO.name = name;
			gO.transform.parent = screenCanvas.transform;
			Image i = gO.AddComponent<Image>();
			i.color = debugBgColor;
			GameObject gOT = new GameObject();
			gOT.transform.parent = gO.transform;
			gOT.name = name + " Text";
			Text t = gOT.AddComponent<Text>();
			RectTransform rt2 = gOT.GetComponent<RectTransform>();
			rt2.anchorMax = Vector2.up;
			rt2.anchorMin = Vector2.up;
			rt2.sizeDelta = size - Vector2.one * 20;
			rt2.anchoredPosition = new Vector2(10, -10);
			rt2.pivot = Vector2.up;
			t.fontSize = debugFontSize;
			t.supportRichText = true;
			t.font =  (Font) Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");
			t.color = debugFontColor;
			RectTransform rt = gO.GetComponent<RectTransform>();
			rt.anchorMax = anchor;
			rt.anchorMin = anchor;
			rt.pivot = pivot;
			rt.anchoredPosition = offset;
			rt.sizeDelta = size;
			return t;
		}

		// Update is called once per frame
		void Update () {
			if (uiView) {
				if (LetterWriter.InLetterView) {
	//				if (LetterWriter.Cursor)
	//					LetterWriter.Cursor.position = Input.mousePosition;

				}
				return;
			} else 	if (player.cursor)
				player.cursor.position = Input.mousePosition;



			if (player.moveable)
				mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.room.cameraPosition, cameraSmoothness);
			else
				mainCamera.transform.position = Vector3.Lerp(mainCamera.transform.position, player.room.zoomPosition, cameraSmoothness);
		}

		void OnGUI() {
			if (debug) {
				debugText.text = string.Format("<b>Hovering {0}:</b>\n\t", _uiView.Count()) + string.Join("\n\t", _uiView.Select(gO => gO.name).ToArray());
				debugText2.text = string.Format(
					"<b>Word List index:</b>\n\tPages:\t{0}\n\tCurrent page:\t{1}\n\tHas Next:\t{2}\n\tHas Prev:\t{3}",
					WordList.Instance.Length, SaveState.WordListPage, WordList.Instance.HasNextPage(), WordList.Instance.HasPrevPage());
			}
		}

		public void SetUIFocus(GameObject ui) {
	//		Debug.Log(string.Format("Adding {0} to {1} long focus list", ui, _uiView.Count()));
			_uiView.Add(ui);
		}

		public void RemoveUIFocus(GameObject ui) {
			_uiView.Remove(ui);
		}

		public bool UIhasFocus(GameObject ui) {
			return _uiView.Contains(ui);
		}

		public T UIwithFocusByType<T>() where T : UnityEngine.Component {
			GameObject uiGO = _uiView.Where(gO => gO.GetComponent<T>() != null).FirstOrDefault();
			Debug.Log(string.Format("Current UI focus ({0}): {1}", _uiView.Count(), string.Join(", ", _uiView.Select(gO => gO.name).ToArray())));
			if (uiGO != null) 
				return uiGO.GetComponent<T>();
			return null;
		}
	}
}