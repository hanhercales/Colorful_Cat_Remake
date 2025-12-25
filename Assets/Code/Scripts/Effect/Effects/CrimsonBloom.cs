using UnityEngine;

[CreateAssetMenu(fileName = "CrimsonBloom", menuName = "Effects/Crimson Bloom")]
public class CrimsonBloom : OvertimeEffect
{
    public float damage;
    public float healAmount;
    public GameObject explosionVFX;
    public GameObject healVFX;

    public override bool OnApply(GameObject target, GameObject source)
    {
        //Gán ấn
        return  true;
    }

    public override void OnTick(GameObject target)
    {
        //Dmg Flashing
    }

    public override void OnRemove(GameObject target, GameObject source)
    {
        //Nổ dmg & heal
        if (target.TryGetComponent(out Stats targetStats))
        {
            targetStats.TakeDamage(damage);
            if(explosionVFX != null) 
                Instantiate(explosionVFX, target.transform.position, Quaternion.identity);
        }
        if (source.TryGetComponent(out Stats sourceStats))
        {
            sourceStats.Heal(healAmount);
            if(healVFX != null)
                Instantiate(healVFX, source.transform.position, Quaternion.identity);
        }
    }
}
