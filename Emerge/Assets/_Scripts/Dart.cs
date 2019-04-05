﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dart : CarryableObject
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public override bool Interact(PlayerInteraction interactor)
    {
        Debug.Log("Interact via DART");
        base.Interact(interactor);
        return true;

    }

    public override bool InteractWhileHeld(PlayerInteraction interactor)
    {
        Debug.Log("InteractWhileHeld via DART");
        interactor.releaseObj();
        return true;
    }
}
