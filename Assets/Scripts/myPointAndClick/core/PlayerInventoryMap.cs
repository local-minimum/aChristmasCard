using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

	[System.Serializable]
	public struct InventoryLayout {


		public InventoryVector shape;
		public int[,,] grid;

		public int this[InventoryVector pos] {
			get {
				return grid[pos.x, pos.y, pos.z];
			}

			set {
				grid[pos.x, pos.y, pos.z] = value;
			}
		}

		public InventoryLayout(int[] shapeArray) {
			shape = new InventoryVector(shapeArray);
			grid = new int[shape.x, shape.y, shape.z];
		}

		public int dimensions {
			get {
				if (shape.x <= 1)
					return 0;
				else if (shape.y <= 1)
					return 1;
				else if (shape.z <= 1)
					return 2;
				else
					return 3;
			}
		}

		public void Resize(int[] size) {
			shape = new InventoryVector(size);
		}
	}

	[System.Serializable]
	public struct InventoriedItem {
		public GameObject item;
		public Interactable interactable;
		public InventoryVector position;
		public InventoryVector shape;

		public InventoriedItem(GameObject item, Interactable interactable, InventoryVector position) {
			this.item = item;
			this.interactable = interactable;
			this.position = position;
			this.shape = new InventoryVector(interactable.inventoryShape);
		}
	}

	[System.Serializable]
	public struct InventoryVector {

		private static InventoryVector _outOfBounds = new InventoryVector(-1, -1, -1);

		public int x;
		public int y;
		public int z;

		public InventoryVector(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}

		public InventoryVector(int[] shapeArray) {
			shapeArray = InventoryVector._PrepareArrayForInventoryVector(shapeArray).ToArray();
			this.x = shapeArray[0];
			this.y = shapeArray[1];
			this.z = shapeArray[2];
		}

		public static InventoryVector Invalid {
			get {
				return _outOfBounds;
			}
		}

		static IEnumerable<int> _PrepareArrayForInventoryVector(int[] arr) {
			for (int i=0; i<3; i++)
				yield return i < arr.Length ? arr[i] : 1;
		}

		public bool valid {
			get {
				return x >= 0 && y >= 0 && z >= 0;
			}
		}

		public static int[] AsArray(InventoryVector v) {
			return new int[3] {v.x, v.y, v.z};
		}
	}

	[System.Serializable]
	public class PlayerInventoryMap {

		[SerializeField]
		public string name;

		[SerializeField]
		private InventoryLayout layout;

		[SerializeField]
		private InventoryTypeRestriction _permissableObjects;

		[SerializeField]
		private Dictionary<int, InventoriedItem> items = new Dictionary<int, InventoriedItem>();

		public InventoryTypeRestriction permissableObjects {
			get {
				return _permissableObjects;
			}

			set {
				_permissableObjects = value;
			}
		}

		public string sizeString {
			get {
				int dimensions = layout.dimensions;

				if (dimensions == 0)
					return "1";
				else if (dimensions == 1)
					return layout.shape.x.ToString();
				else if (dimensions == 2)
					return string.Format("{0}x{1}", layout.shape.x, layout.shape.y);
				else
					return string.Format("{0}x{1}x{2}", layout.shape.x, layout.shape.y, layout.shape.z);
			}
		}

		public int[] shape {
			get {
				return InventoryVector.AsArray(layout.shape);
			}

			set {
				layout.Resize(value);
			}
		}

		public int dimensions {
			get {
				return layout.dimensions;
			}
		}

		public PlayerInventoryMap(string name, int[] shape, InventoryTypeRestriction permissableObjects) {
			this.name = name;
			layout = new InventoryLayout(shape);
			_permissableObjects = permissableObjects;
		}

		public bool CanTake(Interactable interactable) {

			return permissableObjects.HasIntersection(interactable.inventoryType) && fits(new InventoryVector(
				interactable.inventoryShape));
			
		}

		public void Take(GameObject item, Interactable interactable) {
			InventoryVector shape = new InventoryVector(interactable.inventoryShape);
			InventoryVector anchor = FindFirstPosition(shape);
			if (anchor.valid) {
				int itemId = GetNewItemIdentifier();
				items.Add(itemId, new InventoriedItem(item, interactable, anchor));
				takeAt(anchor, shape, itemId);
			}
		}

		public GameObject Drop(InventoryVector position) {
			int itemId = layout[position];
			if (itemId > 0) {
				InventoriedItem item = items[itemId];
				items.Remove(itemId);
				takeAt(item.position, item.shape, 0);
				return item.item;
			}
			return null;
		}

		private InventoryVector FindFirstPosition(InventoryVector shape) {
			for (int x=0; x<=layout.shape.x - shape.x; x++) {
				for (int y=0; y<=layout.shape.y - shape.y; y++) {
					for (int z=0; z<=layout.shape.z - shape.z; z++) {
						if (fitsAt(new InventoryVector(x, y, z), shape))
							return new InventoryVector(new int[3] {x, y, z});
					}
				}
			}
			return InventoryVector.Invalid;
		}

		private int GetNewItemIdentifier() {
			int newId = 1;
			while (items.ContainsKey(newId))
				newId++;
			return newId;
		}

		private IEnumerable<InventoryVector> coordLister(InventoryVector origin, InventoryVector shape) {
			int xMax = origin.x + shape.x;
			int yMax = origin.y + shape.y;
			int zMax = origin.z + shape.z;
			for (int xx=origin.x; xx<xMax; xx++) {
				for (int yy=origin.y; yy<yMax;yy++) {
					for (int zz=origin.z; zz<zMax;zz++)
						yield return new InventoryVector(xx, yy, zz);
				}
			}
		}

		private void takeAt(InventoryVector origin, InventoryVector shape, int value) {
			foreach (InventoryVector coord in coordLister(origin, shape)) {
				if (value != 0 && layout[coord] != 0)
					Debug.LogWarning("Two overlapping items in inventory");
				layout[coord] = value;

			}
		}

		private bool fitsAt(InventoryVector origin, InventoryVector shape) {

			foreach (InventoryVector coord in coordLister(origin, shape)) {
				if (layout[coord] != 0)
					return false;
			}
			return true;
		}

		private bool fits(InventoryVector shape) {
			return FindFirstPosition(shape).valid;
		}

	}

}
