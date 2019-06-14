﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColisManager : MonoBehaviour
{
    public List<Colis> listeColisTraiter;
    public GameObject colisGameObject;

    public Transform positionApparition;

    public GameObject[] listeColisActuel;

    public List<Article> articleOnTable;

    //A affecté au Colis
    public GameObject menuTourner;
    public GameObject spriteArticleTable;
    public Image circleImage;

    //Auquel le colis doit être affecté
    public List<BoutonDirection> listeBoutonsMenuTourner;
    public RecountTab recountTab;
    public PistolScan scanPistol;

    public AnomalieDetection anomDetect;

    private void Start()
    {
        anomDetect.CheckList(listeColisTraiter);
    }

    public void AppelColis()
    {
        listeColisActuel = new GameObject[0];
        listeColisActuel = GameObject.FindGameObjectsWithTag("Colis");

        if (listeColisActuel.Length <= 0)
        {
            GameObject colisTemporaire = Instantiate(colisGameObject, positionApparition.position, Quaternion.identity);
            ColisScript scriptColis = colisTemporaire.GetComponent<ColisScript>();
            scriptColis.colisScriptable = listeColisTraiter[0];
            scriptColis.doesEntrance = true;
            scriptColis.tournerMenu = menuTourner;
            scriptColis.spriteArticleTable = spriteArticleTable;
            scriptColis.circleImage = circleImage;

            foreach (BoutonDirection bouton in listeBoutonsMenuTourner)
            {
                bouton.scriptColis = colisTemporaire.GetComponent<ColisScript>();
            }

            scanPistol.scriptColis = scriptColis;
            recountTab.colis = colisTemporaire;

            listeColisTraiter.RemoveAt(0);
            if (listeColisTraiter[0] == null && listeColisTraiter[1] != null)
            {
                for (int i = 0; i < listeColisTraiter.Count - 1; i++)
                {
                    if (listeColisTraiter[i + 1] != null)
                    {
                        listeColisTraiter[i] = listeColisTraiter[i + 1];
                    }
                    else
                    {
                        listeColisTraiter.RemoveAt(i);
                    }
                }
            }
        }
    }

    public void RenvoieColis(GameObject colisRenvoye)
    {
        if(!listeColisTraiter.Contains(colisRenvoye.GetComponent<ColisScript>().colisScriptable))
        {
            anomDetect.CheckColis(colisRenvoye.GetComponent<ColisScript>().colisScriptable);
            if (colisRenvoye.GetComponent<ColisScript>().colisScriptable.nbAnomalie > 0)
            {
                listeColisTraiter.Add(colisRenvoye.GetComponent<ColisScript>().colisScriptable);
            }
            GameObject.Destroy(colisRenvoye);
        }
    }
}
