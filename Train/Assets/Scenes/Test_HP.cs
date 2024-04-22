using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test_HP : MonoBehaviour
{
    [Range(0, 100)]
    public int HP;

    //HP관련 변수
    float minRotation;
    float maxRotation;

    float minHP;
    float maxHP;

    public Transform HP_Arrow;


    // Start is called before the first frame update
    void Start()
    {
        minRotation = 105f;
        maxRotation = -41f;

        minHP = 0f;
        maxHP = 100f;
    }

    // Update is called once per frame
    void Update()
    {
        HP_Arrow.rotation = Quaternion.Euler(0f, 0f, HPToRotation(HP));
    }

    private float HPToRotation(float HP)
    {
        return (HP - minHP) * (maxRotation - minRotation) / (maxHP - minHP) + minRotation;
    }
}
