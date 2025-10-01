using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class Signal_Flare : Bullet
{
    bool bombFlag;
    float cutline;
    int firecount = 0;
    int maxFirecount = 20;
    public GameObject DownBullet;
    public GameObject SkyEffect;

    // Start is called before the first frame update
    protected override void Start()
    {
        SkyEffect.SetActive(false);
        cutline = MonsterDirector.MaxPos_Sky.y + 2f;
        base.Start();
        DownBullet.GetComponent<Bullet>().atk = atk;
        Bullet_Player();
    }

    void Bullet_Player()
    {
        Camera _cam = Camera.main;

        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
    }

    private void FixedUpdate()
    {
        if (!bombFlag && transform.position.y > cutline)
        {
            Explode();
        }
    }

    void Explode()
    {
        if (!bombFlag)
        {
            rid.velocity = Vector2.zero;
            SkyEffect.SetActive(true);
            StartCoroutine(SkyFire());
            bombFlag = true;
        }
    }

    IEnumerator SkyFire()
    {
        while(firecount < maxFirecount)
        {
            float pos = Random.Range(transform.position.x - 5f, transform.position.x + 5f);
            Vector2 newVec = new Vector2(pos, transform.position.y);
            DownBullet.transform.position = newVec;
            DownBullet.transform.rotation = Quaternion.Euler(0, 0, 180);
            Instantiate(DownBullet);
            firecount++;
            yield return new WaitForSeconds(0.2f);
        }

        yield return new WaitForSeconds(0.5f);
        Destroy(gameObject);
    }
}
