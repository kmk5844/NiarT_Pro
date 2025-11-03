using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PlayerStatusDirector : MonoBehaviour
{
    //public GameDirector gamedirector;
    Player player;

    int origin_Atk;
    float origin_AtkDelay;
    int origin_Armor;
    float orgin_moveSpeed;

    int now_ATK;
    float now_AtkDelay;
    int now_Armor;
    float now_moveSpeed;

    public TextMeshProUGUI AtkText;
    public TextMeshProUGUI AtkDelayText;
    public TextMeshProUGUI ArmorText;
    public TextMeshProUGUI MoveSpeedText;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Player>();
    }

    private void Update()
    {
        AtkTextChange();
        AtkDelayTextChange();
        ArmorTextChange();
        MoveSpeedTextChange();
    }

    void AtkTextChange()
    {
        if (now_ATK - origin_Atk > 0)
        {
            AtkText.text = origin_Atk + "<color=green> + " + (now_ATK - origin_Atk);
        }
        else if (now_ATK - origin_Atk < 0)
        {
            AtkText.text = origin_Atk + "<color=red> - " + (now_ATK - origin_Atk);
        }
        else
        {
            AtkText.text = origin_Atk.ToString();
        }
    }

    void AtkDelayTextChange()
    {
        if (now_AtkDelay - origin_AtkDelay > 0)
        {
            AtkDelayText.text = origin_AtkDelay.ToString("F2") + "<color=red> + " + (now_AtkDelay - origin_AtkDelay).ToString("F1");
        }
        else if (now_AtkDelay - origin_AtkDelay < 0)
        {
            AtkDelayText.text = origin_AtkDelay.ToString("F2") + "<color=green> - " + (origin_AtkDelay - now_AtkDelay).ToString("F1");
        }
        else
        {
            AtkDelayText.text = origin_AtkDelay.ToString("F2");
        }
    }

    void ArmorTextChange()
    {
        if (now_Armor - origin_Armor > 0)
        {
            ArmorText.text = origin_Armor + "<color=green> + " + (now_Armor - origin_Armor);
        }
        else if (now_Armor - origin_Armor < 0)
        {
            ArmorText.text = origin_Armor + "<color=red> - " + (origin_Armor - now_Armor);
        }
        else
        {
            ArmorText.text = origin_Armor.ToString();
        }
    }

    void MoveSpeedTextChange()
    {
        if (now_moveSpeed - orgin_moveSpeed > 0)
        {
            MoveSpeedText.text = orgin_moveSpeed.ToString("F1") + "<color=green> + " + (now_moveSpeed - orgin_moveSpeed).ToString("F2");
        }
        else if (now_moveSpeed - orgin_moveSpeed < 0)
        {
            MoveSpeedText.text = orgin_moveSpeed.ToString("F1") + "<color=red> - " + (orgin_moveSpeed - now_moveSpeed).ToString("F2");
        }
        else
        {
            MoveSpeedText.text = orgin_moveSpeed.ToString("F1");
        }
    }


    public void SetOriginStatus(int atk, float atkDelay, int armor, float moveSpeed)
    {
        origin_Atk = atk;
        origin_AtkDelay = atkDelay;
        origin_Armor = armor;
        orgin_moveSpeed = moveSpeed;
    }

    public void GetNowSatus(int atk, float atkDelay, int armor, float moveSpeed)
    {
        now_ATK = atk;
        now_AtkDelay = atkDelay;
        now_Armor = armor;
        now_moveSpeed = moveSpeed;
    }
}