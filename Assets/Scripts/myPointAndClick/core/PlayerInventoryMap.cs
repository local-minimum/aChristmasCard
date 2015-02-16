using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

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
	}

	public class InventoryFilter : object {

		Dictionary<string, bool> tags = new Dictionary<string, bool>();

		public InventoryFilter() {
		}

		public InventoryFilter(string[] keys) {
			foreach (string key in keys)
				tags.Add(key, true);
		}

		public InventoryFilter(Dictionary<string, bool> tags) {
			this.tags = tags;
		}

		public bool hasCommon(InventoryFilter other) {
			return tags.Where(kvp => other.tags.Where(okvp => okvp.Value && kvp.Key == okvp.Key).Any()).Any();
		}

		public InventoryFilter falseClone() {
			return new InventoryFilter(tags.Select(kvp => new {key=kvp.Key, val=false})
			                           .ToDictionary(t => t.key, t => t.val));
		}

		public void Set(string tag) {
			tags[tag] = true;
		}

		public void Unset(string tag) {
			tags[tag] = false;
		}

		public IEnumerable<string> Tags {
			get {
				return tags.Keys.AsEnumerable();
			}
		}
	}

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
	}

	public class PlayerInventoryMap : object {

		[SerializeField]
		private InventoryLayout layout;

		[SerializeField]
		private InventoryFilter _tagFilter;

		[SerializeThis]
		private Dictionary<int, InventoriedItem> items = new Dictionary<int, InventoriedItem>();

		public InventoryFilter tagFilter {
			get {
				return _tagFilter;
			}
		}

		public PlayerInventoryMap(int[] shape, string[] tags) {
			layout = new InventoryLayout(shape);
			_tagFilter = new InventoryFilter(tags);
		}

		public bool CanTake(Interactable interactable) {

			return tagFilter.hasCommon(interactable.inventoryTags) && fits(new InventoryVector(
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

		InventoryVector FindFirstPosition(InventoryVector shape) {
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

		int GetNewItemIdentifier() {
			int newId = 1;
			while (items.ContainsKey(newId))
				newId++;
			return newId;
		}

		IEnumerable<InventoryVector> coordLister(InventoryVector origin, InventoryVector shape) {
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

		void takeAt(InventoryVector origin, InventoryVector shape, int value) {
			foreach (InventoryVector coord in coordLister(origin, shape)) {
				if (value != 0 && layout[coord] != 0)
					Debug.LogWarning("Two overlapping items in inventory");
				layout[coord] = value;

			}
		}

		bool fitsAt(InventoryVector origin, InventoryVector shape) {

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
