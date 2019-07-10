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

    public List<Colis> colisPossibles;

    public List<Palette> palettes;

    public Vector2 startPos;

    public float coefPosColonne;
    public float coefPosRangee;
    public float coefPosPalette;

    public int nbColisTotal;
    private int nbColisParColonne;
    private int nbColonnes;
    private int nbRangee;
    private int nbPalettes;

    private int nbColisParColonneMax = 4;
    private int nbColonnesMax = 5;
    private int nbRangeeMax = 100;
    private int nbPalettesMax = 100;

    public Image barreProgression;
    public int nbColisTraite;
    private int nbCurrentColis;

    public List<ScriptColisRecep> colisActuels;

    int i = 0;
    int j = 0;
    int k = 0;
    int l = 0;

    // Start is called before the first frame update
    void Start()
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
                            if (colisPossibles.Count > 0)
                            {
                                Debug.Log(i + " " + j + " " + k + " " + l);
                                Colis leColis = Colis.CreateInstance<Colis>();
                                leColis = colisPossibles[Random.Range(0, colisPossibles.Count)];
                                palettes[i].rangees[j].collones[k].colis[l].GetComponent<ScriptColisRecep>().colisScriptable = leColis;
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
            colisActuels.Add(palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>());
        }
    }

    private void Update()
    {
        List<ScriptColisRecep> colisToRemove = new List<ScriptColisRecep>();
        if(colisActuels.Count > 0)
        {
            foreach(ScriptColisRecep colis in colisActuels)
            {
                if(colis.canMove)
                {
                    colisToRemove.Add(colis);
                    nbColisTraite++;
                }
            }

            foreach (ScriptColisRecep colis in colisToRemove)
            {
                colisActuels.Remove(colis);
            }

            barreProgression.fillAmount = ((float)nbColisTotal - (float)nbColisTraite) / (float)nbColisTotal;
        }
        else
        {
            colisActuels = new List<ScriptColisRecep>();
            Debug.Log(i + " " + j + " " + k + " " + l);
            k--;
            if (k <= 0)
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
                        j = nbRangeeMax - 1;
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
                //k--;
                l = nbColisParColonneMax - 1;
            }
            for (int m = 0; m < palettes[i].rangees[j].collones[k].colis.Count; m++)
            {
                palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>().canBePicked = true;
                colisActuels.Add(palettes[i].rangees[j].collones[k].colis[m].GetComponent<ScriptColisRecep>());
                Color theColor = new Color();
                theColor.b = 260;
                theColor.g = 260;
                theColor.r = 260;
                theColor.a = 1;
                palettes[i].rangees[j].collones[k].colis[m].GetComponent<SpriteRenderer>().color = theColor;
            }
        }
    }
}
