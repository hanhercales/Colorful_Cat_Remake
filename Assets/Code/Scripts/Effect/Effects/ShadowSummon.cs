using UnityEngine;

[CreateAssetMenu(fileName = "ShadowSummon", menuName = "Effects/Shadow Summon")]
public class ShadowSummon : OvertimeEffect
{
    public GameObject shadowPrefab;
    
    private GameObject shadow;

    public override bool OnApply(GameObject target, GameObject source)
    {
        shadow = Instantiate(shadowPrefab, target.transform.position, Quaternion.identity);
        //shadow.GetComponent<ShadowController>().SetOwner(target);
        return true;
    }

    public override void OnTick(GameObject target) {}

    public override void OnRemove(GameObject target, GameObject source)
    {
        Destroy(shadow);
    }
}
