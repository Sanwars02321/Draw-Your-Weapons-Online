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

public class AudioManager : MonoBehaviour, IShuffle
{
    public static AudioManager Instance { get; private set; }

    //[SerializeField] private Sound[] GameSounds;
    [SerializeField] private AudioSource soundAudioSource;
    [SerializeField] private Music[] GameMusic;
    [SerializeField] private AudioSource musicAudioSource;

    [SerializeField] private List<SoundCategory> SoundCategories = new List<SoundCategory>();
    
    private Dictionary<string, AudioClip> IdAndClip = new Dictionary<string, AudioClip>();


    private float normalPitch = 1.0f;
    public AudioSource MusicAudioSource => musicAudioSource;

    [SerializeField] private List<ShuffleElement> shufflesList = new List<ShuffleElement>();
    public List<ShuffleElement> ShuffleData { get => shufflesList; set => shufflesList = value; }

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);

        //AddAudiosToDicctionary();

        //foreach(var shuffle in shufflesList)
        //{
        //    shuffle.Awake();
        //}
    }

    void Start()
    {
        AddAudiosToDicctionary();
    }

    //public AudioClip GetSoundClipFromID(string id)
    //{
    //    for (int i = 0; i < GameSounds.Length; i++)
    //    {
    //        var sound = GameSounds[i];
    //        if (sound.soundID.ToLower() == id.ToLower())
    //        {
    //            return sound.clip;
    //        }
    //    }
    //    return default;
    //}
    
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


    public void PlaySoundClip(string id)
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

    public string RandomSound(List<string> sounds)
    {
        var validSounds = sounds.Where(s => !string.IsNullOrEmpty(s)).ToList();

        if (validSounds.Count == 0)
        {
            Debug.LogError("No hay sonidos válidos para sortear.");
            return null;
        }

        int index = Random.Range(0, validSounds.Count);
        //Debug.Log($"[SHUFFLE] Elegido índice {index} de {validSounds.Count}");
        return validSounds[index];
    }

    public void PlayAudioOnSource(string audioName, AudioSource Target)
    {
            //Debug.Log("Nombre del audio a ejecutar en sorce: " + audioName);
            Target.clip = AudioManager.Instance.GetSoundClipFromID(audioName);
            Target.PlayOneShot(Target.clip);
    }

    public ShuffleElement GetShuffleById(string id)
    {
        foreach (var shuffle in shufflesList)
        {
            if (shuffle.name.ToLower() == id.ToLower())
            {
                return shuffle;
            }
        }
        Debug.LogError("NO SE ENCONTRÓ EL SHUFFLE BUSCADO. REVISE SI EL NOMBRE ES CORRECTO O SI EL SHUFFLE EN CUESTIÓN ESTÁ AGREGADO A LA LISTA DE SHUFFLES Y VUELVA A INTENTARLO.");
        return null;
    }

    public void ChangePitchOnSorce(float pitch, AudioSource sorce)
    {
        sorce.pitch = pitch;
    }

    public float RandomPitch(float Min, float Max)
    {
        float result = Random.Range(Min, Max);
        //Debug.Log(result);
        return result;
    }

    public void ChangePitch(float pitch)
    {
        soundAudioSource.pitch = pitch;
    }

    public void SetNormalPitch()
    {
        soundAudioSource.pitch = normalPitch;
    }

    
}

