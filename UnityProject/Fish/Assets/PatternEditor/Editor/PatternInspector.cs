using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

namespace GameWish.Game
{
	[CustomEditor(typeof(Pattern))]
	public class PatternInspector : Editor {

		Pattern pattern;
		bool inEditMode = false;
		bool placeSolitary = true;
		string[] levelTileNames;
		int selectedLevelTile = 0;
		bool primedForPrefab = false;
		float instantiatingDelay = 1f;
		float currentInstantiatingDelay = 0f;

		public void OnEnable() {
			#if UNITY_EDITOR
			EditorApplication.update += OnEditorUpdate;
			#endif

			pattern = (Pattern)target;

			string levelTilesFolder = Application.dataPath + "/" + PatternSettings.levelTilePath;
			string[] levelTileStrings = System.IO.Directory.GetFiles (levelTilesFolder, "*.prefab");

			levelTileNames = new string[levelTileStrings.Length];
			for (int i = 0; i < levelTileStrings.Length; i++) {
				string filename = System.IO.Path.GetFileNameWithoutExtension (levelTileStrings [i]);
				levelTileNames [i] = filename;
			}

			if (PrefabUtility.GetPrefabType (pattern.gameObject) != PrefabType.Prefab) {
				InitializePattern ();
			}

			Tools.current = Tool.View;
			Tools.viewTool = ViewTool.FPS;
		}

		private void InitializePattern() {
			LevelTiles[] children = pattern.GetComponentsInChildren<LevelTiles> ();
			for (int i = 0; i < children.Length; i++) {
				children [i].Initialize ();
			}
		}

		private void InitializePatternFlipX() {
			LevelTiles[] children = pattern.GetComponentsInChildren<LevelTiles> ();
			for (int i = 0; i < children.Length; i++) {
				children [i].Initialize ();
				children [i].transform.localPosition = new Vector3 (-children [i].transform.localPosition.x, children [i].transform.localPosition.y, children [i].transform.localPosition.z);
				//rename the child tile
				Vector2 newTilePosition = GetTilePosition(children[i].transform.position);
				children[i].name = string.Format ("Tile_{0}_{1}", newTilePosition.x, newTilePosition.y);
			}
		}

		public override void OnInspectorGUI() {
			serializedObject.Update();

			bool hasPrefab = true;
			bool notAPrefab = PrefabUtility.GetPrefabType (pattern.gameObject) != PrefabType.Prefab;
			if (pattern.transform.parent) {
				if (pattern.transform.parent.name == "All Patterns") {
					notAPrefab = false;
				}
			}

			if (PrefabUtility.GetPrefabParent (pattern.gameObject) == null || PrefabUtility.GetPrefabObject (pattern.transform) == null) {
				hasPrefab = false;
			}

			GUI.color = Color.white;
			if (notAPrefab) {
				if (hasPrefab) {
					GUI.color = Color.green;
					if (GUILayout.Button ("Update Pattern")) {
						UpdatePrefab ();
					}
				} else {
					GUI.color = Color.red;
					if (GUILayout.Button ("Save Pattern")) {
						SaveDialog ();
					}
				}
			} else {
				GUI.color = Color.green;
				bool readyForEditing = true;
				if (pattern.transform.parent) {
					if (pattern.transform.parent.name == "All Patterns") {
						readyForEditing = false;
					}
				}
				if (readyForEditing) {
					if (GUILayout.Button ("Edit Pattern")) {
						EditPattern ();
					}
				}
			}

			GUI.color = Color.white;

			pattern.type = (Pattern.Type)EditorGUILayout.EnumPopup ("Pattern Type", pattern.type);

			if (pattern.type == Pattern.Type.Enemy) {
				pattern.enemyType = (Pattern.EnemyType)EditorGUILayout.EnumPopup ("Enemy Type", pattern.enemyType);
				int tempEnemyCount = EditorGUILayout.IntField ("Enemy Count", pattern.enemyCount);
				if (tempEnemyCount != pattern.enemyCount) {
					pattern.enemyCount = tempEnemyCount;
					if (pattern.enemyCount < 0) {
						pattern.enemyCount = 0;
					}
				}
			} else {
				pattern.enemyCount = 0;
			}

			if (notAPrefab) {

				int tempGridY;
				tempGridY = EditorGUILayout.IntField ("Grid Y", pattern.gridY);
				if (tempGridY != pattern.gridY) {
					//Check if there are children that lie outside the grid area
					if (TilesOutsideModifiedGrid (tempGridY) == false) {
						pattern.gridY = tempGridY;
						if (pattern.gridY < 1) {
							pattern.gridY = 1;
						}
					} else {
						Debug.Log ("There are tiles outside the grid area you are trying to use!");
					}
				}

				if (GUILayout.Button ("Clear Grid")) {
					if (EditorUtility.DisplayDialog ("Are you sure?",
						"Do you want to clear this pattern of all the level tiles?",
						"Yes", "No")) {
						ClearGrid ();
					}
				}

				if (GUILayout.Button ("Flip Pattern on X-Axis")) {
					if (EditorUtility.DisplayDialog ("Are you sure?",
						"Do you want to flip the pattern along it's X-axis?",
						"Yes", "No")) {
						InitializePatternFlipX ();
					}
				}

				if (GUILayout.Button ("Refresh Prefab Connections")) {
					InitializePattern ();
				}

				if (levelTileNames != null) {
					if (levelTileNames.Length > 0) {
						if (inEditMode) {
							GUI.color = Color.green;
							if (GUILayout.Button ("Disable Editing")) {
								inEditMode = false;
								placeSolitary = true;
							}
						} else {
							GUI.color = Color.white;
							if (GUILayout.Button ("Enable Editing")) {
								inEditMode = true;
							}
						}

						GUI.color = Color.white;
						placeSolitary = EditorGUILayout.Toggle ("Place Solitary", placeSolitary);

						GUILayout.Label ("Choose Level Tile To Place");
						selectedLevelTile = GUILayout.SelectionGrid (selectedLevelTile, levelTileNames, 3);
					}
				}

				EditorGUILayout.Separator ();
				GUILayout.Label ("Bottom Entrances");
				GUILayout.BeginHorizontal ();
				char[] strBottomEntrances = System.Convert.ToString (pattern.bottomEntrances, 2).ToCharArray();
				for (int i = 0; i < PatternSettings.gridX; i++) {
					if (i < strBottomEntrances.Length) {
						if (strBottomEntrances [strBottomEntrances.Length - 1 - i] == '1') {
							GUI.color = Color.green;
							if (GUILayout.Button (i.ToString ())) {
								strBottomEntrances [strBottomEntrances.Length - 1 - i] = '0';
								pattern.bottomEntrances = SetEntrancesInt (strBottomEntrances, -1);
							}
						} else {
							GUI.color = Color.red;
							if (GUILayout.Button (i.ToString ())) {
								strBottomEntrances [strBottomEntrances.Length - 1 - i] = '1';
								pattern.bottomEntrances = SetEntrancesInt (strBottomEntrances, -1);
							}
						}
					} else {
						GUI.color = Color.red;
						if (GUILayout.Button (i.ToString ())) {
							pattern.bottomEntrances = SetEntrancesInt (strBottomEntrances, i);
						}
					}
				}
				if (strBottomEntrances.Length > PatternSettings.gridX) {
					int difference = strBottomEntrances.Length - PatternSettings.gridX;
					for (int i = 0; i < difference; i++) {
						strBottomEntrances [i] = '0';
					}
					pattern.bottomEntrances = SetEntrancesInt (strBottomEntrances, -1);
				}
				GUILayout.EndHorizontal ();

				EditorGUILayout.Separator ();
				GUILayout.Label ("Top Entrances");
				GUILayout.BeginHorizontal ();
				char[] strTopEntrances = System.Convert.ToString (pattern.topEntrances, 2).ToCharArray();
				for (int i = 0; i < PatternSettings.gridX; i++) {
					if (i < strTopEntrances.Length) {
						if (strTopEntrances [strTopEntrances.Length - 1 - i] == '1') {
							GUI.color = Color.green;
							if (GUILayout.Button (i.ToString ())) {
								strTopEntrances [strTopEntrances.Length - 1 - i] = '0';
								pattern.topEntrances = SetEntrancesInt (strTopEntrances, -1);
							}
						} else {
							GUI.color = Color.red;
							if (GUILayout.Button (i.ToString ())) {
								strTopEntrances [strTopEntrances.Length - 1 - i] = '1';
								pattern.topEntrances = SetEntrancesInt (strTopEntrances, -1);
							}
						}
					} else {
						GUI.color = Color.red;
						if (GUILayout.Button (i.ToString ())) {
							pattern.topEntrances = SetEntrancesInt (strTopEntrances, i);
						}
					}
				}
				if (strTopEntrances.Length > PatternSettings.gridX) {
					int difference = strTopEntrances.Length - PatternSettings.gridX;
					for (int i = 0; i < difference; i++) {
						strTopEntrances [i] = '0';
					}
					pattern.topEntrances = SetEntrancesInt (strTopEntrances, -1);
				}
				GUILayout.EndHorizontal ();
			}

			serializedObject.ApplyModifiedProperties ();
		}


		private PreviewRenderUtility _previewRenderUtility;
		private void ValidateData() {
			if (_previewRenderUtility == null) {
				_previewRenderUtility = new PreviewRenderUtility ();

				_previewRenderUtility.camera.transform.position = new Vector3 (18.7f, 17.9f, -19.3f);
				_previewRenderUtility.camera.transform.rotation = Quaternion.Euler (35, 315, 0);
				_previewRenderUtility.camera.orthographic = true;
				_previewRenderUtility.camera.orthographicSize = 16;
				_previewRenderUtility.camera.nearClipPlane = 0.3f;
				_previewRenderUtility.camera.farClipPlane = 1000f;
			}
		}

		public override bool HasPreviewGUI() {
			ValidateData ();
			return true;
		}

		public override void OnPreviewGUI(Rect r, GUIStyle background) {
			if (Event.current.type == EventType.Repaint) {
				_previewRenderUtility.BeginPreview (r, background);



				LevelTiles[] levelTiles = (target as Pattern).gameObject.GetComponentsInChildren<LevelTiles> ();
				for (int i = 0; i < levelTiles.Length; i++) {
					
					Transform prefab = levelTiles[i].levelTilePrefab;
					MeshFilter[] meshFilters = prefab.GetComponentsInChildren<MeshFilter> ();
					for (int j = 0; j < meshFilters.Length; j++) {
						Vector3 scale = Vector3.Scale(meshFilters [j].transform.lossyScale,levelTiles[i].transform.localScale);
						Vector3 pos = levelTiles[i].transform.position + (meshFilters [j].transform.position - meshFilters[j].transform.root.position);
						Quaternion rot = Quaternion.Euler(levelTiles[i].transform.rotation.eulerAngles + meshFilters [j].transform.rotation.eulerAngles);
						Matrix4x4 matrix = Matrix4x4.TRS (pos, rot, scale);

						Material mat = meshFilters [j].transform.GetComponent<MeshRenderer> ().sharedMaterial;

						int subMeshCount = meshFilters [j].sharedMesh.subMeshCount;
						//Debug.Log (subMeshCount);

						_previewRenderUtility.DrawMesh (meshFilters [j].sharedMesh, matrix, mat, 0);
					}

					/**Transform[] childTransforms = prefab.GetComponentsInChildren<Transform> ();
					for (int j = 0; j < childTransforms.Length; j++) {
						if (childTransforms [j].GetComponent<MeshFilter> () != null) {
							MeshFilter myMeshFilter = childTransforms [j].GetComponent<MeshFilter> ();
							Vector3 scale = Vector3.Scale(myMeshFilter.transform.lossyScale,levelTiles[i].transform.localScale);
							Vector3 pos = levelTiles[i].transform.position + (myMeshFilter.transform.position - myMeshFilter.transform.root.position);
							Quaternion rot = Quaternion.Euler(levelTiles[i].transform.rotation.eulerAngles + myMeshFilter.transform.rotation.eulerAngles);
							Matrix4x4 matrix = Matrix4x4.TRS (pos, rot, scale);

							Material mat = myMeshFilter.transform.GetComponent<MeshRenderer> ().sharedMaterial;

							_previewRenderUtility.DrawMesh (myMeshFilter.sharedMesh, matrix, mat, 0);
						}
					}**/
				}



				_previewRenderUtility.camera.Render ();

				Texture resultRender = _previewRenderUtility.EndPreview ();

				GUI.DrawTexture (r, resultRender, ScaleMode.StretchToFill, false);
			}
		}

		void OnDestroy() {
			_previewRenderUtility.Cleanup ();
		}

		private int SetEntrancesInt(char[] strEntrances, int index) {
			string tmpStrEntrances = "";
			int returnInt;
			if (index == -1) {
				for (int i = 0; i < strEntrances.Length; i++) {
					tmpStrEntrances += strEntrances [i].ToString ();
				}
				returnInt = System.Convert.ToInt32 (tmpStrEntrances, 2);
				//Debug.Log (returnInt);
				return returnInt;
			}

			for (int i = index; i > -1; i--) {
				if (i == index) {
					tmpStrEntrances += "1";
				} else {
					if (i < strEntrances.Length) {
						tmpStrEntrances += strEntrances [strEntrances.Length-1-i].ToString();
					} else {
						tmpStrEntrances += "0";
					}
				}
			}
			returnInt = System.Convert.ToInt32 (tmpStrEntrances, 2);
			//Debug.Log (returnInt);
			return returnInt;
		}

		private void OnSceneGUI() {
			SceneView.RepaintAll ();
			if (inEditMode) {
				UpdateHitPosition ();
				RecalculateMarkerPosition ();
				Event current = Event.current;

				if (IsMouseOnLayer ( mouseHitPosition, pattern.gridY)) {
					if (current.type == EventType.MouseDown || current.type == EventType.MouseDrag) {
						if (current.button == 1) {
							//right mouse button click
							Erase();
							current.Use();
						} else if (current.button == 0) {
							//left mouse button click
							Draw();
							current.Use();
						}
					}
				}
				Selection.activeGameObject = pattern.gameObject;

				Handles.BeginGUI ();
				GUI.Label (new Rect (10, Screen.height - 80, 100, 100), "LMB: Draw");
				GUI.Label (new Rect (10, Screen.height - 65, 100, 100), "RMB: Erase");
				Handles.EndGUI ();
			}
		}

		/**
		 * METHODS USED TO PREPARE AND SAVE THE PATTERN AS A PREFAB
		 * */
		private void SaveDialog() {
			string newName = "Pattern_"+(GetLastPrefabNumber()+1);

			if (EditorUtility.DisplayDialog ("Which one do I use?",
				"Current Name : " + pattern.name + "\nNew Name : " + newName,
				"Use New Name", "Use Existing Name")) {
				pattern.name = newName;
			}

			SavePrefab ();
		}

		private void SavePrefab() {
			string currentName = pattern.name;

			string path = "Assets/" + PatternSettings.patternPath + currentName + ".prefab";
			if (AssetDatabase.LoadAssetAtPath (path, typeof(GameObject))) {
				if (EditorUtility.DisplayDialog ("Are you sure?",
					"The prefab already exists. Do you want to overwrite it?",
					"Yes", "No")) {
					pattern.name = currentName;
					SaveUpdatePrefab (path);
				}
			} else {
				pattern.name = currentName;
				SaveUpdatePrefab (path);
			}
		}



		private void UpdatePrefab() {
			string currentName = pattern.name;
			string path = "Assets/" + PatternSettings.patternPath + currentName + ".prefab";
			SaveUpdatePrefab (path);
		}

		private void EditPattern() {
			GameObject allPatternContainer = GameObject.Find ("All Patterns");
			if (allPatternContainer) {
				DestroyImmediate (allPatternContainer);
			}

			GameObject existingPattern = GameObject.FindGameObjectWithTag ("Pattern");
			if (existingPattern) {
				if (EditorUtility.DisplayDialog ("Are you sure?",
					"Do you want to delete the current pattern on screen?",
					"Yes", "No")) {
					DestroyImmediate (existingPattern);
				}
			}
			GameObject selectedPattern = PrefabUtility.InstantiatePrefab (pattern.gameObject) as GameObject;
			Selection.activeGameObject = selectedPattern;
		}

		private int GetLastPrefabNumber() {
			int lastPrefabNumber = 0;
			string patternPrefabFolder = Application.dataPath + "/" + PatternSettings.patternPath;
			string[] patternPrefabNames = System.IO.Directory.GetFiles (patternPrefabFolder, "*.prefab");

			for (int i = 0; i < patternPrefabNames.Length; i++) {
				string filename = System.IO.Path.GetFileNameWithoutExtension (patternPrefabNames [i]);
				int indexOfNumberStart = filename.IndexOf ("_");
				string strPrefabNumber = filename.Substring (indexOfNumberStart + 1, (filename.Length - 1) - indexOfNumberStart);
				int prefabNumber = 0;
				int.TryParse (strPrefabNumber, out prefabNumber);
				if (prefabNumber > lastPrefabNumber) {
					lastPrefabNumber = prefabNumber;
				}
			}

			return lastPrefabNumber;
		}

		private void PrimePrefabForUpdateSave() {
			LevelTiles[] children = pattern.GetComponentsInChildren<LevelTiles> ();
			//used the store the level tiles as part of the pattern prefab while
			//preserving the connection to the prefab instantiated by the levelblock
			//used to get around the problem with nested prefabs and unity
			for (int i = 0; i < children.Length; i++) {
				DestroyImmediate (children [i].transform.GetChild (0).gameObject);
			}
			currentInstantiatingDelay = Time.realtimeSinceStartup;
			primedForPrefab = true;
		}

		private void SaveUpdatePrefab(string prefabPath) {
			PrimePrefabForUpdateSave ();
			AssetDatabase.Refresh ();
			Object patternPrefab = PrefabUtility.CreatePrefab (prefabPath, pattern.gameObject, ReplacePrefabOptions.ConnectToPrefab);
			AssetDatabase.Refresh ();
		}
		/**
		 * ---------------------------------------------------------------
		 * END OF METHODS USED TO PREPARE AND SAVE THE PATTERN AS A PREFAB
		 * */


		/**
		* METHODS USED TO DEAL WITH EDITING OF THE PATTERN
		* */
		private Vector3 mouseHitPosition;
		private Vector2 tilePosition;

		private bool UpdateHitPosition() {
			Plane gridPlane = new Plane (Vector3.forward, pattern.transform.position);
			Ray ray = HandleUtility.GUIPointToWorldRay (Event.current.mousePosition);
			Vector3 hit = Vector3.zero;
			float dist;

			if (gridPlane.Raycast (ray, out dist)) {
				hit = ray.GetPoint (dist);
			}

			if (hit != mouseHitPosition) {
				mouseHitPosition = hit;
                Debug.Log("Hit position is: " + hit);
				return true;
			}

			return false;
		}

		private void RecalculateMarkerPosition() {
			tilePosition = GetTilePositionFromMouseLocation ();
			DrawWireCube(new Vector3(tilePosition.x * PatternSettings.tiledSize + pattern.xOffset, (tilePosition.y * PatternSettings.tiledSize), pattern.transform.position.z), PatternSettings.tiledSize);
		}

		private Vector2 GetTilePositionFromMouseLocation() {
			return GetTilePosition(mouseHitPosition);
		}

		private Vector2 GetTilePosition(Vector3 fromPosition) {
            Debug.Log("From position is: " + fromPosition);
			Vector3 pos = new Vector3 (fromPosition.x / PatternSettings.tiledSize, fromPosition.y / PatternSettings.tiledSize, fromPosition.z);
            if (PatternSettings.gridX % 2 != 0)
            {
                pos.x = Mathf.RoundToInt(pos.x);
            }
            else
            {
                pos.x = Mathf.FloorToInt(pos.x);
            }

			pos.y = Mathf.RoundToInt (pos.y);

            if (pos.x < -PatternSettings.gridX * 0.5f) {
            	pos.x = Mathf.FloorToInt (-PatternSettings.gridX * 0.5f)+1;
            } else if (pos.x > PatternSettings.gridX * 0.5f) {
            	pos.x = Mathf.FloorToInt (PatternSettings.gridX * 0.5f);
            }
            //if (pos.x < -PatternSettings.gridX * 0.5f)
            //{
            //    if (Mathf.Approximately(pattern.xOffset, 0))
            //    {
            //        pos.x = Mathf.FloorToInt(-PatternSettings.gridX * 0.5f) + 1;
            //    }
            //    else
            //    {
            //        pos.x = Mathf.FloorToInt(-PatternSettings.gridX * 0.5f);
            //    }
            //}
            //else if (pos.x > (PatternSettings.gridX * 0.5f) - (pattern.yOffset / PatternSettings.tiledSize))
            //{
            //    if (Mathf.Approximately(pattern.xOffset, 0))
            //    {
            //        pos.x = Mathf.FloorToInt(PatternSettings.gridX * 0.5f);
            //    }
            //    else
            //    {
            //        pos.x = Mathf.FloorToInt(PatternSettings.gridX * 0.5f) - 1;
            //    }
            //}

            if (pos.y < -pattern.gridY * 0.5f) {
				if (Mathf.Approximately (pattern.yOffset, 0)) {
					pos.y = Mathf.FloorToInt (-pattern.gridY * 0.5f) + 1;
				} else {
					pos.y = Mathf.FloorToInt (-pattern.gridY * 0.5f);
				}
			} else if (pos.y > (pattern.gridY * 0.5f) - (pattern.yOffset/PatternSettings.tiledSize)) {
				if (Mathf.Approximately (pattern.yOffset, 0)) {
					pos.y = Mathf.FloorToInt (pattern.gridY * 0.5f);
				} else {
					pos.y = Mathf.FloorToInt (pattern.gridY * 0.5f)-1;
				}
			}
            Debug.Log("Return position is: " + new Vector2(pos.x, pos.y));
            return new Vector2 (pos.x, pos.y);
		}

		private bool IsMouseOnLayer(Vector3 testPosition, int testGridY) {
			Vector3 pos = new Vector3 (testPosition.x / PatternSettings.tiledSize, testPosition.y / PatternSettings.tiledSize, testPosition.z);
			pos.x = Mathf.RoundToInt (pos.x);
			pos.y = Mathf.RoundToInt (pos.y);

			return !(pos.x < -PatternSettings.gridX * 0.5f || pos.x > PatternSettings.gridX * 0.5f || pos.y < -testGridY * 0.5f || pos.y > testGridY * 0.5f);
		}

		public static void DrawWireCube(Vector3 position, float size)
		{
			float half = size / 2f;
			// draw front
			Handles.color = Color.green;
			Handles.DrawLine(position + new Vector3(-half, -half, half), position + new Vector3(half, -half, half));
			Handles.DrawLine(position + new Vector3(-half, -half, half), position + new Vector3(-half, half, half));
			Handles.DrawLine(position + new Vector3(half, half, half), position + new Vector3(half, -half, half));
			Handles.DrawLine(position + new Vector3(half, half, half), position + new Vector3(-half, half, half));
			// draw back
			Handles.DrawLine(position + new Vector3(-half, -half, -half), position + new Vector3(half, -half, -half));
			Handles.DrawLine(position + new Vector3(-half, -half, -half), position + new Vector3(-half, half, -half));
			Handles.DrawLine(position + new Vector3(half, half, -half), position + new Vector3(half, -half, -half));
			Handles.DrawLine(position + new Vector3(half, half, -half), position + new Vector3(-half, half, -half));
			// draw corners
			Handles.DrawLine(position + new Vector3(-half, -half, -half), position + new Vector3(-half, -half, half));
			Handles.DrawLine(position + new Vector3(half, -half, -half), position + new Vector3(half, -half, half));
			Handles.DrawLine(position + new Vector3(-half, half, -half), position + new Vector3(-half, half, half));
			Handles.DrawLine(position + new Vector3(half, half, -half), position + new Vector3(half, half, half));
			Handles.color = Color.white;
		}

		private void Draw() {
			GameObject tile = null;
			List<string> tilePrefabNames = new List<string> ();

			if (placeSolitary == false) {
				for (int i = 0; i < pattern.transform.childCount; i++) {
					if (pattern.transform.GetChild (i).name == string.Format ("Tile_{0}_{1}", tilePosition.x, tilePosition.y)) {
						if (pattern.transform.GetChild (i).GetComponent<LevelTiles> ()) {
							tilePrefabNames.Add (pattern.transform.GetChild (i).GetComponent<LevelTiles> ().levelTilePrefab.name);
						}
					}
				}
			} else {
				tile = GameObject.Find (string.Format ("Tile_{0}_{1}", tilePosition.x, tilePosition.y));
			}
			if (tile != null && tile.transform.parent == pattern.transform) {
				return;
			}

			bool destroyTile = false;
			if (tile == null && levelTileNames.Length > 0 && selectedLevelTile < levelTileNames.Length) {
				string prefabPath = "Assets/" + PatternSettings.levelTilePath + levelTileNames [selectedLevelTile] + ".prefab";
				//Debug.Log (prefabPath);
				GameObject selectedPrefab = AssetDatabase.LoadAssetAtPath (prefabPath, typeof(GameObject)) as GameObject;
				tile = PrefabUtility.InstantiatePrefab (selectedPrefab) as GameObject;

				bool creatingOverlappingSimilarType = false;
				for (int i = 0; i < tilePrefabNames.Count; i++) {
					if (tile.GetComponent<LevelTiles> ().levelTilePrefab.name == tilePrefabNames [i]) {
						creatingOverlappingSimilarType = true;
						break;
					}
				}

				if (creatingOverlappingSimilarType == false) {
					//Debug.Log (tile);
					tile.GetComponent<LevelTiles> ().Initialize ();
					Undo.RegisterCreatedObjectUndo (tile.gameObject, "Created Game Tile");
				} else {
					DestroyImmediate (tile);
					destroyTile = true;
				}
			}

			if (tile != null && destroyTile == false) {
				tile.transform.parent = pattern.transform;
				tile.transform.localPosition = new Vector3 (tilePosition.x * PatternSettings.tiledSize + pattern.xOffset, tilePosition.y * PatternSettings.tiledSize,pattern.transform.position.z);
				tile.name = string.Format ("Tile_{0}_{1}", tilePosition.x, tilePosition.y);
			}
		}

		private void Erase() {
			GameObject tile = GameObject.Find (string.Format ("Tile_{0}_{1}", tilePosition.x, tilePosition.y));
			if (tile != null && tile.transform.parent == pattern.transform) {
				Undo.DestroyObjectImmediate (tile.gameObject);
				DestroyImmediate (tile);
			}
		}

		private void ClearGrid() {
			int childCount = pattern.transform.childCount;
			while (childCount > 0) {
				childCount--;
				DestroyImmediate (pattern.transform.GetChild (childCount).gameObject);
			}
			pattern.topEntrances = 0;
			pattern.bottomEntrances = 0;
		}

		private bool TilesOutsideModifiedGrid(int testGridZ) {
			LevelTiles[] children = pattern.GetComponentsInChildren<LevelTiles> ();
			for (int i = 0; i < children.Length; i++) {
				if (IsMouseOnLayer (children [i].transform.position, testGridZ) == false) {
					return true;
				}
			}
			return false;
		}

		protected virtual void OnDisable() {
			#if UNITY_EDITOR
			EditorApplication.update -= OnEditorUpdate;
			#endif
		}

		protected virtual void OnEditorUpdate() {
			if (primedForPrefab) {
				if (Time.realtimeSinceStartup - currentInstantiatingDelay > instantiatingDelay) {
					primedForPrefab = false;
					InitializePattern ();
				}
			}
		}
	}
}
