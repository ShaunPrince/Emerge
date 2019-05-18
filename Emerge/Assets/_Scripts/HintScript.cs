﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HintScript : MonoBehaviour
{
    private GameObject hintObj;
    private Text hintText;
    // Start is called before the first frame update
    void Start()
    {
        hintObj = this.transform.GetChild(1).gameObject;
        hintText = hintObj.GetComponent<Text>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            bool active = hintObj.activeSelf;
            hintObj.SetActive(!active);
        }
    }
}