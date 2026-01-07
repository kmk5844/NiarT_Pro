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
        { // X는 계속 이동
          transform.Translate(Vector3.right * SpeedX * Time.deltaTime);
          transform.Rotate(0f, 0f, RoateSpeed * Time.deltaTime * 2f); 
        } 
    }

    IEnumerator lightSetting()
    {
        // 최초 딜레이
        yield return new WaitForSeconds(Random.Range(1f, 3f));

        while (true)
        {
            yield return StartCoroutine(lightOnOff());
            yield return new WaitForSeconds(Random.Range(0.5f, 2f));
        }
    }

    IEnumerator lightOnOff()
    {
        flag = true;
        // 랜덤 세팅
        SpeedX = Random.Range(0.5f, 0.7f);
        RoateSpeed = Random.Range(-2f, 2f);

        transform.position = new Vector3(
            Random.Range(InitVec.x - 3f, InitVec.x + 3f),
            InitVec.y
        );

        float roation = Random.Range(MinRadius, MaxRadius);
        transform.localRotation = Quaternion.Euler(0, 0, roation);

        float innerAngle = Random.Range(MinSpotAngle, MaxSpotAngle);
        _light.pointLightInnerAngle = innerAngle;
        _light.pointLightOuterAngle = Random.Range(innerAngle, MaxSpotAngle);

        float maxIntensity = MaxIntensity;

        // =====================
        // Fade In
        // =====================
        float fadeInDuration = Random.Range(0.3f, 0.8f);
        float t = 0f;

        while (t < fadeInDuration)
        {
            t += Time.deltaTime;
            float eased = Mathf.SmoothStep(0f, 1f, t / fadeInDuration);
            _light.intensity = Mathf.Lerp(0f, maxIntensity, eased);
            yield return null;
        }
        _light.intensity = maxIntensity;

        // 살짝 유지
        yield return new WaitForSeconds(Random.Range(0.1f, 0.4f));

        // =====================
        // Fade Out
        // =====================
        float fadeOutDuration = Random.Range(0.5f, 1.2f);
        t = 0f;

        while (t < fadeOutDuration)
        {
            t += Time.deltaTime;
            float eased = Mathf.SmoothStep(0f, 1f, t / fadeOutDuration);
            _light.intensity = Mathf.Lerp(maxIntensity, 0f, eased);
            yield return null;
        }
        _light.intensity = 0f;
        flag = false;
    }
}
