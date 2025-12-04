using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaySoundOnClick : MonoBehaviour
{
    [SerializeField] private string id;

    public void PlaySound()
    {
        AudioManager.Instance.PlayLocalSoundClip(id);
    }
}
