﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CartonVideLink : MonoBehaviour
{
    public bool isFree1 = true;
    public bool isFree2 = true;
    public bool isFree3 = true;

    public GameObject ColisPrefab;

    public void PutAnotherColis(Vector3 position)
    {
        Instantiate(ColisPrefab, position, Quaternion.identity);
    }
}
