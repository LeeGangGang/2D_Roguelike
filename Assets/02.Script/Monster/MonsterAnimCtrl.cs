using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class MonsterAnimCtrl : MonoBehaviour
{
    private string AnimTrigger = "AnimState";
    private Animator Anim;

    private UnitCtrl MonCtrl;
    public AnimState BeforeState;

    Coroutine AttCoroutine;
    private int RandomAtt = -1;
    private Transform MyTr;
    public Vector2[] AttCenter;
    public Vector2[] AttRect;

    // Start is called before the first frame update
    void Start()
    {
        MyTr = this.transform;
        Anim = this.GetComponent<Animator>();
        MonCtrl = this.GetComponentInParent<UnitCtrl>();
    }

    // Update is called once per frame
    void Update()
    {
        if (BeforeState == MonCtrl.CurState)
            return;

        BeforeState = MonCtrl.CurState;
        switch (MonCtrl.CurState)
        {
            case AnimState.Idle:
                Anim.SetInteger(AnimTrigger, 0);
                break;
            case AnimState.Walk:
                Anim.SetInteger(AnimTrigger, 1);
                break;
            case AnimState.Attack:
                RandomAtt = Random.Range(0, AttRect.Length);
                Anim.SetInteger("Attack", RandomAtt);
                break;
            case AnimState.Skill:
                Anim.SetTrigger("Skill");
                break;
            case AnimState.Hit:
                Anim.SetTrigger("Hit");
                break;
            case AnimState.Die:
                Anim.SetBool("Die", true);
                break;
        }
    }

    void OnDrawGizmos()
    {
        if (RandomAtt < 0)
            return;

        float AttCenterLR = MonCtrl.MySprite.flipX ? AttCenter[RandomAtt].x : -AttCenter[RandomAtt].x;
        Vector2 attCenter = new Vector2(MyTr.position.x + AttCenterLR, MyTr.position.y + AttCenter[RandomAtt].y);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attCenter, AttRect[RandomAtt]);
    }

    void AttackStart()
    {
        AttCoroutine = StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        if (RandomAtt != -1)
        {
            bool isAttack = false;
            while (isAttack == false)
            {
                float attCenterLR = MonCtrl.MySprite.flipX ? AttCenter[RandomAtt].x : -AttCenter[RandomAtt].x;
                Vector2 attCenter = new Vector2(MyTr.position.x + attCenterLR, MyTr.position.y + AttCenter[RandomAtt].y);
                Collider2D[] arrCol2D = Physics2D.OverlapBoxAll(attCenter, AttRect[RandomAtt], 0);
                foreach (Collider2D col in arrCol2D)
                {
                    if (col.gameObject.CompareTag("Player"))
                    {
                        float dmg = Random.Range(MonCtrl.unit.Attack - 1, MonCtrl.unit.Attack + 2);
                        bool isCritical = Random.Range(0f, 100f) <= MonCtrl.unit.Critical_Per;
                        col.transform.GetComponent<UnitCtrl>().TakeDamage(dmg, isCritical);

                        int knockbackX = col.transform.position.x > this.transform.position.x ? 2 : -2;
                        Vector2 knockback = new Vector2(knockbackX, 2);
                        col.transform.GetComponent<Rigidbody2D>().velocity = knockback;
                        
                        isAttack = true;
                        break;
                    }
                }
                yield return null;
            }
        }
    }

    void ActionAnimEnd()
    {
        if (AttCoroutine != null)
            StopCoroutine(AttCoroutine);

        Anim.SetInteger("Attack", -1);
        MonCtrl.CurState = AnimState.Idle;
    }
}
