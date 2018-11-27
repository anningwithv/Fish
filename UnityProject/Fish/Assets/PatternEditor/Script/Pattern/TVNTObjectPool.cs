using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace GameWish.Game {
	public class TVNTObjectPool : MonoBehaviour {

		public static TVNTObjectPool instance = null;
		public bool isPersistant = true;

		//public Transform objectPoolContainer;
		private Dictionary <string, List<Transform>> myPool = new Dictionary<string, List<Transform>>();
		private List<Transform> tempObjectList = new List<Transform> ();

		void Awake() {
			if (instance == null) {
				instance = this;
			} else if (instance != this) {
				Destroy (gameObject);
			}
			if (isPersistant) {
				DontDestroyOnLoad (gameObject);
			}
		}

		public Transform SpawnObject(Transform prefab) {
			Transform objectToRelease = null;
			if (myPool.ContainsKey (prefab.name)) {
				tempObjectList.Clear ();
				tempObjectList = new List<Transform> (myPool [prefab.name]);
				for (int i = 0; i < tempObjectList.Count; i++) {
					if (tempObjectList [i].gameObject.activeInHierarchy == false) {
						objectToRelease = tempObjectList [i];
					}
				}
				if (objectToRelease == null) {
					objectToRelease = (Transform)Instantiate (prefab);
					objectToRelease.name = prefab.name;
					myPool [prefab.name].Add (objectToRelease);
				}
			} else {
				tempObjectList.Clear ();
				objectToRelease = (Transform)Instantiate (prefab);
				objectToRelease.name = prefab.name;
				tempObjectList.Add (objectToRelease);
				myPool [prefab.name] = new List<Transform> (tempObjectList);
			}
			objectToRelease.parent = null;
			//objectToRelease.gameObject.SetActive (true);
			return objectToRelease;
		}

		public void ReleaseObject(Transform instance) {
			instance.parent = transform;
			instance.gameObject.SetActive (false);
		}
	}
}
