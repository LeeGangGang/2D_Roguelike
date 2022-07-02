using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
    public PlayerCtrl Player;

    void Attack()
    {
        Weapon weapon = Player.WeaponCtrl;
        if (!ReferenceEquals(weapon, null))
            weapon.Attack();
    }

    void Skill()
    {
        Weapon weapon = Player.WeaponCtrl;
        if (!ReferenceEquals(weapon, null))
            weapon.Skill();
    }

    void AttackEnd()
    {
        Player.IsAttack = false;
    }

    void SkillEnd()
    {
        Player.IsSkill = false;
    }
}
