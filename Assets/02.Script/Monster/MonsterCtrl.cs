using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterCtrl : UnitCtrl
{
    [HideInInspector] public UnitInfo MonInfo = new UnitInfo();

    private Transform TargetPlayerTr;
    public float TraceDist;
    public float AttackDist;

    private float CurAttCool;

    public bool IsFly = false;
    private Vector2 NextMove;
    private float MoveLimitX, MoveLimitY;

    private Rigidbody2D Rigid;
    private float PlayerColliderRadius = 0.18f - 0.02f;

    void Awake()
    {
        TargetPlayerTr = GameObject.Find("Player").transform;

        MonInfo.MaxHp = 100;
        MonInfo.CurHp = 100;
        CurAttCool = MonInfo.AttackCool;

        Rigid = this.GetComponent<Rigidbody2D>();

        Collider2D bodyCol = this.GetComponent<Collider2D>();
        MoveLimitX = bodyCol.bounds.size.x / 2f + bodyCol.offset.x;
        MoveLimitY = bodyCol.bounds.size.y / 2f + bodyCol.offset.y;
        AttackDist += PlayerColliderRadius;

        PatrolMoveStep();

        base.Init(MonInfo);
    }

    void Update()
    {
        Vector2 AttDist = new Vector2(Rigid.position.x + AttackDist, Rigid.position.y);
        Debug.DrawRay(AttDist, Vector3.down, new Color(0, 1, 0));

        if (CurState == AnimState.Die)
        {
            Collider2D[] colliders = GetComponents<Collider2D>();
            for (int i = 0; i < colliders.Length; i++)
                colliders[i].enabled = false;

            Rigid.velocity = new Vector2(0, 0);
            Destroy(this.gameObject, 5f);
        }
        else
        {
            if (CurState == AnimState.Idle)
                Rigid.velocity = new Vector2(0, 0);

            if (IsFly)
                FlyMonsterAI();
            else
                WalkMonsterAI();
        }
    }

    void WalkMonsterAI()
    {
        CurAttCool -= Time.deltaTime;

        if (TargetPlayerTr == null)
            TargetPlayerTr = GameObject.Find("Player").transform;

        float dist = Mathf.Abs(this.transform.position.x - TargetPlayerTr.position.x);
        Vector3 attPos = this.transform.position;
        attPos.x += AttackDist;
        Debug.DrawRay(attPos, Vector3.down, new Color(1, 0, 0));
        if (dist < TraceDist)
        {
            if (dist < AttackDist)
            {
                if (CurAttCool <= 0)
                {
                    MySprite.flipX = this.transform.position.x > TargetPlayerTr.position.x;
                    Rigid.velocity = new Vector2(0, 0);
                    CurState = AnimState.Attack;
                    CurAttCool = unit.AttackCool;
                }
            }
            else
            {
                float traceMove = TargetPlayerTr.position.x - this.transform.position.x > 0 ? 3 : -3;
                Walk(traceMove);
            }
        }
        else
        {
            if (NextMove.x != 0)
                Walk(NextMove.x);
            else
                CurState = AnimState.Idle;
        }
    }

    void Walk(float nextMove)
    {
        if (CurState == AnimState.Attack)
            return;

        CurState = AnimState.Walk;

        // 벽 밖으로 못나가도록
        float dir = nextMove > 0 ? MoveLimitX : -MoveLimitX;
        Vector2 frontVec = new Vector2(Rigid.position.x + dir, Rigid.position.y);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        // 탐지된 오브젝트가 null : 그 앞에 지형이 없음
        if (raycast.collider == null)
        {
            Rigid.velocity = Vector2.zero;
        }
        else
        {
            Rigid.velocity = new Vector2(nextMove, Rigid.velocity.y);
            MySprite.flipX = nextMove < 0;
        }
    }

    void FlyMonsterAI()
    {
        CurAttCool -= Time.deltaTime;

        if (TargetPlayerTr == null)
            TargetPlayerTr = GameObject.Find("Player").transform;

        float dist = (this.transform.position - TargetPlayerTr.position).magnitude;
        Debug.Log(dist);
        Vector3 attPos = this.transform.position;
        attPos.x += AttackDist;
        Debug.DrawRay(attPos, Vector3.down, new Color(1, 0, 0));
        if (dist < TraceDist)
        {
            if (dist < AttackDist)
            {
                if (CurAttCool <= 0)
                {
                    MySprite.flipX = this.transform.position.x > TargetPlayerTr.position.x;
                    Rigid.velocity = new Vector2(0, 0);
                    CurState = AnimState.Attack;
                    CurAttCool = unit.AttackCool;
                }
            }
            else
            {
                float traceMoveX = TargetPlayerTr.position.x - this.transform.position.x > 0 ? 3 : -3;
                float traceMoveY = TargetPlayerTr.position.y - this.transform.position.y > 0 ? 3 : -3;
                Vector2 traceMove = new Vector2(traceMoveX, traceMoveY);
                Fly(traceMove);
            }
        }
        else
        {
            if (NextMove != Vector2.zero)
                Fly(NextMove);
            else
                CurState = AnimState.Idle;
        }
    }

    void Fly(Vector2 nextMove)
    {
        if (CurState == AnimState.Attack)
            return;

        CurState = AnimState.Walk;

        // 벽 밖으로 못나가도록
        float dirX = nextMove.x > 0 ? MoveLimitX : -MoveLimitX;
        float dirY = nextMove.y > 0 ? MoveLimitY : -MoveLimitY;
        Vector2 frontVec = new Vector2(Rigid.position.x + dirX, Rigid.position.y + dirY);
        Debug.DrawRay(frontVec, Vector3.down, new Color(0, 1, 0));
        RaycastHit2D raycast = Physics2D.Raycast(frontVec, Vector3.down, 1, LayerMask.GetMask("Ground"));

        // 탐지된 오브젝트가 null : 그 앞에 지형이 없음
        //if (raycast.collider == null)
        //{
        //    Rigid.velocity = Vector2.zero;
        //}
        //else
        {
            Rigid.velocity = nextMove;
            MySprite.flipX = nextMove.x < 0;
        }
    }

    void PatrolMoveStep()
    {
        NextMove = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));

        float MoveNextTime = Random.Range(2f, 5f);
        Invoke("PatrolMoveStep", MoveNextTime);
    }
}
