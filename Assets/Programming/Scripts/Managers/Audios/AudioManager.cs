using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public AudioClip clip;
    public string soundID;
}

[System.Serializable]
public class Music
{
    public AudioClip clip;
    public string musicID;
}

public class AudioManager : MonoBehaviourPun
{
    public static AudioManager Instance { get; private set; }

    //[SerializeField] private Sound[] GameSounds;
    [SerializeField] private AudioSource soundAudioSource;
    [SerializeField] private Music[] GameMusic;
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] private List<SoundCategory> SoundCategories = new List<SoundCategory>();
    
    private Dictionary<string, AudioClip> IdAndClip = new Dictionary<string, AudioClip>();

    public AudioSource MusicAudioSource => musicAudioSource;


    void Awake()//NO ES UN SINGLETON XQ AL TENER PHOTON VIEW EL GO SE DESTRUYE, PONERLO EN CADA ESCENA
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        AddAudiosToDicctionary();
    }

  public void PlayLocalSoundClip(string id)//Suena solo de manera local
  {
        PlaySoundClip(id);
  }
    public void PlayMultiplayerSoundClip(string id)//Suena en todas las PCs
    {
        photonView.RPC("PlaySoundClip", RpcTarget.AllBuffered,id);
    }
    
    public void AddAudiosToDicctionary()
    {
        foreach (var Category in SoundCategories)
        {
            foreach(var sound in Category.Sounds)
            {
                IdAndClip.Add(sound.soundID.ToLower(), sound.clip);
            }
        }
    }

    public AudioClip GetSoundClipFromID(string id)
    {
       
            if(IdAndClip.ContainsKey(id.ToLower()))
            {

              return IdAndClip[id.ToLower()];

            }

         return default;
        
    }

    public void CrossfadeOut(AudioSource source, float crossfadetime)
    {
        Debug.Log("Fading out music");
        StartCoroutine(FadeItOut(source, crossfadetime));

    }

    public void CrossfadeIn(AudioSource source, float crossfadetime,float targetVolume)
    {
        Debug.Log("Fading in music");
        StartCoroutine(FadeItIn(source, crossfadetime, targetVolume));

    }

    private IEnumerator FadeItOut(AudioSource source, float time)
    {
        float timer = 0f;

        while(timer < time)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(source.volume, 0f, timer / time);
            yield return null;
        }

        source.volume = 0f;

        yield return new WaitForSeconds(time);
    }

    private IEnumerator FadeItIn(AudioSource source, float time, float targetVolume)
    {
        float timer = 0f;

        while (timer < time)
        {
            timer += Time.deltaTime;
            source.volume = Mathf.Lerp(0f, targetVolume,timer / time);
            yield return null;
        }

        source.volume = targetVolume;

        yield return new WaitForSeconds(time);
    }

    [PunRPC]
    private void PlaySoundClip(string id)
    {
        AudioClip clipToPlay = GetSoundClipFromID(id);

        if (clipToPlay == default)
        {
            return;
        }

        if (clipToPlay != null)
        {
            soundAudioSource.PlayOneShot(clipToPlay);
        }
    }

    public AudioClip GetMusicClipFromID(string id)
    {
        for (int i = 0; i < GameMusic.Length; i++)
        {
           var music = GameMusic[i];
           if (music.musicID.ToLower() == id.ToLower())
           {
              return music.clip;
           }
        }
        return default;
    }

    public void PlayMusicClip(string id)
    {
        AudioClip clipToPlay = GetMusicClipFromID(id);

        if (clipToPlay == default)
        {
            return;
        }

        if (clipToPlay != null)
        {
            musicAudioSource.clip = clipToPlay;
            musicAudioSource.Play();
        }
    }
    public void StopMusic()
    {
        musicAudioSource?.Stop();
    }
    
}

