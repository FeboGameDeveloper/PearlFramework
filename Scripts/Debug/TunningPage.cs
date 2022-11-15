#if STOMPYROBOT

using Pearl.Storage;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Debug
{
    public abstract class TunningPage
    {
        private Dictionary<string, object> _defaultValues = new();

        protected virtual void Init()
        {
            if (_defaultValues != null)
            {
                var props = GetType().GetProperties();
                foreach (var prop in props)
                {
                    _defaultValues.Update(prop.Name, prop.GetValue(this));
                }
            }

            if (TunningManager.UseTunningVars)
            {
                LoadVarsPrivate();
            }
        }

        protected abstract void LoadVarsPrivate();

        protected void ResetDefault()
        {
            if (_defaultValues != null)
            {
                var props = GetType().GetProperties();
                foreach (var prop in props)
                {
                    if (_defaultValues.TryGetValue(prop.Name, out var defaultValue))
                    {
                        prop.SetValue(this, defaultValue);
                    }
                }
            }
        }

        protected void SaveFloat(string nameVar, float value, bool ignore = false)
        {
            if (ignore || TunningManager.UseTunningVars)
            {
                PlayerPrefs.SetFloat(nameVar, value);
                PlayerPrefs.Save();
            }
        }

        protected void SaveInt(string nameVar, int value, bool ignore = false)
        {
            if (ignore || TunningManager.UseTunningVars)
            {
                PlayerPrefs.SetInt(nameVar, value);
                PlayerPrefs.Save();
            }
        }

        protected void SaveString(string nameVar, in string value, bool ignore = false)
        {
            if (ignore || TunningManager.UseTunningVars)
            {
                PlayerPrefs.SetString(nameVar, value);
                PlayerPrefs.Save();
            }
        }

        protected void SaveBool(string nameVar, bool value, bool ignore = false)
        {
            if (ignore || TunningManager.UseTunningVars)
            {
                PlayerPrefsExtend.SetBool(nameVar, value);
                PlayerPrefs.Save();
            }
        }

    }
}

#endif


