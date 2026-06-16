using UnityEngine;

[CreateAssetMenu(fileName = "NewFormSkill", menuName = "Ability/FormSkill")]
public class FormSkill : ActiveAbility
{
    public bool canDestroyProjectile;
    public bool isRangedAttack;
    public GameObject projectile;

    public override void Activate(GameObject source, GameObject target)
    {
        if (isRangedAttack && projectile != null)
        {
            GameObject prjClone = SimpleObjectPool.Instance.GetFromPool(projectile);
            prjClone.transform.position = source.transform.position;

            if (prjClone.TryGetComponent(out Projectile prj))
            {
                Vector2 shootDirection = source.transform.localScale.x > 0 ? Vector2.right : Vector2.left;
                prj.Initialize(source, shootDirection, effectsToApply);
            }
        }
        else
        {
            base.Activate(source, target);
        }
    }
}
