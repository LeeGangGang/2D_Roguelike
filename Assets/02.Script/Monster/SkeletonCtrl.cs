using UnityEngine;

public class SkeletonCtrl : MonsterCtrl
{
    private float Shield_Per = 10f;

    public override void TakeDamage(Vector2 attPos, float dmg, bool isCritical, bool isStun)
    {
        if (unit.CurMp >= 10)
        {
            float RandShield_Per = Random.Range(0f, 100f);
            if (Shield_Per >= RandShield_Per)
            {
                unit.CurMp -= 10;
                CurState = AnimState.Skill;
                return;
            }
        }

        base.TakeDamage(attPos, dmg, isCritical);
    }
}
