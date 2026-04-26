using UnityEngine;

[CreateAssetMenu(fileName = "NewMechanic", menuName = "Ability/UnlockMechanic")]
public class UnlockMechanic : PassiveAbility
{
    public enum MechanicType
    {
        DoubleJump,
        JumpAttack,
        JumpShoot,
        Dash,
        ClimbLadder,
        DoubleStrike,
        SpecialAttack,
        Pull,
        Push
    }
    
    public MechanicType mechanicType;

    public override void OnEquip(GameObject player)
    {
        if (player.TryGetComponent(out PlayerMovement controller))
            SetMechanic(controller, true);
    }

    public override void OnUnequip(GameObject player)
    {
        if (player.TryGetComponent(out PlayerMovement controller))
            SetMechanic(controller, false);
    }

    private void SetMechanic(PlayerMovement controller, bool value)
    {
        switch (mechanicType)
        {
            case MechanicType.DoubleJump: 
                controller.canDoubleJump = value; 
                break;
            case MechanicType.JumpAttack: 
                controller.canJumpAttack = value; 
                break;
            case MechanicType.Dash: 
                controller.canDash = value; 
                break;
            case MechanicType.JumpShoot:
                controller.canJumpShoot = value;
                break;
            case MechanicType.ClimbLadder: 
                controller.canClimbLadder = value; 
                break;
            case MechanicType.DoubleStrike: 
                controller.canDoubleStrike = value; 
                break;
            case MechanicType.SpecialAttack: 
                controller.canSpecialAttack = value; 
                break;
            case MechanicType.Pull:
                controller.canPull = value;
                break;
            case MechanicType.Push:
                controller.canPush = value;
                break;
        }
    }
}
