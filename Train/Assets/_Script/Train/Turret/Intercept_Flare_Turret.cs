using MoreMountains.Feel;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Intercept_Flare_Turret : Turret
{
    public Transform BulletObject;
    GameObject[] Flare_List;

    protected override void Start()
    {
        base.Start();
        Flare_List = new GameObject[transform.childCount];

        for(int i = 0; i < transform.childCount; i++)
        {
            Flare_List[i] = transform.GetChild(i).gameObject;
        }
    }

    private void Update()
    {
        if (!trainData.DestoryFlag)
        {
            BulletFire();
        }
    }

    void BulletFire()
    {
        if (Time.time >= lastTime + (train_Attack_Delay - Item_Attack_Delay))
        {
            int Random_X = Random.Range(0, Flare_List.Length);
            StartCoroutine(Flare_Opne_And_Close(Random_X));
            lastTime = Time.time;
        }
    }

    IEnumerator Flare_Opne_And_Close(int num)
    {
        while (Flare_List[num].transform.localEulerAngles.z <= 120)
        {
            Flare_List[num].transform.Rotate(new Vector3(0, 0, 300f * Time.deltaTime));
            yield return null;
        }
        Vector2 Flare_Positinon = new Vector2 (Flare_List[num].transform.position.x + 0.2f, Flare_List[num].transform.position.y - 0.1f);
        Instantiate(BulletObject, Flare_Positinon, transform.rotation, Bullet_List);
        yield return new WaitForSeconds(0.5f);
        while (Flare_List[num].transform.localEulerAngles.z  >= 1)
        {
            Flare_List[num].transform.Rotate(new Vector3(0, 0, -300f * Time.deltaTime));
            yield return null;
        }
    }
}