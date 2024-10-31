using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Monster_ShortAtk : MonoBehaviour
{
    public int Atk;
    public float Force;
    public float xPos;

    Monster monster;

    // Start is called before the first frame update
    void Start()
    {
        monster = GetComponentInParent<Monster>();
        Atk = monster.getMonsterAtk();
        Force = monster.getMonsterBulletSpeed();
    }

    private void Update()
    {
        xPos = monster.monster_xPos;
    }
}
