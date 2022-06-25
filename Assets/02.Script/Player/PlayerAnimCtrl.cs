using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
    void Attack()
    {
        if (!ReferenceEquals(GameManager.Inst.PlayerCtrl.WeaponCtrl, null))
            GameManager.Inst.PlayerCtrl.WeaponCtrl.Attack();
    }

    void Skill()
    {
        if (!ReferenceEquals(GameManager.Inst.PlayerCtrl.WeaponCtrl, null))
            GameManager.Inst.PlayerCtrl.WeaponCtrl.Skill();
    }

    void AttackEnd()
    {
        GameManager.Inst.PlayerCtrl.IsAttack = false;
    }

    void SkillEnd()
    {
        GameManager.Inst.PlayerCtrl.IsSkill = false;
    }
}
