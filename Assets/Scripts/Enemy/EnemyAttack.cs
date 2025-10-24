using UnityEngine;

public enum AttackType
{
    Straight,
    Cross,
    FireBomb,
    Ice,
    StraightProjectile
}

[CreateAssetMenu(fileName = "Attack", menuName = "ScriptableObjects/Enemy")]
public class EnemyAttack : ScriptableObject
{
    public int damage;
    public float attackRange;
    public AttackType attackType;
}
