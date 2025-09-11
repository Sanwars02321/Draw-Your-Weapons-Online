using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Shuffle", menuName = "Scriptable Objects/Shuffle")]
public class ShuffleElement : ScriptableObject
{
    public List<string> Audios = new List<string>();

    public string CurrentAudio;

    public string PreviousAudio;

    public float MinPitch, MaxPitch;

    public bool ChangesPitch;
    public void Awake()
    {
        if (!Audios.Contains(CurrentAudio))
        {
            Audios.Add(CurrentAudio);
        }
        if (!Audios.Contains(PreviousAudio))
        {
            Audios.Add(PreviousAudio);
        }
        CurrentAudio = null;
        PreviousAudio = null;
    }
    public void SelectedAudioName()
    {
        if (PreviousAudio != null)
        {
            Audios.Add(PreviousAudio);
        }
        if (CurrentAudio != null)
        {
            PreviousAudio = CurrentAudio;
            Audios.Remove(PreviousAudio);
        }
        //if (string.IsNullOrEmpty(CurrentAudio))
        //{
        //    SelectedAudioName();
        //    return;
        //}
        CurrentAudio = AudioManager.Instance.RandomSound(Audios);

       
        
    }

    public string GenerateAndGetAudio()
    {
        SelectedAudioName();
        return CurrentAudio;
    }
    
}
