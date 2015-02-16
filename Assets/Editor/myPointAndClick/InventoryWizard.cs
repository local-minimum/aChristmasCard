using UnityEngine;
using System.Collections;
using UnityEditor;
using PointClick;

public class InventoryWizard : ScriptableWizard {

	protected PlayerInventory inventories;
	protected PlayerInventoryMap inventory = null;
	public string inventory_name = "backpack";

	[Range(0,3)]
	public int dimensions;

	private int[] shape = new int[3];

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
