﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ManagerColisVider : MonoBehaviour
{
    public List<bool> etatColis = new List<bool>();

    public List<GameObject> colisAnimationVenir;

    public List<Transform> positionParEmmplacements;
    public List<GameObject> emplacementsScripts;

    public List<Colis> colisVider;
    public List<Colis> colisAvecPack;
    private int nbCurrentColis = 0;
    public int chanceColisPasRemplit;
    public int chanceArticlePasBon;

    public Image photoArticle;

    public ManagerColisAttendu managerColis;

    public List<RemplissageColisGTP> colisActuellementsPose;

    public GameObject RemainingQuantityWindow;

    public bool aEteVerifier;

    private int emplacement = 0;
    private int AncientEmplacementTempo = -1;

    private float tempsReponseChangementColis;
    int emplacementTempo;
    // Start is called before the first frame update
    void Start()
    {
        etatColis.Add(false);
        etatColis.Add(false);
        StartCoroutine(FaireVenirPremiersColis());
    }

    private void Update()
    {
        if(tempsReponseChangementColis>0)
        {
            tempsReponseChangementColis -= Time.deltaTime;
        }
    }

    public IEnumerator FaireVenirPremiersColis()
    {
        yield return new WaitForSeconds(1f);
        FaireVenirNouveauColis(emplacement);
        emplacement++;
        StartCoroutine(ActiverAutreColis(0));
        yield return new WaitForSeconds(3f);

        FaireVenirNouveauColis(emplacement);
        emplacement = 0;
    }

    public Colis ChoixNouveauColis(int empalcementColisCree)
    {
        Colis newColis = new Colis();
        if (managerColis.colisActuellementTraite[0] != null || managerColis.colisActuellementTraite[1] != null || managerColis.colisActuellementTraite[2] != null)
        {
            if (colisVider != null)
            {
                if (colisVider.Count > 1)
                {
                    int emplacementTempo = Random.Range(0, 3);
                    if (emplacementTempo == AncientEmplacementTempo)
                    {
                        emplacementTempo = (emplacementTempo + 1) % 3;
                    }
                    int randomArticleVoulu = 0;
                    int nb = 0;
                    while (managerColis.colisActuellementTraite[emplacementTempo] == null && nb < 10)
                    {
                        emplacementTempo = (emplacementTempo + 1) % 3;
                        nb++;
                    }
                    //Debug.Log("L'ancien : " + AncientEmplacementTempo);
                    //Debug.Log("Le nouveau : " + emplacementTempo);
                    AncientEmplacementTempo = emplacementTempo;
                    if (colisActuellementsPose != null && colisActuellementsPose.Count > emplacementTempo)
                    {
                        if (colisActuellementsPose[emplacementTempo] != null)
                        {
                            for (int c = 0; c < managerColis.colisActuellementTraite[emplacementTempo].listArticles.Count; c++)
                            {
                                if (randomArticleVoulu == 0)
                                {
                                    if (colisActuellementsPose[emplacementTempo] == null)
                                    {
                                        randomArticleVoulu = 0;
                                    }
                                    else if (!colisActuellementsPose[emplacementTempo].colisScriptable.listArticles.Contains(managerColis.colisActuellementTraite[emplacementTempo].listArticles[c]) && c != 0)
                                    {
                                        for(int d = 0; d < colisActuellementsPose[emplacementTempo].colisScriptable.listArticles.Count; d++)
                                        {
                                            //Debug.Log("La liste actuelle : " + colisActuellementsPose[emplacementTempo].colisScriptable.listArticles[d].name);
                                        }
                                        //Debug.Log("L'article choisit : " + managerColis.colisActuellementTraite[emplacementTempo].listArticles[c].name);
                                        //Debug.Log("L'emplacement choisit : " + emplacementTempo + " Et le C actuel : " + c);
                                        randomArticleVoulu = c;
                                    }
                                }
                            }
                        }
                    }

                    for (int m = 0; m < colisVider.Count; m++)
                    {
                        int nbSecond = 0;
                        while (managerColis.colisActuellementTraite[emplacementTempo] == null && nbSecond < 10)
                        {
                            emplacementTempo = (emplacementTempo - 1) % 3;
                            nbSecond++;
                        }
                        if (managerColis.colisActuellementTraite[emplacementTempo].listArticles[randomArticleVoulu] == colisVider[m].listArticles[0])
                        {
                            //Debug.Log("randomArticleVoulu : " + randomArticleVoulu);
                            //Debug.Log("Nom du nouvel Article : " + managerColis.colisActuellementTraite[emplacementTempo].listArticles[randomArticleVoulu].name);
                            //Debug.Log("Le colis choisit en fonction : " + colisVider[m].listArticles[0]);

                            int nbMemeArticle = 0;
                            if(colisAvecPack != null && colisAvecPack.Count>0)
                            {
                                Debug.Log("Test Pack d'article 2");
                                foreach (Article art in managerColis.colisActuellementTraite[emplacementTempo].listArticles)
                                {
                                    if(art == colisVider[m].listArticles[0])
                                    {
                                        nbMemeArticle++;
                                    }
                                }

                                if(nbMemeArticle%3 == 0 && Random.Range(0,100) < 100 && nbMemeArticle != 0)
                                {
                                    Debug.Log("Test Pack d'article 1");
                                    emplacementsScripts[empalcementColisCree].GetComponent<AffichagePileArticleGTP>().isFulledWithPack = nbMemeArticle;
                                }
                                else
                                {
                                    emplacementsScripts[empalcementColisCree].GetComponent<AffichagePileArticleGTP>().isFulledWithPack = 0;
                                }
                            }
                            else
                            {
                                Debug.Log("Test Pack d'article 3");
                                emplacementsScripts[empalcementColisCree].GetComponent<AffichagePileArticleGTP>().isFulledWithPack = 0;
                            }

                            newColis = Instantiate(colisVider[m]);
                        }
                    }
                }
                else if (colisVider.Count > 0)
                {
                    Debug.Log("Test Fin Creation Colis à vider");
                    newColis = Instantiate(colisVider[0]);
                }

                if (Random.Range(0, 100) < chanceColisPasRemplit)
                {
                    int nbArticleDebut = newColis.listArticles.Count;
                    for (int i = 0; i < nbArticleDebut * 2 / 3; i++)
                    {
                        newColis.listArticles.RemoveAt(newColis.listArticles.Count - 1);
                    }
                }
                else if (Random.Range(0, 100) < chanceArticlePasBon)
                {
                    while (newColis.listArticles[0] == newColis.listArticles[newColis.listArticles.Count - 1])
                    {
                        newColis.listArticles[newColis.listArticles.Count - 1] = colisVider[Random.Range(0, colisVider.Count - 1)].listArticles[0];
                    }
                }
                else if (Random.Range(0, 100) < chanceArticlePasBon)
                {
                    newColis.gtpSupposedToBe = newColis.listArticles[0];
                    Article newArticleMauvais = colisVider[Random.Range(0, colisVider.Count - 1)].listArticles[0];
                    while (newArticleMauvais == newColis.listArticles[0])
                    {
                        newArticleMauvais = colisVider[Random.Range(0, colisVider.Count - 1)].listArticles[0];
                    }
                    for (int m = 0; m < newColis.listArticles.Count; m++)
                    {
                        newColis.listArticles[m] = newArticleMauvais;
                    }
                }
            }
        }
        return newColis;
    }

    public void FaireVenirNouveauColis(int emplacement)
    {
        StartCoroutine(colisAnimationVenir[emplacement].GetComponent<AnimationFaireVenirColis>().AnimationColis(emplacementsScripts[emplacement]));
        if (colisVider != null)
        {
            emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis = ChoixNouveauColis(emplacement);
            if(emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles.Count <= 3)
            {
                emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().isSupposedToBeEmpty = true;
            }
        }
    }


    public bool PeutFairePartirColis()
    {
        if(emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().isSupposedToBeEmpty)
        {
            RemainingQuantityWindow.SetActive(true);
            RemainingQuantityWindow.GetComponent<RemainingQuantityWindow>().articleNb = emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles.Count;
        }
        else if (tempsReponseChangementColis <= 0)
        {
            if (emplacementsScripts[0].activeSelf && emplacementsScripts[1].activeSelf)
            {
                if(!emplacementsScripts[0].GetComponent<AffichagePileArticleGTP>().isOpen && !emplacementsScripts[1].GetComponent<AffichagePileArticleGTP>().isOpen && (colisActuellementsPose[0] == null || !colisActuellementsPose[0].isOpen) && (colisActuellementsPose[1] == null || !colisActuellementsPose[1].isOpen) && (colisActuellementsPose[2] == null || !colisActuellementsPose[2].isOpen))
                return true;
            }
        }
        return false;
    }

    public void FairePartirUnColis()
    {
        /*if (tempsReponseChangementColis <= 0)
        {
            tempsReponseChangementColis = 0.7f;
            if (emplacementsScripts[0].activeSelf && emplacementsScripts[0].activeSelf)
            {*/
        tempsReponseChangementColis = 0.7f;
        etatColis[emplacement] = false;
        for(int p = 1; p < emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles.Count; p++)
        {
            if (emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles[0] != emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles[p] && !aEteVerifier)
            {
                Debug.Log("Le colis n'a pas été signalé alors qu'il a une anomalie");
                //Malus
            }
        }
        if(aEteVerifier)
        {
            Scoring.instance.WinPointGTP(70);
        }
        aEteVerifier = false;
        emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().enabled = false;
        emplacementsScripts[emplacement].SetActive(false);
        StartCoroutine(colisAnimationVenir[emplacement].GetComponent<AnimationFaireVenirColis>().AnimationColisRenvoie(this));
        if (colisVider != null)
        {
            //emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis = ChoixNouveauColis();
        }
        if (emplacementsScripts[(emplacement + 1) % 2] != null)
        {
            StartCoroutine(ActiverAutreColis((emplacement + 1) % 2));
        }
        emplacement = (emplacement + 1) % 2;
        /*}
    }*/
    }

    IEnumerator ActiverAutreColis(int emplacement)
    {
        Scoring.instance.BeginComboGTP(90);

        for(int m = 0; m < 100; m++)
        {
            if (photoArticle.enabled)
            {
                photoArticle.enabled = false;
            }
            photoArticle.sprite = null;
            emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().enabled = true;
            emplacementsScripts[emplacement].transform.position += new Vector3(0, 1, 0)  * Time.fixedDeltaTime * 0.3f;
            emplacementsScripts[(emplacement + 1) % 2].GetComponent<BoxCollider2D>().enabled = false;
            emplacementsScripts[(emplacement+1)%2].transform.position -= new Vector3(0, 1, 0) * Time.fixedDeltaTime * 0.3f;
            if (m>=99)
            {
                emplacementsScripts[emplacement].GetComponent<BoxCollider2D>().enabled = true;
                //managerColis.AjoutArticleColisVoulu(emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles[0], Random.Range(0,2), Random.Range(1,7));
                List<int> emplacementsConcerne = new List<int>();
                List<int> nombreArticles = new List<int>();
                for (int i = 0; i < managerColis.colisActuellementTraite.Count; i++)
                {
                    if (managerColis.colisActuellementTraite[i] != null)
                    {
                        for (int p = 0; p < managerColis.colisActuellementTraite[i].listArticles.Count; p++)
                        {
                            if (managerColis.colisActuellementTraite[i].listArticles[p] == emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles[0])
                            {
                                if (!emplacementsConcerne.Contains(i))
                                {
                                    emplacementsConcerne.Add(i);
                                    nombreArticles.Add(1);
                                }
                                else
                                {
                                    for (int q = 0; q < emplacementsConcerne.Count; q++)
                                    {
                                        if (emplacementsConcerne[q] == i)
                                        {
                                            nombreArticles[q]++;
                                        }
                                    }

                                }
                            }
                        }
                    }
                    int nbEmplacementPrendre = 0;
                    if (nbEmplacementPrendre < emplacementsConcerne.Count && colisActuellementsPose[emplacementsConcerne[nbEmplacementPrendre]] != null)
                    {
                        if (colisActuellementsPose[emplacementsConcerne[nbEmplacementPrendre]].colisScriptable.listArticles.Contains(emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles[0]))
                        {
                            nbEmplacementPrendre = (nbEmplacementPrendre + 1) % 3;
                            //Debug.Log(nbEmplacementPrendre);
                        }
                        if (nbEmplacementPrendre < emplacementsConcerne.Count && colisActuellementsPose[emplacementsConcerne[nbEmplacementPrendre]] != null)
                        {
                            if (colisActuellementsPose[emplacementsConcerne[nbEmplacementPrendre]].colisScriptable.listArticles.Contains(emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles[0]))
                            {
                                nbEmplacementPrendre = (nbEmplacementPrendre + 1) % 3;
                            }
                        }
                    }
                    if (nbEmplacementPrendre < emplacementsConcerne.Count)
                    {
                        managerColis.AjoutArticleColisVoulu(emplacementsConcerne[nbEmplacementPrendre], nombreArticles[nbEmplacementPrendre]);
                    }
                    if (!photoArticle.enabled)
                    {
                        photoArticle.enabled = true;
                    }
                    if (emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.gtpSupposedToBe != null)
                    {
                        photoArticle.sprite = emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.gtpSupposedToBe.photoGTP;
                    }
                    else
                    {
                        photoArticle.sprite = emplacementsScripts[emplacement].GetComponent<AffichagePileArticleGTP>().currentColis.listArticles[0].photoGTP;
                    }
                    /* for (int u = 0; u < emplacementsConcerne.Count; u++)
                     {
                         managerColis.AjoutArticleColisVoulu(emplacementsConcerne[u], nombreArticles[u]);
                     }*/
                }
            }
            yield return new WaitForSeconds(Time.deltaTime);
        }
    }
}
