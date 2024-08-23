using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_MapCamera : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField]
    GameObject Player;
    [Header("카메라 세팅")]
    [SerializeField]
    float CameraSpeed;
    Transform Cam_Trans;
    Vector3 offset;

    void Start()
    {
        Cam_Trans = GetComponent<Transform>();
        Cam_Trans.position = new Vector3(Player.transform.position.x, Cam_Trans.position.y, Cam_Trans.position.z);
    }

    void Update()
    {
        Vector3 postion = new Vector3(Player.transform.position.x, 3, -10.1f);
        offset = Vector3.Lerp(Cam_Trans.position, postion, Time.deltaTime * CameraSpeed * 5);
    }

    void FixedUpdate()
    {
        Cam_Trans.position = offset;
    }
}
