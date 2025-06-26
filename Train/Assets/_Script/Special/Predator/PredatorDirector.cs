using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PredatorDirector : MonoBehaviour
{
    public Transform IconList;
    public GameObject[] Icon;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Space)){
            Destroy(IconList.GetChild(0).gameObject);
            Instantiate(Icon[0], IconList);
        }
    }
}
