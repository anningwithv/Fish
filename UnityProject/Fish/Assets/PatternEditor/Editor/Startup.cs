using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using TVNT;

[InitializeOnLoad]
public class Startup {
	static Startup() {
		Debug.Log ("Loading data");
		XmlSerializer serializer = new XmlSerializer (typeof(PatternSettingsContainer));
		FileStream stream = new FileStream (Application.dataPath + "/PatternEditor/Resources/XML/patternSettings.xml", FileMode.Open);
		PatternSettingsEditor.patternSettingsContainer = serializer.Deserialize (stream) as PatternSettingsContainer;
		stream.Close ();

		PatternSettings.patternPath = PatternSettingsEditor.patternSettingsContainer.patternPath;
		PatternSettings.levelTilePath = PatternSettingsEditor.patternSettingsContainer.levelTilePath;
		PatternSettings.tiledSize = PatternSettingsEditor.patternSettingsContainer.tiledSize;
		PatternSettings.gridX = PatternSettingsEditor.patternSettingsContainer.gridX;
		//PatternSettings.playerYOffset = PatternSettingsEditor.patternSettingsContainer.playerYOffset;
	}
}
