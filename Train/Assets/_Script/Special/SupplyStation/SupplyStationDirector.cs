using System.Collections;
using UnityEngine;

public class SupplyStationDirector : MonoBehaviour
{
    public Vector2 Min_Vec;
    public Vector2 Max_Vec;
    float SupplySpeed;
    public SupplyObject Supply;
    bool spawnFlag;
    bool timeflag;
    public SupplyObject Spawingsupply;
    int count = 0;

    // Start is called before the first frame update
    void Start()
    {
        timeflag = false;
        spawnFlag = true;
        SupplySpeed = 3;
        Supply.Setting(Min_Vec, Max_Vec, SupplySpeed, this);
    }

    // Update is called once per frame
    void Update()
    {
        if (!timeflag)
        {
            if (spawnFlag)
            {
                Supply.Setting(Min_Vec, Max_Vec, SupplySpeed, this);
                Spawingsupply = Instantiate(Supply, Min_Vec, Quaternion.identity);
                spawnFlag = false;
            }

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Spawingsupply.ClickSpace();
                StartCoroutine(SpawnTime());
            }
        }
    }

    IEnumerator SpawnTime()
    {
        timeflag = true;
        yield return new WaitForSeconds(2f);
        timeflag = false;
        spawnFlag = true;
    }

    public void Count()
    {
        count++;
        SupplySpeed = 3 + ((float)count * 0.3f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(Max_Vec, Min_Vec);
    }
}
