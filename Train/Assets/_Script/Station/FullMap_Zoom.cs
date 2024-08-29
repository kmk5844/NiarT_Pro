using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FullMap_Zoom : MonoBehaviour
{
    public float camSpeed;

    public float minZoom;
    public float maxZoom;

    float zoom;
    public Transform FullMap_Content;
    public Slider Zoom_Slider;

    private void Start()
    {
        Zoom_Slider.minValue = minZoom;
        Zoom_Slider.maxValue = maxZoom;
        Zoom_Slider.onValueChanged.AddListener(sliderZoom);
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
}
