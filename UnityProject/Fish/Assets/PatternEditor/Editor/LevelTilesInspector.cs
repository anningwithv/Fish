using UnityEngine;
using UnityEditor;
using System.Collections;

namespace GameWish.Game
{
	[CustomEditor(typeof(LevelTiles),true)]
	public class LevelTilesInspector : Editor {

		LevelTiles levelTile;

		public void OnEnable() {
			levelTile = (LevelTiles)target;
		}

		public override void OnInspectorGUI() {
			DrawDefaultInspector ();
			if (Application.isPlaying == false) {
				levelTile.InspectorUpdate ();
			}
		}
	}
}
