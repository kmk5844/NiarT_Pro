using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

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
    public Transform TrainCam_List;
    int max_trainCam_Count;
    int trainCam_Count;
    // Start is called before the first frame update
    void Start()
    {
        trainCam_Count = -1;
        max_trainCam_Count = TrainCam_List.childCount;
        transform.position = new Vector3(Player.transform.position.x, CameraOffset.y, transform.position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            prev_Cam();
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            next_Cam();
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if(trainCam_Count != -1)
            {
                Player_Cam();
            }
        }

    }
    private void FixedUpdate()
    {
        Vector3 postion = new Vector3(Player.transform.position.x + CameraOffset.x, CameraOffset.y, Player.transform.position.z + CameraOffset.z);
        offset = Vector3.Lerp(transform.position, postion, Time.deltaTime * CameraSpeed);
    }

    void LateUpdate()
    {

        transform.position = offset;
    }

    void prev_Cam()
    {
        if(trainCam_Count == -1)
        {
            trainCam_Count = 0;
        }
        else
        {
            if(trainCam_Count < max_trainCam_Count - 1)
            {
                trainCam_Count++;
                TrainCam_List.GetChild(trainCam_Count-1).gameObject.SetActive(false);
            }
            else
            {
                trainCam_Count = 0;
                TrainCam_List.GetChild(max_trainCam_Count-1).gameObject.SetActive(false);
            }
        }
        TrainCam_List.GetChild(trainCam_Count).gameObject.SetActive(true);
    }

    void Player_Cam()
    {
        TrainCam_List.GetChild(trainCam_Count).gameObject.SetActive(false);
        trainCam_Count = -1;
    }

    void next_Cam()
    {
        if (trainCam_Count == -1)
        {
            trainCam_Count = 0;
        }
        else
        {
            if(trainCam_Count > 0)
            {
                trainCam_Count--;
                TrainCam_List.GetChild(trainCam_Count+1).gameObject.SetActive(false);
            }
            else
            {
                trainCam_Count = max_trainCam_Count - 1;
                TrainCam_List.GetChild(0).gameObject.SetActive(false);
            }
        }
        TrainCam_List.GetChild(trainCam_Count).gameObject.SetActive(true);
    }
}
