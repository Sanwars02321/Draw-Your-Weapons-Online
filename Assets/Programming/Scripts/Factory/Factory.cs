using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Factory : MonoBehaviour
{
    [SerializeField] private List<GameObject> prefabs = new List<GameObject>();
    private Dictionary<string , GameObject> prefabDictonary = new Dictionary<string , GameObject>();
    [HideInInspector] public UnityEvent OnObjectCreated = new UnityEvent();

    public List<GameObject> Prefabs => prefabs;

    private void Awake()
    {
        foreach (var prefab in prefabs)
        {
            prefabDictonary.Add(prefab.GetComponent<IFactoryzable>().PrefabID, prefab);
        }
    }

    public GameObject CreateGameObject(string id)
    {
        if(prefabDictonary.ContainsKey(id))
        {
            OnObjectCreated.Invoke();
            return Instantiate(prefabDictonary[id]);
        }
        return null;
    }
}
