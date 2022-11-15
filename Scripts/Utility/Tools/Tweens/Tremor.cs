using System.Collections.Generic;
using UnityEngine;

namespace Pearl.Tweens
{
    public static class Tremor
    {
        //Crate tremor effect by tweens
        public static TweenContainer CreateTremor(this Transform @this, SpaceTranslation space, float time, bool isAutoKill, float amplitude, float degradation, int tremorNumbers)
        {
            List<Vector3> aux = new List<Vector3>();

            for (int i = 0; i < tremorNumbers; i++)
            {
                aux.Add(new Vector3(amplitude / 2, 0, 0));
                aux.Add(new Vector3(-amplitude, 0, 0));
                aux.Add(new Vector3(amplitude / 2, 0, 0));

                amplitude /= degradation;
            }

            var tween = @this.TranslateTween(time, isAutoKill, FunctionEnum.EaseOut_Quadratic, space, ChangeModes.Time, 0, TypeTranslation.Transform, aux.ToArray());
            tween.PathReference = TypeReferenceEnum.Relative;
            return tween;
        }
    }
}
