using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxBackGround_Root : MonoBehaviour
{
    public Transform player;

    private void Start()
    {
        if (player == null)
        {
            player = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void LateUpdate()
    {
        transform.position = new Vector3(
            player.position.x,
            transform.position.y,
            transform.position.z
        );
    }
}
