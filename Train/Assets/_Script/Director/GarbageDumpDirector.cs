using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GarbageDumpDirector : MonoBehaviour
{
    [Header("스토리")]
    public DialogSystem Special_Story;
    public Dialog dialog;

    [Header("Window")]
    public GameObject GarbageDumpWindow;
    public GameObject SelectStage;

    [Header("데이터")]
    public SA_ItemList sa_ItemList_;
    List<ItemDataObject> garbageList;
    int randomCount;

    [Header("UI")]
    public Transform garbageTransform;
    public GameObject garbageObject;
    public GameObject garbageWindow;
    public GameObject NextButton;

    bool startFlag;
    bool gabageFlag;
    private void Awake()
    {
        Special_Story.Story_Init(null, 0, 0, 0);
        GarbageDumpWindow.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        if (QualitySettings.vSyncCount != 1)
        {
            QualitySettings.vSyncCount = 1;
        }

        garbageList = new List<ItemDataObject>();
        for(int i = 65; i < 70; i++)
        {
            garbageList.Add(sa_ItemList_.Item[i]);
        }

        randomCount = Random.Range(0, 10);
        NextButton.GetComponent<Button>().onClick.AddListener(() => Click_NextButton());
        NextButton.SetActive(false);
    }

    private void Update()
    {
        if (dialog.storyEnd_SpecialFlag && !startFlag)
        {
            StartEvent();
        }

        if (Input.GetMouseButtonDown(0) && gabageFlag)
        {
            OpenGarbage();
        }
    }

    private void StartEvent()
    {
        GarbageDumpWindow.SetActive(true);
        gabageFlag = true;
        startFlag = true;
    }

    void OpenGarbage()
    {
        garbageWindow.SetActive(true);
        gabageFlag = false;
        StartCoroutine(Open());
    }

    IEnumerator Open()
    {
        for(int i = 0; i < randomCount; i++)
        {
            ItemDataObject item;
            int TF = Random.Range(0, 101);
            if(TF > 90)
            {
                int rand = Random.Range(0, 20);
                item = sa_ItemList_.Item[rand];
            }
            else
            {
                int rand = Random.Range(0, garbageList.Count);
                item = garbageList[rand];
            }
            garbageObject.GetComponent<GarbageCard>().Setting_item(item);
            Instantiate(garbageObject, garbageTransform);
            yield return new WaitForSeconds(0.5f);
        }
        NextButton.SetActive(true);
    }
    public void Click_NextButton()
    {
        /*MMSoundManagerSoundControlEvent.Trigger(MMSoundManagerSoundControlEventTypes.Stop, 10);
        MMSoundManagerSoundPlayEvent.Trigger(MissionSelectBGM, MMSoundManager.MMSoundManagerTracks.Music, this.transform.position, loop: true, ID: 20);*/
        SelectStage.SetActive(true);
    }
}
