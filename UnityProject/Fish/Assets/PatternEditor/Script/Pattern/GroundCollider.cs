using UnityEngine;
using System.Collections;

namespace GameWish.Game {
	[RequireComponent(typeof(BoxCollider))]
	public class GroundCollider: MonoBehaviour {

		public bool occupied = false;
		[HideInInspector]
		public Transform levelTile;

	}
}
