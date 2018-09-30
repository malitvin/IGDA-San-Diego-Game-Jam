//Unity
using UnityEngine;

//C#
using System.Collections.Generic;

//Game
using Common.Util;


namespace Audio
{
    /// <summary>
    /// Sound Controller class that will go in Unity
    /// </summary>
    public class AudioSystem : GhostGen.EventDispatcher
    {
        //overall level audio
        private Dictionary<SoundBank.Type, SoundObject> _levelAudioMap;

        //Game Audio Blueprint reference
        private AudioConfig gameAudio;

        //Audio Parent
        private Transform _audioParent;

        /// <summary>
        /// Init
        /// </summary>
        public AudioSystem(GameConfig masterConfig)
        {
            _audioParent = GameObject.FindGameObjectWithTag("ScenePool").transform;
            gameAudio = masterConfig.audioConfig;
            GenerateAudioLookupForLevel();
        }

        /// <summary>
        /// Generates Audio Map based on enum audio type for this level
        /// </summary>
        private void GenerateAudioLookupForLevel()
        {
            //create map
            _levelAudioMap = new Dictionary<SoundBank.Type, SoundObject>(new FastEnumIntEqualityComparer<SoundBank.Type>());
            AddToMap(gameAudio.sounds);
        }

        private void AddToMap(AudioConfig.SoundBlueprint[] files)
        {
            //INIT UI AUDIO
            int i;
            int allAudio = files.Length;
            SoundObject tempSoundObj;
            AudioClip clip;
            string audioName;

            for (i = 0; i < allAudio; i++)
            {
                AudioConfig.SoundBlueprint file = files[i];
                SoundBank.Type key = file.key;
                clip = file.clip;
                audioName = file.name;
                if (clip)
                {
                    if (!_levelAudioMap.ContainsKey(key))
                    {
                        tempSoundObj = new SoundObject(file, _audioParent);
                        _levelAudioMap[key] = tempSoundObj;
                    }
                    else
                    {
                        Debug.Log("Key " + key + "exists in the Audio file map which means there are duplicate keys for Level audio in the Audio ScriptableObject");
                    }
                }
                else
                {
                    Debug.LogError("There is no Audio Clip Attached to " + audioName + " in the Level Audio List");
                }
            }
        }

        public void PlaySound(SoundBank.Type key, GameObject obj = null,bool Randompitch = false)
        {
            if (!_levelAudioMap.ContainsKey(key))
            {
                Debug.LogError(key + " Not Found in Sound Lookup");
            }
            else
            {
                SoundObject soundObj = _levelAudioMap[key];
                soundObj.PlaySound(obj, Randompitch);
            }
        }
    }

    /// <summary>
    /// Object that controls and audio source
    /// </summary>
    public class SoundObject
    {
        private AudioSource _source;
        private GameObject _sourceGO;
        private Transform _sourceTR;
        private AudioClip _clip;

        public SoundObject(AudioConfig.AudioFile blueprint,Transform parent)
        {
            _sourceGO = new GameObject("AudioSource: " + blueprint.name);
            _sourceGO.transform.SetParent(parent);
            _sourceTR = _sourceGO.transform;
            _source = _sourceGO.AddComponent<AudioSource>();
            _source.playOnAwake = false;
            _source.clip = blueprint.clip;
            _source.volume = blueprint.volume;
            _source.pitch = blueprint.pitch;
            _source.spatialBlend = blueprint.spatialBlend;
            if (blueprint.group != null)
            {
                _source.outputAudioMixerGroup = blueprint.group;
            }
            _clip = blueprint.clip;
        }

        public void PlaySound(GameObject obj = null,bool randomPitch=false)
        {
            if (obj && _source.spatialBlend == 1)
            {
                _sourceTR.position = obj.transform.position;
            }
            if(randomPitch)
            {
                _source.pitch = Random.Range(0.95f, 1.1f);
            }
            _source.PlayOneShot(_clip);
        }
    }
}
