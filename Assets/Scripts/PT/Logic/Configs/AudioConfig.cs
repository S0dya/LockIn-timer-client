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
        // [SerializeField] private SerializableKeyValue<SoundEventEnum, EventReference> kvSounds;
        [SerializeField] private SerializableKeyValue<SoundEventEnum, object> kvSounds; // Placeholder object instead of EventReference
        
        // public SerializableKeyValue<SoundEventEnum, EventReference> KvSounds => kvSounds;
        public SerializableKeyValue<SoundEventEnum, object> KvSounds => kvSounds; // Return placeholder object
    }
}