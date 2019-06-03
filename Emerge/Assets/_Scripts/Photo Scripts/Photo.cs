﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photo : MonoBehaviour
{
    public GameObject player;
    public GameObject noteFragment;
    public List<GameObject> solution = new List<GameObject>();
    public bool isSolved = false;

    private GameObject lookingAt;


    // Main Functions //
    private void Start()
    {
        player = GameObject.Find("Player");
        GameObject photoParent = GameObject.Find("AlteredPhoto");
        foreach (Transform child in photoParent.transform)
            if (child.name.Split(' ')[0] == "CorrectChange")
                solution.Add(child.gameObject);
    }

    private void Update()
    {
        if (solution.Count == 0)
            CheckSolution();

        lookingAt = player.GetComponent<PlayerInteraction>().LookingAt(50);
        if (lookingAt != null && Input.GetKeyDown(KeyCode.E))
        {
            if (solution.Contains(lookingAt))
                solution.Remove(lookingAt);
        }
    }


    private void CheckSolution()
    {
        // If successful, play animation and freeze paintings
        Debug.Log("Photo Minigame COMPLETE!");
        isSolved = true;
        GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>().PuzzleSolved();

        GameObject alteredPhoto = GameObject.Find("AlteredPhoto");
        for (int i = 1; i < alteredPhoto.transform.childCount; i++)
            alteredPhoto.transform.GetChild(i).GetComponent<SphereCollider>().enabled = false;

        // "Spawn" the Note Fragment after winning
        noteFragment.transform.GetChild(0).gameObject.SetActive(true);
        noteFragment.GetComponent<Rigidbody>().isKinematic = false;

        // "Spawn" the bottles of Absinthe
        GameObject absinthe = GameObject.Find("absinthe");
        absinthe.GetComponent<MeshRenderer>().enabled = true;
        absinthe.GetComponent<MeshCollider>().enabled = true;
        absinthe.GetComponent<Rigidbody>().isKinematic = false;
        absinthe.transform.GetChild(0).GetComponent<MeshRenderer>().enabled = true;
        absinthe.transform.GetChild(1).GetComponent<Canvas>().enabled = true;

        // Enable Reset_Bottles (was causing problems w/ resetting absinthe when it's not available)
        GameObject resetButton = GameObject.Find("Reset_Bottles");
        resetButton.GetComponent<BoxCollider>().enabled = true;
        resetButton.transform.GetChild(0).GetComponent<BoxCollider>().enabled = true;

        // Prevents script from running anymore
        this.GetComponent<Photo>().enabled = false;
    }
}
