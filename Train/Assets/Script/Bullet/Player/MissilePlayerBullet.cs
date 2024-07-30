using UnityEngine;

public class MissilePlayerBullet : Bullet
{
    bool TargetFlag;
    GameObject Target;

    protected override void Start()
    {
        base.Start();
        TargetFlag = false;
        Bullet_Player();
    }

    private void FixedUpdate()
    {
        if (!TargetFlag)
        {
            Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 2);
            foreach (Collider2D collider in colliders)
            {
                if (collider.CompareTag("Monster"))
                {
                    Target = collider.gameObject;
                    TargetFlag = true;
                    break;
                }
            }
        }
        else
        {
            if (Target != null)
            {
                // Ÿ�� �������� ȸ��
                transform.up = (Target.transform.position - transform.position).normalized;

                // Ÿ�� �������� �̵�
                rid.velocity = transform.up * Speed;
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }

    void Bullet_Player()
    {
        Camera _cam = Camera.main;

        Vector3 mousePos = _cam.ScreenToWorldPoint(Input.mousePosition);
        dir = mousePos - transform.position;
        Vector3 rotation = transform.position - mousePos;
        float rot = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, rot + 90);
        rid.velocity = new Vector2(dir.x, dir.y).normalized * Speed;
        Destroy(gameObject, 3f);
    }
}
