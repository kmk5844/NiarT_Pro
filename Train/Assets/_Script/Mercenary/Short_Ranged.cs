using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static TreeEditor.TreeGroup;

public class Short_Ranged : Mercenary
{
    public int unit_Attack;
    public float unit_Attack_Delay;
    Short_Ranged_KillZone attackZone;
    [SerializeField]
    BoxCollider2D Zone_Collider;
    [SerializeField]
    BoxCollider2D Attack_Collider;
    bool attackFlag;
    public bool lockFlag;

    public Transform Target;

    protected override void Awake()
    {
        base.Awake();
    }

    protected override void Start()
    {
        base.Start();
        act = Active.move;
        attackFlag = false;
        lockFlag = false;
        attackZone = GetComponentInChildren<Short_Ranged_KillZone>();
        Zone_Collider = attackZone.GetComponent<BoxCollider2D>();
        Attack_Collider = attackZone.transform.GetChild(0).GetComponentInChildren<BoxCollider2D>();
        
        Zone_Collider.enabled = true;
        Attack_Collider.enabled = false;
    }

    protected override void Update()
    {
        base.Update();

        if (Target != null && Target.gameObject != null && Target.gameObject.activeInHierarchy)
        {
            act = Active.work;
        }
        else
        {
            Target = null; // 안전하게 초기화
            act = Active.move;
            lockFlag = false;
        }

        if (Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss || Mer_GameType == GameType.Refreshing)
        {
            if (HP <= 0 && act != Active.die)
            {
                HP = 0;
                act = Active.die;
                Zone_Collider.enabled = false;
                isDying = true;
            }

            if (act == Active.move)
            {
                if (!Zone_Collider.enabled)
                {
                    Zone_Collider.enabled = true;
                }
            }

            if (act == Active.work)
            {
                if (!attackFlag)
                {
                    StartCoroutine(Attack());
                }
            }
        }
    }

    private void FixedUpdate()
    {
        if (Mer_GameType == GameType.Playing || Mer_GameType == GameType.Boss || Mer_GameType == GameType.Refreshing)
        {
            if (act == Active.move)
            {
                base.Combatant_Move();
            }

            if (act == Active.work && Target != null)
            {
                if (transform.position.x < MonsterDirector.MinPos_Ground.x + 1f)
                {
                    Target = null;
                    lockFlag = false;
                    act = Active.move;
                    rb2D.velocity = Vector2.zero;
                    return;
                }
                else if (transform.position.x > MonsterDirector.MaxPos_Ground.x - 1f)
                {
                    Target = null;
                    lockFlag = false;
                    act = Active.move;
                    rb2D.velocity = Vector2.zero;
                    return;
                }

                Vector2 targetPos = Target.position;
                Vector2 selfPos = rb2D.position;

                Vector2 direction = targetPos - selfPos;
                direction.y = 0f; // y축 이동 제거

                // 반대 방향으로 약간 떨어진 위치를 목표로 설정
                Vector2 offset = direction.normalized * -1f;
                float followDistance = 1f;
                Vector2 desiredPos = targetPos + offset * followDistance;

                // 그 위치를 향해 이동
                Vector2 moveDir = (desiredPos - selfPos).normalized;
                moveDir.y = 0f; // y축 이동 제거
                float speed = 10f;
                float stopDistance = 0.1f;
                if (Vector2.Distance(selfPos, desiredPos) < stopDistance)
                {
                    rb2D.velocity = Vector2.zero;
                }
                else
                {
                    rb2D.velocity = moveDir * speed;
                }
            }

            if (act == Active.die)
            {
                rb2D.velocity = Vector2.zero;
            }
        }
        else if (Mer_GameType == GameType.Ending)
        {
            act = Active.Game_Wait;
            rb2D.velocity = Vector2.zero;
        }
    }

    public void M_Buff_Atk(int buff_atk, bool flag)
    {
        if (flag)
        {
            unit_Attack += buff_atk;
        }
        else
        {
            unit_Attack -= buff_atk;
        }
    }

    IEnumerator Attack()
    {
        attackFlag = true;
        yield return new WaitForSeconds(0.5f);
        Attack_Collider.enabled = true;
        yield return new WaitForSeconds(0.5f);
        Attack_Collider.enabled = false;
        attackFlag = false;
    }
    public void Level_AddStatus_ShortRanged(List<Info_Level_Mercenary_Short_ranged> type, int level)
    {
        unit_Attack = type[level].Unit_Attack;
        unit_Attack_Delay = type[level].Unit_Atk_Delay;
    }
}