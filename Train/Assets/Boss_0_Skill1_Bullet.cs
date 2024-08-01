using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_0_Skill1_Bullet : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        transform.position = new Vector2(transform.position.x, transform.position.y + 10f);
        Destroy(gameObject, 3f);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Translate(0, -20 * Time.deltaTime, 0);
    }
}
