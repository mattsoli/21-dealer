using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCard : MonoBehaviour
{
    public Card cardObject;

    void Awake()
    {
        // instatiate the 3D model prefab of the card, assigned in the Card object
        GameObject cardModel = Instantiate(cardObject.model, this.transform) as GameObject;
        
        Debug.Log(cardObject.ToString());
    }

}
