using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeeHome : MonoBehaviour
{
    int Count = 0;

    public GameObject SmallBee;
    public GameObject SoldierBee;
    Transform MonsterListSky;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Play());
    }

    public void Set(Transform sky)
    {
        MonsterListSky = sky;
    }

    IEnumerator Play()
    {
        while (true)
        {
            if (transform.position.y > MonsterDirector.MinPos_Ground.y + 0.5f)
            {
                transform.Translate(Vector3.down * Time.deltaTime * 6f);
                yield return null;
            }
            else
            {
                if (Count < 1)
                {
                    int random = Random.Range(0, 11);
                    if (random < 9)
                    {
                        GameObject Monster_S = Instantiate(SmallBee, transform.localPosition, Quaternion.identity, MonsterListSky);
                        Monster_S.GetComponent<Monster_23>().BeeHome_Flag = true;
                    }
                    else
                    {
                        GameObject Monster_L = Instantiate(SoldierBee, transform.localPosition, Quaternion.identity, MonsterListSky);
                        Monster_L.GetComponent<Monster_24>().BeeHome_Flag = true;
                    }
                    Count++;
                    yield return new WaitForSeconds(0.2f);
                }
                else
                {
                    yield return new WaitForSeconds(1f);
                    Destroy(gameObject);
                    yield break;
                }
            }
        }
    }
}
