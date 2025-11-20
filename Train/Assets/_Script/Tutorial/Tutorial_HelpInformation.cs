using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class Tutorial_HelpInformation : MonoBehaviour
{
    public GameObject[] help_object;
    public LocalizeStringEvent HelpText;

    public Button PrevButton;
    public Button NextButton;

    public string Help_String;
    int help_num;
    int before_help_num;

    public GameObject CheckButton;
    public bool ReadEndFlag;

    private void Start()
    {
        help_num = 0;
        HelpText.StringReference.TableReference = "Tutorial_St";
        CheckButton.SetActive(false);
        ReadEndFlag = false;
        Check();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            ClickPrevButton();
        }
        else if (Input.GetKeyDown(KeyCode.D))
        {
            ClickNextButton();
        }
    }

    public void ClickNextButton()
    {
        if (help_num < help_object.Length - 1)
        {
            before_help_num = help_num;
            help_num++;
            Check();
        }
    }

    public void ClickPrevButton()
    {
        if(help_num > 0)
        {
            before_help_num = help_num;
            help_num--;
            Check();
        }
    }

    void Check()
    {
        if(help_num == 0)
        {
            PrevButton.interactable = false;
        }
        else
        {
            PrevButton.interactable = true;
        }

        if(help_num == help_object.Length - 1)
        {
            NextButton.interactable = false;
            CheckButton.SetActive(true);
        }
        else
        {
            NextButton.interactable = true; 
        }

        help_object[before_help_num].SetActive(false);
        help_object[help_num].SetActive(true);
        //HelpText.StringReference.TableEntryReference = Help_String + "_" + help_num;
    }

    public void ReadEnd_Click()
    {
        gameObject.SetActive(false);
        ReadEndFlag = true;
    }
}