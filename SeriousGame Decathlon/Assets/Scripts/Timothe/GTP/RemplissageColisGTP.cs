﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RemplissageColisGTP : MonoBehaviour
{
    public Colis colisScriptable;

    public Transform positionVisee;
    public float speed = 1;
    public Vector3 startPosition;

    public bool didArrive;
    private bool doesTouch;
    public bool canBeTouch = true;

    public List<GameObject> tasArticle;

    public int currentPhase = 0;
    public float tauxRemplissage;
    public bool besoinEtreVide;
    public Image remplissageImage;

    public Canvas barreCanvas;

    public bool isOpen;
    public BoxCollider2D boxDesactivee;

    public int nbArticleScanned;
    public bool estParti;

    public GameObject spriteRemplit;

    private void Start()
    {
        colisScriptable = Instantiate(colisScriptable);
    }

    void Update()
    {
        if(tauxRemplissage>1 && !spriteRemplit.activeSelf)
        {
            spriteRemplit.SetActive(true);
        }
        else if (spriteRemplit.activeSelf)
        {
            spriteRemplit.SetActive(false);
        }
        if (Input.touchCount > 0 && !estParti)
        {
            touchObject();

            if (Input.GetTouch(0).phase == TouchPhase.Ended)
            {
                doesTouch = false;
            }

            if (doesTouch && canBeTouch)
            {
                doesTouch = false;
                List<List<Article>> newListes    = new List<List<Article>>();
                List<Article> articlesDejaConnus = new List     <Article> ();

                if (!tasArticle[0].activeSelf)
                {
                    if (colisScriptable.listArticles.Count > 0)
                    {
                        for (int i = 0; i < colisScriptable.listArticles.Count; i++)
                        {
                            if (!articlesDejaConnus.Contains(colisScriptable.listArticles[i]))
                            {
                                newListes.Add(new List<Article>());
                                newListes[newListes.Count - 1].Add(colisScriptable.listArticles[i]);
                                articlesDejaConnus.Add(colisScriptable.listArticles[i]);
                            }
                            else
                            {
                                for (int j = 0; j < newListes.Count; j++)
                                {
                                    if (newListes[j].Contains(colisScriptable.listArticles[i]))
                                    {
                                        newListes[j].Add(colisScriptable.listArticles[i]);
                                    }
                                }
                            }
                        }
                        colisScriptable.listArticles = new List<Article>();
                        tauxRemplissage = (float)colisScriptable.listArticles.Count / 10f;
                        remplissageImage.fillAmount = tauxRemplissage;
                        for (int l = 0; l < newListes.Count; l++)
                        {
                            if (!tasArticle[l].activeSelf && newListes != null && newListes[l] != null)
                            {
                                tasArticle[l].SetActive(true);
                                StartCoroutine(tasArticle[l].GetComponent<TasArticleGTP>().ApparitionArticle(newListes[l], 0, this));
                                isOpen = true;
                            }
                        }
                        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 0.4f);
                    }
                }
                else
                {

                    bool repack = false;
                    for (int m = 0; m < tasArticle.Count; m++)
                    {
                        if (tasArticle[m].activeSelf)
                        {
                            tasArticle[m].SetActive(false);
                            nbArticleScanned = tasArticle[m].GetComponent<TasArticleGTP>().ReturnNumberScanned();
                            newListes.Add(tasArticle[m].GetComponent<TasArticleGTP>().CloseTasArticle());
                            repack = true;
                        }
                    }
                    GetComponent<SpriteRenderer>().color = new Color(255,255,255, 1f);
                    isOpen = false;

                    if (repack)
                    {
                        //colisScriptable.listArticles = new List<Article>();
                        for (int i = 0; i < newListes.Count; i++)
                        {
                            for (int p = 0; p < newListes[i].Count; p++)
                            {
                                colisScriptable.listArticles.Add(newListes[i][p]);
                                tauxRemplissage = (float)colisScriptable.listArticles.Count / 10f;
                                remplissageImage.fillAmount = tauxRemplissage;
                            }
                        }
                    }

                    if (besoinEtreVide)
                    {
                        besoinEtreVide = false;
                    }
                }
            }
        }
    }

    public void AddArticle(Article articleToHad)
    {
        if(colisScriptable.listArticles == null)
        {
            colisScriptable.listArticles = new List<Article>();
        }
        colisScriptable.listArticles.Add(articleToHad);
        tauxRemplissage = (float)colisScriptable.listArticles.Count / 10f;
        remplissageImage.fillAmount = tauxRemplissage;
    }

    public IEnumerator AnimationColisRenvoie()
    {
        estParti = true;
        if (canBeTouch)
        {
            canBeTouch = false;
            GetComponent<SpriteRenderer>().sortingOrder--;
            barreCanvas.sortingOrder-=2;
            remplissageImage.enabled = false;
        }
        if (Vector3.Distance(startPosition, transform.position) < 1.5f && !didArrive)
        {
            transform.position   += new Vector3(0, 1, 0) * Time.fixedDeltaTime * 2f;
            //transform.localScale -=     Vector3.one      * Time.fixedDeltaTime * 0.13f;
            yield return new WaitForSeconds(Time.fixedDeltaTime);
            StartCoroutine(AnimationColisRenvoie());
        }
        else if (Vector3.Distance(startPosition, transform.position) < 25f)
        {
            transform.position -= new Vector3(-1,0,0) * Time.deltaTime * speed;
            yield return new WaitForSeconds(Time.deltaTime);
            StartCoroutine(AnimationColisRenvoie());
            didArrive = true;
        }
        else
        {
            didArrive = false;
            StopAllCoroutines();
            Destroy(gameObject);
        }
        yield return null;
    }

    void touchObject()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if (hit.collider != null && hit.collider.gameObject != null && gameObject != null && hit.collider.gameObject == gameObject && hit.collider.gameObject.name == gameObject.name)
            {
                doesTouch = true;
            }
        }
    }
}
