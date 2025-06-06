using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class parallex : MonoBehaviour
{
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
        if (GameDirector.gameType == GameType.Playing ||GameDirector.gameType == GameType.Boss ||
            GameDirector.gameType == GameType.Refreshing || GameDirector.gameType == GameType.Ending)
        {
            for (int i = 0; i < backgrounds.Length; i++)
            {
                float speed = backSpeed[i] * parallaxSpeed;
                offset += (Time.deltaTime * speed + (GameDirector.TrainSpeed / 10000f));
                mat[i].SetTextureOffset("_MainTex", new Vector2(offset, 0) * speed);
            }
        }/*else if()
        {
            float EndingSpeed = 0.1f;
            for (int i = 0; i < backgrounds.Length; i++)
            {
                float speed = backSpeed[i] * EndingSpeed;
                offset += (Time.deltaTime * speed + (GameDirector.TrainSpeed / 10000f));
                mat[i].SetTextureOffset("_MainTex", new Vector2(offset, 0) * speed);
            }
        }*/
    }
    private void FixedUpdate()
    {
        transform.position = new Vector3(cam.position.x, transform.position.y, transform.position.z);
    }
}
