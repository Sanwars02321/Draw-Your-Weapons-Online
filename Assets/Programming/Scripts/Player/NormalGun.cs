using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalGun : Weapon
{
    [SerializeField] private GameObject LOCALBulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public override void Shoot()
    {
        base.Shoot();
        if (CDTimer <= 0)
        {
            GameObject newBulletGO = PUNManager.Instance.InstantiateWithPhoton("Bullet", transform.position, new Quaternion());
            newBulletGO.GetComponent<Bullet>().SetOwner(photonView);
            newBulletGO.GetComponent<Bullet>().SetDirection(MyMath.RotationToDirection(transform.eulerAngles.z));
            CDTimer = Cooldown;
        }
    }
}
