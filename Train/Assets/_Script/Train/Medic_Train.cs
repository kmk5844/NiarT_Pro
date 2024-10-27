using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Medic_Train : MonoBehaviour
{
    Train_InGame medicTrain;
    public GameObject[] On_Off_Object;

    int Heal_Amount;
    float Heal_timeBet;
    bool isPlayerHealing;
    public bool isMercenaryHealing;
    bool isHeal;
    public bool isOpen;
    Mercenary col;
    // Start is called before the first frame update
    void Start()
    {
        medicTrain = GetComponentInParent<Train_InGame>();
        Heal_Amount = medicTrain.Train_Heal_Amount;
        Heal_timeBet = medicTrain.Train_Heal_timeBet;
        isMercenaryHealing = false; //�뺴 ����
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerHealing = medicTrain.isHealing;
        isOpen = medicTrain.Not_DestoryTrain;

        if (!isPlayerHealing && !isMercenaryHealing)
        {
            On_Off_Object[0].SetActive(false);
            On_Off_Object[1].SetActive(true);
        }
        else
        {
            On_Off_Object[0].SetActive(true);
            On_Off_Object[1].SetActive(false);
        }

        if (isMercenaryHealing)
        {
            if(col.Check_HpParsent() >= 60f || !medicTrain.Not_DestoryTrain) //�ı��ǰų� �뺴 60�� �̻� ġ�ᰡ �Ǹ� ������.
            {
                isMercenaryHealing = false;
                col.transform.gameObject.SetActive(true);
            }
            else
            {
                if (!isHeal)
                {
                    StartCoroutine(Train_Healing());
                }
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOpen)
        {
            if (collision.CompareTag("Mercenary") && !isPlayerHealing && !isMercenaryHealing) //�뺴�� �÷��̾ ġ���ϴ� ���°� �ƴ϶��
            {
                col = collision.GetComponentInParent<Mercenary>();
                if (col.Check_HpParsent() <= 20f)
                {
                    isMercenaryHealing = true;
                    col.transform.gameObject.SetActive(false);
                }
            }
        }
    }

    IEnumerator Train_Healing()
    {
        isHeal = true;
        if(medicTrain.Train_Heal - Heal_Amount < 0)
        {
            medicTrain.Train_Heal = 0;
        }
        else{
            medicTrain.Train_Heal -= Heal_Amount;
        }
        col.HP += Heal_Amount;
        yield return new WaitForSeconds(Heal_timeBet);
        isHeal = false;
    }
}