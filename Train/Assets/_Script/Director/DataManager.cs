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
    public SA_ItemList InventoryItem_Data;
    public SA_ItemData PlayerItem_Data;
    // Start is called before the first frame update
    void Start()
    {

    }

    public void Load()
    {
        playerData.Load();
        trainData.Load();
        turretData.Load();
        boosterData.Load();
        mercenaryData.Load();
        localData.Load();
        PlayerItem_Data.Load();
    }

    public void Init()
    {
        playerData.Init();
        trainData.Init();
        turretData.Init();
        boosterData.Init();
        mercenaryData.Init();
        localData.Init();
        InventoryItem_Data.PlayGame_ItemList_Init();
        PlayerItem_Data.Init();
    }
}
