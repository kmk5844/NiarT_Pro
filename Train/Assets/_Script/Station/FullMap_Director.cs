using Radishmouse;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullMap_Director : MonoBehaviour
{
/*    [Header("카메라 줌")]
    public float camSpeed;

    public float minZoom;
    public float maxZoom;

    float zoom;
    public Transform FullMap_Content;
    public Slider Zoom_Slider;

    [Header("전체맵 오브젝트")]
    public GameObject MapObject;
    public Station_GameStart gameStartDirector;
    public SA_StageList sa_stagelist;
    public GameObject FullMap_StageButton_Object;
    Vector3[] pos;
    public FullMap_StageButton[] stageButton;

    int lastIndex;


    private void Start()
    {
        Zoom_Slider.minValue = minZoom;
        Zoom_Slider.maxValue = maxZoom;
        Zoom_Slider.onValueChanged.AddListener(sliderZoom);

        UILineRenderer ui_line = MapObject.GetComponent<UILineRenderer>();
        pos = new Vector3[ui_line.points.Length];
        for (int i = 0; i < ui_line.points.Length; i++)
        {
            pos[i] = ui_line.points[i];
        }
        lastIndex = gameStartDirector.Select_StageNum;
        CreateObject();
    }

    private void OnEnable()
    {
        zoom = 1;
        FullMap_Content.localPosition = Vector3.zero;
        FullMap_Content.localScale = new Vector3(zoom, zoom, 0);
    }

    // Update is called once per frame
    void Update()
    {
        Zoom();
        zoom = Mathf.Clamp(zoom, minZoom, maxZoom);
        Zoom_Slider.value = zoom;
        FullMap_Content.localScale = new Vector3(zoom, zoom, 0);
    }

    void sliderZoom(float value)
    {
        zoom = value;
    }

    void Zoom()
    {
        if (Input.mouseScrollDelta.y < 0)
        {
            zoom -= camSpeed * Time.deltaTime * 10f;
        }

        if (Input.mouseScrollDelta.y > 0)
        {
            zoom += camSpeed * Time.deltaTime * 10f;
        }
    }



    void CreateObject()
    {
        Vector3 lastPosition;
        stageButton = new FullMap_StageButton[sa_stagelist.Stage.Count];

        for (int i = 0; i < sa_stagelist.Stage.Count; i++)
        {
            lastPosition = pos[i];
            GameObject obj = Instantiate(FullMap_StageButton_Object, MapObject.transform);
            obj.transform.localPosition = lastPosition;
            stageButton[i] = obj.GetComponent<FullMap_StageButton>();
            stageButton[i].gamestartDirection = gameStartDirector;
            stageButton[i].stageData = sa_stagelist.Stage[i];
            stageButton[i].Load();
            if (lastIndex == i)
            {
                stageButton[i].SelectStage_MarkObject.SetActive(true);
            }
        }
    }

    public void OpenFullMap()
    {
        stageButton[lastIndex].SelectStage_MarkObject.SetActive(false);
        stageButton[gameStartDirector.Select_StageNum].SelectStage_MarkObject.SetActive(true);
        lastIndex = gameStartDirector.Select_StageNum;
    }*/
}
