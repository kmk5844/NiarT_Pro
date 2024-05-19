using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;

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
    Transform Cam_Trans;

    GameObject V_Cam;
    public float V_Cam_X;

    Vector3 offset;
    public Transform TrainCam_List;
    int max_trainCam_Count;
    int trainCam_Count;
    public bool CameraFlag;

    // Start is called before the first frame update
    void Start()
    {
        CameraFlag = false;
        Cam_Trans = GetComponent<Transform>();
        trainCam_Count = -1;
        max_trainCam_Count = TrainCam_List.childCount;
        Cam_Trans.position = new Vector3(Player.transform.position.x, CameraOffset.y, Cam_Trans.position.z);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            prev_Cam();
            CameraFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.C))
        {
            next_Cam();
            CameraFlag = true;
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            if(trainCam_Count != -1)
            {
                Player_Cam();
                CameraFlag = false;
            }
        }
        Vector3 postion = new Vector3(Player.transform.position.x + CameraOffset.x, CameraOffset.y, Player.transform.position.z + CameraOffset.z);
        offset = Vector3.Lerp(Cam_Trans.position, postion, Time.deltaTime * CameraSpeed * 5);
    }

    void FixedUpdate()
    {
        Cam_Trans.position = offset;
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
        V_Cam = TrainCam_List.GetChild(trainCam_Count).gameObject;
        V_Cam.gameObject.SetActive(true);
        V_Cam_X = V_Cam.transform.position.x;
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
        V_Cam = TrainCam_List.GetChild(trainCam_Count).gameObject;
        V_Cam.gameObject.SetActive(true);
        V_Cam_X = V_Cam.transform.position.x;
    }
}
