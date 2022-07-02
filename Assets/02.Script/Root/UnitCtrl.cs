using UnityEngine;

public class UnitInfo
{
    public string Name = "";

    public int CurHp = 100;             // 현재 체력
    public int MaxHp = 100;             // 최대 체력

    public int CurMp = 100;             // 현재 마력
    public int MaxMp = 100;             // 최대 마력

    public float Attack = 1f;           // 공격력
    public float Critical_Per = 10f;    // 치명타 확률
    public float AttackCool = 2f;       // 공격 쿨타임

    public float Defence = 0f;          // 방어력

    public Vector2[] AttCenter;
    public Vector2[] AttSize;

    public float TraceDist = 3f;        // 추격 거리

    public void Init(UnitData data)
    {
        Name = data.Name;
        MaxHp = CurHp = data.Hp;
        MaxMp = CurMp = data.Mp;
        Attack = data.Attack;
        Critical_Per = data.Critical_Per;
        AttackCool = data.AttackCool;
        Defence = data.Defence;

        AttCenter = data.AttackCenter;
        AttSize = data.AttackSize;

        TraceDist = data.TraceDist;
    }
}

public enum AnimState
{
    Null,
    Idle,
    Walk,
    Jump,
    Attack,
    Skill,
    Hit,
    Stun,
    Die
}

public class UnitCtrl : MonoBehaviour
{
    public UnitInfo unit;
    public SpriteRenderer MySprite;

    public GameObject DmgTxt;
    private AnimState State;
    public AnimState CurState
    {
        get { return State; }
        set
        {
            if (State == value)
                return;
            if (State == AnimState.Die)
                return;

            if (State == AnimState.Attack ||
                State == AnimState.Skill)
            {
                if (value == AnimState.Null ||
                    value == AnimState.Hit ||
                    value == AnimState.Die)
                    State = value;
                else
                    return;
            }
            else
            {
                State = value;
            }
        }
    }

    public void Init(UnitInfo info)
    {
        unit = info;
    }

    public float AttackDmg()
    {
        float Dmg = unit.Attack;

        float RandomCritical = Random.Range(0f, 100f);
        if (RandomCritical <= unit.Critical_Per)
            Dmg *= 2;

        return Dmg;
    }

    public virtual void TakeDamage(Vector2 attPos, float dmg, bool isCritical, bool isStun = false)
    {
        if (unit.CurHp <= 0)
            return;

        int Dmg = (int)(dmg - unit.Defence);
        if (Dmg <= 0)
            Dmg = 1;

        Dmg = isCritical ? Dmg * 2 : Dmg;
        unit.CurHp -= Dmg;
        if (unit.CurHp < 0)
            unit.CurHp = 0;

        if (!ReferenceEquals(DmgTxt, null))
        {
            GameObject hudText = Instantiate(DmgTxt);
            float sizeY = this.GetComponentInChildren<SpriteRenderer>().size.y;
            hudText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + sizeY * 1.5f, 0f);
            hudText.GetComponent<DamageTxtCtrl>().DamageVal = Dmg;
            hudText.GetComponent<DamageTxtCtrl>().IsCritical = isCritical;
        }

        if (unit.Name != "Bringer of death")
        {
            int knockbackX = attPos.x > this.transform.position.x ? 2 : -2;
            Vector2 knockback = new Vector2(knockbackX, 2);
            this.GetComponent<Rigidbody2D>().velocity = knockback;
        }

        if (unit.CurHp <= 0)
            CurState = AnimState.Die;
        else
        {
            if (isStun)
                CurState = AnimState.Stun;
            else
                CurState = AnimState.Hit;
        }
    }
}