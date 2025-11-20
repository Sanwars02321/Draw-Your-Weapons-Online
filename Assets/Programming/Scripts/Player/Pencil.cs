using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Photon.Pun;

public class Pencil : Weapon
{
    [SerializeField] private float spawnOffset;
    [SerializeField] private float minDistanceBetweenDrawings = 0.2f; // Distancia mínima
    [SerializeField] private int maxActiveDrawings = 50;
    
    private bool isDrawing = false;
    private Vector3 lastDrawPosition;
    private List<GameObject> activeDrawings = new List<GameObject>();

    public override void Shoot()
    {
        base.Shoot();
        isDrawing = true;
        lastDrawPosition = Vector3.positiveInfinity; // Reset
        shootActionRef.canceled += CancelDrawing;
    }

    private void CancelDrawing(InputAction.CallbackContext callback)
    {
        CancelDrawing();
    }
    
    public void CancelDrawing()
    {
        isDrawing = false;
        shootActionRef.canceled -= CancelDrawing;
    }

    public override void FixedUpdateWeapon()
    {
        base.FixedUpdateWeapon();
        
        if (isDrawing)
        {
            Vector3 spawnPosition = transform.position + 
                (Vector3)(-spawnOffset * MyMath.RotationToDirection(transform.eulerAngles.z));
            
            if (Vector3.Distance(spawnPosition, lastDrawPosition) >= minDistanceBetweenDrawings)
            {
                SpawnDrawing(spawnPosition);
                lastDrawPosition = spawnPosition;
            }
        }
    }

    private void SpawnDrawing(Vector3 position)
    {
        // Limitar cantidad de dibujos activos
        if (activeDrawings.Count >= maxActiveDrawings)
        {
            GameObject oldest = activeDrawings[0];
            activeDrawings.RemoveAt(0);
            if (oldest != null)
            {
                PUNManager.Instance.DestroyWithPhoton(oldest);
            }
        }
        
        GameObject drawing = PUNManager.Instance.InstantiateWithPhoton(
            "Drawing", 
            position, 
            Quaternion.identity
        );
        
        activeDrawings.Add(drawing);
    }
    
    private void OnDisable()
    {
        if (shootActionRef != null)
        {
            shootActionRef.canceled -= CancelDrawing;
        }
    }
}