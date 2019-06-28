﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnomalieDetection : MonoBehaviour
{
    public List<int> RFIDtagKnowned;

    /* Anomalie actuellement traitées :
     * - PCB différents (1)
     * - RefArticles différents (8)
     * - Poids trop important (7)
     * - RefArticle inconnue (5)
     * - Le colis est abimé (2)
     * - Le cois est mal orienté (3)
     * - Pas de RFID (6)
     * - Vérification Qualité (4)
     * (A faire)
     * - Dimension plateau
     * - Repack from FP
     */

    private void Start()
    {
        if(ChargementListeColis.instance != null)
        {
            RFIDtagKnowned = ChargementListeColis.instance.RFIDKnowed;
        }
    }

    public void CheckColis(Colis colis)
    {
        if(colis.listAnomalies.Contains("Quality control") && !colis.aEteVide)
        {
            Scoring.instance.MidPenalty();
            Scoring.instance.AffichageErreur("Quality control : Colis non vidé");
        }

        if(colis.listAnomalies.Contains("Repacking from FP") && !colis.hasBeenRecount)
        {
            Scoring.instance.MidPenalty();
            Scoring.instance.AffichageErreur("Repack from FP : Colis non recompté");
        }

        colis.nbAnomalie = 0;
        colis.listAnomalies = new List<string>();

        int RFIDnb = 0;
        foreach (Article article in colis.listArticles)
        {
            if (article.rfid != null && article.rfid.estFonctionnel)
            {
                RFIDnb++;
            }
            else
            {
                //Pas de RFID
            }
        }

        /* Résolution des problèmes de RFID
         * - Scan colis. Scan RFID. Bouton Inventaire. Crée nouveau ticket IWAY dans système. PrintHU pour imprimer. Mettre le ticket sur le colis.
         * - Scan colis. Scan RFID. Vider colis. Voir nombre Article. Bouton Inventaire. Crée un nouveau RFID. Mettre RFID sur pile Article.
         */
        //Debug.Log("Test");
        if(colis.needQualityControl)
        {
            colis.nbAnomalie++;
            colis.listAnomalies.Add("Quality control");
        }

        if(colis.estOuvert)
        {
            colis.nbAnomalie++;
            colis.listAnomalies.Add("Repacking from FP");
        }

        if(RFIDnb <= 0)
        {
            colis.nbAnomalie++;
            colis.listAnomalies.Add("RFID tags to be applied");
        }

        if (colis.wayTicket == null || RFIDnb != colis.wayTicket.PCB)
        {
            if(RFIDnb>colis.wayTicket.PCB)
            {
                colis.nbAnomalie++;
                colis.listAnomalies.Add("RFID tag over Tolerance");
            }
            else if(RFIDnb < colis.wayTicket.PCB)
            {
                colis.nbAnomalie++;
                colis.listAnomalies.Add("RFID tag under Tolerance");
            }
        }
        //Debug.Log("Test Anomalie 1");

        bool isBreakable = false;
        foreach (Article article in colis.listArticles) //Scanner le colis. Scanner les RFID. Vider le colis. Imprimer le RFID. Mettre le nouveau RFID.
        {
            if (article.rfid != null && colis.wayTicket != null && (article.rfid.refArticle.numeroRef != colis.wayTicket.refArticle.numeroRef && !isBreakable))
            {
                colis.nbAnomalie++;
                colis.listAnomalies.Add("RFID tag for unexpected product");
                isBreakable = true;
            }
        }
        //Debug.Log("Test Anomalie 2");

        if (colis.poids > 20) //A voir comment rectifier en jeu ET à dupliquer pour le supp 35
        {
            colis.nbAnomalie++;
            colis.listAnomalies.Add("TU too heavy (20-25)");
        }
        //Debug.Log("Test Anomalie 3");

        bool isCompatible = false; //Scanner le colis, Scanner les RFID, faire un inventaire pour ajouter les nouveaux RFID.
        for (int i = 0; i < RFIDtagKnowned.Count; i++)
        {
            //Debug.Log("Test Anomalie 4");

            if (colis.wayTicket != null && colis.listArticles.Count > 0 && colis.wayTicket.refArticle.numeroRef == RFIDtagKnowned[i])
            {
                isCompatible = true;
            }
        }
        if (!isCompatible)
        {
            colis.nbAnomalie++;
            colis.listAnomalies.Add("RFID tag scanned for unknown product");
        }

        if (colis.estAbime) //A voir comment rectifier en jeu
        {
            colis.nbAnomalie++;
            colis.listAnomalies.Add("Dimensions out of tolerance");
            /*if (Random.Range(0f, 1f) > 0.5f)
            {
                colis.listAnomalies.Add("Dimensions out of tolerance");
            }
            else
            {
                colis.listAnomalies.Add("Dimensions out of dimmension for tray");
            }*/
        }
        //Debug.Log("Test Anomalie 5");

        if (colis.isBadOriented) //A voir comment rectifier en jeu (Voir avec les graph)
        {
            colis.nbAnomalie++;
            colis.listAnomalies.Add("Wrong carton orientation");
        }
        //Debug.Log("Test Anomalie 6");

        //A la fin
        if (colis.nbAnomalie <= 0)
        {
            Debug.Log("Tout est bon dans le colis " + colis.name);
        }
        else
        {
            //Debug.Log("T'arrive là ?");
        }
    }

    public void CheckList(List<Colis> listColis)
    {
        foreach (Colis colis in listColis)
        {
            CheckColis(colis);
            /*colis.nbAnomalie = 0;
            colis.listAnomalies = new List<string>();

            int RFIDnb = 0;
            foreach (Article article in colis.listArticles)
            {
                Debug.Log(article.rfid);
                if (article.rfid != null)                                 //D'abord vérification si nul, sinon rfid.fonctionnel met une erreur
                {
                    if (article.rfid.estFonctionnel)
                    {
                        RFIDnb++;
                    }
                }
            }
            Debug.Log("Test Anomalie 1");

            if (colis.wayTicket == null || RFIDnb != colis.wayTicket.PCB)
            {
                colis.nbAnomalie++;
                colis.listAnomalies.Add("RFID tag over Tolerance");
            }
            Debug.Log("Test Anomalie 2");

            bool isBreakable = false;
            foreach (Article article in colis.listArticles)
            {
                if (article.rfid != null && colis.wayTicket != null &&(article.rfid.refArticle.numeroRef != colis.wayTicket.refArticle.numeroRef && !isBreakable))
                {
                    colis.nbAnomalie++;
                    colis.listAnomalies.Add("RFID tag for unexpected product");
                    isBreakable = true;
                }
            }
            Debug.Log("Test Anomalie 3");

            if (colis.poids > 20)
            {
                colis.nbAnomalie++;
                colis.listAnomalies.Add("TU too heavy (20-25)");
            }
            Debug.Log("Test Anomalie 4");

            bool isCompatible = false;
            for (int i = 0; i < RFIDtagKnowned.Count; i++)
            {
                Debug.Log("Test Anomalie 5");
                if (colis.listArticles[0].rfid != null)
                {
                    if (colis.listArticles[0].rfid.refArticle.numeroRef == RFIDtagKnowned[i])
                    {
                        isCompatible = true;
                    }
                }
            }
            if (!isCompatible)
            {
                colis.nbAnomalie++;
                colis.listAnomalies.Add("RFID tag scanned for unknown product");
            }
           

            if (colis.estAbime)
            {
                colis.nbAnomalie++;
                colis.listAnomalies.Add("Quality Control");
            }
            Debug.Log("Test Anomalie 6");

            if (colis.isBadOriented)
            {
                colis.nbAnomalie++;
                colis.listAnomalies.Add("Wrong carton orientation");
            }
            */

        }
    }
}
