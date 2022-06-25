using UnityEngine;

public class UnitInfo
{
    public string Name = "";

    public int CurHp = 100;             // ���� ü��
    public int MaxHp = 100;             // �ִ� ü��

    public int CurMp = 100;             // ���� ����
    public int MaxMp = 100;             // �ִ� ����

    public float Attack = 1f;           // ���ݷ�
    public float Critical_Per = 10f;    // ġ��Ÿ Ȯ��
    public float AttackCool = 2f;       // ���� ��Ÿ��

    public float Defence = 0f;          // ����

    public Vector2[] AttCenter;
    public Vector2[] AttSize;

    public float TraceDist = 3f;        // �߰� �Ÿ�

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
            if (State == AnimState.Die)
                return;

            if (State == AnimState.Hit ||
                State == AnimState.Attack ||
                State == AnimState.Skill)
            {
                if (value == AnimState.Null ||
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

    public virtual void TakeDamage(float dmg, bool isCritical)
    {
        if (unit.CurHp <= 0)
            return;

        int Dmg = (int)(dmg - unit.Defence);
        if (Dmg <= 0)
            Dmg = 1;

        Dmg = isCritical ? Dmg * 2 : Dmg;
        unit.CurHp -= Dmg;

        if (!ReferenceEquals(DmgTxt, null))
        {
            GameObject hudText = Instantiate(DmgTxt);
            float sizeY = this.GetComponentInChildren<SpriteRenderer>().size.y;
            hudText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + sizeY * 1.5f, 0f);
            hudText.GetComponent<DamageTxtCtrl>().DamageVal = Dmg;
            hudText.GetComponent<DamageTxtCtrl>().IsCritical = isCritical;
        }

        if (unit.CurHp <= 0)
            CurState = AnimState.Die;
        else
            CurState = AnimState.Hit;

        if (unit.Name == "Player")
            Camera.main.GetComponent<CameraCtrl>().Hurt();
    }
}