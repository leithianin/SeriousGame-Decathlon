﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Monitor : MonoBehaviour
{
    public TextMeshProUGUI nbText;
    public int nbMonitor = 0;

    public GameObject colis1;
    public GameObject colis2;
    public GameObject colis3;

    public GameObject Tapis1;
    public GameObject Tapis2;
    public GameObject Tapis3;

    public GameObject colisAPrelever1;
    public GameObject colisAPrelever2;

    public void Start()
    {
        nbText.text = "";
    }

    public void UpdateAffichage(int nb)
    {
        nbMonitor = nb;
        nbText.text = nb.ToString();
    }

    public void Colis1Actif() //Il faut savoir comment on définit quel colis sera actif
    {
        Tapis1.GetComponent<ColisLink>().console.UpdateAffichage();
        Tapis1.GetComponent<ColisLink>().cm.UpdateAffichage(nbMonitor);
        //Animator Colis1 activé SUR ECRAN
        //Animator Colis1 activé sur poste
    }

    public void Colis2Actif() //Il faut savoir comment on définit quel colis sera actif
    {
        Tapis2.GetComponent<ColisLink>().console.UpdateAffichage();
        Tapis2.GetComponent<ColisLink>().cm.UpdateAffichage(nbMonitor);
        //Animator Colis2
    }

    public void Colis3Actif() //Il faut savoir comment on définit quel colis sera actif
    {
        Tapis3.GetComponent<ColisLink>().console.UpdateAffichage();
        Tapis3.GetComponent<ColisLink>().cm.UpdateAffichage(nbMonitor);
        //Animator Colis3
    }

    public void UpdateColisAPrelever()
    {
        /*if(colisAPrelever1.isActiver)
        {
            Animator.getBool()
        }
        if (colisAPrelever2.isActiver)
        {
            Animator.GetBool()
        }*/
    }
}
