using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CowBoy_Grap : MonoBehaviour
{
    CowBoy unit;

    LineRenderer line;
    float grappleShootSpeed;
    float grappleCollectSpeed;

    bool isCounting;
    bool isGrappling;
    bool isRetracting;
    Vector2 NonTargetPos;
    public GameObject target;
    private void Start()
    {
        isCounting = false;
        isGrappling = false;
        isRetracting = false;
        grappleShootSpeed = 10f;
        grappleCollectSpeed = 5f;

        unit = GetComponentInParent<CowBoy>();
        line = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
/*        if (target == null && isGrappling)
        {
            NonTargetPos = line.GetPosition(1);
            isRetracting = false;
            if (!isRetracting)
            {`
                StartCoroutine(NonGrapple());
            }
        }
*/
        if(target != null)
        {
            if (target.GetComponent<Monster>().monster_gametype == Monster_GameType.Die && isGrappling)
            {
                NonTargetPos = line.GetPosition(1);
                isRetracting = false;
                StartCoroutine(NonGrapple());
                target = null;
            }
        }


        if (isRetracting)
        {
            line.SetPosition(0, transform.position);

            if (!target) // null 또는 파괴된 경우 자동으로 걸러짐
            {
                isRetracting = false;
                StartCoroutine(NonGrapple());
                target = null;
                return;
            }

            try
            {
                line.SetPosition(1, target.transform.position);
            }
            catch(MissingReferenceException)
            {
                isRetracting = false;
                StartCoroutine(NonGrapple());
                target = null;
            }
        }
    }
    IEnumerator Grapple()
    {
        float t = 0;
        float time = 3;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        Vector2 newPos;
        for(; t < time; t+= grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, target.transform.position, t / time);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);
            yield return null;
        }

        line.SetPosition(1, target.transform.position);
        unit.mercenaryActive_Change(Active.work);
        isRetracting = true;
        isCounting = false;
    }

    IEnumerator NonGrapple()
    {
        float t = 0;
        float time = 3;

        Vector2 newPos;
        for(;t < time; t += grappleCollectSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(NonTargetPos, transform.position, t / time);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);
            yield return null;
        }

        line.SetPosition(1, transform.position);
        if (!isCounting)
        {
            unit.workCountUP();
            isCounting = true;
        }
        if (unit.refreshFlag)
        {
            unit.mercenaryActive_Change(Active.refresh);
        }
        else
        {
            unit.mercenaryActive_Change(Active.move);
        }
        isGrappling = false;
        line.enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster"))
        {
            if (collision.GetComponent<Monster>() != null)
            {
                if (collision.GetComponent<Monster>().Monster_Type.Equals("Sky"))
                {
                    if (!isGrappling)
                    {
                        if (unit.mercenaryActive_Check() == Active.move)
                        {
                            isGrappling = true;
                            target = collision.gameObject;
                            line.enabled = true;
                            line.positionCount = 2;

                            StartCoroutine(Grapple());
                            target.GetComponent<Monster>().grapTrigger();
                        }
                    }
                }
            }
        }
    }
}
