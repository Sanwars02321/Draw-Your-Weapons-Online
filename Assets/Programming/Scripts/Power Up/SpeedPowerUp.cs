using Photon.Pun;
using UnityEngine;

public class SpeedPowerUp : PowerUp
{


    protected override void Update()
    {
        base.Update();
    }

    protected override void RPCDispatcher(PhotonView hitView)
    {
        hitView.RPC("RPC_ApplyEffect", RpcTarget.All, lifeSpan);
    }

}