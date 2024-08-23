using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    protected Train_InGame trainData;
    protected Transform Bullet_List;
    protected float train_Attack_Delay;
    protected float train_Rotation_Delay;
    protected float lastTime;

    //item ºÎºÐ
    protected float Item_Attack_Delay;
    protected bool rotation_TurretFlag;
    protected float Item_Rotation_Delay; 

    // Start is called before the first frame update
    protected virtual void Start()
    {
        trainData = transform.GetComponentInParent<Train_InGame>();
        Bullet_List = GameObject.Find("Bullet_List").GetComponent<Transform>();
        train_Attack_Delay = trainData.Train_Attack_Delay;
        train_Rotation_Delay = 0;
        lastTime = 0;

        rotation_TurretFlag = false;
        Item_Attack_Delay = 0;
        Item_Rotation_Delay = 0;
    }

    public void Item_Turret_Attack_Speed_UP(float persent, bool flag)
    {
        float delay = train_Attack_Delay * (persent / 100f);
        if (flag)
        {
            Item_Attack_Delay += delay;
            //Debug.Log("attack : " + (Item_Attack_Delay + train_Attack_Delay));
        }
        else
        {
            Item_Attack_Delay -= delay;
            //Debug.Log("attack : " + (Item_Attack_Delay + train_Attack_Delay));
        }
    }

    public void Item_Turret_Rotattion_Speed_UP(float persent, bool flag)
    {
        float delay = train_Rotation_Delay * (persent / 100f);
        if (flag)
        {
            Item_Rotation_Delay += delay;
            //Debug.Log("rotation : " + (Item_Rotation_Delay + train_Rotation_Delay));
        }
        else
        {
            Item_Rotation_Delay -= delay;
            //Debug.Log("rotation : " + (Item_Rotation_Delay + train_Rotation_Delay));
        }
    }

    public float Bullet_Delay_Percent()
    {
        float elapsedTime = Time.time - lastTime;
        float progress = Mathf.Clamp01(elapsedTime / (train_Attack_Delay - Item_Attack_Delay));
        return progress;
    }
}
