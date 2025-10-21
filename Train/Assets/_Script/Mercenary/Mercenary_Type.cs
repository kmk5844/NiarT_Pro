using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary_Type : MonoBehaviour
{
    mercenaryType mercenary_type;

    public float medic_checkHpParsent;

    private void Start()
    {
        mercenary_type = GetComponent<Mercenary>().Type;
    }

    private void Update()
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                medic_checkHpParsent = GetComponent<Engineer>().Check_HpParsent();
                break;
            case mercenaryType.Long_Ranged:
                medic_checkHpParsent = GetComponent<Long_Ranged>().Check_HpParsent();
                break;
            case mercenaryType.Short_Ranged:
                medic_checkHpParsent = GetComponent<Short_Ranged>().Check_HpParsent();
                break;
            case mercenaryType.Medic:
                medic_checkHpParsent = GetComponent<Medic>().Check_HpParsent();
                break;
            case mercenaryType.Engine_Driver:
                medic_checkHpParsent = GetComponent<Engine_Driver>().Check_HpParsent();
                break;
            case mercenaryType.Bard:
                medic_checkHpParsent = GetComponent<Bard>().Check_HpParsent();
                break;
            case mercenaryType.CowBoy:
                medic_checkHpParsent = GetComponent<CowBoy>().Check_HpParsent();
                break;
        }
    }

    public void Medic_Heal_HP(int Medic_Heal)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                PlayerLogDirector.MercenaryHeal(4, 1);
                GetComponent<Engineer>().HP += Medic_Heal;
                break;
            case mercenaryType.Long_Ranged:
                PlayerLogDirector.MercenaryHeal(4, 2);
                GetComponent<Long_Ranged>().HP += Medic_Heal;
                break;
            case mercenaryType.Short_Ranged:
                PlayerLogDirector.MercenaryHeal(4, 3);
                GetComponent<Short_Ranged>().HP += Medic_Heal;
                break;
            case mercenaryType.Medic:
                PlayerLogDirector.MercenaryHeal(4, 4);
                GetComponent<Medic>().HP += Medic_Heal;
                break;
            case mercenaryType.Engine_Driver:
                PlayerLogDirector.MercenaryHeal(4, 0);
                GetComponent<Engine_Driver>().HP += Medic_Heal;
                break;
            case mercenaryType.Bard:
                PlayerLogDirector.MercenaryHeal(4, 5);
                GetComponent<Bard>().HP += Medic_Heal;
                break;
            case mercenaryType.CowBoy:
                PlayerLogDirector.MercenaryHeal(4, 6);
                GetComponent<CowBoy>().HP += Medic_Heal;
                break;
        }
    }

    public void Heal_Revive(int Medic_Heal)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                //StartCoroutine(GetComponent<Engineer>().Revive(Medic_Heal));
                break;
            case mercenaryType.Long_Ranged:
                //StartCoroutine(GetComponent<Long_Ranged>().Revive(Medic_Heal));
                break;
            case mercenaryType.Short_Ranged:
                //StartCoroutine(GetComponent<Short_Ranged>().Revive(Medic_Heal));
                break;
            case mercenaryType.Medic:
                //StartCoroutine(GetComponent<Medic>().Revive(Medic_Heal));
                break;
            case mercenaryType.Engine_Driver:
                //StartCoroutine(GetComponent<Engine_Driver>().Revive(Medic_Heal));
                break;
            case mercenaryType.Bard:
                //StartCoroutine(GetComponent<Bard>().Revive(Medic_Heal));
                break;
            case mercenaryType.CowBoy:
                //StartCoroutine(GetComponent<CowBoy>().Revive(Medic_Heal));
                break;
        }
    }

    public void Buff_HP(int hp, bool flag)
    {
        switch (mercenary_type)
        {
            case mercenaryType.Engineer:
                GetComponent<Engineer>().Mer_Buff_HP(hp,flag);
                break;
            case mercenaryType.Long_Ranged:
                GetComponent<Long_Ranged>().Mer_Buff_HP(hp, flag);
                break;
            case mercenaryType.Short_Ranged:
                GetComponent<Short_Ranged>().Mer_Buff_HP(hp, flag);
                break;
            case mercenaryType.Medic:
                GetComponent<Medic>().Mer_Buff_HP(hp, flag);
                break;
            case mercenaryType.Engine_Driver:
                GetComponent<Engine_Driver>().Mer_Buff_HP(hp, flag);
                break;
            case mercenaryType.Bard:
                GetComponent<Bard>().Mer_Buff_HP(hp, flag);
                break;
            case mercenaryType.CowBoy:
                GetComponent<CowBoy>().Mer_Buff_HP(hp, flag);
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
                GetComponent<Engineer>().Mer_Buff_Def(def, flag);
                break;
            case mercenaryType.Long_Ranged:
                GetComponent<Long_Ranged>().Mer_Buff_Def(def, flag);
                break;
            case mercenaryType.Short_Ranged:
                GetComponent<Short_Ranged>().Mer_Buff_Def(def, flag);
                break;
            case mercenaryType.Medic:
                GetComponent<Medic>().Mer_Buff_Def(def, flag);
                break;
            case mercenaryType.Engine_Driver:
                GetComponent<Engine_Driver>().Mer_Buff_Def(def, flag);
                break;
            case mercenaryType.Bard:
                GetComponent<Bard>().Mer_Buff_Def(def, flag);
                break;
            case mercenaryType.CowBoy:
                GetComponent<CowBoy>().Mer_Buff_Def(def,flag);
                break;
        }
    }
}