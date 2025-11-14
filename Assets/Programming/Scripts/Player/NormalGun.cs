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
            GameObject newBulletGO = Instantiate(LOCALBulletPrefab, transform.position, new Quaternion(0, 0, 0, 0));
            //newBulletGO.GetComponent<Bullet>().SetOwner(photonView.Owner);
            newBulletGO.GetComponent<LOCALBullet>().SetDirection(MyMath.RotationToDirection(transform.eulerAngles.z));
            CDTimer = Cooldown;
        }
    }
}
