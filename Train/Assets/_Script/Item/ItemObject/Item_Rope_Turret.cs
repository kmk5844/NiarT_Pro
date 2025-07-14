using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_Rope_Turret : MonoBehaviour
{
    LineRenderer line;
    float grappleShootSpeed;
    float grappleCollectSpeed;

    bool isGrappling;
    bool isRetracting;
    Vector2 NonTargetPos;
    public GameObject target;

    private void Start()
    {
        isGrappling = false;
        isRetracting = false;
        grappleShootSpeed = 10f;
        grappleCollectSpeed = 5f;

        line = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        /*        if (target == null && isGrappling)
                {
                    NonTargetPos = line.GetPosition(1);
                    isRetracting = false;
                    if (!isRetracting)
                    {
                        StartCoroutine(NonGrapple());
                    }
                }
        */
        if (target != null)
        {
            if (target.GetComponent<Monster>().monster_gametype == Monster_GameType.Die && isGrappling)
            {
                NonTargetPos = line.GetPosition(1);
                isRetracting = false;
                if (!isRetracting)
                {
                    StartCoroutine(NonGrapple());
                    target = null;
                }
            }
        }


        if (isRetracting)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, target.transform.position);
        }
    }
    IEnumerator Grapple()
    {
        float t = 0;
        float time = 3;

        line.SetPosition(0, transform.position);
        line.SetPosition(1, transform.position);

        Vector2 newPos;
        for (; t < time; t += grappleShootSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(transform.position, target.transform.position, t / time);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);
            yield return null;
        }

        line.SetPosition(1, target.transform.position);
        isRetracting = true;
    }

    IEnumerator NonGrapple()
    {
        float t = 0;
        float time = 3;

        Vector2 newPos;
        for (; t < time; t += grappleCollectSpeed * Time.deltaTime)
        {
            newPos = Vector2.Lerp(NonTargetPos, transform.position, t / time);
            line.SetPosition(0, transform.position);
            line.SetPosition(1, newPos);
            yield return null;
        }

        line.SetPosition(1, transform.position);

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
