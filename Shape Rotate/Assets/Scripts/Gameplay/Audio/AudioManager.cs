using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public string startMusic;

    [Range(0, 1)] public float sfxMasterVolume;
    [Range(0, 1)] public float musicMasterVolume;

    public static AudioManager instance;

    public List<Music> musicList;
    public List<SoundEffect> sfxList;

    private AudioSource musicSource;
    private AudioSource musicSource2;

    private Music music1;
    private Music music2;

    [SerializeField] private bool firstMusicSourceIsPlaying;

    //public bool isMusicPlaying = false;
    private bool isGameFocused = true;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (SoundEffect s in sfxList)
        {
            s.source = gameObject.AddComponent<AudioSource>();
            s.source.clip = s.clip;
            s.source.volume = s.volume * sfxMasterVolume;
            s.source.pitch = s.GetRandomPitch();
            s.source.loop = s.loop;
        }

        // Create music audio sources
        musicSource = gameObject.AddComponent<AudioSource>();
        musicSource2 = gameObject.AddComponent<AudioSource>();

        // Loop the music tracks
        musicSource.loop = true;
        musicSource2.loop = true;

        // Set ignore listener pause for music source
        musicSource.ignoreListenerPause = true;
        musicSource2.ignoreListenerPause = true;

        firstMusicSourceIsPlaying = true;
        isGameFocused = true;

        PlayMusicWithFade(startMusic, 0.5f);
    }

    private void OnApplicationFocus(bool focus)
    {
        isGameFocused = focus;
    }

    public void PlaySFX(string _name)
    {
        SoundEffect s = FindSFX(_name);

        if (s == null) { Debug.LogWarning("Sound Effect named '" + _name + "' not found!"); return; }

        s.source.volume = IsSFXMuted() ? 0 : s.volume * sfxMasterVolume;
        s.source.pitch = s.GetRandomPitch();

        if (s.loop)
        {
            s.source.clip = s.clip;
            s.source.Play();
        }
        else
            s.source.PlayOneShot(s.clip, s.volume);
    }
    public void StopSFX(string _name)
    {
        SoundEffect s = FindSFX(_name);

        if (s == null) { Debug.LogWarning("Sound Effect named '" + _name + "' not found!"); return; }

        s.source.Stop();
    }
    public void PlayMusic(string _name)
    {
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;

        Music m = FindMusic(_name);

        if (m == null) { Debug.LogWarning("Music named '" + _name + "' not found!"); return; }

        activeSource.volume = IsMusicMuted() ? 0 : musicMasterVolume;
        StartCoroutine(ChangeMusic(activeSource, m));
    }
    public void PlayMusicWithFade(string _musicName, float _transitionTime = 1.0f)
    {
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;

        Music m = FindMusic(_musicName);

        if (m == null) { Debug.LogWarning("Music named '" + _musicName + "' not found!"); return; }

        StartCoroutine(UpdateMusicWithFade(activeSource, m, _transitionTime));
    }
    public void PlayMusicWithCrossFade(string _musicName, float _transitionTime = 1.0f)
    {
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
        AudioSource newSource = (firstMusicSourceIsPlaying) ? musicSource2 : musicSource;

        firstMusicSourceIsPlaying = !firstMusicSourceIsPlaying;

        Music m = FindMusic(_musicName);

        if (m == null) { Debug.LogWarning("Music named '" + _musicName + "' not found!"); return; }

        StartCoroutine(ChangeMusic(newSource, m));

        StartCoroutine(UpdateMusicWithCrossFade(activeSource, newSource, _transitionTime));
    }

    public void StopMusicWithFade(float _transitionTime = 1)
    {
        AudioSource activeSource = (firstMusicSourceIsPlaying) ? musicSource : musicSource2;
        Music currentMusic = (firstMusicSourceIsPlaying) ? music1 : music2;

        StartCoroutine(FadeOutMusic(activeSource, _transitionTime, currentMusic));
    }

    public void SetSFXVolume(float _volume)
    {
        sfxMasterVolume = _volume;

        for (int i = 0; i < sfxList.Count; i++)
        {
            sfxList[i].source.volume = sfxList[i].volume * sfxMasterVolume;
        }

        PlayerPrefs.SetFloat("sfxMasterVolume", sfxMasterVolume);
    }
    public void SetMusicVolume(float _volume)
    {
        musicMasterVolume = _volume;

        musicSource.volume = musicMasterVolume;
        musicSource2.volume = musicMasterVolume;

        PlayerPrefs.SetFloat("musicMasterVolume", musicMasterVolume);
    }
    public void SetSFXMute(bool _b)
    {
        if (_b)
        {
            for (int i = 0; i < sfxList.Count; i++)
            {
                sfxList[i].source.volume = 0;
            }
        }
        else
        {
            for (int i = 0; i < sfxList.Count; i++)
            {
                sfxList[i].source.volume = sfxList[i].volume * sfxMasterVolume;
            }
        }

        PlayerPrefs.SetInt("SFXMuted", _b ? 1 : 0);
    }
    public void SetMusicMute(bool _b)
    {
        if (_b)
        {
            musicSource.volume = 0;
            musicSource2.volume = 0;
        }
        else
        {
            musicSource.volume = musicMasterVolume;
            musicSource2.volume = musicMasterVolume;
        }

        PlayerPrefs.SetInt("MusicMuted", _b ? 1 : 0);
    }
    public bool IsSFXMuted()
    {
        return PlayerPrefs.GetInt("SFXMuted") == 1 ? true : false;
    }
    public bool IsMusicMuted()
    {
        return PlayerPrefs.GetInt("MusicMuted") == 1 ? true : false;
    }
    public float GetMusicVolume()
    {
        musicMasterVolume = PlayerPrefs.GetFloat("musicMasterVolume", musicMasterVolume);
        return musicMasterVolume;
    }
    public float GetSoundVolume()
    {
        sfxMasterVolume = PlayerPrefs.GetFloat("sfxMasterVolume", sfxMasterVolume);
        return sfxMasterVolume;
    }

    private IEnumerator ChangeMusic(AudioSource _activeSource, Music _music)
    {
        if (_music != null)
        {
            //isMusicPlaying = true;
            _music.isPlaying = true;

            if (firstMusicSourceIsPlaying)
                music1 = _music;
            else
                music2 = _music;

            if (_music.introClip)
            {
                _activeSource.loop = false;
                _activeSource.clip = _music.introClip;
                _activeSource.Play();

                yield return new WaitWhile(() => IsAudioSourcePlaying(_activeSource));
            }

            if (IsActiveMusicSource(_activeSource) && _music.isPlaying)
            {
                _activeSource.loop = true;
                _activeSource.clip = _music.clip;
                _activeSource.Play();
            }
        }
    }

    private IEnumerator UpdateMusicWithFade(AudioSource _activeSource, Music _newMusic, float _transitionTime)
    {
        float currentTime;

        if (!IsMusicMuted() && IsAudioSourcePlaying(_activeSource))
        {
            currentTime = 0;
            float startVolume = _activeSource.volume;

            while (currentTime < _transitionTime)
            {
                currentTime += Time.unscaledDeltaTime;
                _activeSource.volume = Mathf.Lerp(startVolume, 0, currentTime / _transitionTime);
                yield return null;
            }
            _activeSource.volume = 0;
        }

        _activeSource.Stop();
        StartCoroutine(ChangeMusic(_activeSource, _newMusic));

        if (!IsMusicMuted())
        {
            currentTime = 0;

            while (currentTime < _transitionTime)
            {
                currentTime += Time.unscaledDeltaTime;
                _activeSource.volume = Mathf.Lerp(0, musicMasterVolume, currentTime / _transitionTime);
                yield return null;
            }
            _activeSource.volume = musicMasterVolume;
        }
        else
            _activeSource.volume = 0;
    }
    private IEnumerator UpdateMusicWithCrossFade(AudioSource _originalSource, AudioSource _newSource, float _transitionTime)
    {
        if (!IsMusicMuted())
        {
            float currentTime = 0;
            float startVolumeOriginal = _originalSource.volume;

            while (currentTime < _transitionTime)
            {
                currentTime += Time.unscaledDeltaTime;
                _originalSource.volume = Mathf.Lerp(startVolumeOriginal, 0, currentTime / _transitionTime);
                _newSource.volume = Mathf.Lerp(0, musicMasterVolume, currentTime / _transitionTime);
                yield return null;
            }
        }
        else
        {
            _newSource.volume = 0;
            _originalSource.volume = 0;
        }

        _originalSource.Stop();
    }
    private IEnumerator FadeOutMusic(AudioSource _activeSource, float _transitionTime, Music _currentMusic)
    {
        if (!IsMusicMuted())
        {
            float currentTime = 0;
            float startVolume = _activeSource.volume;

            while (currentTime < _transitionTime)
            {
                currentTime += Time.unscaledDeltaTime;
                _activeSource.volume = Mathf.Lerp(startVolume, 0, currentTime / _transitionTime);
                yield return null;
            }
        }

        //isMusicPlaying = false;
        _currentMusic.isPlaying = false;
        _activeSource.Stop();
    }

    private Music FindMusic(string _name)
    {
        return musicList.Find(sound => sound.name == _name);
        //return Array.Find(musicList, sound => sound.name == _name);
    }
    private SoundEffect FindSFX(string _name)
    {
        return sfxList.Find(sound => sound.name == _name);
    }

    private bool IsActiveMusicSource(AudioSource _audioSource)
    {
        return (firstMusicSourceIsPlaying && _audioSource == musicSource) || (!firstMusicSourceIsPlaying && _audioSource == musicSource2);
    }

    private bool IsAudioSourcePlaying(AudioSource _audioSource)
    {
        return !_audioSource.isPlaying && isGameFocused ? false : true;
    }
}
