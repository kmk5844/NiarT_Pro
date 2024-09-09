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
    private string emerging_monster;
    public string Emerging_Monster { get {  return emerging_monster; } }

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

    [SerializeField]
    List<int> grade_score;
    public List<int> Grade_Score { get { return grade_score; } } 

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
        int _stage_num, int _destination_distance, string _emerging_monster,int _monster_count, 
        int _reward_point, string _reward_item, string _reward_itemcount,
        int _d, int _c, int _b, int _a, int _s, bool _boss_flag, string _emerging_boss,
        string _boss_monster_count, string _boss_distance
        )
    {
        stage_num = _stage_num;
        emerging_monster = _emerging_monster;
        destination_distance = _destination_distance;
        monster_count = _monster_count;
        reward_point = _destination_distance;

        reward_item = _reward_item;
        reward_itemcount = _reward_itemcount;

        grade_score = new List<int>();
        grade_score.Add(_d);
        grade_score.Add(_c);
        grade_score.Add(_b);
        grade_score.Add(_a);
        grade_score.Add(_s);

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

    public void New_Stage_Chage()
    {
        if (!stage_openflag)
        {
            stage_openflag = true;
        }
        Save();
    }


    public void GameEnd(bool WinAndLoseFlag, int Score, string grade = "F")
    {
        if (!player_firstplay)
        {
            player_firstplay = true;
        }
        
        if(player_score < Score)
        {
            player_score = Score;
        }

        int beforeNum = -1;
        int gradeNum = -1;

        switch (player_grade)
        {
            case Grade.S:
                beforeNum = 4;
                break;
            case Grade.A:
                beforeNum = 3;
                break;
            case Grade.B:
                beforeNum = 2;
                break;
            case Grade.C:
                beforeNum = 1;
                break;
            case Grade.D:
                beforeNum = 0;
                break;
            case Grade.F:
                beforeNum = -1;
                break;
        }

        if (WinAndLoseFlag)
        {
            switch (grade)
            {
                case "S":
                    gradeNum = 4;
                    break;
                case "A":
                    gradeNum = 3;
                    break;
                case "B":
                    gradeNum = 2;
                    break;
                case "C":
                    gradeNum = 1;
                    break;
                case "D":
                    gradeNum = 0;
                    break;
                case "F":
                    gradeNum = -1;
                    break;
            }

            if(gradeNum > beforeNum)
            {
                if (gradeNum == 4)
                {
                    player_grade = Grade.S;
                }
                else if (gradeNum == 3)
                {
                    player_grade = Grade.A;
                }
                else if (gradeNum == 2)
                {
                    player_grade = Grade.B;
                }
                else if(gradeNum == 1)
                {
                    player_grade = Grade.C;
                }else if(gradeNum == 0)
                {
                    player_grade = Grade.D;
                }else if(gradeNum == -1)
                {
                    player_grade = Grade.F;
                }
            }
        }
        else
        {
            if(beforeNum == -1)
            {
                player_grade = Grade.F;
            }
        }

        Save();
    }

    public void Save()
    {
        ES3.Save(name + "_stage_openflag", stage_openflag);
        ES3.Save(name + "_player_firstplay", player_firstplay);
        ES3.Save(name + "_player_score", player_score);
        ES3.Save(name + "_player_grade", player_grade);
    }

    public void Load()
    {
        player_firstplay = ES3.Load(name + "_stage_openflag", player_firstplay);
        player_score = ES3.Load(name + "_stage_openflag", player_score);
        player_grade = ES3.Load(name + "_stage_openflag", player_grade);
        stage_openflag = ES3.Load(name + "_stage_openflag", stage_openflag);
    }

    public void Init()
    {
        player_firstplay = false;
        player_score = 0;
        player_grade = Grade.F;
        if (stage_num == 0)
        {
            stage_openflag = true;
        }
        else
        {
            stage_openflag = false;
        }
        Save();
    }
}
