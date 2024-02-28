using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public class Medic_Train : MonoBehaviour
{
    Train medicTrain;
    int Heal_Amount;
    int Heal_timeBet;
    bool isPlayerHealing;
    public bool isMercenaryHealing;
    bool isHeal;
    public bool isOpen;
    Mercenary col;
    // Start is called before the first frame update
    void Start()
    {
        medicTrain = GetComponentInParent<Train>();
        Heal_Amount = medicTrain.Heal_Amount;
        Heal_timeBet = medicTrain.Heal_timeBet;
        isMercenaryHealing = false; //용병 전용
    }

    // Update is called once per frame
    void Update()
    {
        isPlayerHealing = medicTrain.isHealing;
        isOpen = medicTrain.openMedicTrian;
        if (isMercenaryHealing)
        {
            if(col.check_HpParsent() >= 60f || !medicTrain.openMedicTrian) //파괴되거나 용병 60퍼 이상 치료가 되면 내보냄.
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
            if (collision.CompareTag("Mercenary") && !isPlayerHealing && !isMercenaryHealing) //용병과 플레이어가 치료하는 상태가 아니라면
            {
                col = collision.GetComponentInParent<Mercenary>();
                if (col.check_HpParsent() <= 20f)
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