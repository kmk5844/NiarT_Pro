using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCoolTime : MonoBehaviour
{
    public Image SkillIcon;
    public Image SkillCoolTime_Panel;
    float CoolTime;
    [SerializeField]
    float DelayTime;

    // Start is called before the first frame update
    void Start()
    {
        CoolTime = 0;
        SkillCoolTime_Panel.fillAmount = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        CoolTime += Time.deltaTime;
        SkillCoolTime_Panel.fillAmount = Mathf.Clamp01(CoolTime / DelayTime);

        if(CoolTime >= DelayTime)
        {
            SkillCoolTime_Panel.fillAmount = 1f;
        }

        if (CoolTime >= DelayTime + 0.2f)
        {
            Destroy(gameObject);
        }
    }

    public void SetSetting(Sprite image, float during)
    {
        SkillIcon.sprite = image;
        DelayTime = during;

        if(during == 0)
        {
            DelayTime = 0.05f;
        }
        else
        {
            DelayTime = during;
        }
    }
}
