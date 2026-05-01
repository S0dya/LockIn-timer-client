using FMODUnity;
using PT.Tools.Helper;
using UnityEngine;

namespace PT.Logic.Configs
{
    public enum SoundEventEnum
    {
        none,
        Music,
        UIButton,
        
        ConfettiSmall,
        
        Run,
        Meow,
        TikTokCheck,
        Noobs,
    }
    
    [CreateAssetMenu(menuName = "Configs/AudioConfig", fileName = "AudioConfig")]
    public class AudioConfig : ScriptableObject
    {
        [SerializeField] private SerializableKeyValue<SoundEventEnum, EventReference> kvSounds;
        
        public SerializableKeyValue<SoundEventEnum, EventReference> KvSounds => kvSounds;
    }
}