using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
    PlayerCtrl PlayerCtrl;

    // Start is called before the first frame update
    void Start()
    {
        PlayerCtrl = this.GetComponentInParent<PlayerCtrl>();
    }

    void Attack()
    {
        if (PlayerCtrl.WeaponCtrl != null)
            PlayerCtrl.WeaponCtrl.Attack();
    }

    void Skill()
    {
        if (PlayerCtrl.WeaponCtrl != null)
            PlayerCtrl.WeaponCtrl.Skill();
    }

    void SkillEnd()
    {
        PlayerCtrl.IsSkill = false;
    }
}
