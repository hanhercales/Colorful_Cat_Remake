using UnityEngine;

[CreateAssetMenu(fileName = "NewFormSkill", menuName = "Ability/FormSkill")]
public class FormSkill : ActiveAbility
{
    public bool canDestroyProjectile;
    public bool isRangedAttack;
    public GameObject projectile;
}
