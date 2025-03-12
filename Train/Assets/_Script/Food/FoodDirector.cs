using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;
using UnityEngine.UI;

public class FoodDirector : MonoBehaviour
{
    public DialogSystem Special_Story;
    public Dialog dialog;



    [Header("---------Data-------")]
    public Game_DataTable EX_GameData;

    [Header("--------UI-------")]
    public GameObject FoodWindow;
    public Button RerollButton;
    public GameObject ChoiceButton;

    [Header("--------Card--------")]
    public FoodCard Card;
    public Transform CardList;
    bool startFlag;
    public List<Button> ButtonList;
    int choiceCardNum;

    [Header("--------Food---------")]
    public List<int> Common_FoodList;
    public List<int> Rare_FoodList;
    public List<int> Epic_FoodList;
    public List<int> Legendary_FoodList;
    public List<int> Mythic_FoodList;

    Dictionary<string, int> lootTable = new Dictionary<string, int>()
    {
        {"Common", 70 },
        {"Rare", 24 },
        {"Epic",3 },
        {"Legendary", 2 },
        {"Mythic",1 }
    };


    private void Awake()
    {
        Special_Story.Story_Init(null, 1 , 0);
    }


    private void Start()
    {
        RerollButton.interactable = false;
        ChoiceButton.SetActive(false);
        FoodWindow.SetActive(false);
        Check_FoodRarity();
        RandomCard(false);
    }

    private void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
            startFlag = true;
        }
    }

    private void StartEvent()
    {
        FoodWindow.SetActive(true);
    }

    private void RandomCard(bool flag)
    {
        if (flag)
        {
            foreach(Transform child in CardList)
            {
                Destroy(child.gameObject);
            }
            ButtonList.Clear();
        }

        for (int i = 0; i < 3; i++)
        {
            int totalWeight = 0;
            foreach (var item in lootTable.Values)
            {
                totalWeight += item;
            }

            int randomValue = Random.Range(1, totalWeight + 1);
            int currentWeight = 0;
            string rarity = "";

            Debug.Log(randomValue);
            foreach(var item in lootTable)
            {
                currentWeight += item.Value;
                Debug.Log(currentWeight);
                if(randomValue <= currentWeight)
                {
                    rarity = item.Key;
                    break;
                }
            }

            if(rarity.Equals(""))
            {
                rarity = "Common";
            }

            int randomList;
            int randomNumber = 0;

            switch (rarity)
            {
                case "Common":
                    randomList = Random.Range(0,Common_FoodList.Count);
                    randomNumber = Common_FoodList[randomList];
                    break;
                case "Rare":
                    randomList = Random.Range(0, Rare_FoodList.Count);
                    randomNumber = Rare_FoodList[randomList];
                    break;
                case "Epic":
                    randomList = Random.Range(0, Epic_FoodList.Count);
                    randomNumber = Epic_FoodList[randomList];
                    break;
                case "Legendary":
                    randomList = Random.Range(0, Legendary_FoodList.Count);
                    randomNumber = Legendary_FoodList[randomList];
                    break;
                case "Mythic":
                    randomList = Random.Range(0, Mythic_FoodList.Count);
                    randomNumber = Mythic_FoodList[randomList];
                    break;
            }

            Card.SettingCard(this, randomNumber, null, EX_GameData.Information_FoodCard[randomNumber].Name, EX_GameData.Information_FoodCard[randomNumber].Story, EX_GameData.Information_FoodCard[randomNumber].Information);
            Instantiate(Card, CardList);
        }
    }

    public void receiveFoodCard(int x)
    {
        foreach(Button button in ButtonList)
        {
            button.interactable = false;
        }

        if (!RerollButton.interactable)
        {
            RerollButton.interactable = true;
        }

        if (!ChoiceButton.activeSelf)
        {
            ChoiceButton.SetActive(true);
        }
        choiceCardNum = x;
    }

    public void Click_RerollButton()
    {
        RandomCard(true);
        if(ChoiceButton.activeSelf)
        {
            ChoiceButton.SetActive(false);
        }
    }

    public void Click_ChoiceButton()
    {

    }

    private void Check_FoodRarity()
    {
        for(int i = 0; i < EX_GameData.Information_FoodCard.Count; i++)
        {
            switch(EX_GameData.Information_FoodCard[i].Rarity)
            {
                case "Common":
                    Common_FoodList.Add(i);
                    break;
                case "Rare":
                    Rare_FoodList.Add(i);
                    break;
                case "Epic":
                    Epic_FoodList.Add(i);
                    break;
                case "Legendary":
                    Legendary_FoodList.Add(i);
                    break;
                case "Mythic":
                    Mythic_FoodList.Add(i);
                    break;

            }
        }
    }
}
