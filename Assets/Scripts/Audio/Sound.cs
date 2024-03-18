using UnityEngine;

//Script Made By Daniel Alvarado

    [System.Serializable]
    public class Sound 
    {
        public string name;
    
        public AudioClip clip;

        [Range(0f, 2f)]
        public float pitch;
        [Range(0f, 1f)]
        public float volume;
        [Range(0f, 3f)]
        public float ambience;
        
        public AudioSource source, fxSource;
        [HideInInspector]
        public AudioLowPassFilter AudioLowPassFilter;
        public bool loop;
        
    }
