using TMPro;
using UnityEngine;

public class Hangar_Train : MonoBehaviour
{
    Train_InGame trainData;
    GameDirector gameDirector;
    SA_PlayerData playerdata;
    GameObject itemdirector_object;
    UseItem useitemScript;
    SA_ItemList itemList;

    public float SpawnTime;
    public int[] coin = new int[3];

    public float elapsed;

    public bool useFlag;
    public bool doorFlag;
    int doorNum = -1;

    public Collider2D[] DoorCollider;
    public GameObject[] DoorPrideObject;
    bool changeFlag = false;

    public GameObject SpawnItem;
    public Transform Spawn_Position_Item;

    private void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        gameDirector = trainData.gameDirector.GetComponent<GameDirector>();
        playerdata = gameDirector.player.playerSet();
        itemdirector_object = GameObject.Find("ItemDirector");
        useitemScript = itemdirector_object.GetComponent<UseItem>();
        itemList = useitemScript.itemList;

        SpawnTime = float.Parse(trainData.trainData_Special_String[0]);
        coin[0] = int.Parse(trainData.trainData_Special_String[1]);
        coin[1] = int.Parse(trainData.trainData_Special_String[2]);
        coin[2] = int.Parse(trainData.trainData_Special_String[3]);
        foreach (Collider2D door in DoorCollider)
        {
            door.GetComponent<Hangar_Door>().Set(this);
        }
        for (int i = 0; i < 3; i++)
        {
            {
                DoorPrideObject[i].SetActive(false);
                DoorPrideObject[i].GetComponentInChildren<TextMeshProUGUI>().text = "-" + coin[i] + "G";
            }
        }
    }
    void Update()
    {
        if (gameDirector.gameType == GameType.Playing || gameDirector.gameType == GameType.Boss)
        {
            elapsed += Time.deltaTime;

            // 강제로 초 단위로 변환
            int elapsedSeconds = Mathf.FloorToInt(elapsed);
            int spawnSeconds = Mathf.FloorToInt(SpawnTime);

            // 시간이 충분하면 useflag 허용 (한 번만)
            if (!useFlag && elapsedSeconds >= spawnSeconds)
            {
                useFlag = true;
                if (changeFlag == false)
                {
                    for (int i = 0; i < DoorCollider.Length; i++)
                    {
                        DoorCollider[i].enabled = true;
                    }

                    changeFlag = true;
                }
            }
        }
    }

    public void ClickWeapon()
    {
        ChoiceWeapon();
        useFlag = false;
        changeFlag = false;
        for (int i = 0; i < DoorCollider.Length; i++)
        {
            DoorCollider[i].enabled = false;
        }
        ExitDoor();
        elapsed = 0;
    }

    void ChoiceWeapon()
    {
        int num = 0;
        switch (doorNum)
        {
            case 0:
                num = Random.Range(52, 60);
                playerdata.SA_Buy_Coin_InGame(coin[0]);
                break;
            case 1:
                num = Random.Range(102, 109);
                playerdata.SA_Buy_Coin_InGame(coin[1]);
                break;
            case 2:
                num = Random.Range(109, 114);
                playerdata.SA_Buy_Coin_InGame(coin[2]);
                break;
        }
        GameObject spawnitem = Instantiate(SpawnItem, Spawn_Position_Item.position, Quaternion.identity);
        spawnitem.GetComponent<HangarSpawn_Item>().SetItem(itemList.Item[num], useitemScript);
    }

    public void EnterDoor(int num)
    {
        if(playerdata.Coin > coin[num])
        {
            DoorPrideObject[num].SetActive(true);
            doorFlag = true;
        }
        else
        {
            doorFlag = false;
        }
        doorNum = num;
    }

    public void ExitDoor()
    {
        DoorPrideObject[doorNum].SetActive(false);
        doorFlag = false;
        doorNum = -1;
    }
}
