using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour {
	[SerializeField] private string itemName;		// type the name of this item in the Inspector.

	void OnTriggerEnter(Collider other) {
		Debug.Log ("Item collected: " + itemName);
		Destroy (this.gameObject);
	}
}
