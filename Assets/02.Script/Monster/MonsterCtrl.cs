using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonsterCtrl : UnitCtrl
{
    public UnitData UnitData;
    private Transform TargetPlayerTr;

    private float CurAttCool;

    public bool IsFly = false;
    private Vector2 NextMove;
    private float MoveLimitX, MoveLimitY;

    private Rigidbody2D Rigid;

    public Image CurHpImg;

    void Awake()
    {
        UnitInfo MonInfo = new UnitInfo();
        MonInfo.Init(UnitData);

        TargetPlayerTr = GameObject.Find("Player").transform;

        CurAttCool = MonInfo.AttackCool;

        Rigid = this.GetComponent<Rigidbody2D>();

        Collider2D bodyCol = this.GetComponent<Collider2D>();
        MoveLimitX = bodyCol.bounds.size.x / 2f + bodyCol.offset.x;
        MoveLimitY = bodyCol.bounds.size.y / 2f + bodyCol.offset.y;

        PatrolMoveStep();

        base.Init(MonInfo);
    }

    void Update()
    {
        Update_HpBar();

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

        if (ReferenceEquals(TargetPlayerTr, null))
            TargetPlayerTr = GameObject.Find("Player").transform;

        float distXY = (this.transform.position - TargetPlayerTr.position).magnitude;
        if (distXY < unit.TraceDist)
        {
            float attDistX = Mathf.Abs(unit.AttSize[0].x / 2f - unit.AttCenter[0].x);
            float distX = Mathf.Abs(this.transform.position.x - TargetPlayerTr.position.x);
            if (distX < attDistX)
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
        if (ReferenceEquals(raycast.collider, null))
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

        float distXY = (this.transform.position - TargetPlayerTr.position).magnitude;
        if (distXY < unit.TraceDist)
        {
            float attDistX = Mathf.Abs(unit.AttSize[0].x / 2f - unit.AttCenter[0].x);
            float distX = Mathf.Abs(this.transform.position.x - TargetPlayerTr.position.x);
            if (distX < attDistX)
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
                float traceMoveY = TargetPlayerTr.position.y + 1f - this.transform.position.y > 0 ? 3 : -3;
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

        float dirX = nextMove.x > 0 ? MoveLimitX : -MoveLimitX;
        float dirY = nextMove.y > 0 ? MoveLimitY : -MoveLimitY;

        Rigid.velocity = nextMove;
        MySprite.flipX = nextMove.x < 0;
    }

    void PatrolMoveStep()
    {
        NextMove = new Vector2(Random.Range(-1, 2), Random.Range(-1, 2));

        float MoveNextTime = Random.Range(2f, 5f);
        Invoke("PatrolMoveStep", MoveNextTime);
    }

    void Update_HpBar()
    {
        if (unit.CurHp <= 0)
            CurHpImg.gameObject.SetActive(false);
        else
            CurHpImg.fillAmount = (float)unit.CurHp / unit.MaxHp;
    }
}
