using UnityEngine;
using System.Collections.Generic;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace PointClick {
	
	public class InventoryTypes : Singleton<InventoryTypes> {

		[SerializeField]
		private string[] names = new string[32];

		[Range(0, 32)]
		public int size = 0;

#if UNITY_EDITOR
		[MenuItem("Edit/Project Settings/Inventory Types")]
		public static void SetInInspector() {
			Selection.activeObject = Instance;
		}
#endif

		public static int GetAllSelectedValue() {
			int intRepresentation = GetAllUnselectedValue();
			for (int position=0; position<Size;position++)
				intRepresentation += (1 << position);
			return intRepresentation;
		}

		public static int GetAllUnselectedValue() {
			return 0;
		}

		public static string GetName(int position) {
			return Instance.names[position];
		}

		public static void SetName(int position, string newName) {
			Instance.names[position] = newName;
		}

		public static int Size {
			get {
				return Instance.size;
			}
		}

		public static IEnumerable<int> PositionEnumerator {
			get {
				for (int position=0; position<Size; position++)
					yield return position;
			}
		}

		public static IEnumerable<string> NamesEnumerator {
			get {
				foreach (int position in PositionEnumerator)
					yield return GetName(position);
			}
		}
	}

	[System.Serializable]
	public class InventoryTypeRestriction {

		[HideInInspector]
		public int flags = 0;

		public bool HasIntersection(InventoryTypeRestriction other) {
			return (flags & other.flags) != 0;
		}
		
		public bool IsIdentical(InventoryTypeRestriction other) {
			return flags == other.flags;
		}

		public IEnumerable<string> SelectedNames {
			get {
				foreach (int position in InventoryTypes.PositionEnumerator) {
					if (IsSelected(position))
						yield return InventoryTypes.GetName(position);
				}
			}
		}

		public IEnumerable<InventoryTypeMap> AsEnumerableContent() {
			foreach (int position in InventoryTypes.PositionEnumerator) {
				string positionName = InventoryTypes.GetName(position);
				yield return new InventoryTypeMap(position, IsSelected(position), positionName);
			}
		}

		private bool IsSelected(int position) {
			return (flags & (1 << position)) != 0;
		}

		public void Set(int position) {
			flags |= (1 << position);
		}
		public void Unset(int position) {

			flags &= ~(1 << position);
		}

		public void SetAll() {
			flags = InventoryTypes.GetAllSelectedValue();
		}

		public void UnsetAll() {
			flags = InventoryTypes.GetAllUnselectedValue();
		}

		public bool Any() {
			return flags != InventoryTypes.GetAllUnselectedValue();
		}
	}

	public struct InventoryTypeMap {
		public int position;
		public bool selected;
		public string name;

		public InventoryTypeMap(int position, bool selected, string name) {
			this.position = position;
			this.selected = selected;
			this.name = name;
		}
	}
}
