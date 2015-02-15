using UnityEngine;
using System.Collections.Generic;
using System.Linq;

namespace PointClick {

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

	public struct InventoryPosition {
		public int[] anchor;
		public bool rotated;

		public InventoryPosition(int[] anchor) {
			this.anchor = anchor;
			rotated = false;
		}

		public InventoryPosition(int[] anchor, bool rotated) {
			this.anchor = anchor;
			this.rotated = rotated;
		}

		public bool valid {
			get {
				return anchor.Length > 0;
			}
		}
	}

	public struct InventoriedItem {
		public GameObject item;
		public Interactable interactable;
		public InventoryPosition position;

		public InventoriedItem(GameObject item, Interactable interactable, InventoryPosition position) {
			this.item = item;
			this.interactable = interactable;
			this.position = position;
		}
	}

	public struct InventoryVector {
		public int x;
		public int y;
		public int z;

		public InventoryVector(int x, int y, int z) {
			this.x = x;
			this.y = y;
			this.z = z;
		}
	}

	public class PlayerInventoryMap : object {

		[SerializeField]
		private InventoryVector _shape = new InventoryVector();

		[SerializeField]
		private int[,,] layout;

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
			_shape = AsInventoryVector(shape);
			layout = new int[_shape.x,_shape.y,_shape.z];
			_tagFilter = new InventoryFilter(tags);
		}

		public bool CanTake(Interactable interactable) {

			return tagFilter.hasCommon(interactable.inventoryTags) && fits(AsInventoryVector(
				interactable.inventoryShape));
			
		}

		public void Take(GameObject item, Interactable interactable) {
			InventoryPosition pos = FindFirstPosition(AsInventoryVector(interactable.inventoryShape));
			if (pos.valid) {
				int itemId = GetNewItemIdentifier();
				items.Add(itemId, new InventoriedItem(item, interactable, pos));

			}
		}

		InventoryPosition FindFirstPosition(InventoryVector shape) {
			for (int x=0; x<=_shape.x - shape.x; x++) {
				for (int y=0; y<=_shape.y - shape.y; y++) {
					for (int z=0; z<=_shape.z - shape.z; z++) {
						if (empty(new InventoryVector(x, y, z), shape))
							return new InventoryPosition(new int[3] {x, y, z});
					}
				}
			}
			return new InventoryPosition();
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

		bool empty(InventoryVector origin, InventoryVector shape) {

			foreach (InventoryVector coord in coordLister(origin, shape)) {
				if (layout[coord.x, coord.y, coord.z] != 0)
					return false;
			}
			return true;
		}

		private bool fits(InventoryVector shape) {
			return FindFirstPosition(shape).valid;
		}

 		InventoryVector AsInventoryVector(int[] arr) {
			arr = _PrepareArrayForInventoryVector(arr).ToArray();
			return new InventoryVector(arr[0], arr[1], arr[2]);
		}
		
		IEnumerable<int> _PrepareArrayForInventoryVector(int[] arr) {
			for (int i=0; i<3; i++)
				yield return i < arr.Length ? arr[i] : 1;
		}

	}

}
