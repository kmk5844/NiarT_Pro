using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class FoodCard : MonoBehaviour
{
    public FoodDirector foodDirector;

    [SerializeField]
    int Food_Num;
    public Image Food_Image;
    public GameObject NameTagObject;
    public LocalizeStringEvent Food_Name;
    //public TextMeshProUGUI Food_Story;
    public LocalizeStringEvent Food_Information;
    string grade;
    public Transform HeartList_Red;
    public Transform HeartList_Green;
    Button btn;
    [SerializeField]
    bool HeartColor; // 참이면 레드, 불이면 그린
    public AudioClip OpenSFX;

    private void Start()
    {
        btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(ClickFoodCard);
        foodDirector.ButtonList.Add(btn);
        NameTagObject.SetActive(false);

    }

    public void SettingCard(FoodDirector director,int num, string _grade, bool _color)
    {
        Food_Name.StringReference.TableReference = "Food";
        Food_Information.StringReference.TableReference = "Food";

        foodDirector = director;
        Food_Num = num;
        Food_Image.sprite = Resources.Load<Sprite>("FoodIcon/Food_" + Food_Num);
        Food_Name.StringReference.TableEntryReference = "Food_Name_" + num;
        //Food_Story.text = story;
        Food_Information.StringReference.TableEntryReference = "Food_Effect_" + num;
        grade = _grade;
        HeartColor = _color;
        if (HeartColor)
        {
            HeartGrade_Red();
        }
        else
        {
            HeartGrade_Green();
        }
        HeartList_Red.gameObject.SetActive(false);
        HeartList_Green.gameObject.SetActive(false);
    }

    void HeartGrade_Red()
    {
        switch (grade)
        {
            case "Common":
                HeartList_Red.GetChild(0).gameObject.SetActive(true);
                HeartList_Red.GetChild(1).gameObject.SetActive(false);
                HeartList_Red.GetChild(2).gameObject.SetActive(false);
                HeartList_Red.GetChild(3).gameObject.SetActive(false);
                HeartList_Red.GetChild(4).gameObject.SetActive(false);
                break;
            case "Rare":
                HeartList_Red.GetChild(0).gameObject.SetActive(true);
                HeartList_Red.GetChild(1).gameObject.SetActive(true);
                HeartList_Red.GetChild(2).gameObject.SetActive(false);
                HeartList_Red.GetChild(3).gameObject.SetActive(false);
                HeartList_Red.GetChild(4).gameObject.SetActive(false);
                break;
            case "Epic":
                HeartList_Red.GetChild(0).gameObject.SetActive(true);
                HeartList_Red.GetChild(1).gameObject.SetActive(true);
                HeartList_Red.GetChild(2).gameObject.SetActive(true);
                HeartList_Red.GetChild(3).gameObject.SetActive(false);
                HeartList_Red.GetChild(4).gameObject.SetActive(false);
                break;
            case "Legendary":
                HeartList_Red.GetChild(0).gameObject.SetActive(true);
                HeartList_Red.GetChild(1).gameObject.SetActive(true);
                HeartList_Red.GetChild(2).gameObject.SetActive(true);
                HeartList_Red.GetChild(3).gameObject.SetActive(true);
                HeartList_Red.GetChild(4).gameObject.SetActive(false);
                break;
            case "Mythic":
                HeartList_Red.GetChild(0).gameObject.SetActive(true);
                HeartList_Red.GetChild(1).gameObject.SetActive(true);
                HeartList_Red.GetChild(2).gameObject.SetActive(true);
                HeartList_Red.GetChild(3).gameObject.SetActive(true);
                HeartList_Red.GetChild(4).gameObject.SetActive(true);
                break;
        }
    }

    void HeartGrade_Green()
    {
        switch (grade)
        {
            case "Common":
                HeartList_Green.GetChild(0).gameObject.SetActive(true);
                HeartList_Green.GetChild(1).gameObject.SetActive(false);
                HeartList_Green.GetChild(2).gameObject.SetActive(false);
                HeartList_Green.GetChild(3).gameObject.SetActive(false);
                HeartList_Green.GetChild(4).gameObject.SetActive(false);
                break;
            case "Rare":
                HeartList_Green.GetChild(0).gameObject.SetActive(true);
                HeartList_Green.GetChild(1).gameObject.SetActive(true);
                HeartList_Green.GetChild(2).gameObject.SetActive(false);
                HeartList_Green.GetChild(3).gameObject.SetActive(false);
                HeartList_Green.GetChild(4).gameObject.SetActive(false);
                break;
            case "Epic":
                HeartList_Green.GetChild(0).gameObject.SetActive(true);
                HeartList_Green.GetChild(1).gameObject.SetActive(true);
                HeartList_Green.GetChild(2).gameObject.SetActive(true);
                HeartList_Green.GetChild(3).gameObject.SetActive(false);
                HeartList_Green.GetChild(4).gameObject.SetActive(false);
                break;
            case "Legendary":
                HeartList_Green.GetChild(0).gameObject.SetActive(true);
                HeartList_Green.GetChild(1).gameObject.SetActive(true);
                HeartList_Green.GetChild(2).gameObject.SetActive(true);
                HeartList_Green.GetChild(3).gameObject.SetActive(true);
                HeartList_Green.GetChild(4).gameObject.SetActive(false);

                break;
            case "Mythic":
                HeartList_Green.GetChild(0).gameObject.SetActive(true);
                HeartList_Green.GetChild(1).gameObject.SetActive(true);
                HeartList_Green.GetChild(2).gameObject.SetActive(true);
                HeartList_Green.GetChild(3).gameObject.SetActive(true);
                HeartList_Green.GetChild(4).gameObject.SetActive(true);
                break;
        }
    }


    public void ClickFoodCard()
    {
        btn.gameObject.SetActive(false);
        MMSoundManagerSoundPlayEvent.Trigger(OpenSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
        NameTagObject.SetActive(true);
        if (HeartColor)
        {
            HeartList_Red.gameObject.SetActive(true);
        }
        else
        { 
            HeartList_Green.gameObject.SetActive(true);
        }
        foodDirector.receiveFoodCard(Food_Num);
    }
}