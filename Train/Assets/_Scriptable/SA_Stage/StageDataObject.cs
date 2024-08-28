using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageDataObject : ScriptableObject
{
    [SerializeField]
    private int stage_num;
    public int Stage_Num { get { return stage_num; } }

    [SerializeField]
    private int destination_distance;
    public int Destination_Distance { get {  return destination_distance; } }
    [SerializeField]
    private int monster_count;
    public int Monster_Count {  get { return monster_count; } }

    [SerializeField]
    private int reward_point;
    public int Reward_Point {  get { return reward_point; } }

    [SerializeField]
    private string reward_item;
    public string Reward_Item { get {  return reward_item; } }

    [SerializeField]
    private string reward_itemcount;
    public string Reward_Itemcount { get { return reward_itemcount; } }

    public struct _Grade_Score
    {
        public int d_grade;
        public int c_grade;
        public int b_grade;
        public int a_grade;
        public int s_grade;
    }
    [SerializeField]
    _Grade_Score grade_score;
    public _Grade_Score Grade_Score { get { return grade_score; } } 

    [SerializeField]
    bool boss_flag;
    public bool Boss_Flag { get { return boss_flag; } }

    [SerializeField]
    string emerging_boss;
    public string Emerging_boss { get {  return emerging_boss; } }

    [SerializeField]
    string boss_monster_count;
    public string Boss_Monster_Count { get { return boss_monster_count; } }

    [SerializeField]
    string boss_distance;
    public string Boss_Distance { get {  return boss_distance; } }

    [SerializeField]
    int player_score;
    public int Player_Score { get { return player_score; } }

    [SerializeField]
    Grade player_grade;

    public Grade Player_Grade { get { return player_grade; } }

    [SerializeField]
    bool player_firstplay;
    public bool Player_FirstPlay { get { return player_firstplay; } }

    [SerializeField]
    bool stage_openflag;
    public bool Stage_OpenFlag {  get {  return stage_openflag; } }
    
    public enum Grade
    {
        F, D, C, B, A, S
    }

    public void Auto_Stage_Insert(
        int _stage_num, int _destination_distance, int _monster_count, 
        int _reward_point, string _reward_item, string _reward_itemcount,
        int _d, int _c, int _b, int _a, int _s, bool _boss_flag, string _emerging_boss,
        string _boss_monster_count, string _boss_distance
        )
    {
        stage_num = _stage_num;
        destination_distance = _destination_distance;
        monster_count = _monster_count;
        reward_point = _destination_distance;

        reward_item = _reward_item;
        reward_itemcount = _reward_itemcount;

        grade_score.d_grade = _d;
        grade_score.c_grade = _c;
        grade_score.b_grade = _b;
        grade_score.a_grade = _a;
        grade_score.s_grade = _s;

        boss_flag = _boss_flag;
        emerging_boss = _emerging_boss;
        boss_monster_count = _boss_monster_count;
        boss_distance = _boss_distance;
        player_score = 0;
        player_grade = Grade.F;
        player_firstplay = false;
        if(_stage_num == 0)
        {
            stage_openflag = true;
        }
        else
        {
            stage_openflag = false;
        }

    }
}
