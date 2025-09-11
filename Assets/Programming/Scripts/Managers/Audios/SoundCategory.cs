using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewCategory", menuName = "Scriptable Objects/SoundCategory")]
public class SoundCategory : ScriptableObject
{
    public List<Sound> Sounds = new List<Sound>();
   

}
