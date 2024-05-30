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
    public SA_MercenaryData mercenaryData;

    // Start is called before the first frame update
    void Start()
    {

    }

    public void Load()
    {
        playerData.Load();
        trainData.Load();
        mercenaryData.Load();
    }

    public void Init()
    {
        playerData.Init();
        trainData.Init();
        mercenaryData.Init();
    }
}
