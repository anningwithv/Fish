using UnityEngine;
using UnityEditor;
using System.Collections;

using System.Xml;
using System.Xml.Serialization;
using System.IO;

namespace TVNT {
public class PatternSettingsEditor : EditorWindow {

	public static PatternSettingsContainer patternSettingsContainer;

	public static string tempPatternPath;
	public static string tempLevelTilePath;
	public static float tempTiledSize;
	public static int tempGridX;
	public static float tempPlayerYOffset;

	private Vector2 scrollPosition = Vector2.zero;

	[MenuItem ("Pattern Editor/Pattern Settings")]
	static void Init() {
			tempPatternPath = PatternSettings.patternPath;
			tempLevelTilePath = PatternSettings.levelTilePath;
			tempTiledSize = PatternSettings.tiledSize;
			tempGridX = PatternSettings.gridX;
			//tempPlayerYOffset = PatternSettings.playerYOffset;

			PatternSettingsEditor window = (PatternSettingsEditor)EditorWindow.GetWindow (typeof(PatternSettingsEditor));
		window.Show ();
	}

	void OnGUI() {
			bool toSave = false;
		scrollPosition = EditorGUILayout.BeginScrollView (scrollPosition);
		GUILayout.Label ("Pattern Directory:", EditorStyles.boldLabel);
		//EditorGUILayout.TextField ("",patternPath);
			GUILayout.Label(PatternSettings.patternPath, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
		if (GUILayout.Button ("Choose Pattern Directory")) {
			tempPatternPath = EditorUtility.OpenFolderPanel ("Choose Pattern Directory", "", "");
			if (tempPatternPath.StartsWith (Application.dataPath)) {
				tempPatternPath = tempPatternPath.Substring (Application.dataPath.Length + 1, tempPatternPath.Length - (Application.dataPath.Length + 1)) + "/";

					if (PatternSettings.patternPath != tempPatternPath) {
						PatternSettings.patternPath = tempPatternPath;
						//EditorPrefs.SetString ("TVNTPatternPath", PatternSettings.patternPath);
						toSave = true;
				}
			} else {
				EditorUtility.DisplayDialog ("Wrong Location!", "The pattern path must be within the scope of the asset directory!", "Ok");
			}
		}

		EditorGUILayout.Space ();
		GUILayout.Label ("Level Tile Directory:", EditorStyles.boldLabel);
		//EditorGUILayout.TextField ("",levelTilePath);
			GUILayout.Label(PatternSettings.levelTilePath, EditorStyles.textField, GUILayout.Height(EditorGUIUtility.singleLineHeight));
		if (GUILayout.Button ("Choose Level Tile Directory")) {
			tempLevelTilePath = EditorUtility.OpenFolderPanel ("Choose Level Tile Directory", "", "");
			if (tempLevelTilePath.StartsWith (Application.dataPath)) {
				tempLevelTilePath = tempLevelTilePath.Substring (Application.dataPath.Length + 1, tempLevelTilePath.Length - (Application.dataPath.Length + 1)) + "/";

					if (PatternSettings.levelTilePath != tempLevelTilePath) {
						PatternSettings.levelTilePath = tempLevelTilePath;
						//EditorPrefs.SetString ("TVNTLevelTilePath", PatternSettings.levelTilePath);
						toSave = true;
				}
			} else {
				EditorUtility.DisplayDialog ("Wrong Location!", "The level tile path must be within the scope of the asset directory!", "Ok");
			}
		}

		EditorGUILayout.Space ();
		GUILayout.Label ("Tile Size:", EditorStyles.boldLabel);
			tempTiledSize = EditorGUILayout.FloatField ("", PatternSettings.tiledSize);
			if (Mathf.Approximately (tempTiledSize, PatternSettings.tiledSize) == false) {
			if (tempTiledSize > 0) {
					PatternSettings.tiledSize = tempTiledSize;
					//EditorPrefs.SetFloat ("TVNTTiledSize", PatternSettings.tiledSize);
					toSave = true;
			}
		}

		EditorGUILayout.Space ();
		GUILayout.Label ("Grid X:", EditorStyles.boldLabel);
			tempGridX = EditorGUILayout.IntField ("", PatternSettings.gridX);
			if (tempGridX != PatternSettings.gridX) {
			if (tempGridX > 0) {
					PatternSettings.gridX = tempGridX;
					EditorPrefs.SetInt ("TVNTGridX", PatternSettings.gridX);
					toSave = true;
			}
		}

		EditorGUILayout.Space ();
		//GUILayout.Label ("Player Y Offset:", EditorStyles.boldLabel);
		//	tempPlayerYOffset = EditorGUILayout.FloatField ("", PatternSettings.playerYOffset);
		//	if (Mathf.Approximately (tempPlayerYOffset, PatternSettings.playerYOffset) == false) {
		//	if (tempPlayerYOffset > 0) {
		//			PatternSettings.playerYOffset = tempPlayerYOffset;
		//			//EditorPrefs.SetFloat ("TVNTPlayerYOffset", PatternSettings.playerYOffset);
		//			toSave = true;
		//	}
		//}

			if (toSave) {
				if (patternSettingsContainer == null) {
					patternSettingsContainer = new PatternSettingsContainer ();
				}
				patternSettingsContainer.patternPath = PatternSettings.patternPath;
				patternSettingsContainer.levelTilePath = PatternSettings.levelTilePath;
				patternSettingsContainer.tiledSize = PatternSettings.tiledSize;
				patternSettingsContainer.gridX = PatternSettings.gridX;
				//patternSettingsContainer.playerYOffset = PatternSettings.playerYOffset;

				XmlSerializer serializer = new XmlSerializer (typeof(PatternSettingsContainer));
				FileStream stream = new FileStream (Application.dataPath + "/PatternEditor/Resources/XML/patternSettings.xml", FileMode.Create);
				serializer.Serialize (stream, patternSettingsContainer);
				stream.Close ();
			}

		EditorGUILayout.Space ();
		EditorGUILayout.EndScrollView ();
	}
}
}
