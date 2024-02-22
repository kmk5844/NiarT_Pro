using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mercenary : MonoBehaviour
{
    [SerializeField]
    protected Active act;

    protected Transform Train_List;
    int TrainCount;
    protected float move_X;
    protected float MaxMove_X;
    protected float MinMove_X;
    [Header("�뺴 ����")]
    public int HP;
    [HideInInspector]
    public int MaxHP;
    public int Stamina;
    [HideInInspector]
    public int MaxStamina;
    [SerializeField] protected float moveSpeed;
    [SerializeField] private int Refresh_Amount;
    [SerializeField] private float Refresh_Delay;

    [Header("���� ����")]
    [SerializeField]
    int def;
    float era;
    [SerializeField]
    float def_constant;

    [Header("�Ҹ�Ǵ� ���׹̳� ��")]
    [SerializeField]
    protected int useStamina;
    bool isRefreshing;
    protected bool isRefreshing_weak;
    protected bool isDying;
    bool isCombatantWalking;
    bool isCombatantIdling;
    float Combatant_Idle_LastTime;
    int combatant_move_x;
    Vector3 combatant_BeforePosition;

   [HideInInspector]
    public bool isHealWithMedic;

    protected SpriteRenderer sprite;

    protected virtual void Start()
    {
        Train_List = GameObject.Find("Train_List").GetComponent<Transform>();
        TrainCount = Train_List.childCount;
        move_X = 0.01f;
        MaxMove_X = 3f;
        MinMove_X = -4f + ((TrainCount - 1) * -8.5f);
        transform.position = new Vector3(Random.Range(MinMove_X, MaxMove_X), -1, 0);
        sprite = GetComponent<SpriteRenderer>();

        era = 1f - (float)def / def_constant;

        isRefreshing = false;
        isRefreshing_weak = false;
        isHealWithMedic = false;
        isDying = false;
        isCombatantWalking = false;
        isCombatantIdling = false;
        MaxHP = HP;
        MaxStamina = Stamina;
    }

    protected void combatant_Move()
    {
        if (Stamina < MaxStamina && !isRefreshing)
        {
            StartCoroutine(Refresh());
        }

        if (!isCombatantWalking && !isCombatantIdling)
        {
            if (transform.position.x > MaxMove_X - 7)
            {
                combatant_move_x = Random.Range(-6, 0);
            } else if (transform.position.x < MinMove_X + 7)
            {
                combatant_move_x = Random.Range(0, 6);
            }
            else
            {
                combatant_move_x = Random.Range(-6, 6);
            }

            if (combatant_move_x > 0)
            {
                move_X = 0.01f;
                sprite.flipX = false;
            }
            else if (combatant_move_x < 0)
            {
                move_X = -0.01f;
                sprite.flipX = true;
            }
            else if (combatant_move_x == 0 || combatant_move_x == 1)
            {
                combatant_move_x += 1;
                move_X = 0.01f;
            } else if (combatant_move_x == -1)
            {
                combatant_move_x -= 1;
                move_X = 0.01f;
            }
            combatant_BeforePosition = transform.position;
            isCombatantWalking = true;
        }
        else if (isCombatantWalking)
        {
            if (combatant_move_x > 0 && transform.position.x > combatant_BeforePosition.x + combatant_move_x)
            {
                isCombatantWalking = false;
            }
            else if (combatant_move_x < 0 && transform.position.x < combatant_BeforePosition.x + combatant_move_x)
            {
                isCombatantWalking = false;
            }
            else if (transform.position.x < MinMove_X)
            {
                isCombatantWalking = false;
            }
            else if (transform.position.x > MaxMove_X)
            {
                isCombatantWalking = false;
            }

            if (isCombatantWalking)
            {
                transform.Translate(move_X * moveSpeed, 0, 0);
            }
            else
            {
                combatant_Idle();
            }
        } else if (!isCombatantWalking && isCombatantIdling)
        {
            combatant_Idle();
        } else if (isCombatantIdling && (transform.position.x < MinMove_X || transform.position.x > MaxMove_X))
        {
            if (transform.position.x < MinMove_X)
            {
                move_X = 0.01f;
                transform.Translate(move_X * moveSpeed, 0, 0);
            } else if (transform.position.x > MaxMove_X)
            {
                move_X = -0.01f;
                transform.Translate(move_X * moveSpeed, 0, 0);
            }
        }
    }

    protected void non_combatant_Move()
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
        if (Stamina + Refresh_Amount > MaxStamina)
        {
            Stamina = MaxStamina;
        }
        else
        {
            Stamina += Refresh_Amount;
        }

        isRefreshing = false;
    }

    protected IEnumerator Refresh_Weak()
    {
        isRefreshing_weak = true;
        yield return new WaitForSeconds(10);
        Stamina = 70;
        isRefreshing_weak = false;

    }

    protected void combatant_Idle()
    {
        if (!isCombatantIdling)
        {
            isCombatantIdling = true;
            Combatant_Idle_LastTime = Time.time;

        }
        else if (Time.time >= Combatant_Idle_LastTime + Random.Range(1, 4))
        {
            isCombatantIdling = false;
        }
    }

    public float check_HpParsent()
    {
        return (float)HP / (float)MaxHP * 100f;
    }
    public float check_StaminaParsent()
    {
        return (float)Stamina / (float)MaxStamina * 100f;
    }

    public IEnumerator Revive(int Heal_HpParsent) //�ִϸ��̼� �߰��ϸ� ����
    {
        act = Active.revive;
        HP = MaxHP * Heal_HpParsent / 100;
        Debug.Log("���� ��Ȱ�ϴ� �ִϸ��̼�");
        yield return new WaitForSeconds(2);
        act = Active.move;
    }
    //��Ȱ�� �޵��̶� �� ������ �ý����� ������ ���� ����

    public bool Check_Work()
    {
        if (act == Active.move)
        {
            return true;
        } else if (act == Active.work)
        {
            return false;
        }
        return false;
    }

    public float Check_moveX()
    {
        return move_X;
    }

    public bool Check_Live()
    {
        if(act == Active.die)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Monster_Bullet"))
        {
            int damageTaken = Mathf.RoundToInt(collision.GetComponent<Bullet>().atk * era);
            if(HP - damageTaken < 0)
            {
                HP = 0;
            }
            else
            {
                HP -= damageTaken;
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
    move,   // �̵�
    work,   // �۾�
    die,    // ����
    revive, // ��Ȱ
    weak,   // ���׹̳� ����
    drained, // Ż��
    call    //�÷��̾� ȣ��
    //�÷��̾� ��ȣ�ۿ� �߰��ϸ� -> �÷��̾� ���� ���� ��ȣ�ۿ� �Ѵ�
}