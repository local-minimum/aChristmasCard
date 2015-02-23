using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick;

public class InventoryWizard : ScriptableWizard {

	protected PlayerInventory inventories;
	protected PlayerInventoryMap inventory = null;
	public string inventory_name = "backpack";
	
	public int dimensions = 2;

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
	}

	void OnGUI() {
		inventory_name = EditorGUILayout.TextField("Name", inventory_name);
		dimensions = EditorGUILayout.IntSlider("Dimensions", dimensions, 0, 3);
		if (dimensions>0) 
			GetDimensions();
		else
			shape[0] = 1;


	}

	void GetDimensions() {
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

	void OnWizardCreate() {
		if (inventory != null)
			UpdateInventory();
		else
			AddInventory();
	}

	void AddInventory() {
		inventories.AddInventory(inventory_name, shape);
	}

	void UpdateInventory() {
		inventory.name = inventory_name;

	}

}
