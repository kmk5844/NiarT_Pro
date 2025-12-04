using UnityEngine;

public class BossAim : MonoBehaviour
{
    Transform player;

    private Vector2 aimDir;

    private void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
    }

    void Update()
    {
        aimDir = (player.position - transform.position).normalized;

        if (player.position.x >= transform.position.x)
        {
            transform.localScale = new Vector3(-1, 1, 1);
        }
        else
        {
            transform.localScale = new Vector3(1, -1, 1);
        }

        float angle = Mathf.Atan2(aimDir.y, aimDir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}