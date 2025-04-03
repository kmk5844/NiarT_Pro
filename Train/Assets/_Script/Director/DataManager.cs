using ES3Types;
using System.Collections;
using System.Collections.Generic;
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
    public SA_TrainBoosterData boosterData;
    public SA_MercenaryData mercenaryData;
    public SA_LocalData localData;
    public SA_StageList stageData;
    public SA_ItemList InventoryItem_Data;
    public SA_ItemData PlayerItem_Data;
    public SA_Tutorial tutorialData;
    public SA_StoryLIst storyData;
    public SA_MissionData missionData;

    public void Load()
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
    }

    public IEnumerator LoadSync()
    {
        yield return StartCoroutine(playerData.LoadSync());
        yield return StartCoroutine(trainData.LoadSync());
        yield return StartCoroutine(turretData.LoadSync());
        yield return StartCoroutine(boosterData.LoadSync());
        yield return StartCoroutine(mercenaryData.LoadSync());
        yield return StartCoroutine(stageData.PlayGame_StageList_LoadSync(this));
        yield return StartCoroutine(storyData.PlayGame_StoryList_LoadSync(this));
        localData.Load();
        yield return new WaitForSeconds(0.001f);
        yield return StartCoroutine(PlayerItem_Data.LoadSync());
        yield return StartCoroutine(missionData.LoadSync(this));
    }

    public void Init()
    {
        playerData.Init();
        trainData.Init();
        turretData.Init();
        boosterData.Init();
        mercenaryData.Init();
        //localData.Init();
        stageData.PlayGame_StageList_Init();
        storyData.PlayGame_StoryList_Init();
        InventoryItem_Data.PlayGame_ItemList_Init();
        PlayerItem_Data.Init();
        tutorialData.Init();
        missionData.Init();
    }

    public IEnumerator InitAsync()
    {
        yield return StartCoroutine(playerData.InitAsync(this));
        yield return StartCoroutine(trainData.InitAsync(this));
        yield return StartCoroutine(turretData.InitAsync(this));
        yield return StartCoroutine(boosterData.InitAsync(this));
        yield return StartCoroutine(mercenaryData.InitAsync(this));
        // yield return StartCoroutine(localData.InitAsync());
        yield return StartCoroutine(stageData.PlayGame_StageList_InitAsync(this));
        yield return StartCoroutine(storyData.PlayGame_StoryList_InitAsync(this));
        yield return StartCoroutine(InventoryItem_Data.PlayGame_ItemList_InitAsync(this));
        yield return StartCoroutine(PlayerItem_Data.InitAsync(this));
        yield return StartCoroutine(tutorialData.InitAsync());
        yield return StartCoroutine(missionData.InitAsync(this));
    }
}
