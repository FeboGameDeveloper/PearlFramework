using Pearl.Debug;
using Pearl.Events;
using Pearl.UI;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Pearl.Tweens
{
    public enum TweensEnum
    {
        MixerTween,
        ScaleTween,
        TranslateTween,
        VolumeTween,
        FadeTween,
        ColorTween,
    }

    public enum SpaceTranslation
    {
        WorldSpace,
        ScreenSpace,
        ScreenSpaceWithAnchors,
    }

    // tween for the audio mixer parameters.
    public static class TweensExtend
    {
        #region Mixer

        // tween for the audio mixer parameters.
        /// <param name = "mixer">The audio mixer container</param>
        /// <param name = "nameParamMixer">the name of the parameter you want to change</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer MixerParamTween(this AudioMixer mixer, string nameParamMixer, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params float[] values)
        {
            return MixerParamTween(mixer, nameParamMixer, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for the audio mixer parameters.
        /// <param name = "mixer">The audio mixer container</param>
        /// <param name = "nameParamMixer">the name of the parameter you want to change</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>

        public static TweenContainer MixerParamTween(this AudioMixer mixer, string nameParamMixer, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params float[] values)
        {
            if (mixer == null)
            {
                return null;
            }

            void SetNewValue(Vector4 newValue)
            {
                if (mixer != null)
                {
                    mixer.GetFloat(nameParamMixer, out float currentValue);
                    currentValue += newValue.x;
                    mixer.SetFloat(nameParamMixer, currentValue);
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
            }

            Vector4 GetInitValue()
            {
                float result = 0;
                if (mixer != null)
                {
                    mixer.GetFloat(nameParamMixer, out result);
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
                return new Vector4(result, 0, 0, 0);
            }

            AnimationCurveInfo curve = new(functionEnum);
            Vector4[] finalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);

            return TweenContainer.CreateTween(GetInitValue, timeOrVelocity, SetNewValue, isAutoKill, curve, changeMode, finalValues);
        }
        #endregion

        #region Scale
        // tween for the transform scale
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ScaleTween(this Transform transform, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params Vector3[] scaleVectors)
        {
            return ScaleTween(transform, timeOrVelocity, isAutoKill, AxisCombined.XYZ, FunctionEnum.EaseIn_Quadratic, changeMode, scaleVectors);
        }

        // tween for the transform scale
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ScaleTween(this Transform transform, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params Vector3[] scaleVectors)
        {
            return ScaleTween(transform, timeOrVelocity, isAutoKill, AxisCombined.XYZ, functionEnum, changeMode, scaleVectors);
        }

        // tween for the transform scale
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "AxisCombined">the spcific axis for changes</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ScaleTween(this Transform transform, float timeOrVelocity, bool isAutoKill, AxisCombined axisCombined, ChangeModes changeMode, params Vector3[] scaleVectors)
        {
            return ScaleTween(transform, timeOrVelocity, isAutoKill, axisCombined, FunctionEnum.EaseIn_Quadratic, changeMode, scaleVectors);
        }

        // tween for the transform scale
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "AxisCombined">the spcific axis for changes</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ScaleTween(this Transform transform, float timeOrVelocity, bool isAutoKill, AxisCombined axisCombined, FunctionEnum functionEnum, ChangeModes changeMode, params Vector3[] values)
        {
            if (transform == null)
            {
                return null;
            }

            void SetNewValue(Vector4 newValue)
            {
                if (transform != null)
                {
                    transform.SetScale(newValue, ChangeTypeEnum.Modify, axisCombined);
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
            }

            Vector4 GetInitValue()
            {
                if (transform != null)
                {
                    return transform.localScale;
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                    return default;
                }
            }

            AnimationCurveInfo curve = new(functionEnum);
            Vector4[] finalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);

            var tween = TweenContainer.CreateTween(GetInitValue, timeOrVelocity, SetNewValue, isAutoKill, curve, changeMode, finalValues);

            return tween;
        }
        #endregion

        #region Translate
        // tween for the transform translation
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer TranslateTween(this Transform transform, float timeOrVelocity, bool isAutoKill, SpaceTranslation space, ChangeModes changeMode, float curveFactor, TypeTranslation typeTranslation, params Vector3[] translateVectors)
        {
            return TranslateTween(transform, timeOrVelocity, isAutoKill, AxisCombined.XYZ, space, FunctionEnum.EaseIn_Quadratic, changeMode, curveFactor, typeTranslation, translateVectors);
        }

        // tween for the transform translation
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer TranslateTween(this Transform transform, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, SpaceTranslation space, ChangeModes changeMode, float curveFactor, TypeTranslation typeTranslation, params Vector3[] translateVectors)
        {
            return TranslateTween(transform, timeOrVelocity, isAutoKill, AxisCombined.XYZ, space, functionEnum, changeMode, curveFactor, typeTranslation, translateVectors);
        }

        // tween for the transform translation
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "AxisCombined">the spcific axis for changes</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer TranslateTween(this Transform transform, float timeOrVelocity, bool isAutoKill, AxisCombined axisCombined, SpaceTranslation space, ChangeModes changeMode, float curveFactor, TypeTranslation typeTranslation, params Vector3[] translateVectors)
        {
            return TranslateTween(transform, timeOrVelocity, isAutoKill, axisCombined, space, FunctionEnum.EaseIn_Quadratic, changeMode, curveFactor, typeTranslation, translateVectors);
        }

        // tween for the transform translation
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer TranslateTween(this Transform transform, float timeOrVelocity, bool isAutoKill, SpaceTranslation space, ChangeModes changeMode, float curveFactor, TypeTranslation typeTranslation, Transform destination)
        {
            if (destination == null)
            {
                return default;
            }

            return TranslateTween(transform, timeOrVelocity, isAutoKill, AxisCombined.XYZ, space, FunctionEnum.EaseIn_Quadratic, changeMode, curveFactor, typeTranslation, destination.position);
        }

        // tween for the transform translation
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer TranslateTween(this Transform transform, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, SpaceTranslation space, ChangeModes changeMode, float curveFactor, TypeTranslation typeTranslation, Transform destination)
        {
            if (destination == null)
            {
                return default;
            }

            return TranslateTween(transform, timeOrVelocity, isAutoKill, AxisCombined.XYZ, space, functionEnum, changeMode, curveFactor, typeTranslation, destination.position);
        }

        // tween for the transform translation
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "AxisCombined">the spcific axis for changes</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer TranslateTween(this Transform transform, float timeOrVelocity, bool isAutoKill, AxisCombined axisCombined, SpaceTranslation space, ChangeModes changeMode, float curveFactor, TypeTranslation typeTranslation, Transform destination)
        {
            if (destination == null)
            {
                return default;
            }

            return TranslateTween(transform, timeOrVelocity, isAutoKill, axisCombined, space, FunctionEnum.EaseIn_Quadratic, changeMode, curveFactor, typeTranslation, destination.position);
        }

        // tween for the transform translation
        /// <param name = "transform">The transform container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "AxisCombined">the spcific axis for changes</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer TranslateTween(this Transform transform, float timeOrVelocity, bool isAutoKill, AxisCombined axisCombined, SpaceTranslation space, FunctionEnum functionEnum, ChangeModes changeMode, float curveFactor, TypeTranslation typeTranslation, params Vector3[] values)
        {
            if (transform == null)
            {
                return null;
            }

            Rigidbody2D _body2D = transform.GetComponent<Rigidbody2D>();
            Rigidbody _body = transform.GetComponent<Rigidbody>();

            void SetNewValue(Vector4 newValue)
            {
                if (transform)
                {
                    if (typeTranslation == TypeTranslation.Transform)
                    {
                        transform.SetPosition(newValue, ChangeTypeEnum.Modify, axisCombined);
                    }
                    else if (typeTranslation == TypeTranslation.Rigidbody2D && _body2D != null)
                    {
                        _body2D.MovePosition(_body2D.position + (Vector2)newValue);
                    }
                    else if (_body != null)
                    {
                        _body.MovePosition(_body.position + (Vector3)newValue);
                    }
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
            }

            Vector4 GetInitValue()
            {
                if (transform != null)
                {
                    return transform.position;
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                    return default;
                }
            }

            AnimationCurveInfo curve = new(functionEnum);
            Vector3[] originalValues = (Vector3[])values.Clone();
            Vector2 size = new(Screen.width, Screen.height);
            float originalCurveFactor = curveFactor;

            if (originalValues != null && space != SpaceTranslation.WorldSpace)
            {
                for (int i = 0; i < originalValues.Length; i++)
                {
                    float xScreen = MathfExtend.ChangeRange01(Mathf.Abs(originalValues[i].x), Screen.width);
                    xScreen *= Mathf.Sign(originalValues[i].x);
                    float yScreen = MathfExtend.ChangeRange01(Mathf.Abs(originalValues[i].y), Screen.height);
                    yScreen *= Mathf.Sign(originalValues[i].y);

                    values[i] = new Vector3(xScreen, yScreen, originalValues[i].z);
                }

                curveFactor = MathfExtend.ChangeRange01(Mathf.Clamp01(originalCurveFactor), Mathf.Min(Screen.width, Screen.width));
            }

            Vector4[] finalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);
            var tween = TweenContainer.CreateTween(GetInitValue, timeOrVelocity, SetNewValue, isAutoKill, curve, changeMode, finalValues);

            if (space != SpaceTranslation.WorldSpace)
            {
                PearlEventsManager.AddAction(ConstantStrings.ChangeResolution, ChangeScreenSize);

                tween.OnKill += (x) => { PearlEventsManager.RemoveAction(ConstantStrings.ChangeResolution, ChangeScreenSize); };

                if (space == SpaceTranslation.ScreenSpaceWithAnchors)
                {
                    tween.OnComplete += SetCorners;
                    tween.OnKill += (x) => { tween.OnComplete -= SetCorners; };
                }
            }

            void SetCorners(TweenContainer tween, float error)
            {
                if (transform != null)
                {
                    if (transform.TryGetComponent<RectTransform>(out var rect))
                    {
                        rect.AnchorsToCorners();
                    }
                }
            }

            void ChangeScreenSize()
            {
                var initValue = tween.InitValue;
                initValue.x = MathfExtend.ChangeRange(initValue.x, 0, size.x, 0, Screen.width);
                initValue.y = MathfExtend.ChangeRange(initValue.y, 0, size.y, 0, Screen.height);

                size = new Vector2(Screen.width, Screen.height);
                tween.InitValue = initValue;

                for (int i = 0; i < originalValues.Length; i++)
                {
                    float xScreen = MathfExtend.ChangeRange01(Mathf.Abs(originalValues[i].x), Screen.width);
                    xScreen *= Mathf.Sign(originalValues[i].x);
                    float yScreen = MathfExtend.ChangeRange01(Mathf.Abs(originalValues[i].y), Screen.height);
                    yScreen *= Mathf.Sign(originalValues[i].y);

                    values[i] = new Vector3(xScreen, yScreen, originalValues[i].z);
                }

                Vector4[] finalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);
                tween.FinalValues = finalValues;
                curveFactor = MathfExtend.ChangeRange01(Mathf.Clamp01(originalCurveFactor), Mathf.Min(Screen.width, Screen.width));
                tween.CurveFactor = curveFactor;
            }

            tween.CurveFactor = curveFactor;
            return tween;
        }
        #endregion

        #region Volume
        // tween for the volume in a audioSource
        /// <param name = "AudioSource">The AudioSource container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer VolumeTween(this AudioSource audioSource, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params float[] values)
        {
            return VolumeTween(audioSource, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for the volume in a audioSource
        /// <param name = "AudioSource">The AudioSource container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer VolumeTween(this AudioSource audioSource, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params float[] values)
        {
            if (audioSource == null)
            {
                return null;
            }

            void SetNewValue(Vector4 newValue)
            {
                if (audioSource)
                {
                    audioSource.volume = newValue.x;
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
            }

            Vector4 GetInitValue()
            {
                float volume = 0;
                if (audioSource)
                {
                    volume = audioSource.volume;
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
                return new Vector4(volume, 0, 0, 0);
            }

            AnimationCurveInfo curve = new(functionEnum);
            Vector4[] finalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);

            return TweenContainer.CreateTween(GetInitValue, timeOrVelocity, SetNewValue, isAutoKill, curve, changeMode, finalValues);
        }
        #endregion

        #region Fade
        // tween for change alpha channel in a sprite
        /// <param name = "image">The image container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer FadeTween(this SpriteRenderer image, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params float[] values)
        {
            return FadeTween(image, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for change alpha channel in a sprite
        /// <param name = "image">The image container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer FadeTween(this SpriteRenderer image, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params float[] values)
        {
            SpriteManager spriteManager = new(image);
            return FadeTween(spriteManager, timeOrVelocity, isAutoKill, functionEnum, changeMode, values);
        }

        // tween for change alpha channel in a sprite
        /// <param name = "image">The image container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer FadeTween(this Image image, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params float[] values)
        {
            return FadeTween(image, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for change alpha channel in a sprite
        /// <param name = "image">The image container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer FadeTween(this Image image, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params float[] values)
        {
            SpriteManager spriteManager = new(image);
            return FadeTween(spriteManager, timeOrVelocity, isAutoKill, functionEnum, changeMode, values);
        }

        // tween for change alpha channel in a sprite
        /// <param name = "text">The text container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer FadeTween(this TMP_Text text, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params float[] values)
        {
            return FadeTween(text, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for change alpha channel in a sprite
        /// <param name = "text">The text container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer FadeTween(this TMP_Text text, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params float[] values)
        {
            SpriteManager spriteManager = new(text);
            return FadeTween(spriteManager, timeOrVelocity, isAutoKill, functionEnum, changeMode, values);
        }

        // tween for change alpha channel in a sprite
        /// <param name = "spriteManager">The spriteManager container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer FadeTween(this SpriteManager spriteManager, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params float[] values)
        {
            return FadeTween(spriteManager, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for change alpha channel in a sprite
        /// <param name = "spriteManager">The spriteManager container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer FadeTween(this SpriteManager spriteManager, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params float[] values)
        {
            if (spriteManager == null)
            {
                return null;
            }

            void SetNewValue(Vector4 newValue)
            {
                if (spriteManager != null)
                {
                    Color color = spriteManager.GetColor();
                    float alpha = color.a + newValue.x;
                    color.a = alpha;
                    spriteManager.SetColor(color);
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
            }

            Vector4 GetInitValue()
            {
                Color color = default;
                if (spriteManager != null)
                {
                    color = spriteManager.GetColor();
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
                return new Vector4(color.a, 0, 0, 0);
            }

            AnimationCurveInfo curve = new(functionEnum);
            Vector4[] finalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);

            return TweenContainer.CreateTween(GetInitValue, timeOrVelocity, SetNewValue, isAutoKill, curve, changeMode, finalValues);
        }
        #endregion

        #region Color
        // tween for change color in a sprite
        /// <param name = "image">The image container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ColorTween(this SpriteRenderer image, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params Color[] values)
        {
            return ColorTween(image, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for change color in a sprite
        /// <param name = "image">The image container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ColorTween(this SpriteRenderer image, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params Color[] values)
        {
            SpriteManager spriteManager = new(image);
            return ColorTween(spriteManager, timeOrVelocity, isAutoKill, functionEnum, changeMode, values);
        }

        // tween for change color in a sprite
        /// <param name = "image">The image container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ColorTween(this Image image, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params Color[] values)
        {
            return ColorTween(image, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for change color in a sprite
        /// <param name = "image">The image container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ColorTween(this Image image, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params Color[] values)
        {
            SpriteManager spriteManager = new(image);
            return ColorTween(spriteManager, timeOrVelocity, isAutoKill, functionEnum, changeMode, values);
        }

        // tween for change color in a sprite
        /// <param name = "text">The text container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ColorTween(this TMP_Text text, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params Color[] values)
        {
            return ColorTween(text, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for change color in a sprite
        /// <param name = "text">The text container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ColorTween(this TMP_Text text, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params Color[] values)
        {
            SpriteManager spriteManager = new(text);
            return ColorTween(spriteManager, timeOrVelocity, isAutoKill, functionEnum, changeMode, values);
        }

        // tween for change color in a sprite
        /// <param name = "spriteManager">The spriteManager container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ColorTween(this SpriteManager spriteManager, float timeOrVelocity, bool isAutoKill, ChangeModes changeMode, params Color[] values)
        {
            return ColorTween(spriteManager, timeOrVelocity, isAutoKill, FunctionEnum.EaseIn_Quadratic, changeMode, values);
        }

        // tween for change color in a sprite
        /// <param name = "spriteManager">The spriteManager container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer ColorTween(this SpriteManager spriteManager, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params Color[] values)
        {
            if (spriteManager == null)
            {
                return null;
            }

            void SetNewValue(Vector4 newValue)
            {
                if (spriteManager != null)
                {
                    Color color = spriteManager.GetColor();
                    Color newColor = newValue;
                    color += newColor;

                    spriteManager.SetColor(color);
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
            }

            Vector4 GetInitValue()
            {
                Color color = default;
                if (spriteManager != null)
                {
                    color = spriteManager.GetColor();
                }
                else
                {
                    LogManager.LogWarning("The tween is is referring to object that is not exist", "Tween");
                }
                return color;
            }

            AnimationCurveInfo curve = new(functionEnum);
            Vector4[] finalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);

            return TweenContainer.CreateTween(GetInitValue, timeOrVelocity, SetNewValue, isAutoKill, curve, changeMode, finalValues);
        }
        #endregion

        #region EulerAngles
        // tween for change color in a sprite
        /// <param name = "spriteManager">The spriteManager container</param>
        /// <param name = "timeOrVelocity">the time of tween</param>
        /// <param name = "isAutoKill">Does the tween have to destroy itself when it ends?</param>
        /// <param name = "functionEnum">the function to soften the tween</param>
        /// <param name = "values">The values ​​that the tween must reach</param>
        public static TweenContainer EulerAngleTween(this Transform tranform, float timeOrVelocity, bool isAutoKill, FunctionEnum functionEnum, ChangeModes changeMode, params Vector3[] values)
        {
            if (tranform == null)
            {
                return null;
            }

            void SetNewValue(Vector4 newValue)
            {
                tranform.SetEulerAngles(newValue, ChangeTypeEnum.Modify);
            }

            Vector4 GetInitValue()
            {
                return tranform.eulerAngles;
            }

            AnimationCurveInfo curve = new(functionEnum);
            Vector4[] finalValues = ArrayExtend.ConvertNumbersAndVectors<Vector4>(values);
            var container = TweenContainer.CreateTween(GetInitValue, timeOrVelocity, SetNewValue, isAutoKill, curve, changeMode, finalValues);
            container.Angle = true;
            return container;
        }
        #endregion
    }
}