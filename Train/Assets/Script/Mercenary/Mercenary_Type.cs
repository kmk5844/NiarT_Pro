using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary_Type : MonoBehaviour
{
    public mercenaryType mercenary_type;

    public float medic_checkHpParsent;
    public float medic_checkStaminaParsent;

    private void Update()
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                medic_checkHpParsent = GetComponent<Engineer>().check_HpParsent();
                medic_checkStaminaParsent = GetComponent<Engineer>().check_StaminaParsent();
                break;
            case mercenaryType.Long_Ranged:
                medic_checkHpParsent = GetComponent<Long_Ranged>().check_HpParsent();
                medic_checkStaminaParsent = GetComponent<Long_Ranged>().check_StaminaParsent();
                break;
            case mercenaryType.Short_Ranged:
                medic_checkHpParsent = GetComponent<Short_Ranged>().check_HpParsent();
                medic_checkStaminaParsent = GetComponent<Short_Ranged>().check_StaminaParsent();
                break;
            case mercenaryType.Medic:
                medic_checkHpParsent = GetComponent<Medic>().check_HpParsent();
                medic_checkStaminaParsent = GetComponent<Medic>().check_StaminaParsent();
                break;
            case mercenaryType.Engine_Driver:
                medic_checkHpParsent = GetComponent<Engine_Driver>().check_HpParsent();
                medic_checkStaminaParsent = GetComponent<Engine_Driver>().check_StaminaParsent();
                break;
        }
    }

    public void Heal_HP(int Medic_Heal)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                GetComponent<Engineer>().HP += Medic_Heal;
                break;
            case mercenaryType.Long_Ranged:
                GetComponent<Long_Ranged>().HP += Medic_Heal;
                break;
            case mercenaryType.Short_Ranged:
                GetComponent<Short_Ranged>().HP += Medic_Heal;
                break;
            case mercenaryType.Medic:
                GetComponent<Medic>().HP += Medic_Heal;
                break;
            case mercenaryType.Engine_Driver:
                GetComponent<Engine_Driver>().HP += Medic_Heal;
                break;
        }
    }

    public void Heal_Stamina(int Medic_Heal)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                GetComponent<Engineer>().Stamina += Medic_Heal;
                break;
            case mercenaryType.Long_Ranged:
                GetComponent<Long_Ranged>().Stamina += Medic_Heal;
                break;
            case mercenaryType.Short_Ranged:
                GetComponent<Short_Ranged>().Stamina += Medic_Heal;
                break;
            case mercenaryType.Medic:
                GetComponent<Medic>().Stamina += Medic_Heal;
                break;
            case mercenaryType.Engine_Driver:
                GetComponent<Engine_Driver>().Stamina += Medic_Heal;
                break;
        }
    }

    public void Heal_Revive(int Medic_Heal)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                StartCoroutine(GetComponent<Engineer>().Revive(Medic_Heal));
                break;
            case mercenaryType.Long_Ranged:
                StartCoroutine(GetComponent<Long_Ranged>().Revive(Medic_Heal));
                break;
            case mercenaryType.Short_Ranged:
                StartCoroutine(GetComponent<Short_Ranged>().Revive(Medic_Heal));
                break;
            case mercenaryType.Medic:
                StartCoroutine(GetComponent<Medic>().Revive(Medic_Heal));
                break;
            case mercenaryType.Engine_Driver:
                StartCoroutine(GetComponent<Engine_Driver>().Revive(Medic_Heal));
                break;
        }
    }
}
public enum mercenaryType
{
    Engineer,
    Long_Ranged,
    Short_Ranged,
    Medic,
    Engine_Driver
}
