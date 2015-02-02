using UnityEngine;
using System.Collections;

namespace PointClick {

	public abstract class PlayerAspect : MonoBehaviour {
		
		private Player _player;

		protected Player player {
			get {
				if (!_player) 
					_player = GetComponent<Player>();
				return _player;
				
			}
		}
	}
}
