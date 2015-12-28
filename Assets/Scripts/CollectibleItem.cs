using UnityEngine;
using System.Collections;

public class CollectibleItem : MonoBehaviour
{
    [SerializeField]
    private string itemName;		// type the name of this item in the Inspector.
    [SerializeField]
    private float restoreEnergy = 0f;
    void OnTriggerEnter(Collider other)
    {
        if (itemName == "energy")
            GameObject.Find("EnergyUI").GetComponent<TiredPlayer>().Exhaust(-restoreEnergy);
        Debug.Log("Item collected: " + itemName);
        Destroy(this.gameObject);
    }
}
