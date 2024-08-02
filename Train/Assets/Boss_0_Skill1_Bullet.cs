using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0_Skill1_Bullet : MonoBehaviour
{

    void Start()
    {
        transform.position = new Vector2(transform.position.x, 3.9f);
    }

    public void BulletAniEnd()
    {
        Destroy(gameObject);
    }
}
