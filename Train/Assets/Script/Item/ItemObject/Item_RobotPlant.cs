using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item_RobotPlant : MonoBehaviour
{
    public int Robot_Bomb_Atk;
    public int DelayTime;
    public int SpawnTime;
    public GameObject Robot;
    bool SpawnFlag;
    // Start is called before the first frame update
    void Start()
    {
        Robot.GetComponent<Item_RobotPlant_Robot>().Atk = Robot_Bomb_Atk;
        SpawnFlag = false;
        Destroy(gameObject, DelayTime);
    }

    // Update is called once per frame
    void Update()
    {
        if (!SpawnFlag)
        {
            StartCoroutine(SpawnRobot());
        }
    }

    IEnumerator SpawnRobot()
    {
        SpawnFlag = true;
        yield return new WaitForSeconds(SpawnTime);
        Instantiate(Robot, transform.position, Quaternion.identity);
        SpawnFlag = false;
    }
}
