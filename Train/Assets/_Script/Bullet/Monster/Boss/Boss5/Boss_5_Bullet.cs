using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_5_Bullet : MonsterBullet
{
    public bool ShotGunFlag;
    public bool RifleFlag;
    public bool RoDanteFlag;
    public float Angle;
    public bool AcerSkillFlag;
    protected override void Start()
    {
        base.Start();
        Speed = Random.Range(30f, 45f);
        BulletFire();
    }

    void BulletFire()
    {
        // 플레이어 방향 설정
        Vector2 dir = (base.player_target.position - transform.position).normalized;

        // 무작위 각도 추가
        float angleOffset = Random.Range(-9f, 9f);
        dir = Quaternion.Euler(0, 0, angleOffset) * (Vector3)dir;

        // 회전 설정
        float rot = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90f);

        // 속도 설정
        rid.velocity = dir * Speed;

        Destroy(gameObject, 3f);
    }
}
