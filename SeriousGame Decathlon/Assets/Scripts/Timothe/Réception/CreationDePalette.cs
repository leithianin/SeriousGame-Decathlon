﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class Palette
{
    public List<Rangee> rangees = new List<Rangee>();
}

[System.Serializable]
public class Rangee
{
    public List<Colonne> collones = new List<Colonne>();
}

[System.Serializable]
public class Colonne
{
    public List<GameObject> colis = new List<GameObject>();
}

public class CreationDePalette : MonoBehaviour
{
    public GameObject colisObj;

    public float chanceHavingAnomaliesMF;
    public int nbColisAvecAnomalie;

    public List<Colis> colisPossibles;
    public List<Colis> colisAvecAnomalieMF;

    public List<Palette> palettes;

    public Vector2 startPos;

    public float coefPosColonne;
    public float coefPosRangee;
    public float coefPosPalette;

    public  int nbColisTotal;
    private int nbColisParColonne;       //Non utilisé
    private int nbColonnes;              //Non utilisé
    private int nbRangee;                //Non utilisé
    private int nbPalettes;              //Non utilisé

    private int nbColisParColonneMax = 4;
    private int nbColonnesMax = 5;
    private int nbRangeeMax = 100;
    private int nbPalettesMax = 100;

    public Image barreProgression;
    public Image feedbackPileEtage;
    public int nbColisTraite;
    private int nbCurrentColis;

    public List<Colis> colisDeCote;

    public List<GameObject> colisDansOrdre;

    public List<ScriptColisRecep> colisActuels;

    int i = 0;
    int j = 0;
    int k = 0;
    int l = 0;

    // Start is called before the first frame update
    void Start()
    {
        if (ChargementListeColis.instance != null)
        {
            colisPossibles          = ChargementListeColis.instance.colisProcessReception;
            chanceHavingAnomaliesMF = ChargementListeColis.instance.chanceAnomalieRecep;
            nbColisTotal            = ChargementListeColis.instance.nombreColisRecep;
        }

        if(chanceHavingAnomaliesMF>100)
        {
            chanceHavingAnomaliesMF = 100;
            nbColisAvecAnomalie = nbColisTotal;
        }
        else
        {
            nbColisAvecAnomalie = (int)(nbColisTotal * (chanceHavingAnomaliesMF/100));
        }

        if (colisDansOrdre != null && colisDansOrdre.Count > 0)
        {
            nbColisTotal = colisDansOrdre.Count;
            for (i = 0; i < nbPalettesMax; i++)
            {
                palettes.Add(new Palette());
                for (j = 0; j < nbRangeeMax; j++)
                {
                    palettes[i].rangees.Add(new Rangee());
                    for (k = 0; k < nbColonnesMax; k++)
                    {
                        palettes[i].rangees[j].collones.Add(new Colonne());
                        for (l = 0; l < nbColisParColonneMax; l++)
                        {
                            if (nbCurrentColis >= nbColisTotal)
                            {
                                goto labelUn;
                            }
                            else
                            {
                                nbCurrentColis++;
                                Vector2 newPos = startPos + new Vector2(l * coefPosColonne, i * coefPosPalette + j * coefPosRangee);
                                palettes[i].rangees[j].collones[k].colis.Add(colisDansOrdre[nbCurrentColis-1]);
                                palettes[i].rangees[j].collones[k].colis[l].transform.position = newPos;
                                palettes[i].rangees[j].collones[k].colis[l].GetComponent<SpriteRenderer>().sortingOrder = k + 2;
                                palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().canBePicked = false;
                                palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().currentHauteur = k;
                                if ((Random.Range(0, nbColisTotal - nbCurrentColis) < nbColisAvecAnomalie /*(chanceHavingAnomaliesMF != 0 && Random.Range(0f, 1f) <= chanceHavingAnomaliesMF / 100f)*/ || chanceHavingAnomaliesMF >= 100) && colisAvecAnomalieMF.Count > 0)
                                {
                                    nbColisAvecAnomalie--;
                                    int currentCartonPossible = Random.Range(0, colisPossibles.Count);
                                    palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable = colisDansOrdre[nbCurrentColis-1].GetComponent<ScriptColisRecep>().colisScriptable;
                                    //palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable.carton = colisPossibles[currentCartonPossible].carton;
                                    //palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable.isBadOriented = colisPossibles[currentCartonPossible].isBadOriented;
                                    //palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable.estAbime = colisPossibles[currentCartonPossible].estAbime;
                                }
                                else if (colisPossibles.Count > 0)
                                {
                                    palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable = colisDansOrdre[nbCurrentColis - 1].GetComponent<ScriptColisRecep>().colisScriptable;
                                }
                            }
                        }
                    }
                }
            }
        }
        else
        {
            for (i = 0; i < nbPalettesMax; i++)
            {
                palettes.Add(new Palette());
                for (j = 0; j < nbRangeeMax; j++)
                {
                    palettes[i].rangees.Add(new Rangee());
                    for (k = 0; k < nbColonnesMax; k++)
                    {
                        palettes[i].rangees[j].collones.Add(new Colonne());
                        for (l = 0; l < nbColisParColonneMax; l++)
                        {
                            if (nbCurrentColis >= nbColisTotal)
                            {
                                goto labelUn;
                            }
                            else
                            {
                                nbCurrentColis++;
                                Vector2 newPos = startPos + new Vector2(l * coefPosColonne, i * coefPosPalette + j * coefPosRangee);
                                palettes[i].rangees[j].collones[k].colis.Add(Instantiate(colisObj, newPos, Quaternion.identity));
                                palettes[i].rangees[j].collones[k].colis[l].GetComponent<SpriteRenderer>().sortingOrder = k + 2;
                                palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().canBePicked = false;
                                palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().currentHauteur = k;
                                if ((Random.Range(0, nbColisTotal - nbCurrentColis) < nbColisAvecAnomalie /*(chanceHavingAnomaliesMF != 0 && Random.Range(0f, 1f) <= chanceHavingAnomaliesMF / 100f)*/ || chanceHavingAnomaliesMF >= 100) && colisAvecAnomalieMF.Count > 0)
                                {
                                    nbColisAvecAnomalie--;
                                    int currentCartonPossible = Random.Range(0, colisPossibles.Count);
                                    palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable = Instantiate(colisAvecAnomalieMF[Random.Range(0, colisAvecAnomalieMF.Count)]);
                                    palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable.carton = colisPossibles[currentCartonPossible].carton;
                                    palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable.isBadOriented = colisPossibles[currentCartonPossible].isBadOriented;
                                    palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable.estAbime = colisPossibles[currentCartonPossible].estAbime;
                                }
                                else if (colisPossibles.Count > 0)
                                {
                                    palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable = Instantiate(colisPossibles[Random.Range(0, colisPossibles.Count)]);
                                }
                            }
                        }
                    }
                }
            }
        }

    labelUn:

        if (l == 0)
        {
            if (k == 0)
            { 
                if (j == 0)
                {
                    if (i == 0)
                    {
                        return;
                    }
                    else
                    {
                        i--;
                        j = nbRangeeMax-1;
                        k = nbColonnesMax - 1;
                        l = nbColisParColonneMax - 1;
                    }
                }
                else
                {
                    j--;
                    k = nbColonnesMax - 1;
                    l = nbColisParColonneMax - 1;
                }
            }
            else
            {
                k--;
                l = nbColisParColonneMax - 1;
            }
        }
        Debug.Log(i + " " + j + " " + k + " " + l);
        for (int m = 0; m < palettes[i].rangees[j].collones[k].colis.Count; m++)
        {
            palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>().canBePicked = true;
            palettes[i].rangees[j].collones[k].colis[m].GetComponent<BoxCollider2D>().enabled = true;
            colisActuels.Add(palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>());
            feedbackPileEtage.fillAmount = ((float)palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>().currentHauteur + 1f) / 5f;
        }
    }

    private void Update()
    {
        List<ScriptColisRecep> colisToRemove = new List<ScriptColisRecep>();
        if(colisActuels.Count > 0)
        {
            foreach(ScriptColisRecep colis in colisActuels)
            {
                if(colis.canMove || colis==null)
                {
                    colisToRemove.Add(colis);
                    nbColisTraite++;
                }
            }

            for(int i = 0; i < colisActuels.Count; i++)
            {
                if(colisActuels[i] == null)
                {
                    colisActuels.RemoveAt(i);
                    i--;
                }
            }

            foreach (ScriptColisRecep colis in colisToRemove)
            {
                colisActuels.Remove(colis);
            }

        }
        else
        {
            feedbackPileEtage.fillAmount = 0;
            colisActuels = new List<ScriptColisRecep>();
            k--;
            if (Scoring.instance != null)
            {
                Scoring.instance.PauseCombo(6f);
            }
            if (k < 0)
            {
                j--;
                if (Scoring.instance != null)
                {
                    Scoring.instance.PauseCombo(12f);
                }
                if (j < 0)
                {
                    i--;
                    if (i < 0)
                    {
                        return;
                    }
                    else
                    {
                        j = nbRangeeMax - 1;
                        k = nbColonnesMax - 1;
                        l = nbColisParColonneMax - 1;
                    }
                }
                else
                {
                    k = nbColonnesMax - 1;
                    l = nbColisParColonneMax - 1;
                }
            }
            else
            {
                //k--;
                l = nbColisParColonneMax - 1;
            }
            for (int m = 0; m < palettes[i].rangees[j].collones[k].colis.Count; m++)
            {
                palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>().canBePicked = true;
                colisActuels.Add(palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>());
                feedbackPileEtage.fillAmount = ((float)palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>().currentHauteur+1f) / 5f;
                Color theColor = new Color();
                theColor.b = 260;
                theColor.g = 260;
                theColor.r = 260;
                theColor.a = 1;
                palettes[i].rangees[j].collones[k].colis[m].GetComponent<SpriteRenderer>().color = theColor;
                palettes[i].rangees[j].collones[k].colis[m].GetComponent<BoxCollider2D>().enabled = true;
                //Mettre à jour la Phase Colis du tuto

            }
            Debug.Log(i + " " + j + " " + k + " " + l);
        }
    }

    public int GetNBColonnes()
    {
        return nbColonnesMax;
    }
}
