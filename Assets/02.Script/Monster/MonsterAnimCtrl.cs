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
                RandomAtt = Random.Range(0, MonCtrl.unit.AttCenter.Length);
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
        float attCenterLR;
        if (MonCtrl.unit.Name == "Bringer of death")
            attCenterLR = MonCtrl.transform.localScale.x > 0 ? MonCtrl.unit.AttCenter[RandomAtt].x : -MonCtrl.unit.AttCenter[RandomAtt].x;
        else
            attCenterLR = MonCtrl.MySprite.flipX ? MonCtrl.unit.AttCenter[RandomAtt].x : -MonCtrl.unit.AttCenter[RandomAtt].x;

        Vector2 attCenter = new Vector2(MyTr.position.x + attCenterLR, MyTr.position.y + MonCtrl.unit.AttCenter[RandomAtt].y);
        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(attCenter, MonCtrl.unit.AttSize[RandomAtt]);
    }

    void AttackStart()
    {
        AttCoroutine = StartCoroutine(AttackCo());
    }

    IEnumerator AttackCo()
    {
        if (RandomAtt != -1)
        {
            bool takeDamage = false;
            while (takeDamage == false)
            {
                float attCenterLR;
                if (MonCtrl.unit.Name == "Bringer of death")
                    attCenterLR = MonCtrl.transform.localScale.x > 0 ? MonCtrl.unit.AttCenter[RandomAtt].x : -MonCtrl.unit.AttCenter[RandomAtt].x;
                else
                    attCenterLR = MonCtrl.MySprite.flipX ? MonCtrl.unit.AttCenter[RandomAtt].x : -MonCtrl.unit.AttCenter[RandomAtt].x;

                Vector2 attCenter = new Vector2(MyTr.position.x + attCenterLR, MyTr.position.y + MonCtrl.unit.AttCenter[RandomAtt].y);
                Collider2D[] arrCol2D = Physics2D.OverlapBoxAll(attCenter, MonCtrl.unit.AttSize[RandomAtt], 0);
                foreach (Collider2D col in arrCol2D)
                {
                    if (col.gameObject.CompareTag("Player"))
                    {
                        float dmg = Random.Range(MonCtrl.unit.Attack - 1, MonCtrl.unit.Attack + 2);
                        bool isCritical = Random.Range(0f, 100f) <= MonCtrl.unit.Critical_Per;
                        col.GetComponentInParent<UnitCtrl>().TakeDamage(this.transform.position, dmg, isCritical);
                        
                        takeDamage = true;
                        break;
                    }
                }
                yield return null;
            }
        }
    }

    void ActionAnimEnd()
    {
        if (!ReferenceEquals(AttCoroutine, null))
            StopCoroutine(AttCoroutine);

        Anim.SetInteger("Attack", -1);
        MonCtrl.CurState = AnimState.Null;
    }
}
