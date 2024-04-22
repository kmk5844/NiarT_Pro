using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [Header("플레이어")]
    [SerializeField]
    GameObject Player;
    [Header("카메라 세팅")]
    [SerializeField]
    Vector3 CameraOffset;
    [SerializeField]
    float CameraSpeed;

    Vector3 offset;
    bool Left_Key;
    bool Right_Key;
    public float Key_Speed;

    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector3(Player.transform.position.x, CameraOffset.y, transform.position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Left_Key = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            Right_Key = true;
        }

        if (Input.GetKeyUp(KeyCode.Z))
        {
            Left_Key = false;
        }

        if (Input.GetKeyUp(KeyCode.C))
        {
            Right_Key = false;
        }
    }

    private void FixedUpdate()
    {
        Vector3 postion = new Vector3(Player.transform.position.x + CameraOffset.x, CameraOffset.y, Player.transform.position.z + CameraOffset.z);
        offset = Vector3.Lerp(transform.position, postion, Time.deltaTime * CameraSpeed);
    }

    void LateUpdate()
    {
        if (Left_Key || Right_Key)
        {
            if (Left_Key)
            {
                transform.position += new Vector3(-1f * Key_Speed, 0f, 0f);
            }

            if (Right_Key)
            {
                transform.position += new Vector3(1f * Key_Speed, 0f, 0f);
            }
        }
        else
        {
            transform.position = offset;

        }
    }
}
