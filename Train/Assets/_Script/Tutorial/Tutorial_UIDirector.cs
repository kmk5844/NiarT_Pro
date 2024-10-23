using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class Tutorial_UIDirector : MonoBehaviour
{
    public Tutorial_Player Player;
    public GamePlay_Tutorial_Director tutorialDirector;

    public List<GameObject> GameUI;
    public List<GameObject> GameUI_Information;
    int count;
    int MaxCount;

    public Sprite[] Tutorial_Icon_Sprite;
    public Image[] Item_Icon;
    public Image[] Skill_Icon;
    public Image[] Skill_Icon_CoolTime;
    public GameObject item_Icon_Count_Object;

    public Image PlayerHP_Image;
    public TextMeshProUGUI Score_Text;
    public TextMeshProUGUI Gold_Text;
    public TextMeshProUGUI Speed_Text;
    public Slider Speed_Arrow;
    public TextMeshProUGUI Fuel_Text;
    public Image Fuel_Image;
    public Slider Distance_UI;
    public GameObject ClearObject;
    [Header("게임 튜토리얼")]
    public GameObject GameTutorial_Window;
    public LocalizeStringEvent Title_Text;
    public LocalizeStringEvent Information_Text;
    public GameObject Compelte_Object;

    private void Start()
    {
        Title_Text.StringReference.TableReference = "Tutorial_St";
        Information_Text.StringReference.TableReference = "Tutorial_St";
        Title_Text.StringReference.TableEntryReference = "Tutorial_Game_Title_0";
        Information_Text.StringReference.TableEntryReference = "Tutorial_Game_Information_0";

        foreach (Image image in Item_Icon)
        {
            image.sprite = Tutorial_Icon_Sprite[0];
        }
        item_Icon_Count_Object.SetActive(false);

        foreach (GameObject game in GameUI) {
            game.SetActive(false);
        }

        foreach (GameObject gameInfo in GameUI_Information){
            gameInfo.SetActive(false);
        }

        count = 0;

        MaxCount = GameUI.Count;
    }

    private void Update()
    {
        PlayerHP_Image.fillAmount = ((float)Player.PlayerHP / (float)Player.Max_PlayerHP);
        Score_Text.text = tutorialDirector.score.ToString();
        Gold_Text.text = tutorialDirector.gold.ToString();
        Speed_Text.text = (int)tutorialDirector.speed + " Km/H";
        Speed_Arrow.value = tutorialDirector.speed/tutorialDirector.Max_Speed;
        float fuelPersent = (float)(tutorialDirector.Fuel / (float)tutorialDirector.Max_Fuel);
        Fuel_Text.text = (int)(fuelPersent * 100) + "%";
        Fuel_Image.fillAmount = fuelPersent;
        Distance_UI.value = (float)tutorialDirector.distance / (float)tutorialDirector.max_distance;
    }

    public void nextTutorial()
    {
        GameUI[count].SetActive(true);
        if(count == 0)
        {
            GameUI_Information[count].SetActive(true);
        }
        else
        {
            if(count < MaxCount - 1)
            {
                GameUI_Information[count - 1].SetActive(false);
                GameUI_Information[count].SetActive(true);
            }
        }
        count++;
    }

    public void item_changeIcon(bool flag)
    {
        if (flag)
        {
            Item_Icon[0].sprite = Tutorial_Icon_Sprite[1];
            item_Icon_Count_Object.SetActive(true);
        }
        else
        {
            Item_Icon[0].sprite = Tutorial_Icon_Sprite[0];
            item_Icon_Count_Object.SetActive(false);
        }
    }

    public void skill_changeIcon(bool flag)
    {
        if (flag)
        {
            Skill_Icon[0].sprite = Tutorial_Icon_Sprite[2];
        }
        else
        {
            Skill_Icon[1].sprite = Tutorial_Icon_Sprite[3];
        }
    }

    public void skill_coolTime(int i)
    {
        Skill_Icon_CoolTime[i].fillAmount = 1f;
    }

    public void lastTutorial()
    {
        GameUI_Information[GameUI_Information.Count-1].SetActive(false);
    }

    public bool checkFlag()
    {
        if(count == MaxCount)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void changeText(int index)
    {
        Title_Text.StringReference.TableEntryReference = "Tutorial_Game_Title_"+ index;
        Information_Text.StringReference.TableEntryReference = "Tutorial_Game_Information_"+ index;
    }
}
