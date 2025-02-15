using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class NPC_Character : MonoBehaviour
{
    // attributes
    // current way to determine shapes
    [SerializeField] private int shapeSet;
    [SerializeField] private int setElement;


    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    bool DoesMatch(NPC_Character other) {
        return shapeSet == other.shapeSet && setElement != other.setElement;
    }

    public void OnTriggerEnter(Collider c)
    {
        NPC_Character other = c.gameObject.GetComponent<NPC_Character>();
        if (other is not null && DoesMatch(other)) {
            // if it's a match -- 
            // TODO: add points or whatever
            print("Matched!");
            Destroy(other.gameObject);
            Destroy(gameObject);
        }
    }
}
