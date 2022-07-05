using UnityEngine;

[CreateAssetMenu(fileName = "Monster Data", menuName = "Scriptable Object/Monster Data", order = int.MaxValue)]
public class UnitData : ScriptableObject
{
    [SerializeField]
    public string Name;
    public int Hp;
    public int Mp;
    public float Attack;
    public float Critical_Per;
    public float AttackCool;
    public float Defence;

    public Vector2[] AttackCenter;
    public Vector2[] AttackSize;

    public float TraceDist;
}
