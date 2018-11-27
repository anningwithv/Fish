using UnityEngine;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;

public class PatternSettings {
	/*********************************READ FIRST****************************************/
	/**
	 * THE WAY TO EDIT THIS FILE HAS CHANGED SINCE VER 1.2.
	 * PLEASE DO NOT CHANGE THE VALUES IN THIS FILE.
	 * GO TO THE TVNT MENU ITEM, AND CLICK ON THE PATTERN SETTINGS TAB AND CHANGE THE VALUE THERE.
	 **/
	public static string patternPath;
	public static string levelTilePath;
	public static float tiledSize;
	public static int gridX;
	//public static float playerYOffset;

	[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
	static void PatternSettingsLoad() {
		TextAsset _xml = Resources.Load<TextAsset> ("XML/patternSettings");
		XmlSerializer serializer = new XmlSerializer (typeof(PatternSettingsContainer));
		StringReader reader = new StringReader (_xml.ToString ());
		PatternSettingsContainer patternSettingsContainer = serializer.Deserialize (reader) as PatternSettingsContainer;
		reader.Close();

		PatternSettings.patternPath = patternSettingsContainer.patternPath;
		PatternSettings.levelTilePath = patternSettingsContainer.levelTilePath;
		PatternSettings.tiledSize = patternSettingsContainer.tiledSize;
		PatternSettings.gridX = patternSettingsContainer.gridX;
		//PatternSettings.playerYOffset = patternSettingsContainer.playerYOffset;
	}
}
