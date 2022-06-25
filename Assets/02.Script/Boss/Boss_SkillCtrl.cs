using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss_SkillCtrl : MonoBehaviour
{
    public BoxCollider2D SkillRange;
    public float Damage = 10f;
    public bool TakeDamage = false;

    public void UseSkill()
    {
        GetComponentInChildren<Animator>().SetTrigger("OneShot");
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (TakeDamage == false)
        {
            if (col.gameObject.CompareTag("Player"))
            {
                float dmg = Random.Range(Damage - 2, Damage + 2);
                bool isCritical = Random.Range(0f, 100f) <= 30f;
                col.transform.GetComponent<UnitCtrl>().TakeDamage(dmg, isCritical);
                int knockbackX = col.transform.position.x > this.transform.position.x ? 2 : -2;
                Vector2 knockback = new Vector2(knockbackX, 2);
                col.transform.GetComponent<Rigidbody2D>().velocity = knockback;

                TakeDamage = true;
            }
        }
    }

    #region --- 스킬 Anim 함수
    void AttackOn()
    {
        SkillRange.enabled = true;
        TakeDamage = false;
    }

    void AttackOff()
    {
        SkillRange.enabled = false;
    }

    void SkillEnd()
    {
        this.transform.parent.gameObject.SetActive(false);
    }
    #endregion
}
