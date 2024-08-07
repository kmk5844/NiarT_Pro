using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Localization.Components;
public class StoreList_Tooltip : MonoBehaviour
{
    public Image ItemIcon;
    public LocalizeStringEvent Train_Name;
    public LocalizeStringEvent Train_Information;
    public TextMeshProUGUI Train_Pride;
    bool TooltipFlag;

    float halfwidth;
    float halfheight;
    float pivot_x;
    float pivot_y;
    RectTransform rt;

    private void Start()
    {
        halfwidth = GetComponentInParent<CanvasScaler>().referenceResolution.x * 0.5f;
        halfheight = GetComponentInParent<CanvasScaler>().referenceResolution.y * 0.016f;
        rt = GetComponent<RectTransform>();
        Train_Name.StringReference.TableReference = "ExcelData_Table_St";
        Train_Information.StringReference.TableReference = "ExcelData_Table_St";
        Tooltip_Off();
    }

    private void Update()
    {
        if (TooltipFlag)
        {
            transform.position = Input.mousePosition + new Vector3(40, -40, 0);
        }

        if (rt.anchoredPosition.x + rt.sizeDelta.x > halfwidth)
        {
            // ����
            pivot_x = 1;
        }
        else
        {
            // ������
            pivot_x = 0;
        }

        if (rt.anchoredPosition.y + rt.sizeDelta.y > halfheight)
        {
            // ��
            pivot_y = 1f;
        }
        else
        {
            // �Ʒ�
            pivot_y = -0.58f;
        }

        rt.pivot = new Vector2(pivot_x, pivot_y);
    }

    public void Tooltip_ON(bool TrainORMercenary, int Pride, int Store_Num, int Store_Num2 = -1)
    {
        TooltipFlag = true;
        if (TrainORMercenary) // �������
        {
            if(Store_Num == 51)
            {
                if(Store_Num2 == -1)
                {
                    Train_Name.StringReference.TableEntryReference = "Train_Name_51";
                    Train_Information.StringReference.TableEntryReference = "Train_Information_51";
                }
                else
                {
                    Train_Name.StringReference.TableEntryReference = "Train_Turret_Name_" + (Store_Num2/10);
                    Train_Information.StringReference.TableEntryReference = "Train_Turret_Information_" + (Store_Num2 / 10);
                }
            }else if(Store_Num == 52)
            {
                if(Store_Num2 == -1)
                {
                    Train_Name.StringReference.TableEntryReference = "Train_Name_52";
                    Train_Information.StringReference.TableEntryReference = "Train_Information_52";
                }
                else
                {
                    Train_Name.StringReference.TableEntryReference = "Train_Booster_Name_" + (Store_Num2 / 10);
                    Train_Information.StringReference.TableEntryReference = "Train_Booster_Information_" + (Store_Num2 / 10);
                }
            }
            else
            {
                Train_Name.StringReference.TableEntryReference = "Train_Name_" + (Store_Num/10);
                Train_Information.StringReference.TableEntryReference = "Train_Information_" + (Store_Num/10);
            }
        }
        else // �뺴�̶��
        {
            Train_Name.StringReference.TableEntryReference = "Mercenary_Name_" + Store_Num;
            Train_Information.StringReference.TableEntryReference = "Mercenary_Information_" + Store_Num;
        }
        //Train_Name.text = storeName;
        //Train_Information.text = storeInformation;
        Train_Pride.text = Pride.ToString() + "G";

        gameObject.SetActive(true);
    }

    public void Tooltip_Off()
    {
        TooltipFlag = false;
        Train_Name.StringReference.TableEntryReference = null;
        Train_Information.StringReference.TableEntryReference = null;
        Train_Name.GetComponent<TextMeshProUGUI>().text = "";
        Train_Information.GetComponent<TextMeshProUGUI>().text = "";

        gameObject.SetActive(false);
    }
}