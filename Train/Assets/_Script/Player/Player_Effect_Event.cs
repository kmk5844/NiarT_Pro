using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Effect_Event : MonoBehaviour
{
    [Header("±‚∫ª √— ¿Ã∆Â∆Æ")]
    public ParticleSystem Gun0;
    public ParticleSystem Gun1;
    /*public ParticleSystem Gun2;
    public ParticleSystem Gun3;
    public ParticleSystem Gun4;*/

    [Header("∆Øºˆ √— ¿Ã∆Â∆Æ")]
    public ParticleSystem FlashBang;
    public ParticleSystem Gatling_Gun;
    public ParticleSystem Missile_Gun;
    public ParticleSystem Laser_Gun;
    public ParticleSystem Fire_Gun;
    public ParticleSystem Grenade;
    public ParticleSystem Signal_Flare;
    public ParticleSystem Grenade_Gun;
    public ParticleSystem Sniper_Gun;

    [Header("««∞› ¿Ã∆Â∆Æ")]
    public ParticleSystem Hit_Effect;

    public void GunEvent_0()
    {
        Gun0.Play();
    }
    public void GunEvent_1()
    {
        Gun1.Play();
    }
    public void FlashBangEvent()
    {
        FlashBang.Play();
    }
    public void Gatling_GunEvent()
    {
        Gatling_Gun.Play();
    }
    public void Missile_GunEvent()
    {
        Missile_Gun.Play();
    }
    public void Laser_GunEvent()
    {
        Laser_Gun.Play();
    }
    public void Fire_GunEvent()
    {
        Fire_Gun.Play();
    }
    public void GrenadeEvent()
    {
        Grenade.Play();
    }
    public void Signal_FlareEvent()
    {
        Signal_Flare.Play();
    }
    public void Grenade_GunEvent()
    {
        Grenade_Gun.Play();
    }
    public void Sniper_GunEvent()
    {
        Sniper_Gun.Play();
    }

    public void Hit_EffectEvent()
    {
        Hit_Effect.Play();
    }
}
