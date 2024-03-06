using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SA_PlayerData", menuName = "Scriptable/PlayerData", order = 1)]

public class SA_PlayerData : ScriptableObject
{
    [Header("�÷��̾� �⺻ ����")]
    [SerializeField]
    private Sprite gun;
    public Sprite Gun {  get { return gun; } }
    [SerializeField]
    private GameObject bullet;
    public GameObject Bullet { get { return bullet; } }
    [SerializeField]
    private int atk;
    public int Atk { get { return atk; } }
    [SerializeField]
    private int armor;
    public int Armor { get { return armor; } }
    [SerializeField]
    private float delay;
    public float Delay { get { return delay; } }

    [SerializeField]
    private int hp;
    public int HP { get { return hp; } }
    [SerializeField]
    private float moveSpeed;
    public float MoveSpeed { get {  return moveSpeed; } }
    [Header("����")]
    [SerializeField]
    private int level_atk;
    public int Level_Player_Atk { get {  return level_atk; } }
    [SerializeField]
    private int level_atkdelay;
    public int Level_Player_AtkDelay { get { return level_atkdelay; } }
    [SerializeField]
    private int level_hp;
    public int Level_Player_HP { get { return level_hp; } }
    [SerializeField]
    private int level_armor;
    public int Level_Player_Armor { get { return level_armor; } }
    [SerializeField]
    private int level_speed;
    public int Level_Player_Speed { get { return level_speed; } }
    [Header("��ȭ")]
    [SerializeField]
    private int coin;
    public int Coin { get { return coin; } }
    [SerializeField]
    private int point;
    public int Point { get { return point; } }

    public void SA_Player_Level_Up(int LevelNum)//LevelNum : 0 = Atk / 1= AtkDealy / 2 = HP / 3 = Armor / 4 = Speed
    {
        switch(LevelNum)
        {
            case (0):
                level_atk++;
                break;
            case (1):
                level_atkdelay++;
                break;
            case (2):
                level_hp++;
                break;
            case (3):
                level_armor++;
                break;
            case (4):
                level_speed++;
                break;
        }
    }
}