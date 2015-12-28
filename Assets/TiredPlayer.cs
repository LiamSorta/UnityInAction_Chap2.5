using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class TiredPlayer : MonoBehaviour {
    [HideInInspector] public float energy = 100; //Accessed by RelativeMovement

    public void Exhaust(float negEnergy)
    {
        Debug.Log(energy);
        energy -= negEnergy;

        //Prevents energy going over/under fill bounds
        if (energy > 100)
            energy = 100;
        if (energy < 0)
            energy = 0;

    }
    void Update()
    {
        GetComponent<Image>().fillAmount = energy / 100; 
    }
    
}
