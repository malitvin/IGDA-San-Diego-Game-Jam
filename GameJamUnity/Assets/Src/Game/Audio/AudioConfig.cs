//Unity
using UnityEngine;
using UnityEngine.Audio;

/// <summary>
/// User: MaximusLit
/// Date: 6/3/2018
/// Audio Scriptable Object Blueprint. All Audio for the game
/// </summary>
namespace Audio
{
    [CreateAssetMenu(menuName = "Audio/Level Audio")]
    public class AudioConfig : ScriptableObject
    {
        public class AudioFile
        {
            public string name;
            public AudioClip clip;
            [Range(0, 1)]
            public float volume = 1;
            [Range(0, 1)]
            [Tooltip("2D vs 3D")]
            public float spatialBlend = 0;
            [Range(-3, 3)]
            public float pitch = 1;
            public AudioMixerGroup group;
        }

        [System.Serializable]
        public class SoundBlueprint : AudioFile
        {
            public SoundBank.Type key;
        }

        public SoundBlueprint[] sounds;
    }
}
