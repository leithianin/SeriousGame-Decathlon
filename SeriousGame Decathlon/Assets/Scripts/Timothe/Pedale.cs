﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pedale : MonoBehaviour
{
    public ColisManager colisManag;

    public void ActivationPedale()
    {
        colisManag.AppelColis();
        //Debug.Log(TutoManager.instance != null);

        if (TutoManagerMulti.instance != null)
        {
            TutoManagerMulti.instance.Manager(1);
        }
    }
}
