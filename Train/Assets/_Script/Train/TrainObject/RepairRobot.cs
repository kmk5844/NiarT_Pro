using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairRobot : MonoBehaviour
{
    public GameObject TargetTrain;

    float moveSpeed = 3f;

    public float HealPersent;
    public int HealCount;
    bool moveflag = false;
    bool repairflag = false;

    bool repairCorutineFlag = false;

    private void Update()
    {
        if (!moveflag && !repairflag)
        {
            //�ش� ��ġ �̵�
            if(TargetTrain.transform.position.x > transform.position.x)
            {
                //������ �̵�
                transform.Translate(Vector3.right * Time.deltaTime * moveSpeed);
                if(TargetTrain.transform.position.x - transform.position.x < 0.1f)
                {
                    moveflag = true;
                }
            }
            else
            {
                //���� �̵�
                transform.Translate(Vector3.left * Time.deltaTime * moveSpeed);
                if (transform.position.x - TargetTrain.transform.position.x < 0.1f)
                {
                    moveflag = true;
                }
            }
        }
        else if(moveflag && !repairflag)
        {
            //�ش� ��ġ���� ����
            if (!repairCorutineFlag)
            {
                StartCoroutine(RepairCorutine());
                repairCorutineFlag = true;
            }

        }else if(moveflag && repairflag)
        {
            //���� �Ϸ� �� ����
            Destroy(gameObject);
        }
    }

    IEnumerator RepairCorutine()
    {
        Train_InGame train = TargetTrain.GetComponent<Train_InGame>();
        for (int i = 0; i < HealCount; i++)
        {
            train.Item_Train_Heal_HP(HealPersent);
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(0.5f);
        repairflag = true;
    }

    public void Set(GameObject train, float healpersent, int count)
    {
        TargetTrain = train;
        HealPersent = healpersent;
        HealCount = count;
    }
}
