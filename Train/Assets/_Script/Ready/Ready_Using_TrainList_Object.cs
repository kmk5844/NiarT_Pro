using MoreMountains.Tools;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Localization.Components;
using UnityEngine.UI;

public class Ready_Using_TrainList_Object : MonoBehaviour
{
    public PlayerReadyDirector director;
    [SerializeField]
    bool EmptyTrainFlag;

    [SerializeField]
    int Index;
    [SerializeField]
    int Sub_Index;
    [SerializeField]
    public int TrainNum_1;
    [SerializeField]
    int TrainNum_2;

    public Image TrainImage;
    public TextMeshProUGUI Level_Text;
    public LocalizeStringEvent Name_Text;
    public GameObject Add_Object;
    public GameObject Select_Arrow_Object;
    public Button Btn;
    public Button DeleteBtn;

    bool SelectFlag;
    public AudioClip equipSFX;

    private void Start()
    {
        Name_Text.StringReference.TableReference = "ExcelData_Table_St";
        if (!EmptyTrainFlag)
        {
            TrainImage.gameObject.SetActive(true);
            if(TrainNum_1 == 91 /*|| TrainNum_1 == 52*/)
            {
                TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1 + "_" + (TrainNum_2/10)*10);
                Level_Text.text = "Lv." + ((TrainNum_2 % 10) + 1);
                if(TrainNum_1 == 91)
                {
                    Name_Text.StringReference.TableEntryReference = "Train_Turret_Name_" + (TrainNum_2 / 10);
                }
/*                else if(TrainNum_1 == 52)
                {
                    Name_Text.StringReference.TableEntryReference = "Train_Booster_Name_" + (TrainNum_2 / 10);
                }*/
            }
            else
            {
                TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1);
                Level_Text.text = "Lv." + ((TrainNum_1 % 10) + 1);
                Name_Text.StringReference.TableEntryReference = "Train_Name_" + (TrainNum_1 / 10);
            }
            Add_Object.SetActive(false);
        }
        else
        {
            TrainImage.gameObject.SetActive(false);
            Add_Object.SetActive(true);
        }
        Btn.onClick.AddListener(() => director.Click_Change_Train(Index));
        DeleteBtn.onClick.AddListener(() => director.Click_Delete_Train(Index));
        Btn.interactable = false;
    }


    public void Setting(int _index, int _num1, int _num2, bool empty)
    {
        Index = _index;
        TrainNum_1 = _num1;
        TrainNum_2 = _num2;
        EmptyTrainFlag = empty;
    }

    public void SelectFlag_Change(bool flag)
    {
        if ((TrainNum_1 / 10) != 0  || TrainNum_1 == -1)
        {
            if (flag)
            {
                SelectFlag = true;
                Btn.interactable = true;
                Select_Arrow_Object.SetActive(true);
            }
            else
            {
                SelectFlag = false;
                Btn.interactable = false;
                Select_Arrow_Object.SetActive(false);
            }
        }
    }

    public void Change_Train(int num1, int num2)
    {
        if (EmptyTrainFlag)
        {
            EmptyTrainFlag = false;
            Add_Object.SetActive(false);
            TrainImage.gameObject.SetActive(true);
        }

        TrainNum_1 = num1;
        TrainNum_2 = num2;

        if (TrainNum_1 == 91 /*|| TrainNum_1 == 52*/)
        {
            TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1 + "_" + (TrainNum_2 / 10) * 10);
            Level_Text.text = "Lv." + ((TrainNum_2 % 10) + 1);
            if (TrainNum_1 == 91)
            {
                Name_Text.StringReference.TableEntryReference = "Train_Turret_Name_" + (TrainNum_2 / 10);
            }
            /*else if (TrainNum_1 == 52)
            {
                Name_Text.StringReference.TableEntryReference = "Train_Booster_Name_" + (TrainNum_2 / 10);
            }*/
        }else if (TrainNum_1 == -1)
        {
            EmptyTrainFlag = true;
            Add_Object.SetActive(true);
            TrainImage.gameObject.SetActive(false);
            Name_Text.StringReference.TableEntryReference = null;
            Name_Text.GetComponent<TextMeshProUGUI>().text = "";
        }
        else
        {
            TrainImage.sprite = Resources.Load<Sprite>("Sprite/Train/Train_" + TrainNum_1);
            Level_Text.text = "Lv." + ((TrainNum_1 % 10) + 1);
            if ((TrainNum_1<90))
            {
                Name_Text.StringReference.TableEntryReference = "Train_Name_" + (TrainNum_1 / 10);
            }
            else
            {
                Name_Text.StringReference.TableEntryReference = "Train_Name_" + TrainNum_1;
            }
        }
        MMSoundManagerSoundPlayEvent.Trigger(equipSFX, MMSoundManager.MMSoundManagerTracks.Sfx, this.transform.position);
    }
}
