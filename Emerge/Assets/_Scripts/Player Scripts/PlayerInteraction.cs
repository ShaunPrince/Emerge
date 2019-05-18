﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInteraction : MonoBehaviour
{
    public float breakForce;
    public float throwForce;
    public GameObject heldObject;


    [Header("Private")]
    [SerializeField] private Camera _camera;
    [SerializeField] private GameObject hands;
    [SerializeField] private FixedJoint joint;
    [SerializeField] private Text text;


    // Main Functions //
    private void Update()
    {
        if (lookingAt() != null)
        {
            //Pick up object
            if (heldObject == null && Input.GetKeyDown(KeyCode.E))
            {
                if (lookingAt().GetComponentInParent<InteractableObject>() != null 
                && lookingAt().GetComponentInParent<InteractableObject>() == true 
                || lookingAt().tag == "Pickupable")
                {
                    lookingAt().GetComponentInParent<InteractableObject>().Interact(this);
                    //pickupObj(lookingAt());
                }
            }

            //Drop held object
            else if (heldObject != null && Input.GetKeyDown(KeyCode.E))
            {
                releaseObj();
            }

            //Throw held object
            else if (heldObject != null && Input.GetKeyDown(KeyCode.Mouse0))
            {
                throwObj();
            }
        }

        //Drop held object (not looking at it)
        else if (heldObject != null && Input.GetKeyDown(KeyCode.E))
        {
            releaseObj();
        }

        //Throw held object (not looking at it)
        else if (heldObject != null && Input.GetKeyDown(KeyCode.Mouse0))
        {
            throwObj();
        }

        //If joint breaks due to force, drop object
        if (heldObject != null && joint == null) releaseObj();
    }


    // Public Functions //


    public void pickupObj(GameObject objToPickup)
    {
        if (objToPickup.GetComponent<Rigidbody>().isKinematic == true)
            objToPickup.GetComponent<Rigidbody>().isKinematic = false;

        heldObject = objToPickup;

        if (heldObject.name.Split('_')[0] == "Dart")    // Only do this for darts
            heldObject.transform.rotation = Quaternion.LookRotation(heldObject.transform.position - _camera.transform.position) * Quaternion.Euler(90, 0, 0);

        heldObject.transform.position = GameObject.Find("Hands").transform.position;
        GameObject.Find("Hands").GetComponent<Rigidbody>().sleepThreshold = 0f;
        heldObject.transform.rotation = Quaternion.LookRotation(_camera.transform.forward);
        //heldObject.GetComponent<Rigidbody>().sleepThreshold = 0f;
        joint = hands.AddComponent<FixedJoint>();
        //joint = this.gameObject.AddComponent<FixedJoint>();
        joint.connectedBody = heldObject.GetComponent<Rigidbody>();
        joint.breakForce = breakForce;
    }

    public void releaseObj()
    {
        Destroy(joint);
        heldObject = null;
    }

    public void throwObj()
    {
        Destroy(joint);
        heldObject.GetComponent<Rigidbody>().AddForce(_camera.gameObject.transform.forward * throwForce);
        heldObject = null;
    }


    // Private Functions //
    public GameObject lookingAt()
    {
        Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 4) 
            && (hit.transform.gameObject.GetComponentInParent<InteractableObject>() != null
            || hit.transform.gameObject.CompareTag("Pickupable") 
            || hit.transform.gameObject.CompareTag("Interactable")) )
        {
            Debug.DrawRay(_camera.transform.position, _camera.transform.forward * hit.distance, Color.yellow);
            if (heldObject == null) text.color = Color.green;
            else text.color = Color.white;
            return hit.transform.gameObject;
        }

        else
        {
            text.color = Color.black;
            return null;
        }
    }

    private void OnJointBreak(float breakForce)
    {
        releaseObj();
    }

    public Vector3 GetForward()
    {
        return _camera.transform.forward;
    }
}