using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkillIcon_Event : MonoBehaviour
{
    public int Player_Num;
    public int Skill_Num;

    public SkillICon_ToolTip Tooltip_Object;

    bool skill_Information_Flag;
    bool skill_MouseOver_Flag;
   
    // Update is called once per frame
    void Update()
    {
        if (skill_Information_Flag)
        {
            Tooltip_Object.Tooltip_ON(Player_Num, Skill_Num);
            skill_MouseOver_Flag = true;
        }
        else
        {
            if (skill_MouseOver_Flag)
            {
                Tooltip_Object.Tooltip_Off();
                skill_MouseOver_Flag = false;
            }
        }
    }

    public void OnMouseEnter()
    {
        skill_Information_Flag = true;
    }

    public void OnMouseExit()
    {
        skill_Information_Flag = false;
    }
}
