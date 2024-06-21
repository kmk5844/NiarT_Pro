using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary_Type : MonoBehaviour
{
    mercenaryType mercenary_type;

    public float medic_checkHpParsent;
    public float medic_checkStaminaParsent;

    private void Start()
    {
        mercenary_type = GetComponent<Mercenary_New>().Type;
    }

    private void Update()
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                //medic_checkHpParsent = GetComponent<Engineer>().check_HpParsent();
                medic_checkHpParsent = GetComponent<Engineer_New>().Check_HpParsent();
                //medic_checkStaminaParsent = GetComponent<Engineer>().check_StaminaParsent();
                break;
            case mercenaryType.Long_Ranged:
                medic_checkHpParsent = GetComponent<Long_Ranged_New>().Check_HpParsent();
                //medic_checkHpParsent = GetComponent<Long_Ranged>().check_HpParsent();
                //medic_checkStaminaParsent = GetComponent<Long_Ranged>().check_StaminaParsent();
                break;
            case mercenaryType.Short_Ranged:
                medic_checkHpParsent = GetComponent<Short_Ranged>().check_HpParsent();
                //medic_checkStaminaParsent = GetComponent<Short_Ranged>().check_StaminaParsent();
                break;
            case mercenaryType.Medic:
                medic_checkHpParsent = GetComponent<Medic_New>().Check_HpParsent();
                //medic_checkStaminaParsent = GetComponent<Medic>().check_StaminaParsent();
                break;
            case mercenaryType.Engine_Driver:
                //medic_checkHpParsent = GetComponent<Engine_Driver>().check_HpParsent();
                medic_checkHpParsent = GetComponent<Engine_Driver_New>().Check_HpParsent();
                //medic_checkStaminaParsent = GetComponent<Engine_Driver>().check_StaminaParsent();
                break;
            case mercenaryType.Bard:
                //medic_checkHpParsent = GetComponent<Bard>().check_HpParsent();
                medic_checkHpParsent = GetComponent<Bard_New>().Check_HpParsent();
                //medic_checkStaminaParsent = GetComponent<Bard>().check_StaminaParsent();
                break;
            case mercenaryType.CowBoy:
                medic_checkHpParsent = GetComponent<CowBoy>().check_HpParsent();
                //medic_checkStaminaParsent = GetComponent<CowBoy>().check_StaminaParsent();
                break;
        }
    }

    public void Heal_HP(int Medic_Heal)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                GetComponent<Engineer_New>().HP += Medic_Heal;
                break;
            case mercenaryType.Long_Ranged:
                GetComponent<Long_Ranged>().HP += Medic_Heal;
                break;
            case mercenaryType.Short_Ranged:
                GetComponent<Short_Ranged>().HP += Medic_Heal;
                break;
            case mercenaryType.Medic:
                GetComponent<Medic_New>().HP += Medic_Heal;
                break;
            case mercenaryType.Engine_Driver:
                GetComponent<Engine_Driver_New>().HP += Medic_Heal;
                break;
            case mercenaryType.Bard:
                GetComponent<Bard_New>().HP += Medic_Heal;
                break;
            case mercenaryType.CowBoy:
                GetComponent<CowBoy>().HP += Medic_Heal;
                break;
        }
    }
/*
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
            case mercenaryType.Bard:
                GetComponent<Bard>().Stamina += Medic_Heal;
                break;
        }
    }
*/
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
            case mercenaryType.Bard:
                StartCoroutine(GetComponent<Bard>().Revive(Medic_Heal));
                break;
            case mercenaryType.CowBoy:
                StartCoroutine(GetComponent<CowBoy>().Revive(Medic_Heal));
                break;
        }
    }

    public void Buff_HP(int hp, bool flag)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                GetComponent<Engineer>().M_Buff_HP(hp,flag);
                break;
            case mercenaryType.Long_Ranged:
                GetComponent<Long_Ranged>().M_Buff_HP(hp, flag);
                break;
            case mercenaryType.Short_Ranged:
                GetComponent<Short_Ranged>().M_Buff_HP(hp, flag);
                break;
            case mercenaryType.Medic:
                GetComponent<Medic>().M_Buff_HP(hp, flag);
                break;
            case mercenaryType.Engine_Driver:
                GetComponent<Engine_Driver_New>().Mer_Buff_HP(hp, flag);
                break;
            case mercenaryType.Bard:
                GetComponent<Bard_New>().Mer_Buff_HP(hp, flag);
                break;
            case mercenaryType.CowBoy:
                GetComponent<CowBoy>().M_Buff_HP(hp, flag);
                break;
        }
    }

    public void Buff_Atk(int atk, bool flag)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Long_Ranged:
                GetComponent<Long_Ranged>().M_Buff_Atk(atk, flag);
                break;
            case mercenaryType.Short_Ranged:
                GetComponent<Short_Ranged>().M_Buff_Atk(atk, flag);
                break;
        }
    }

    public void Buff_Def(int def, bool flag)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                GetComponent<Engineer>().M_Buff_Def(def, flag);
                break;
            case mercenaryType.Long_Ranged:
                GetComponent<Long_Ranged>().M_Buff_Def(def, flag);
                break;
            case mercenaryType.Short_Ranged:
                GetComponent<Short_Ranged>().M_Buff_Def(def, flag);
                break;
            case mercenaryType.Medic:
                GetComponent<Medic>().M_Buff_Def(def, flag);
                break;
            case mercenaryType.Engine_Driver:
                GetComponent<Engine_Driver_New>().Mer_Buff_Def(def, flag);
                break;
            case mercenaryType.Bard:
                GetComponent<Bard_New>().Mer_Buff_Def(def, flag);
                break;
            case mercenaryType.CowBoy:
                GetComponent<CowBoy>().M_Buff_Def(def,flag);
                break;
        }
    }
}