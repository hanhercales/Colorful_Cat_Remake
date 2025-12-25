using UnityEngine;

[CreateAssetMenu(fileName = "DmgOT", menuName = "Effects/Dmg OT")]
public class DamageOvertime : OvertimeEffect
{
    public float damage;

    public override bool OnApply(GameObject target, GameObject source)
    {
        return true;
    }

    public override void OnTick(GameObject target)
    {
        if(target.TryGetComponent(out Stats stats))
        {
            stats.TakeDamage(damage);
            //Dmg Flashing
        }
    }

    public override void OnRemove(GameObject target, GameObject source) {}
}
