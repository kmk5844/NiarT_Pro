using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.SmartFormat.GlobalVariables;
using UnityEngine.Rendering.Universal;

public class CustomLight_12 : MonoBehaviour
{
    [Header("라이트 각도")]
    public float MinRadius;
    public float MaxRadius;
    [Header("스팟 각도")]
    public float MinSpotAngle;
    public float MaxSpotAngle;
    [Header("시간")]
    public float Minduration;
    public float Maxduration;
    [Header("밝기 강도")]
    public float MaxIntensity;

    float SpeedX;
    Vector3 InitVec;

    float RoateSpeed;
    Light2D _light;
    bool flag;

    bool firstFlag;

    // Start is called before the first frame update
    void Start()
    {
        InitVec = transform.localPosition;
        _light = GetComponent<Light2D>();
        
        StartCoroutine(lightSetting());
    }

    private void Update()
    {
        if (flag)
        {
            // X는 계속 이동
            transform.Translate(Vector3.right * SpeedX * Time.deltaTime);
            transform.Rotate(0f, 0f, RoateSpeed * Time.deltaTime);
        }
    }

    IEnumerator lightSetting()
    {
        while (true)
        {
            if (!flag)
            {
                StartCoroutine(lightOnOff());
            }
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }

    IEnumerator lightOnOff()
    {
        if (!firstFlag)
        {
            yield return new WaitForSeconds(Random.Range(1f, 3f));
            firstFlag = true;
        }

        flag = true;
        SpeedX = Random.Range(0.5f, 0.7f);
        RoateSpeed = Random.Range(-2f, 2f);
        transform.position = new Vector3(Random.Range(InitVec.x -3f, InitVec.x + 3f),InitVec.y);
        float radius = Random.Range(MinRadius, MaxRadius);
        transform.localRotation = Quaternion.Euler(0, 0, radius);

        float duration = Random.Range(Minduration, Maxduration);

        float innerAngle = Random.Range(MinSpotAngle, MaxSpotAngle);
        _light.pointLightInnerAngle = innerAngle;
        _light.pointLightOuterAngle = Random.Range(innerAngle, MaxSpotAngle);
        
        float t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float eased = Mathf.SmoothStep(0f, 1f, t / duration);
            _light.intensity = Mathf.Lerp(0f, MaxIntensity, eased);
            yield return null;
        }
        _light.intensity = MaxIntensity;

        yield return new WaitForSeconds(1f);

        t = 0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float eased = Mathf.SmoothStep(0f, 1f, t / duration);
            _light.intensity = Mathf.Lerp(MaxIntensity, 0f, eased);
            yield return null;
        }
        _light.intensity = 0f;

        flag = false;
    }
}
