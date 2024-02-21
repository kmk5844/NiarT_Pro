using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Engine_Driver : Mercenary
{
    public Engine_Driver_Type Dirver_Type;
    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        transform.position = new Vector3(-2, -1, 0);
        GameDirector Gd = GameObject.Find("GameDirector").GetComponent<GameDirector>();

        switch (Dirver_Type){
            case Engine_Driver_Type.speed:
                Gd.Engine_Driver_Passive(Engine_Driver_Type.speed, 10);
                break;
            case Engine_Driver_Type.fuel:
                Gd.Engine_Driver_Passive(Engine_Driver_Type.fuel, 4);
                break;
            case Engine_Driver_Type.def:
                for(int i = 0; i < Train_List.childCount; i++)
                {
                    Train_List.GetChild(i).GetComponent<Train>().Train_Anmor += 10;
                }
                break;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (HP <= 0 && act != Active.die)
        {
            act = Active.die;
            isDying = true;
        }

        if (act == Active.work)
        {
            //���ǰ� ��ų�� ������� ����ϸ� ������
            Debug.Log("��ų�� ���׹̳� ���");
        }
        else if (act == Active.die && isDying)
        {

            Debug.Log("���⼭ �ִϸ��̼� �����Ѵ�!5");
            Debug.Log("������ ���ÿ� �ٷ� �нú� �ٿ�");
            isDying = false;
        }
    }
}

public enum Engine_Driver_Type
{
    speed,
    fuel,
    def
}