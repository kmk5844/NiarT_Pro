using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround_Root : MonoBehaviour
{
    public Transform player;

    private float lastPlayerX;
    public float followSmooth = 10f;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }

        lastPlayerX = player.position.x;
    }

    void LateUpdate()
    {
        float targetX = transform.position.x + (player.position.x - lastPlayerX);
        float newX = Mathf.Lerp(transform.position.x, targetX, Time.deltaTime * followSmooth);

        transform.position = new Vector3(newX, transform.position.y, transform.position.z);
        lastPlayerX = player.position.x;
    }
}
