using UnityEngine;

public class PlayerAnimCtrl : MonoBehaviour
{
    [SerializeField] private PlayerCtrl Player;

    void Attack()
    {
        Weapon weapon = Player.WeaponCtrl;
        if (!ReferenceEquals(weapon, null))
        {
            PlayerCtrl.PlayerInfo.CurMp -= weapon.Info.Attack_NeedMp;
            weapon.Attack();
        }
    }

    void Skill()
    {
        Weapon weapon = Player.WeaponCtrl;
        if (!ReferenceEquals(weapon, null))
        {
            PlayerCtrl.PlayerInfo.CurMp -= weapon.Info.Skill_NeedMp;
            weapon.Skill();
        }
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
