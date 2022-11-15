#if LOCALIZATION

using Pearl.Debug;
using Pearl.Events;
using Pearl.Input;
using Pearl.Storage;
using System;
using System.Collections;
using System.Globalization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Pearl
{
    [DisallowMultipleComponent]
    public class LocalizationManager : PearlBehaviour, ISingleton, IStoragePlayerPrefs
    {
        #region Inspector
        /// <summary>
        /// The language of the game
        /// </summary>
        [StoragePlayerPrefs("Language"), ReadOnly, SerializeField, ConditionalField("@useLocalization")]
        private SystemLanguage language;

        [SerializeField]
        private bool useInvariantCulture = true;

        [SerializeField, ConditionalField("!@useInvariantCulture")]
        private bool useCommaDecimal = false;

        [SerializeField]
        private bool useLocalization = false;

        [SerializeField, ConditionalField("@useLocalization")]
        private bool isForceLanguage = false;

        [SerializeField, ConditionalField("@isForceLanguage && @useLocalization")]
        private SystemLanguage forceLanguage = SystemLanguage.English;
        #endregion

        #region Private Fields
        private readonly static CultureInfo settingCulture = System.Globalization.CultureInfo.InvariantCulture;

        private const char wrongLocalize = '!';
        private static bool isDone = false;

        private static Action translationsBeforeDone;
        #endregion

        #region Static

        #region Propiety
        public static CultureInfo SettingCulture
        {
            get
            {
                return settingCulture;
            }
        }

        public static SystemLanguage Language
        {
            get
            {
                if (GetIstance(out var localizationManager))
                {
                    return localizationManager.language;
                }
                return default;
            }
            set
            {
                if (GetIstance(out var localizationManager))
                {
                    localizationManager.ChangeLanguage(value);
                    StoragePlayerPrefs.Save(localizationManager);
                }
            }
        }
        #endregion

        #region Public Methods
        public static AssetType GetAsset<AssetType>(in string tableString, in string ID) where AssetType : UnityEngine.Object
        {
            if (GetIstance(out var localizationManager) && !localizationManager.useLocalization)
            {
                return null;
            }

            if (!IsDone())
            {
                LogManager.LogWarning("The tables aren't initialized");
                return default;
            }

            try
            {
                return LocalizationSettings.AssetDatabase.GetLocalizedAsset<AssetType>(tableString, ID);
            }
            catch (Exception)
            {
                LogManager.LogWarning("There isn't table or id");
                return default;
            }
        }

        public static void SaveLanguage()
        {
            if (GetIstance(out var localizationManager) && localizationManager.useLocalization)
            {
                StoragePlayerPrefs.Save(localizationManager);
            }
        }

        public static string Translate(in string tableString, in string ID, in bool dontVisualizeWithErrorLocalization = true, Action actionIfError = null)
        {
            if (GetIstance(out var localizationManager) && !localizationManager.useLocalization)
            {
                return null;
            }

            if (!IsDone())
            {
                if (actionIfError != null)
                {
                    translationsBeforeDone += actionIfError;
                }
                LogManager.LogWarning("The tables aren't initialized");
                return VisualizeError(ID, dontVisualizeWithErrorLocalization);
            }

            try
            {
                return LocalizationSettings.StringDatabase.GetLocalizedString(tableString, ID.ToLower());
            }
            catch (Exception)
            {
                LogManager.LogWarning("There isn't table or id");
                return VisualizeError(ID, dontVisualizeWithErrorLocalization);
            }
        }

        public static SystemLanguage[] GetCurrentLanguages()
        {
            if (GetIstance(out var localizationManager) && !localizationManager.useLocalization)
            {
                return null;
            }

            if (!IsDone())
            {
                return default;
            }

            var locales = LocalizationSettings.AvailableLocales.Locales;
            SystemLanguage[] result = new SystemLanguage[locales.Count];

            for (int i = 0; i < locales.Count; i++)
            {
                string aux = locales[i].LocaleName;
                string[] split = aux.Split(" ");
                result[i] = EnumExtend.ParseEnum<SystemLanguage>(split[0]);
            }

            return result;
        }

        public static bool IsDone()
        {
            return isDone;
        }
        #endregion

        #region Private Methods
        private static bool GetIstance(out LocalizationManager result)
        {
            return Singleton<LocalizationManager>.GetIstance(out result, CreateSingletonEnum.CreateStatic);
        }

        private static string VisualizeError(in string ID, in bool dontVisualizeWithErrorLocalization)
        {
            Debug.LogManager.LogWarning("The text can't be localized", "Localization");
            if (dontVisualizeWithErrorLocalization)
            {
                return "";
            }
            else
            {
                return wrongLocalize + ID;
            }
        }
        #endregion

        #endregion

        #region Unity Callbacks
        protected override void Awake()
        {
            base.Awake();

            SetSettingCulture();

            if (useLocalization)
            {
                language = isForceLanguage ? forceLanguage : Application.systemLanguage;
                StartCoroutine(SetInit());
            }
        }
        #endregion

        #region Private Methods
        private void SetSettingCulture()
        {
            if (!useInvariantCulture)
            {
                if (settingCulture != null && settingCulture.NumberFormat != null)
                {
                    settingCulture.NumberFormat.NumberDecimalSeparator = useCommaDecimal ? "," : ".";
                    settingCulture.NumberFormat.NumberGroupSeparator = useCommaDecimal ? "." : ",";
                }
            }
        }

        private void SetInitLanguage()
        {
            if (!LocalizationSettings.InitializationOperation.IsDone)
            {
                return;
            }

            if (!isForceLanguage)
            {
                if (GetIstance(out var localizationManager))
                {
                    StoragePlayerPrefs.Load(localizationManager);
                }
            }

            string languageString = language.ToString().ToLower();
            var locales = LocalizationSettings.AvailableLocales.Locales;

            if (!locales.Exists(
                x =>
                {
                    string aux = x.LocaleName;
                    string[] split = aux.Split(" ");
                    return split.IsAlmostSpecificCount() && languageString == split[0].ToLower();
                }))
            {
                language = SystemLanguage.English;
            }

            ChangeLanguage(language);
        }

        private void ChangeLanguage(in SystemLanguage language)
        {
            var locales = LocalizationSettings.AvailableLocales;

            string ID = GetLanguageID(language);
            if (ID != null)
            {
                this.language = language;
                Locale locale = locales.GetLocale(ID);
                LocalizationSettings.SelectedLocale = locale;
                isDone = LocalizationSettings.InitializationOperation.IsDone;
                StartCoroutine(SetLanguage());
            }
            else
            {
                LogManager.LogWarning("The language isn't supported");
            }
        }

        private IEnumerator SetInit()
        {
            bool aux = LocalizationSettings.InitializationOperation.IsDone;
            while (!aux)
            {
                aux = LocalizationSettings.InitializationOperation.IsDone;
                yield return null;
            }

            SetInitLanguage();
        }

        private IEnumerator SetLanguage()
        {
            InputManager.ActiveAllInput(false);
            while (!isDone)
            {
                isDone = LocalizationSettings.InitializationOperation.IsDone;
                yield return null;
            }
            translationsBeforeDone?.Invoke();
            translationsBeforeDone = null;
            PearlEventsManager.CallEvent(ConstantStrings.SetNewLanguageEvent, PearlEventType.Normal);
            InputManager.ActiveAllInput(true);
        }

        private string GetLanguageID(in SystemLanguage language)
        {
            var locales = LocalizationSettings.AvailableLocales.Locales;
            string languageString = language.ToString().ToLower();
            var locale = locales.Find(
                x =>
                {
                    string aux = x.LocaleName;
                    string[] split = aux.Split(" ");
                    return split.IsAlmostSpecificCount() && languageString == split[0].ToLower();
                });

            if (locale != null)
            {
                string localeString = locale.LocaleName;
                localeString = localeString.Replaces(string.Empty, "(", ")");
                string[] split = localeString.Split(" ");
                return split[1];
            }
            return null;
        }
        #endregion
    }
}

#endif