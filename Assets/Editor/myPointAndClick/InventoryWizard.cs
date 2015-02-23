using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick;

public class InventoryWizard : ScriptableWizard {

	protected PlayerInventory inventories;
	protected PlayerInventoryMap inventory = null;
	public string inventory_name = "backpack";
	public InventoryTypeRestriction permissableInventoryTypes = new InventoryTypeRestriction();
	public int dimensions = 2;

	private Vector2 scrollPosition;
	private int[] shape = new int[3] {5, 5, 1};

	public static void NewInventory(PlayerInventory inventories) {
		InventoryWizard wiz = ScriptableWizard.DisplayWizard<InventoryWizard>("Add Inventory", "Create");
		wiz.inventories = inventories;
	}

	public static void EditInventory(PlayerInventory inventories, PlayerInventoryMap inventory) {
		InventoryWizard wiz = ScriptableWizard.DisplayWizard<InventoryWizard>("Edit Inventory", "Apply");
		wiz.inventory = inventory;
		wiz.inventory_name = inventory.name;
		wiz.dimensions = inventory.dimensions;
		wiz.shape = inventory.shape;
		wiz.inventories = inventories;
		wiz.permissableInventoryTypes = inventory.permissableObjects;
	}

	void OnGUI() {
		inventory_name = EditorGUILayout.TextField("Name", inventory_name);
		dimensions = EditorGUILayout.IntSlider("Dimensions", dimensions, 0, 3);
		if (dimensions>0) 
			DrawDimensions();
		else
			shape[0] = 1;

		DrawRestrictions();

		DrawApplyButton();
	}

	void DrawDimensions() {
		EditorGUILayout.LabelField("Shape");

		GUILayout.BeginHorizontal();
		EditorGUILayout.Space();
		for (int i=0;i<3;i++) {
			if (i<dimensions) {
				if (i>0)
					EditorGUILayout.LabelField("X", GUILayout.Width(15));
				shape[i] = EditorGUILayout.IntField(shape[i], GUILayout.Width(75));
			}
			if (shape[i] < 1 || i >= dimensions)
				shape[i] = 1;
		}
		GUILayout.EndHorizontal();
	}

	void DrawRestrictions() {
		EditorGUILayout.LabelField("Permissable objects");
		EditorGUI.indentLevel += 1;

		if (InventoryTypes.Size == 0) {
			EditorGUILayout.HelpBox("You need to define inventory types", MessageType.Info);
			GUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button("View and set inventory types", GUILayout.Width(200))) {
			}
			EditorGUILayout.Space();
			GUILayout.EndHorizontal();
		} else {
			EditorGUILayout.BeginHorizontal();
			EditorGUILayout.Space();
			if (GUILayout.Button("Select All", GUILayout.Width(110)))
				permissableInventoryTypes.SetAll();
			if (GUILayout.Button("Unselect All", GUILayout.Width(110)))
				permissableInventoryTypes.UnsetAll();

			EditorGUILayout.Space();
			EditorGUILayout.EndHorizontal();
			scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition, false, true);
			foreach (InventoryTypeMap map in permissableInventoryTypes.AsEnumerableContent()) {
				if (EditorGUILayout.Toggle(map.name == "" ? "[undefined]" : map.name, map.selected))
					permissableInventoryTypes.Set(map.position);
				else
					permissableInventoryTypes.Unset(map.position);
			}
			EditorGUILayout.EndScrollView();
		}

		EditorGUI.indentLevel -= 1;
	}

	void DrawApplyButton() {
		GUILayout.BeginHorizontal();
		EditorGUILayout.Space();

		if (!permissableInventoryTypes.Any()) {
			EditorGUILayout.HelpBox("At least one object type is needed", MessageType.Info);
		} else if (inventory == null) {
			if (GUILayout.Button("Add", GUILayout.Width(150))) {
				OnWizardCreate();
			}
		} else {
			if (GUILayout.Button("Update", GUILayout.Width(150))) {
				OnWizardCreate();
			}
		}

		GUILayout.EndHorizontal();
	}

	void OnWizardCreate() {
		if (inventory != null)
			UpdateInventory();
		else
			AddInventory();

		Close();
	}

	void AddInventory() {
		inventories.AddInventory(inventory_name, shape, permissableInventoryTypes);
	}

	void UpdateInventory() {
		inventory.name = inventory_name;
		inventory.shape = shape;
		inventory.permissableObjects = permissableInventoryTypes;

	}

}
