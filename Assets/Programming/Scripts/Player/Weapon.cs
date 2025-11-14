using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class Weapon : MonoBehaviourPun
{
    [SerializeField] protected bool hasCD;
    [SerializeField] protected float Cooldown;
    protected float CDTimer  = 0;

    protected InputAction shootActionRef;

    // Start is called before the first frame update
    void Start()
    {

    }

    public virtual void SetWeaponStart(InputAction action)
    {
        shootActionRef = action;
    }

    public virtual void Shoot()
    {

    }
    public virtual void UpdateWeapon()
    {
        //Bullet Cooldown
        if (CDTimer > 0)
        {
            CDTimer -= Time.deltaTime;
            if (CDTimer <= 0)
            {
                CDTimer = 0;
            }
        }
    }

    public virtual void FixedUpdateWeapon()
    {

    }
}
