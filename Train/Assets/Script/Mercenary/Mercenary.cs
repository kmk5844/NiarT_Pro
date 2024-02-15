using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary : MonoBehaviour
{
    public Transform Train_List;
    int TrainCount;
    protected float move_X;
    protected float MaxMove_X;
    protected float MinMove_X;
    [Header("용병 정보")]
    [SerializeField] protected int HP;
    [SerializeField] protected int Stamina;
    private int maxStamina;
    [SerializeField] private int moveSpeed;
    [SerializeField] private int Refresh_Amount;
    [SerializeField] private float Refresh_Delay;
    [Header("소모되는 스테미나 양")]
    [SerializeField] protected int useStamina;
    bool isRefreshing;

    protected SpriteRenderer sprite;

    protected virtual void Start()
    {
        TrainCount = Train_List.childCount;
        move_X = 0.01f;
        MaxMove_X = 3f;
        MinMove_X = -4f + ((TrainCount - 1) * -8.5f);
        transform.position = new Vector3(Random.Range(MinMove_X, MaxMove_X), -1, 0);
        sprite = GetComponent<SpriteRenderer>();
        
        isRefreshing = false;
        maxStamina = Stamina;
    }

    public void move()
    {
        if (Stamina <= 100 && !isRefreshing)
        {
            StartCoroutine(Refresh());
        }

        if (transform.position.x > MaxMove_X)
        {
            move_X *= -1f;
            sprite.flipX = true;
        }
        else if (transform.position.x < MinMove_X)
        {
            move_X *= -1f;
            sprite.flipX = false;
        }
        transform.Translate(move_X * moveSpeed, 0, 0);
    }

    IEnumerator Refresh()
    {
        isRefreshing = true;
        yield return new WaitForSeconds(Refresh_Delay);
        if(Stamina + Refresh_Amount > 100)
        {
            Stamina = maxStamina;
        }
        else
        {
            Stamina += Refresh_Amount;
        }

        isRefreshing = false;
    }

    //부활은 메딕이랑 그 이후의 시스템이 나오면 적을 예정

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            if (HP - collision.GetComponent<Bullet>().atk < 0)
            {
                HP = 0;
            }
            else
            {
                HP -= collision.GetComponent<Bullet>().atk;
            }
            Destroy(collision.gameObject);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(new Vector3(MinMove_X, -3, 0), new Vector3(MaxMove_X, -3, 0));
    }
}
public enum Active
{
    move,
    work,
    die,
    revive
}