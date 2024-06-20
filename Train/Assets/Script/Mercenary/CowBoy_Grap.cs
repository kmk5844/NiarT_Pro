using System.Collections;
using System.Collections.Generic;
using UnityEditor.Localization.Plugins.XLIFF.V12;
using UnityEngine;

public class testGrap : MonoBehaviour
{
    LineRenderer line;
    float grappleShootSpeed;
    float grappleCollectSpeed;

    bool isGrappling = false;
    bool retracting = false;
    Vector2 NonTargetPos;
    GameObject target;
    private void Start()
    {
        isGrappling = false;
        retracting = false;
        grappleShootSpeed = 10f;
        grappleCollectSpeed = 5f;

        line = GetComponent<LineRenderer>();
    }

    private void FixedUpdate()
    {
        if (target == null && isGrappling)
        {
            NonTargetPos = line.GetPosition(1);
            retracting = false;
            if (!retracting)
            {
                StartCoroutine(NonGrapple());
            }
        }

        if (retracting)
        {
            line.SetPosition(0, transform.position);
            line.SetPosition(1, target.transform.position);
        }
    }
    IEnumerator Grapple()
    {
        float t = 0;
        float time = 5;

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
        retracting = true;
    }

    IEnumerator NonGrapple()
    {
        float t = 0;
        float time = 5;

        Vector2 newPos;
        for(;t < time; t += grappleCollectSpeed * Time.deltaTime)
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
        if (!isGrappling)
        {
            if (collision.CompareTag("Monster"))
            {
                isGrappling = true;
                target = collision.gameObject;
                line.enabled = true;
                line.positionCount = 2;

                StartCoroutine(Grapple());
            }
        }
    }
}
