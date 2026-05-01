using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using PT.Backend.Interfaces;
using PT.Logic.Configs;
using PT.Tools.Debugging;
using PT.Tools.Helper;
using UnityEngine;
using Zenject;

namespace PT.Logic.Save
{
    #region classes, interface

    public class SaveData
    {
        public Dictionary<string, int> IntDict;
        public Dictionary<string, bool> BoolDict;
        public Dictionary<string, float> FloatDict;
        public Dictionary<string, string> StringDict;
        
        public long UpdatedAtUnix;

        public SaveData(Dictionary<string, int> intDict, Dictionary<string, float> floatDict, 
            Dictionary<string, bool> boolDict, Dictionary<string, string> stringDict,
            long updatedAtUnix)
        {
            IntDict = intDict;
            FloatDict = floatDict;
            BoolDict = boolDict;
            StringDict = stringDict;
            UpdatedAtUnix = updatedAtUnix;
        }
    }

    public interface ISaveService
    {
        public void Save(SaveData saveData);
        public SaveData Load();
    }

    #endregion


    public class SaveManager : MonoBehaviour
    {
        [Inject] private GameConfig _gameConfig;
        [Inject(Optional = true)] private ISaveService _saveService;
        [Inject(Optional = true)] private ICloudSaveService _cloudSaveService;

        private bool _canSave;
        private bool _initialized;
        
        private float _lastCloudSaveTime = -999f;
        
        public async UniTask Init()
        {
            var local = _saveService?.Load();

            SaveData cloud = null;
            if (_cloudSaveService != null)
                cloud = await _cloudSaveService.Load();

            var chosen = ChooseSave(local, cloud);

            LoadData(chosen);

            _canSave = true;
            _initialized = true;

            if (chosen == cloud && cloud != null)
                _saveService?.Save(cloud);
        }

        private void OnApplicationPause(bool pauseStatus)
        {
            if (pauseStatus) Save();
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            if (!hasFocus) Save();
        }

        private void OnApplicationQuit()
        {
            Save();
        }

        public void Save(bool forceCloudSave = false)
        {
            if (!_initialized || !_canSave)
            {
                DebugManager.Log(DebugCategory.Save, $"Attempted save before initialization.", LogType.Warning);
                return;
            }

            var data = SaveData();
            
            _saveService.Save(data);

            TryCloudSave(data, forceCloudSave);
        }
        public void Load()
        {
            LoadData(_saveService.Load());

            _canSave = true;
        }

        private void TryCloudSave(SaveData data, bool force)
        {
            if (_cloudSaveService == null) return;
            if (!force && !IsCloudSaveCooldownPassed()) return;

            _lastCloudSaveTime = Time.unscaledTime;
            _cloudSaveService.Save(data).Forget();

            DebugManager.Log(DebugCategory.Save, "[Cloud] Save triggered");
        }
        private bool IsCloudSaveCooldownPassed() => Time.unscaledTime - _lastCloudSaveTime >= _gameConfig.CloudSaveCooldownSeconds;
        
        private SaveData SaveData()
        {
            var ints = new Dictionary<string, int>();
            var floats = new Dictionary<string, float>();
            var bools = new Dictionary<string, bool>();
            var strings = new Dictionary<string, string>();
            
            foreach (GameDataKey key in Enum.GetValues(typeof(GameDataKey)))
            {
                var value = GameDataRegistry.Get(key);
                var str = key.ToString();

                switch (value)
                {
                    case int i: ints[str] = i; break;
                    case float f: floats[str] = f; break;
                    case bool b: bools[str] = b; break;
                    case string s: strings[str] = s; break;
                    case Enum e: strings[str] = e.ToString(); break;
                }
            }

            DebugManager.Log(DebugCategory.Save, $"[SAVE] " +
                                                 $"Ints:   {Utils.DictToString(ints)}\n" +
                                                 $"Bools: {Utils.DictToString(bools)}\n" +
                                                 $"Floats: {Utils.DictToString(floats)}\n" +
                                                 $"Strings: {Utils.DictToString(strings)}");
            
            return new SaveData(ints, floats, bools, strings, DateTimeOffset.UtcNow.ToUnixTimeSeconds());
        }
        
        private void LoadData(SaveData data)
        {
            if (data == null) return;

            foreach (GameDataKey key in Enum.GetValues(typeof(GameDataKey)))
            {
                var strKey = key.ToString();
                object val = null;

                if (data.IntDict.TryGetValue(strKey, out var i)) val = i;
                else if (data.FloatDict.TryGetValue(strKey, out var f)) val = f;
                else if (data.BoolDict.TryGetValue(strKey, out var b)) val = b;
                else if (data.StringDict.TryGetValue(strKey, out var s))
                {
                    var type = GameDataRegistry.GetValueType(key);
                    if (type != null && type.IsEnum)
                    {
                        try
                        {
                            val = Enum.Parse(type, s);
                        }
                        catch
                        {
                            DebugManager.Log(DebugCategory.Save, $"[LOAD] Could not parse enum {s} for {key}, using default.", LogType.Warning);
                            val = Activator.CreateInstance(type);
                        }
                    }
                    else
                    {
                        val = s;
                    }
                }

                if (val != null)
                    GameDataRegistry.Set((GameDataKey)key, val);
            }

            DebugManager.Log(DebugCategory.Save, $"[LOAD] " +
                                                 $"Ints:   {Utils.DictToString(data.IntDict)}\n" +
                                                 $"Bools: {Utils.DictToString(data.BoolDict)}\n" +
                                                 $"Floats: {Utils.DictToString(data.FloatDict)}\n" +
                                                 $"Strings: {Utils.DictToString(data.StringDict)}");
        }

        private T GetOrDefault<T>(Dictionary<string, T> dict, GameDataKey key, T fallback = default)
        {
            var strKey = key.ToString();
            return dict != null && dict.TryGetValue(strKey, out var value) ? value : fallback;
        }
        
        private SaveData ChooseSave(SaveData local, SaveData cloud)
        {
            if (local == null && cloud == null) return null;
            if (local == null) return cloud;
            if (cloud == null) return local;

            return cloud.UpdatedAtUnix > local.UpdatedAtUnix ? cloud : local;
        }
    }
}
