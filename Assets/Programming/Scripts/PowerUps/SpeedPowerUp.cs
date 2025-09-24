using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{

    public void ApplyEffect(PlayerController playerController)
    {
        playerController.SetMovementSpeed(10f);
    }


    public void DeactivateEffect(PlayerController playerController)
    {
        playerController.SetMovementSpeed(playerController.initialMoveSpeed);
    }
}
