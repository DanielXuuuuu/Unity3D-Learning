﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAroundSelf : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.RotateAround(this.transform.position, Vector3.up, 1 * Time.deltaTime);
    }
}
