using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicalCard : MonoBehaviour
{
    public Card cardObject;

    void Awake()
    {
        // Get the Renderer component from the prefab object and this one
        Renderer sourceRenderer = cardObject.model.GetComponent<Renderer>();
        Renderer thisRenderer = this.GetComponent<Renderer>();

        if (sourceRenderer != null && thisRenderer != null)
        {
            // Copy the material from the source object to this object
           thisRenderer.material = sourceRenderer.sharedMaterial;
        }

            Debug.Log(cardObject.ToString());
    }

}
