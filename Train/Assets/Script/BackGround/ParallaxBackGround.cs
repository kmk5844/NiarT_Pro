using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class parallex : MonoBehaviour
{
    public bool Brain_Flag;
    public float Mat_Speed;

    public GameObject GameDirector_Object;
    GameDirector GameDirector;
    Transform cam;

    GameObject[] backgrounds;
    Material[] mat;

    [SerializeField]
    float[] backSpeed;

    float farthestBack;

    [Range(0.03f, 1f)]
    public float parallaxSpeed;

    float offset;
    Transform Player;

    // Start is called before the first frame update
    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        GameDirector = GameDirector_Object.GetComponent<GameDirector>();
        cam = Camera.main.transform;
        if (Brain_Flag)
        {
            transform.GetComponent<CinemachineBrain>().enabled = true;
        }
        else
        {
            transform.GetComponent<CinemachineBrain>().enabled = false;
        }

        int backCount = transform.childCount;
        mat = new Material[backCount];
        backSpeed = new float[backCount];
        backgrounds = new GameObject[backCount];

        for (int i = 0; i < backCount; i++)
        {
            backgrounds[i] = transform.GetChild(i).gameObject;
            mat[i] = backgrounds[i].GetComponent<Renderer>().material;
        }

        BackSpeedCalculate(backCount);
    }



    void BackSpeedCalculate(int backCount)
    {
        for (int i = 0; i < backCount; i++)
        {
            if ((backgrounds[i].transform.position.z - cam.position.z) > farthestBack)
            {
                farthestBack = backgrounds[i].transform.position.z - cam.position.z;
            }
        }

        for (int i = 0; i < backCount; i++)
        {
            backSpeed[i] = 1 - (backgrounds[i].transform.position.z - cam.position.z) / farthestBack;
        }

    }

    private void LateUpdate()
    {
        if (GameDirector.gameType == GameType.Playing || GameDirector.gameType == GameType.Ending)
        {
            if (!Brain_Flag)
            {
                float speed = Mat_Speed * parallaxSpeed;
                offset += (Time.deltaTime * speed + (GameDirector.TrainSpeed / 20000f)) / 10f;
                mat[0].SetTextureOffset("_MainTex", new Vector2(offset, 0) * speed);
            }
            else
            {
                for (int i = 0; i < backgrounds.Length; i++)
                {
                    float speed = backSpeed[i] * parallaxSpeed;
                    offset += (Time.deltaTime * speed + (GameDirector.TrainSpeed / 20000f)) / 10f;
                    mat[i].SetTextureOffset("_MainTex", new Vector2(offset, 0) * speed);
                }
            }
        }

    }
    private void FixedUpdate()
    {
        if (!Brain_Flag)
        {
            if (!cam.transform.GetComponent<CameraFollow>().CameraFlag)
            {
                transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);
            }
            else
            {
                transform.position = new Vector3(cam.transform.GetComponent<CameraFollow>().V_Cam_X, transform.position.y, transform.position.z);
            }
        }
    }
}
