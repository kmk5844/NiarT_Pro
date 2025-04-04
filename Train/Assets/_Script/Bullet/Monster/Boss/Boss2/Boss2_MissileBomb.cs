using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss2_MissileBomb : MonsterBullet
{
    private void OnDestroy()
    {
        Destroy(GetComponentInParent<Boss2_MissileSkill>().gameObject);
    }

    protected override void Start()
    {
        atk = GetComponentInParent<Boss2_MissileSkill>().atk;
        slow = GetComponentInParent<Boss2_MissileSkill>().slow;
    }
}
