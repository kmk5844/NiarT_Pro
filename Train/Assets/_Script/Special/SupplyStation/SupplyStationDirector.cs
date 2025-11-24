using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;


public class SupplyStationDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject SupplyStationWindow;
    public GameObject SupplyMiniGameWindow;
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public SA_Event eventData;
    bool spawnFlag;
    bool timeflag;
    bool minigameflag;
    int count = 0;
    int rewardCount;
    int rewardGrade;
    bool startFlag;

    [Header("UI")]
    public TextMeshProUGUI countText;
    public LocalizeStringEvent rewardText;
    public LocalizeStringEvent boxResultText;

    [Header("Game")]
    public Vector2 Min_Vec;
    public Vector2 Max_Vec;
    float SupplySpeed;
    public SupplyObject[] Supply;
    [HideInInspector]
    public SupplyObject Spawingsupply;

    [Header("Tutorial")]
    public GameObject Tutorial_Object;

    private void Awake()
    {
        Special_Story.Story_Init(null, 17, 0, 0);
        SupplyStationWindow.SetActive(false);
    }

    void Start()
    {
        if (QualitySettings.vSyncCount != 0)
        {
            //Debug.Log("작동");
            QualitySettings.vSyncCount = 0;
            Application.targetFrameRate = 60;
        }

        minigameflag = true;
        timeflag = false;
        spawnFlag = true;
        SupplySpeed = 3;
        countText.text = "0";
        rewardText.StringReference.TableReference = "SpecialStage_St";
        rewardText.StringReference.TableEntryReference = "SupplyStation_SucessReward";
        boxResultText.StringReference.TableReference = "SpecialStage_St";
        boxResultText.StringReference.TableEntryReference = "SupplyStation_SucessCount";
        SupplyMiniGameWindow.SetActive(false);
        CheckWindow.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (minigameflag && startFlag)
        {
            if (!timeflag)
            {
                if (spawnFlag)
                {
                    int RandomNum = Random.Range(0, Supply.Length);
                    Supply[RandomNum].Setting(Min_Vec, Max_Vec, SupplySpeed, this);
                    int rotaiton = Random.Range(0, 2);
                    if(rotaiton == 0)
                    {
                        Spawingsupply = Instantiate(Supply[RandomNum], Min_Vec, Quaternion.identity);
                    }
                    else if (rotaiton == 1)
                    {
                        Spawingsupply = Instantiate(Supply[RandomNum], Max_Vec, Quaternion.identity);
                    }
                    spawnFlag = false;
                }

                if (Input.GetKeyDown(KeyCode.Space))
                {
                    Spawingsupply.ClickSpace();
                    StartCoroutine(SpawnTime());
                }
            }
        }

        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }
    void StartEvent()
    {
        Tutorial_Object.SetActive(true);
    }

   public void TutorialEnd()
    {
        Tutorial_Object.SetActive(false);
        SupplyStationWindow.SetActive(true);
        SupplyMiniGameWindow.SetActive(true);
        startFlag = true;
    }

    public void SupplyStationEnd()
    {
        SelectStage.SetActive(true);
    }
    IEnumerator SpawnTime()
    {
        timeflag = true;
        yield return new WaitForSeconds(2.4f);
        countText.text = count.ToString();
        timeflag = false;
        spawnFlag = true;
    }

    public void Count()
    {
        count++;
        SupplySpeed = 3 + ((float)count * 0.6f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Max_Vec, Min_Vec);
    }

    public void Reward()
    {
        int _count = count - 1;
        if (_count < 6)
        {
            rewardGrade = 1;
            rewardCount = 0;
        }
        else if(_count >= 6 && _count < 9)
        {
            rewardGrade = 1;
            rewardCount = 1;
        }
        else if(_count >= 9 && _count < 12)
        {
            rewardGrade = 1;
            rewardCount = 2;
        }
        else if(_count >= 12 && _count < 16)
        {
            rewardGrade = 1;
            rewardCount = 3;
        }
        else if(_count >= 16 && _count < 20)
        {
            rewardGrade = 2;
            rewardCount = 2;
        }
        else if(_count >= 20)
        {
            rewardGrade = 2;
            rewardCount = 3;
        }
        eventData.SupplyStationOn(rewardCount, rewardGrade);
    }

    public void GameEnd()
    {
        SupplyMiniGameWindow.SetActive(false);
        Reward();
        boxResultText.StringReference.Arguments = new object[] { count - 1 };
        rewardText.StringReference.Arguments = new object[] { rewardCount, rewardGrade };
        boxResultText.RefreshString();
        rewardText.RefreshString();
        CheckWindow.SetActive(true);
        minigameflag = false;
    }
}
