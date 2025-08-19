using ES3Types;
using System.Collections;
using UnityEngine;

public class DataManager : MonoBehaviour
{
    #region ΩÃ±€≈Ê
    private static DataManager instance = null;
    void Awake()
    {
        if (null == instance)
        {
            instance = this;
            DontDestroyOnLoad(this.gameObject);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }
    public static DataManager Instance
    {
        get
        {
            if (null == instance)
            {
                return null;
            }
            return instance;
        }
    }
    #endregion

    public SA_PlayerData playerData;
    public SA_TrainData trainData;
    public SA_TrainTurretData turretData;
    public SA_MercenaryData mercenaryData;
    public SA_LocalData localData;
    public SA_StageList stageData;
    public SA_ItemList InventoryItem_Data;
    public SA_ItemData PlayerItem_Data;
    public SA_Tutorial tutorialData;
    public SA_StoryLIst storyData;
    public SA_MissionData missionData;
    public SA_Event eventData;
    public SA_Monster monsterData;

    //public GameObject DebugObject;

/*    public void Load()
    {
        playerData.Load();
        trainData.Load();
        turretData.Load();
        boosterData.Load();
        mercenaryData.Load();
        stageData.PlayGame_StageList_Load();
        storyData.PlayGame_StoryList_Load();
        localData.Load();
        PlayerItem_Data.Load();
        missionData.Load();
    }*/

    public IEnumerator LoadSync()
    {
        Debug.Log("Load");
        Debug.Log(1);
        yield return StartCoroutine(playerData.LoadSync());
        Debug.Log(2);
        yield return StartCoroutine(trainData.LoadSync());
        Debug.Log(3);
        yield return StartCoroutine(turretData.LoadSync());
        Debug.Log(4);
        yield return StartCoroutine(mercenaryData.LoadSync());
        Debug.Log(5);
        yield return StartCoroutine(stageData.PlayGame_StageList_LoadSync(this));
        Debug.Log(6);
        yield return StartCoroutine(storyData.PlayGame_StoryList_LoadSync(this));
        Debug.Log(7);
        localData.Load();
        Debug.Log(8);
        yield return new WaitForSeconds(0.001f);
        Debug.Log(9);
        yield return StartCoroutine(PlayerItem_Data.LoadSync());
        Debug.Log(10);
        yield return StartCoroutine(missionData.LoadSync(this));
        Debug.Log(11);
        yield return StartCoroutine(eventData.LoadSync());
        Debug.Log(12);
        yield return StartCoroutine(monsterData.LoadSync());
        Debug.Log(13);
        yield return StartCoroutine(InventoryItem_Data.LoadDicSync());
        //Destroy(DebugObject);
    }

    public void Init()
    {
        playerData.Init();
        trainData.Init();
        turretData.Init();
        mercenaryData.Init();
        //localData.Init();
        stageData.PlayGame_StageList_Init();
        storyData.PlayGame_StoryList_Init();
        InventoryItem_Data.PlayGame_ItemList_Init();
        PlayerItem_Data.Init();
        tutorialData.Init();
        missionData.Init();
        eventData.Init();
    }

    public IEnumerator InitAsync()
    {
        Debug.Log("Init");
        Debug.Log(1);
        yield return StartCoroutine(playerData.InitAsync(this));
        Debug.Log(2);
        yield return StartCoroutine(trainData.InitAsync(this));
        Debug.Log(3);
        yield return StartCoroutine(turretData.InitAsync(this));
        Debug.Log(4);
        yield return StartCoroutine(mercenaryData.InitAsync(this));
        Debug.Log(5);
        // yield return StartCoroutine(localData.InitAsync());
        yield return StartCoroutine(stageData.PlayGame_StageList_InitAsync(this));
        Debug.Log(6);
        yield return StartCoroutine(storyData.PlayGame_StoryList_InitAsync(this));
        Debug.Log(7);
        yield return StartCoroutine(InventoryItem_Data.PlayGame_ItemList_InitAsync(this));
        Debug.Log(8);
        yield return StartCoroutine(PlayerItem_Data.InitAsync(this));
        Debug.Log(9);
        yield return StartCoroutine(tutorialData.InitAsync());
        Debug.Log(10);
        yield return StartCoroutine(missionData.InitAsync(this));
        Debug.Log(11);
        yield return StartCoroutine(eventData.InitAsync());
        Debug.Log(12);
        yield return StartCoroutine(monsterData.InitAsync());
        Debug.Log(13);
        yield return StartCoroutine(InventoryItem_Data.InitDicSync());
    }
}
