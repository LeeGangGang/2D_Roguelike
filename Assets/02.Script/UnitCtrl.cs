using UnityEngine;

public class UnitInfo
{
    public int CurHp = 100;            // ���� ü��
    public int MaxHp = 100;            // �ִ� ü��

    public int CurMp = 100;            // ���� ����
    public int MaxMp = 100;            // �ִ� ����

    public float Attack = 1f;          // ���ݷ�
    public float Critical_Per = 10f;   // ġ��Ÿ Ȯ��
    public float AttackCool = 2f;    // ���� ��Ÿ��

    public float Defence = 0f;         // ����
}

public enum AnimState
{
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

            State = value;
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

    public void TakeDamage(float dmg, bool isCritical)
    {
        if (unit.CurHp <= 0)
            return;

        int Dmg = (int)(dmg - unit.Defence);
        if (Dmg <= 0)
            Dmg = 1;

        Dmg = isCritical ? Dmg * 2 : Dmg;
        unit.CurHp -= Dmg;

        if (DmgTxt != null)
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
    }
}