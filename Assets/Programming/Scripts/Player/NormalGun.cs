using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : Weapon
{
    private GameObject muzzle;
    private void Start()
    {
        muzzle = transform.Find("Muzzle").gameObject;
    }

    public override void Shoot()
    {
        base.Shoot();
        if (CDTimer <= 0)
        {
            GameObject newBulletGO = PUNManager.Instance.InstantiateWithPhoton("Bullet", muzzle.transform.position, new Quaternion());
            newBulletGO.GetComponent<Bullet>().SetOwner(photonView);
            newBulletGO.GetComponent<Bullet>().SetDirection(MyMath.RotationToDirection(transform.eulerAngles.z));
            CDTimer = Cooldown;
            AudioManager.Instance.PlayMultiplayerSoundClip("Shot");
        }
    }
}
