using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbstractFactory : MonoBehaviour
{

    [SerializeField] Factory factoryToUse;
   
    public void ChangeFactoryToUse(Factory newFactory)
    {  
        factoryToUse = newFactory;     
    }

    public GameObject CreateObjectInAbstractFactory(string prefabId)
    {
        return factoryToUse.CreateGameObject(prefabId);
    }
  
}
