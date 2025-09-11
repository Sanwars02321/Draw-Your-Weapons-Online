using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IdForFactory : MonoBehaviour ,IFactoryzable
{
    [Header("ID for Factory, this Component should be in the parent Game Object")]
    [SerializeField] private string prefabID;
    public string PrefabID => prefabID;
}
