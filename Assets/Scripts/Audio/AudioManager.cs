using System;
using UnityEngine;
using UnityEngine.Audio;

//Script Made By Daniel Alvarado
namespace StaminaSystem
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private Sound[] music,fxSounds;
        [Header("Low Pass Filter Settings")]
        [SerializeField] private float maxCutoffFrequency = 5000;
        [SerializeField] private float minCutoffFrequency = 230;
        [Space(10)]
        [SerializeField] private float maxResonanceQ = 1.75f;
        [SerializeField] private float minResonanceQ = 1f;
        public AudioSource musicSource;
        [SerializeField] private AudioSource sfxSource;
        [SerializeField] private AudioMixerGroup sfxMixerGroup;
        [SerializeField] private AudioMixerGroup musicMixerGroup;
        private bool isLoseSoundPlaying = false;
        private Sound currentMusic;

        public static AudioManager Instance { get; private set; }
        void Awake()
        {
            if (Instance == null)
                Instance = this;
            else
            {
                {
                    Destroy(gameObject);
                    return;
                }
            }

            DontDestroyOnLoad(gameObject);
            foreach (Sound s in music)
            {
                s.source = musicSource;
                s.source = gameObject.AddComponent<AudioSource>();
                s.AudioLowPassFilter = s.source.gameObject.AddComponent<AudioLowPassFilter>();
                s.AudioLowPassFilter = s.source.GetComponent<AudioLowPassFilter>();
                s.source.clip = s.clip;
                s.source.volume = s.volume;
                s.source.pitch = s.pitch;
                s.source.loop = s.loop;
                s.source.outputAudioMixerGroup = musicMixerGroup;
                
                s.ambience = s.ambience;
                s.AudioLowPassFilter.cutoffFrequency = Mathf.Lerp(maxCutoffFrequency, minCutoffFrequency, (s.ambience - 0.1f) / (3 - 0.1f));
                s.AudioLowPassFilter.lowpassResonanceQ = Mathf.Lerp(minResonanceQ, maxResonanceQ, (s.ambience - 0.1f) / (3 - 0.1f));
            }

            
            foreach (Sound s in fxSounds)
            {
                if (s != null)
                {
                    Debug.Log("Object is not null!");
                    s.fxSource = sfxSource;
                    if(s.clip == null)
                        Debug.Log("Object is null!");
                    
                    s.fxSource = gameObject.AddComponent<AudioSource>();
                    s.fxSource.clip = s.clip;
                    s.fxSource.volume = s.volume;
                    s.fxSource.pitch = s.pitch;
                    s.fxSource.loop = s.loop;
                    s.fxSource.outputAudioMixerGroup = sfxMixerGroup;
                    Debug.Log("fx"+s.fxSource.loop);
                    Debug.Log(s.volume);
                    Debug.Log(s.pitch);
                    s.source = sfxSource;
                    
                }
                else
                {
                    Debug.Log("Object is null!");
                }
            }
        }
        private void Start()
        {
            PlayMusic("MainMenu");
        }
        private void AdjustVolume(string name, float volume)
        {
            Sound s = Array.Find(music, sound => sound.name == name);
            if (s == null)
            {
                s = Array.Find(fxSounds, sound => sound.name == name);
                if (s == null)
                {
                    Debug.LogWarning("Sound: " + name + " not found!");
                    return;
                }
            }

            s.source.volume = volume;
            //s.sfxSource.volume = volume;
        }
        public void PlayFX(string name)
        {
            Sound s = Array.Find(fxSounds, sound => sound.name == name);
            if (s == null)
            {
                Debug.LogWarning("Sound: " + name + " not found!");
                return;
            }
            
            if (s.clip != null)
            {
                Debug.Log("Clip is not null!");
                Debug.Log("Assigned AudioClip: " + s.fxSource.clip);
                s.fxSource.Play();
            }
            else
            {
                Debug.LogWarning("Clip is null!");
            }
            Debug.Log("Playing fx");
        }




        public void PlayMusic(string name)
        {
            Sound newMusic = Array.Find(music, sound => sound.name == name);
            if (newMusic == null)
            {
                Debug.LogWarning("Music: " + name + " not found!");
                return;
            }
            
            if (currentMusic != null && currentMusic == newMusic && currentMusic.source.isPlaying)
            {
                return;
            }
            
            if (currentMusic != null && currentMusic.source.isPlaying)
            {
                currentMusic.source.Stop();
            }
            
            newMusic.source.Play();
            currentMusic = newMusic;
        }



        public void StopSound(string name)
        {
            Sound s = Array.Find(music, sound => sound.name == name);
            if (s == null)
            {
                s = Array.Find(fxSounds, sound => sound.name == name);
                if (s == null)
                {
                    Debug.LogWarning("Sound: " + name + " not found!");
                    return;
                }
            }

            s.source.Stop();
        }

        

        private void Update()
        {
            AdjustVolume(music[0].name, music[0].volume);
            AdjustAmbience(music[0].name, music[0].ambience);
            AdjustPitch(music[0].name, music[0].pitch);
            
            
        }
        private void AdjustPitch(string name, float pitch)
        {
            Sound s = Array.Find(music, sound => sound.name == name);
            if (s == null)
            {
                s = Array.Find(fxSounds, sound => sound.name == name);
                if (s == null)
                {
                    Debug.LogWarning("Sound: " + name + " not found!");
                    return;
                }
            }

            s.source.pitch = pitch;
        }
        public void AdjustAmbience(string name, float ambience)
        {
            Sound s = Array.Find(music, sound => sound.name == name);
            if (s == null)
            {
                s = Array.Find(fxSounds, sound => sound.name == name);
                if (s == null)
                {
                    Debug.LogWarning("Sound: " + name + " not found!");
                    return;
                }
            }

            s.ambience = ambience;

            if (s.AudioLowPassFilter != null)
            {
                
                float cutoffFrequency = Mathf.Lerp(maxCutoffFrequency, minCutoffFrequency, (ambience - 0.1f) / (3 - 0.1f));
                s.AudioLowPassFilter.cutoffFrequency = cutoffFrequency;

                
                float resonanceQ = Mathf.Lerp(minResonanceQ, maxResonanceQ, (ambience - 0.1f) / (3 - 0.1f));
                s.AudioLowPassFilter.lowpassResonanceQ = resonanceQ;
            }
            else
            {
                Debug.LogWarning("Sound: " + name + " does not have an AudioLowPassFilter!");
            }
        }
        
    }
}