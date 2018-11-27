using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

namespace GameWish.Game {
	public class Pattern : MonoBehaviour {

		public enum Type {
			Start,
			Connector,
			Easy,
			Medium,
			Hard,
			Enemy,
			End,
			Introductory
		};

		public enum EnemyType
		{
			Easy,
			Easy_Boss,
			Medium,
			Medium_Boss,
			Hard,
			Hard_Boss
		};

		public int enemyCount = 0;

		#if UNITY_EDITOR
		[MenuItem("Pattern Editor/New Pattern")]
		static void CreateNewPattern() {
			try {
				GameObject existingPattern = GameObject.FindGameObjectWithTag ("Pattern");
				if (existingPattern) {
					if (EditorUtility.DisplayDialog ("Are you sure?",
						"Do you want to delete the current pattern on screen?",
						"Yes", "No")) {
						DestroyImmediate (existingPattern);
					}
				}
				GameObject newPattern = new GameObject ("Pattern");
				newPattern.AddComponent<Pattern> ();
				newPattern.tag = "Pattern";
				Selection.activeGameObject = newPattern;
			} catch {
				Debug.LogWarning ("There is no tag named 'Pattern'. Please Create a tag named 'Pattern'.");
				EditorUtility.DisplayDialog ("There is no tag named 'Pattern'",
					"Please Create a tag named 'Pattern'.", "Ok");
			}
		}

		[MenuItem("Pattern Editor/View All Pattern")]
		static void ViewAllPattern() {
			string patternPrefabFolder = Application.dataPath + "/" + PatternSettings.patternPath;
			string[] patternPrefabNames = System.IO.Directory.GetFiles (patternPrefabFolder, "*.prefab");

			Transform allPatternContainer = new GameObject ("All Patterns").transform;

			float patternXDiff = PatternSettings.gridX * PatternSettings.tiledSize * 1.25f;
			float patternZ = 0;
			int numberOfPatternsInColumn = 10;

			for (int i = 0; i < patternPrefabNames.Length; i++) {
				string filename = System.IO.Path.GetFileNameWithoutExtension (patternPrefabNames [i]);
				GameObject selectedPrefab = AssetDatabase.LoadAssetAtPath ("Assets/" + PatternSettings.patternPath + filename + ".prefab", typeof(GameObject)) as GameObject;
				//Debug.Log ("Assets/" + PatternSettings.patternPath + filename + ".prefab");
				//Debug.Log (selectedPrefab.name);

				GameObject selectedPattern = PrefabUtility.InstantiatePrefab (selectedPrefab) as GameObject;
				if (i % numberOfPatternsInColumn != 0) {
					patternZ += selectedPattern.GetComponent<Pattern> ().gridY * 0.75f;
				} else {
					patternZ = 0;
				}
				selectedPattern.transform.parent = allPatternContainer;
				//Debug.Log (patternZ);
				selectedPattern.transform.localPosition = new Vector3 (Mathf.FloorToInt(i / numberOfPatternsInColumn) * patternXDiff, 0, patternZ*PatternSettings.tiledSize);
				Selection.activeTransform = selectedPattern.transform;
				patternZ += selectedPattern.GetComponent<Pattern> ().gridY * 0.75f;
			}
		}
		#endif

		public Type type = Type.Start;
		public EnemyType enemyType = EnemyType.Easy;
		public int gridY = 7;
		[HideInInspector]
		public float yOffset = 0;
        [HideInInspector]
        public float xOffset = 0;
        //Used to match patterns to prevent using patterns that result in a dead end
        public int topEntrances = 0;
		public int bottomEntrances = 0;

		#if UNITY_EDITOR
		private void OnDrawGizmosSelected() {
			Handles.Label (transform.position, transform.name);

            xOffset = 0;
            if (PatternSettings.gridX % 2 == 0)
            {
                xOffset = 0.5f * PatternSettings.tiledSize;
            }

            yOffset = 0;
            if (gridY % 2 == 0)
            {
                yOffset = 0.5f * PatternSettings.tiledSize;
            }

            float gridWidth = PatternSettings.tiledSize * PatternSettings.gridX;
			float gridHeight = PatternSettings.tiledSize * gridY;

			Vector3 bottomLeft = new Vector3 (transform.position.x - gridWidth * 0.5f, (transform.position.y - gridHeight * 0.5f) - yOffset, transform.position.z);
			Vector3 bottomRight = new Vector3(transform.position.x + gridWidth * 0.5f, (transform.position.y - gridHeight * 0.5f) - yOffset, transform.position.z);
			Vector3 topLeft = new Vector3 (transform.position.x - gridWidth * 0.5f, (transform.position.y + gridHeight * 0.5f) - yOffset, transform.position.z);
			Vector3 topRight = new Vector3(transform.position.x + gridWidth * 0.5f, (transform.position.y + gridHeight * 0.5f) - yOffset, transform.position.z);

			Gizmos.color = Color.white;
			Gizmos.DrawLine (topLeft, topRight);
			Gizmos.DrawLine (topLeft, bottomLeft);
			Gizmos.DrawLine (bottomLeft, bottomRight);
			Gizmos.DrawLine (bottomRight, topRight);

			Gizmos.color = Color.cyan;
			//Gizmos.DrawLine (topRight+new Vector3(3,0,0), bottomRight+new Vector3(3,0,0));
			//Gizmos.DrawLine (topRight+new Vector3(3,0,0), topRight + new Vector3(1,3,0));
			//Gizmos.DrawLine (topRight+new Vector3(3,0,0), topRight + new Vector3(5,3,0));
			Handles.Label (bottomRight+new Vector3(3,0,0), "BOTTOM");
			Handles.Label (topRight+new Vector3(3,0,0), "TOP");

			Gizmos.color = Color.grey;
			for (int i = 1; i < PatternSettings.gridX; i++) {
				Gizmos.DrawLine(topLeft+new Vector3(PatternSettings.tiledSize*i,0,0),bottomLeft+new Vector3(PatternSettings.tiledSize*i,0,0));
			}
			for (int i = 1; i < gridY; i++) {
				Gizmos.DrawLine(bottomLeft+ new Vector3(0, i * PatternSettings.tiledSize, 0),bottomRight+new Vector3(0, i * PatternSettings.tiledSize, 0));
			}
		}
		#endif
	}
}
