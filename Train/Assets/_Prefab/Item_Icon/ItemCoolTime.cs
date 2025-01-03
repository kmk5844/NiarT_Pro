using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ItemCoolTime : MonoBehaviour
{
    public Image ItemIcon;
    public Image ItemCoolTime_Panel;
    
    float Cooltime;
    [SerializeField]
    float DelayTime;

    public int ItemCount;
    [SerializeField]
    int Max_ItemCount;

    [SerializeField]
    bool WeaponFlag;
    [SerializeField]
    bool WeaponCountFlag;

    void Start()
    {
        Cooltime = 0;
        ItemCoolTime_Panel.fillAmount = 0f;
    }

    void Update()
    {
        if (WeaponFlag)
        {
            if(WeaponCountFlag == true)
            {
                ItemCoolTime_Panel.fillAmount = Mathf.Clamp01((float)Player.Item_Gun_ClickCount / Max_ItemCount);

                if(Player.Item_Gun_ClickCount == Max_ItemCount)
                {
                    Destroy(gameObject, 0.2f);
                }
            }
            else
            {
                ItemCoolTime_Panel.fillAmount = Mathf.Clamp01(Player.Item_Gun_ClickTime / DelayTime);

                if (Player.Item_Gun_ClickTime >= DelayTime)
                {
                    ItemCoolTime_Panel.fillAmount = 1f;
                    Destroy(gameObject, 0.2f);
                }
            }
        }
        else
        {
            Cooltime += Time.deltaTime; // 경과 시간 업데이트

            ItemCoolTime_Panel.fillAmount = Mathf.Clamp01(Cooltime / DelayTime);

            if (Cooltime >= DelayTime)
            {
                ItemCoolTime_Panel.fillAmount = 1f;
            }

            if (Cooltime >= DelayTime + 0.2f)
            {
                Destroy(gameObject);
            }
        }
    }

    public void SetSetting(ItemDataObject item)
    {
        ItemIcon.sprite = item.Item_Sprite;
        if(item.Item_Type == Information_Item_Type.Weapon)
        {
            WeaponFlag = true;
            if (item.Num == 43 || item.Num == 45)
            {
                WeaponCountFlag = true;
                ItemCount = 0;
                Max_ItemCount = (int)item.Cool_Time;
            }
            else
            {
                WeaponCountFlag = false;
                DelayTime = item.Cool_Time;
            }
        }
        else
        {
            WeaponFlag = false;
            if (item.Cool_Time == 0)
            {
                DelayTime = 0.05f;
            }
            else
            {
                DelayTime = item.Cool_Time;
            }
        }
    }
}
