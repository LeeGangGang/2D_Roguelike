using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bose_SkillCtrl : MonoBehaviour
{
    public BoxCollider2D SkillRange;

    public void UseSkill()
    {
        GetComponentInChildren<Animator>().SetTrigger("OneShot");
    }

    #region --- 스킬 Anim 함수
    void AttackOn()
    {
        SkillRange.enabled = true;
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
