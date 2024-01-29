using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("�÷��̾�")]
    [SerializeField]
    GameObject Player;
    [Header("ī�޶� ����")]
    [SerializeField]
    Vector3 CameraOffset;
    [SerializeField]
    float CameraSpeed;

    Vector3 offset;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Player.transform.position.x, CameraOffset.y, transform.position.z);
    }

    private void FixedUpdate()
    {
        offset = Vector3.Lerp(transform.position, Player.transform.position + CameraOffset, Time.deltaTime * CameraSpeed);
    }
    void LateUpdate()
    {
        transform.position = offset;
    }
}
