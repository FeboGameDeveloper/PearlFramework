#if STOMPYROBOT

using Pearl.Storage;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using static Pearl.ReflectionExtend;

namespace Pearl.Debug
{
    public class GeneralTunning : TunningPage
    {
        protected override void Init()
        {
            bool useTunningVars = PlayerPrefsExtend.GetBool("useTunningVars", SaveVar);
            SaveVar = useTunningVars;

            if (TunningManager.UseTunningVars)
            {
                LoadVarsPrivate();
            }
        }

        protected override void LoadVarsPrivate()
        {
        }

        [Category("General")] // Options will be grouped by category
        public void ResetPlayerPrefs()
        {
            LogManager.Log("Reset Player Prefs");

            PlayerPrefs.DeleteAll();
        }

        [Category("General")]
        public void LoadVars()
        {
            GetField<List<TunningPage>, TunningManager>("_activeTunningPages", out var activeTunningPages);
            if (activeTunningPages != null)
            {
                foreach (var page in activeTunningPages)
                {
                    UseMethod(page, "LoadVarsPrivate");
                    SRDebug.Instance?.RemoveOptionContainer(page);
                }

                foreach (var page in activeTunningPages)
                {
                    SRDebug.Instance?.AddOptionContainer(page);
                }
            }
        }

        [Category("General")]
        public void ResetVars()
        {
            GetField<List<TunningPage>, TunningManager>("_activeTunningPages", out var activeTunningPages);
            if (activeTunningPages != null)
            {
                foreach (var page in activeTunningPages)
                {
                    UseMethod(page, "ResetDefault");
                    SRDebug.Instance?.RemoveOptionContainer(page);
                }

                foreach (var page in activeTunningPages)
                {
                    SRDebug.Instance?.AddOptionContainer(page);
                }
            }
        }

        [Category("General")]
        public bool SaveVar
        {
            get
            {
                bool result = GetValue<bool, TunningManager>("useTunningVars");
                return result;
            }
            set
            {
                SetValue<bool>(typeof(TunningManager), value, "useTunningVars");
                SaveBool("useTunningVars", value, true);
            }
        }
    }
}

#endif