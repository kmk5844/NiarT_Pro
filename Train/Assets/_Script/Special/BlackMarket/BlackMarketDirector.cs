using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BlackMarketDirector : MonoBehaviour
{
    [Header("Ω∫≈‰∏Æ")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject BlackMarketWindow;
    public GameObject CheckWindow;
    public GameObject SelectStage;

    [Header("Data")]
    public SA_PlayerData playerData;
    public SA_ItemList itemList;
    ItemDataObject[] item_Arr;
    [SerializeField]
    int[] itemCount_Arr;
    [SerializeField]
    int[] Persent_Arr;
    [SerializeField]
    int[] Pride_Arr;
    bool startFlag;

    [Header("UI")]
    public BlackMarketCard[] card;
    public TextMeshProUGUI playerGoldText;

    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        BlackMarketWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 0;
        }
        SettingCard();
        playerGoldText.text = playerData.Coin + " G";
    }

    // Update is called once per frame
    void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }
    }
    void StartEvent()
    {
        BlackMarketWindow.SetActive(true);
        startFlag = true;
    }

    public void BlackMarketEnd()
    {
        SelectStage.SetActive(true);
    }

    void SettingCard()
    {
        item_Arr = new ItemDataObject[3];
        itemCount_Arr = new int[3];
        Persent_Arr = new int[3];
        Pride_Arr = new int[3];
        for (int i = 0; i < 3; i++)
        {
            int num = Random.Range(0, 20);
            int count = Random.Range(1,6);
            int persent = Random.Range(5, 50);
            ItemDataObject _Item = itemList.Item[num];
            item_Arr[i] = _Item;
            itemCount_Arr[i]= count;
            Persent_Arr[i] = persent;
            Pride_Arr[i] = ((itemCount_Arr[i] * item_Arr[i].Item_Buy_Pride) * (100 - Persent_Arr[i]) / 100);
            card[i].SettingCard(i, this, _Item, count, persent);
        }
    }

    public bool CheckItem(int index)
    {
        int _pride = Pride_Arr[index];
        if (playerData.Coin > _pride)
        {
            return true;
        }
        else
        {
            return false;
        }
    }


    public void WarningPrideWindow()
    {
        CheckWindow.SetActive(true);
    }

    public void CloaseWarningPrideWindow()
    {
        CheckWindow.SetActive(false);
    }

    public void ClickItem(int index)
    {
        item_Arr[index].Item_Count_UP(itemCount_Arr[index]);
        playerData.SA_Buy_Coin(Pride_Arr[index]);
        playerGoldText.text = playerData.Coin + " G";
    }

}
