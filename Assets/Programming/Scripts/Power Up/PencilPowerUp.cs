using Photon.Pun;
using UnityEngine;

public class PencilPowerUp : PowerUp
{
    [SerializeField] private float lifeSpan = 3f;


    protected override void Update()
    {
        base.Update();
    }

    protected override void RPCDispatcher(PhotonView hitView)
    {
        hitView.RPC("RPC_PencilPowerUp", RpcTarget.All, lifeSpan);
    }

}