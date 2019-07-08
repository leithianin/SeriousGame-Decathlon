﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class CreateHUScript : MonoBehaviour
{
    private string packagingMat   = "";
    private string workStation    = "";
    private string madeIn         = "";
    private string dateExpiration = "";
    private int quantity          =  0;
    private int reference         =  0;

    public TMP_Dropdown dropdownPackaging;
    public TMP_Dropdown dropdownWorkStation;
    public TMP_Dropdown dropdownReference;
    public TMP_Dropdown dropdownMadeIn;
    public TMP_InputField inputExpirationDate;

    public TMP_Text textQuantity;

    public OngletManager om;

    private void Start()
    {
        dropdownPackaging.onValueChanged     .AddListener(delegate { SetPackaging     (dropdownPackaging  ); });
        dropdownWorkStation.onValueChanged   .AddListener(delegate { SetWorkStation   (dropdownWorkStation); });
        dropdownReference.onValueChanged     .AddListener(delegate { SetReference     (dropdownReference  ); });
        dropdownMadeIn.onValueChanged        .AddListener(delegate { SetMadeIn        (dropdownMadeIn     ); });
        inputExpirationDate.onValueChanged   .AddListener(delegate { SetExpirationDate(inputExpirationDate); });
    }

    public void SetPackaging(TMP_Dropdown input)
    {
        packagingMat = input.GetComponent<TMP_Dropdown>().options[input.GetComponent<TMP_Dropdown>().value].text;

        if (TutoManager.instance != null && packagingMat == "CB02")
        {
            TutoManager.instance.Manager(35);
        }
    }

    public void SetWorkStation(TMP_Dropdown input)
    {
        workStation = input.GetComponent<TMP_Dropdown>().options[input.GetComponent<TMP_Dropdown>().value].text;
    }

    public void SetReference(TMP_Dropdown input)
    {
        reference = int.Parse(input.GetComponent<TMP_Dropdown>().options[input.GetComponent<TMP_Dropdown>().value].text);

        if(TutoManager.instance != null && reference == 4635)
        {
            TutoManager.instance.Manager(36);
        }

        //reference =int.Parse(dropdownReference.itemText.ToString());
    }

    public void SetMadeIn(TMP_Dropdown input)
    {
        madeIn = input.GetComponent<TMP_Dropdown>().options[input.GetComponent<TMP_Dropdown>().value].text;
    }

    public void SetExpirationDate(TMP_InputField input)
    {
        //dateExpiration = inputExpirationDate.ToString();
    }

    public void SetQuantity()
    {
        textQuantity.text = quantity.ToString();
        
        if(TutoManager.instance != null && textQuantity.text == "5")
        {
            TutoManager.instance.Manager(37);
        }
    }
    
    public void Plus()
    {
        quantity++;
        SetQuantity();
    }

    public void Moins()
    {
        if (quantity >= 0)
        {
            quantity--;
            SetQuantity();
        }
    }

    public void ClickOK()
    {
        if(reference != 0 && quantity != 0)
        {
            om.CreateHUOK(quantity, reference);
        }
        else
        {

        }
    }

    public void ClickCancel()
    {
        om.CreateHUCancel();
    }
}
