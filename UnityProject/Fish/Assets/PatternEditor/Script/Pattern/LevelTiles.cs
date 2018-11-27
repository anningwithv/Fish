using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif
using System.Collections;

namespace GameWish.Game {
	public class LevelTiles : MonoBehaviour {

		public Transform levelTilePrefab;
		[HideInInspector]
		public Transform myPrefab = null;

		protected virtual void Start() {
			Initialize ();	
		}

		public virtual void Initialize() {
			if (myPrefab == null) {
				if (transform.childCount == 0) {
					#if UNITY_EDITOR
					if (Application.isPlaying == false) {
						myPrefab = PrefabUtility.InstantiatePrefab (levelTilePrefab) as Transform;
					} else {
						//myPrefab = (Transform)Instantiate (levelTilePrefab, Vector3.zero, Quaternion.identity);
						myPrefab = TVNTObjectPool.instance.SpawnObject(levelTilePrefab);
					}
					#else
					//myPrefab = (Transform) Instantiate(levelTilePrefab, Vector3.zero, Quaternion.identity);
					myPrefab = TVNTObjectPool.instance.SpawnObject(levelTilePrefab);
					#endif
					myPrefab.parent = transform;
					myPrefab.localPosition = Vector3.zero;
					myPrefab.localRotation = Quaternion.identity;
					myPrefab.localScale = new Vector3 (Mathf.Abs (myPrefab.localScale.x), Mathf.Abs (myPrefab.localScale.y), Mathf.Abs (myPrefab.localScale.z));
					myPrefab.gameObject.SetActive (true);
				} else {
					myPrefab = transform.GetChild (0);
					myPrefab.localPosition = Vector3.zero;
					myPrefab.localRotation = Quaternion.identity;
					myPrefab.localScale = new Vector3 (Mathf.Abs (myPrefab.localScale.x), Mathf.Abs (myPrefab.localScale.y), Mathf.Abs (myPrefab.localScale.z));
					myPrefab.gameObject.SetActive (true);
				}
			} 
			GroundCollider groundCollider = transform.GetComponentInChildren<GroundCollider> ();
			if (groundCollider) {
				groundCollider.levelTile = transform;
			}
		}

		public virtual void InspectorUpdate() {
			
		}
	}
}
