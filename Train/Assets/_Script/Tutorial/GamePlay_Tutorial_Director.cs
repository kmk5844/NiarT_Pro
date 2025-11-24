using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlay_Tutorial_Director : MonoBehaviour
{
    public Tutorial_UIDirector uiDirector;
    public Tutorial_Player player;
    public GameType_T gameType;
    public Tutorial_List tutorialList;
    bool T_Flag;
    public float clickTime;

    public GameObject ScarecrowObject_Sky;
    GameObject[] scarecrow_sky;

    int Scarecrow_count;
    
    bool scarecrow_DestoryFlag;
    bool T_SpawnWaveItem_Flag;

    Texture2D cursorOrigin;
    Texture2D cursorAim_UnAtk;
    Texture2D cursorAim_Atk;
    Vector2 cursorHotspot_Origin;
    Vector2 cursorHotspot_Aim;

    public GameObject SpawnItemObject;
    public GameObject SpawnWaveItemObject;
    public GameObject TrainObject;
    public int gold;

    public Tutorial_Train EngineTrain;

    public float speed;
    public float Max_Speed;

    public float Fuel;
    [HideInInspector]
    public float Max_Fuel;

    public int distance;
    public int max_distance;
    bool ClearFlag;

    public GameObject[] EmphasisObejct;
    public bool aimFlag;
    

    bool spawnWaveFlag;
    bool uiWaveFlag = false;
    /*{
    0. PlayerHP
    1. Itme
    2. Distance
    3. Speed
    4. Fuel
    5. Train_SpecialGuage
    6. 스킬 Q
    7. 스킬 E
    }*/

    [Header("Sound")]
    public AudioClip TutorialBGM;
    public AudioClip ClearSFX;
    public AudioClip SpawnSFX;

    bool STEAM_TUTORIAL_BACK_FLAG;

    public GameObject EngineWarning;
    public GameObject SpeedWarning;

    public bool Tutorial_4GetFlag;
    public bool Tutorial_5GetFlag;
    public bool Tutorial_AutoGetFlag;

    bool winFlag;
    private void Awake()
    {
        Fuel = 60000;
        Max_Fuel = Fuel;
        Max_Speed = 200;
        max_distance = 9;

        cursorAim_UnAtk = Resources.Load<Texture2D>("Cursor/Aim6464_UnAttack");
        cursorAim_Atk = Resources.Load<Texture2D>("Cursor/Aim6464_Attack");
        cursorOrigin = Resources.Load<Texture2D>("Cursor/Origin6464");
        cursorHotspot_Origin = Vector2.zero;
        cursorHotspot_Aim = new Vector2(cursorAim_UnAtk.width / 2, cursorAim_UnAtk.height / 2);
        aimFlag = false;
        Tutorial_4GetFlag = false;
        Tutorial_5GetFlag = false;
        Tutorial_AutoGetFlag = false;
    }

    private void Start()
    {
        if(QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        gold = 0;
        speed = 20;
        scarecrow_DestoryFlag = false;
        T_Flag = true;
        aimFlag = false;
        gameType = GameType_T.Tutorial;
        tutorialList = Tutorial_List.T_Ready;
        ChangeCursor(false);
        MMSoundManagerSoundPlayEvent.Trigger(TutorialBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true);
    }

    private void Update()
    {
        if(EngineTrain.CheckHP() < 30f)
        {
            if(!EngineWarning.activeSelf)
            {
                EngineWarning.SetActive(true);
            }
        }
        else
        {
            if (EngineWarning.activeSelf)
            {
                EngineWarning.SetActive(false);
            }
        }

        if (speed < 50f)
        {
            if (!SpeedWarning.activeSelf)
            {
                SpeedWarning.SetActive(true);
            }
        }
        else
        {
            if (SpeedWarning.activeSelf)
            {
                SpeedWarning.SetActive(false);
            }
        }

        if (player.PlayerHP < 500 && tutorialList != Tutorial_List.T_Lose)
        {
            player.PlayerHP = 500;
        }

        if(speed < -10f && !STEAM_TUTORIAL_BACK_FLAG)
        {
            if (SteamAchievement.instance != null)
            {
                SteamAchievement.instance.Achieve("TUTORIAL_BACK");
            }
            else
            {
                Debug.Log("TUTORIAL_BACK");
            }

            STEAM_TUTORIAL_BACK_FLAG = true;
        }

        //TEST
/*        if (Input.GetKeyDown("]"))
        {
            if (!DataManager.Instance.playerData.FirstFlag)
            {
                DataManager.Instance.playerData.SA_CheckFirstFlag();
            }
            LoadingManager.LoadScene("Station");
        }*/

        if (gameType == GameType_T.Pause)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (uiDirector.option_Flag)
                {
                    if (!aimFlag)
                    {
                        ChangeCursor(false);
                    }
                    else
                    {
                        ChangeCursor(true);
                    }
                    uiDirector.option_Close_Button();
                    gameType = GameType_T.Tutorial;
                    Time.timeScale = 1;
                }
            }
        }
        else if(gameType == GameType_T.Tutorial)
        {
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                gameType = GameType_T.Pause;
                uiDirector.option_Open_Button();
                ChangeCursor(false);
                Time.timeScale = 0;
            }

            if (tutorialList == Tutorial_List.T_Ready)
            {
                if (uiDirector.UI_Information_Click_Flag)
                {
                    uiDirector.Click_Text_object.SetActive(true);
                    if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                    {
                        uiDirector.UI_Information_Click_Flag = false;
                    }
                }

                if (speed < Max_Speed)
                {
                    speed += (Time.deltaTime * 20f);
                    Fuel -= (Time.deltaTime * 1000f);
                }

                if (!uiDirector.UI_Information_Click_Flag)
                {
                    uiDirector.Click_Text_object.SetActive(false);

                    if (!ClearFlag)
                    {
                        //distance++;//1
                        tutorialList = Tutorial_List.T_Control;
                        T_Flag = true;
                    }
                }
            }

            if (tutorialList == Tutorial_List.T_Control)
            {
                if (!uiDirector.HelpInformation[0].ReadEndFlag && !uiDirector.HelpInformation[0].gameObject.activeSelf)
                {
                    uiDirector.HelpInformation[0].gameObject.SetActive(true);
                }

                if (speed < Max_Speed)
                {
                    speed += (Time.deltaTime * 20f);
                    Fuel -= (Time.deltaTime * 1000f);
                }

                if (uiDirector.HelpInformation[0].ReadEndFlag)
                {
                    uiDirector.Click_Text_object.SetActive(false);
                    if (!ClearFlag)
                    {
                        distance++;//1
                        StartCoroutine(Clear(Tutorial_List.T_Skill));
                        ClearFlag = true;
                    }
                    //T_Flag = true;
                }
            }

            if(tutorialList == Tutorial_List.T_Skill)
            {
                if (T_Flag)
                {
                    aimFlag = true;
                    ChangeCursor(true);
                    player.T_MoveFlag = true;
                    player.T_JumpFlag = true;
                    player.T_FireFlag = true;
                    EmphasisObejct[3].SetActive(true);
                    EmphasisObejct[7].SetActive(true);
                    player.T_Skill_Q = true;
                    player.T_Skill_E = true;
                    uiDirector.skill_changeIcon(true);
                    uiDirector.skill_changeIcon(false);
                    T_Flag = false;
                }

                if(player.T_Skill_Q)
                {
                    if (!EmphasisObejct[6].activeSelf)
                    {
                        EmphasisObejct[6].SetActive(true);
                    }

                    if (player.T_Skill_Q_Click)
                    {
                        if (EmphasisObejct[6].activeSelf)
                        {
                            EmphasisObejct[6].SetActive(false);
                        }

                        if (speed < Max_Speed + 100)
                        {
                            speed += (Time.deltaTime * 20f);
                            Fuel -= (Time.deltaTime * 1000f);
                        }
                        else
                        {
                            player.T_Skill_Q_End = true;
                        }
                    }
                }

                if (player.T_Skill_Q_End && player.T_Skill_E_End)
                {
                    player.QSkillEffect.Stop();
                    player.ESkillEffect.Stop();
                    uiDirector.SkillLock[0].SetActive(true);
                    uiDirector.SkillLock[1].SetActive(true);
                    if (!ClearFlag)
                    {
                        distance++;//6
                        EmphasisObejct[3].SetActive(false);
                        EmphasisObejct[7].SetActive(false);
                        StartCoroutine(Clear(Tutorial_List.T_Spawn_Item));
                        ClearFlag = true;
                    }
                }
            }

            if(tutorialList == Tutorial_List.T_Spawn_Item)
            {
                if (T_Flag)
                {
                    Instantiate(SpawnItemObject);
                    T_Flag = false;
                }


                if (player.T_SpawnItem_Flag)
                {
                    uiDirector.item_changeIcon(0, 1 ,true);
                    if (!ClearFlag)
                    {
                        distance++;//8
                        StartCoroutine(Clear(Tutorial_List.T_Use_Item));
                        ClearFlag = true;
                    }
                }
            }

            if(tutorialList == Tutorial_List.T_Use_Item)
            {
                if (T_Flag)
                {
                    player.T_UseItem = true;
                    EmphasisObejct[1].SetActive(true);
                    T_Flag = false;
                }

                if (player.T_UseItem_Flag)
                {
                    uiDirector.item_changeIcon(0, 0, false);
                    if (!ClearFlag)
                    {
                        distance++;//9
                        EmphasisObejct[1].SetActive(false);
                        StartCoroutine(Clear(Tutorial_List.T_Train));
                        ClearFlag = true;
                    }
                }
            }

            if(tutorialList == Tutorial_List.T_Train)
            {
                if (T_Flag)
                {
                    TrainObject.SetActive(true); // 특수 게이지는 이미 활성화 되어있음
                    player.T_Train = true;
                    EmphasisObejct[4].SetActive(true);
                    T_Flag = false;
                }

                if (player.T_Train_Flag)
                {
                    scarecrow_sky = new GameObject[3];
                    if (!ClearFlag)
                    {
                        distance++;//10
                        EmphasisObejct[4].SetActive(false);
                        EmphasisObejct[5].SetActive(false);   
                        StartCoroutine(Clear(Tutorial_List.T_Monster));
                        ClearFlag = true;
                    }
                }
            }

            if(tutorialList == Tutorial_List.T_Monster)
            {
                if (T_Flag)
                {
                    MMSoundManagerSoundPlayEvent.Trigger(SpawnSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
                    for(int i = 0; i < 3; i++)
                    {
                        scarecrow_sky[i] = Instantiate(ScarecrowObject_Sky, new Vector2(0 - (6 * i), 16), Quaternion.identity);
                        Scarecrow_count++;
                    }
                    EmphasisObejct[3].SetActive(true);
                    T_Flag = false;
                }

                if(Scarecrow_count == 0 && !T_Flag)
                {
                    if (!ClearFlag)
                    {
                        distance++;//11
                        EmphasisObejct[3].SetActive(false);

                        StartCoroutine(Clear(Tutorial_List.T_Wave));
                        ClearFlag = true;
                    }
                }
            }

            if(tutorialList == Tutorial_List.T_Wave)
            {
                if (T_Flag)
                {
                    //UI 디렉터에서 Refresh 문구 뜨도록 함.
                    T_Flag = false;
                }

                if (!spawnWaveFlag)
                {
                    if (speed < 200)
                    {
                        speed += (Time.deltaTime * 50f);
                        Fuel -= (Time.deltaTime * 1000f);
                    }
                    else
                    {
                        Instantiate(SpawnWaveItemObject);
                        spawnWaveFlag = true;
                    }
                }
                else
                {
                    if (T_SpawnWaveItem_Flag)
                    {
                        if (!uiWaveFlag)
                        {
                            StartCoroutine(uiDirector.WaveFillObjectShow());
                        }

                        if(uiDirector.waveFlag)
                        {
                            uiDirector.WaveFillObject.SetActive(false);
                        }

                        if (!ClearFlag && uiDirector.waveFlag)
                        {
                            distance++;
                            StartCoroutine(Clear(Tutorial_List.T_Fuel));
                            ClearFlag = true;
                        }
                    }
                }
            }

            if (tutorialList == Tutorial_List.T_Fuel)
            {
                if (T_Flag)
                {
                    aimFlag = false;
                    ChangeCursor(false);
                    EmphasisObejct[4].SetActive(true);
                    T_Flag = false;
                }

                if (Fuel > 0)
                {
                    Fuel -= (Time.deltaTime * 9000f);
                }
                else
                {
                    if (!ClearFlag)
                    {
                        distance++;//12
                        EmphasisObejct[4].SetActive(false);
                        StartCoroutine(Clear(Tutorial_List.T_Lose));
                        ClearFlag = true;
                    }
                }
            }

            if(tutorialList == Tutorial_List.T_Lose)
            {
                if (T_Flag && !uiDirector.HelpInformation[1].ReadEndFlag && !uiDirector.HelpInformation[1].gameObject.activeSelf)
                {
                    player.T_MoveFlag = false;
                    player.T_JumpFlag = false;
                    player.T_FireFlag = false;
                    uiDirector.HelpInformation[1].gameObject.SetActive(true);
                    T_Flag = false;
                }

                if (uiDirector.HelpInformation[1].ReadEndFlag)
                {
                    if (!ClearFlag)
                    {
                        distance++;//1
                        StartCoroutine(Clear(Tutorial_List.T_Win));
                        ClearFlag = true;
                    }
                    T_Flag = true;
                }
            }

            if (tutorialList == Tutorial_List.T_Win)
            {
                if (T_Flag)
                {
                    player.T_MoveFlag = true;
                    player.T_JumpFlag = true;
                    player.T_FireFlag = true;
                    T_Flag = false;
                }

                if (speed > 20)
                {
                    speed -= (Time.deltaTime * 60f);
                }
                else
                {
                    if (!winFlag)
                    {
                        EmphasisObejct[2].SetActive(true);
                        StartCoroutine(uiDirector.WaitTime());
                        winFlag = true;
                    }

                    if (uiDirector.UI_Information_Click_Flag)
                    {
                        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
                        {
                            distance++;//14
                        }
                    }
                }

                if (distance > max_distance)
                {
                    if (!ClearFlag)
                    {
                        distance++;//14
                        StartCoroutine(Clear(Tutorial_List.T_End));
                        uiDirector.Click_Text_object.SetActive(false);
                        EmphasisObejct[2].SetActive(false);
                        ClearFlag = true;
                    }
                }
            }

            if(tutorialList == Tutorial_List.T_End)
            {
                if (T_Flag)
                {
                    if (!DataManager.Instance.playerData.FirstFlag)
                    {
                        DataManager.Instance.playerData.SA_CheckFirstFlag();
                    }
                    LoadingManager.LoadScene("Station");
                }
            }
        }
    }
    public void UseItem(ItemDataObject item)
    {
        if (item.Num == 78)
        {
            //금지된 아드레날린
            player.PlayerHP_Item(false);
            StartCoroutine(player.Item_78());
        }
        else if (item.Num == 2)
        {
            player.PlayerHP_Item(true);
        }
    }

    public void Get_Score(int _gold, bool flag)
    {
        gold += _gold;
        if (flag)
        {
            Scarecrow_count--;
        }
    }

    public IEnumerator Tutorial_Train_MaxSpeed_Change(int addSpeed, float during)
    {
        speed += addSpeed;
        yield return new WaitForSeconds(during);
        speed -= addSpeed;
    }

    public void Refresh()
    {
        player.PlayerHP = player.Max_PlayerHP;
        Fuel = Max_Fuel;

        T_SpawnWaveItem_Flag = true;
    }

    IEnumerator Clear(Tutorial_List list)
    {
        uiDirector.ClearObject.SetActive(true);
        uiDirector.Compelte_Object.SetActive(true);
        MMSoundManagerSoundPlayEvent.Trigger(ClearSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);

        yield return new WaitForSeconds(1f);
        if (distance > max_distance)
        {
            uiDirector.GameTutorial_Window.SetActive(false);
        }
        uiDirector.changeText(distance);
        //uiDirector.Title_Text.text = distance.ToString();
        yield return new WaitForSeconds(0.5f);
        uiDirector.Compelte_Object.SetActive(false);
        uiDirector.ClearObject.SetActive(false);

        tutorialList = list;
        T_Flag = true;
        ClearFlag = false;
    }

    public void ChangeCursor(bool flag, bool atkFlag = false)
    {
        if (flag) // 게임 진행 중일 때
        {
            if (!atkFlag)
            {
                Cursor.SetCursor(cursorAim_UnAtk, cursorHotspot_Aim, CursorMode.ForceSoftware);
            }
            else
            {
                Cursor.SetCursor(cursorAim_Atk, cursorHotspot_Aim, CursorMode.ForceSoftware);
            }
        }
        else // Pause했을 때
        {
            Cursor.SetCursor(cursorOrigin, cursorHotspot_Origin, CursorMode.Auto);
        }
    }
    public enum GameType_T
    { 
        Tutorial,
        Pause
    }

    public enum Tutorial_List
    {
        T_Ready,
        T_Control,
        T_Skill,
        T_Spawn_Item,
        T_Use_Item,
        T_Train,
        T_Monster,
        T_Wave,
        T_Fuel,
        T_Lose,
        T_Win,
        T_End,
    }
    //UI Option Close
    public void optionClose()
    {
        if (uiDirector.option_Flag)
        {
            if (!aimFlag)
            {
                ChangeCursor(false);
            }
            else
            {
                ChangeCursor(true);
            }
            uiDirector.option_Close_Button();
            gameType = GameType_T.Tutorial;
            Time.timeScale = 1;
        }
    }
}