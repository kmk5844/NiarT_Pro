using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class Item_Dagger : Bullet
{
    [SerializeField]
    int SpecialNum;
    [SerializeField]
    int subAtk;
    bool fall;
    bool changflag;
    int RandomPos_Y;

    protected override void Start()
    {
        base.Start();
        fall = false;
        changflag = false;
        rid.velocity = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z) * Vector2.right * Speed;
        RandomPos_Y = Random.Range(8, 14);
        if (SpecialNum != 0 || SpecialNum != 1)
        {
            Destroy(gameObject, 3f);
        }
    }

    private void Update()
    {
        if (SpecialNum == 2)
        {
            if (!fall && !changflag)
            {
                if (transform.position.y > 11f)
                {
                    fall = true;
                }
            }
            else if(fall && !changflag)
            {
                transform.rotation = Quaternion.Euler(0f, 0f, 270f);
                rid.velocity = Quaternion.Euler(0f, 0f, transform.rotation.eulerAngles.z) * Vector2.right * Speed;
                Destroy(gameObject, 4f);
                changflag = true;
            }
        }
        else if (SpecialNum == 3)
        {
            if(!fall && !changflag)
            {
                if(transform.position.y > RandomPos_Y)
                {
                    fall = true;
                }
            }
            else if(fall && !changflag)
            {
                GameObject Dagger = Resources.Load<GameObject>("ItemObject/Dagger");
                for(int i = 0; i < 5; i++)
                {
                    GameObject DangerPre = Instantiate(Dagger, new Vector2(transform.position.x, transform.position.y), Quaternion.Euler(0, 0, (72 * i)));
                    DangerPre.GetComponent<Item_Dagger>().Set(0, subAtk);
                    DangerPre.transform.localScale = Vector3.one;
                }
                Destroy(this.gameObject);
                changflag = true;
            }
        }
    }

    public void Set(int SpecialNum_, int atk, int subatk_ = 0)
    {
        SpecialNum = SpecialNum_;
        subAtk = subatk_;
    }
}