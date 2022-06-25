using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkeletonCtrl : MonsterCtrl
{
    private float Shield_Per = 10f;

    public override void TakeDamage(float dmg, bool isCritical)
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

        base.TakeDamage(dmg, isCritical);
    }
}
