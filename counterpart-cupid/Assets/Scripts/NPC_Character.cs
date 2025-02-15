using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPC_Character : MonoBehaviour
{
    // attributes


    // current way to determine shapes
    [SerializeField] private int shapeSet;
    [SerializeField] private bool setElement;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    bool DoesMatch(NPC_Character other) {
        return shapeSet == other.shapeSet && setElement == !other.setElement;
    }
}
