using UnityEngine;

public class Boss_SkillCtrl : MonoBehaviour
{
    [SerializeField] private BoxCollider2D SkillRange;
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
                bool isCritical = Random.Range(0, 101) >= 30;
                col.GetComponentInParent<UnitCtrl>().TakeDamage(this.transform.position, dmg, isCritical, true);
                TakeDamage = true;
            }
        }
    }

    #region --- ??ų Anim ?Լ?
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
