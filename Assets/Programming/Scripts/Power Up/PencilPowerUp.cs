using Photon.Pun;
using UnityEngine;

public class PencilPowerUp : PowerUp
{

    protected override void Update()
    {
        base.Update();
    }

    protected override void RPCDispatcher(PhotonView hitView)
    {
        hitView.RPC("RPC_PencilEffect", RpcTarget.All, lifeSpan);
    }

}