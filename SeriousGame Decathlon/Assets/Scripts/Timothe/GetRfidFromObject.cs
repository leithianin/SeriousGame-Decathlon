﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetRfidFromObject : MonoBehaviour
{
    public RFID newRFID;

    private bool doesTouch;

    private Vector3 startPosition;

    private void Start()
    {
        startPosition = transform.position;
    }

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            touchObject();

            if (doesTouch)
            {
                transform.position = new Vector3(Camera.main.ScreenToWorldPoint(touch.position).x, Camera.main.ScreenToWorldPoint(touch.position).y, 0);
            }

            if (touch.phase == TouchPhase.Ended)
            {
                transform.position = startPosition;
                doesTouch = false;
            }
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Ended && collision.gameObject.GetComponent<PileArticle>() != null)
            {
                if (TutoManagerMulti.instance != null) {TutoManagerMulti.instance.Manager(31);}
                collision.gameObject.GetComponent<PileArticle>().ChangeRFID(newRFID);
                GameObject.Destroy(gameObject);
            }
        }
    }

    void touchObject()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint((Input.GetTouch(0).position)), Vector2.zero);
            if (hit.collider.gameObject != null && gameObject != null && hit.collider.gameObject == gameObject)
            {
                doesTouch = true;
            }
        }
    }
}
