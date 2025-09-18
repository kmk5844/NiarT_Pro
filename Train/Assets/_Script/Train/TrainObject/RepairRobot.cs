using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairRobot : MonoBehaviour
{
    public GameObject TargetTrain;
    Animator anic;


    float moveSpeed = 3f;

    public float HealPersent;
    public int HealCount;
    bool moveflag = false;
    bool repairflag = false;

    bool repairCorutineFlag = false;

    public ParticleSystem RepairEffect;
    public ParticleSystem BoomEffect;

    private void Start()
    {
        anic = GetComponent<Animator>();
    }

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
            anic.SetTrigger("Repair");
            yield return new WaitForSeconds(0.14f);
            train.Item_Train_Heal_HP(HealPersent);
            yield return new WaitForSeconds(1f);
        }
        BoomEffect.Play();
        yield return new WaitForSeconds(0.14f);
        repairflag = true;
    }

    public void Set(GameObject train, float healpersent, int count)
    {
        TargetTrain = train;
        HealPersent = healpersent;
        HealCount = count;
    }

    public void PlayRepair()
    {
        RepairEffect.Play();
    }
}
