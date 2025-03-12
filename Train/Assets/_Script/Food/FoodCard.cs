using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FoodCard : MonoBehaviour
{
    public FoodDirector foodDirector;

    [SerializeField]
    int Food_Num;
    public Image Food_Image;
    public TextMeshProUGUI Food_Name;
    public TextMeshProUGUI Food_Story;
    public TextMeshProUGUI Food_Information;
    Button btn;

    private void Start()
    {
        btn = GetComponentInChildren<Button>();
        btn.onClick.AddListener(ClickFoodCard);
        foodDirector.ButtonList.Add(btn);
    }

    public void SettingCard(FoodDirector director,int num, Sprite image, string name, string story, string information)
    {
        foodDirector = director;
        Food_Num = num;
        Food_Image.sprite = image;
        Food_Name.text = name;
        Food_Story.text = story;
        Food_Information.text = information;
    } 

    public void ClickFoodCard()
    {
        btn.gameObject.SetActive(false);
        foodDirector.receiveFoodCard(Food_Num);
    }
}