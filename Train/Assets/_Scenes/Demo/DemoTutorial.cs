using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DemoTutorial : MonoBehaviour
{
    public SA_PlayerData playerData;
    public SA_StoryData storyData;
    int PlayerStageNum;
    //-> 그걸 이용해서 bool 판단

    public Image Tutorial_Image;
    int InGameCount;
    int StationCount;
    public Sprite[] InGame_Sprite;
    public Sprite[] Station_Sprite;
    bool T_InGame_F_Station;
    // Start is called before the first frame update
    void Start()
    {
        PlayerStageNum = playerData.Stage;

        InGameCount = 0;
        StationCount = 0;

        if(PlayerStageNum == 0)
        {
            T_InGame_F_Station = true;
        }
        else
        {
            T_InGame_F_Station = false;
        }

        if (T_InGame_F_Station)
        {
            Tutorial_Image.sprite = InGame_Sprite[0];
        }
        else
        {
            Tutorial_Image.sprite = Station_Sprite[0];
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ChangeImage();
        }
    }

    void ChangeImage()
    {
        if(T_InGame_F_Station) // InGame
        {
            InGameCount++;
            if(InGameCount < InGame_Sprite.Length)
            {
                Tutorial_Image.sprite = InGame_Sprite[InGameCount];
            }
            else
            {
                storyData.End_Tutorial(PlayerStageNum);
            }
        }
        else // Tutorial
        {
            StationCount++;
            if (StationCount < Station_Sprite.Length)
            {
                Tutorial_Image.sprite = Station_Sprite[StationCount];
            }
            else
            {
                storyData.End_Tutorial(PlayerStageNum);
            }
        }
    }
}
