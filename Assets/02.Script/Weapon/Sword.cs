public enum SwordType
{
    ShotSword,
    LongSword,
}

public class Sword : Weapon
{
    public override void Attack()
    {
        SoundManager.Inst.PlayEffSound("Sword1", 0.5f);
    }

    public override void Skill()
    {
        SoundManager.Inst.PlayEffSound("Sword2", 0.5f);
    }
}
