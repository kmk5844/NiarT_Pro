using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrainDirector : MonoBehaviour
{
    Train_InGame trainData;
    public GameObject[] Platform;
    bool flag;

    void Start()
    {
        flag = false;
        trainData = transform.GetComponentInParent<Train_InGame>();
    }

    // Update is called once per frame
    void Update()
    {
        if (trainData.DestoryFlag && !flag )
        {
            foreach(GameObject game in Platform) {
                game.SetActive(false);
            }
            flag = true;
        }
    }
}
