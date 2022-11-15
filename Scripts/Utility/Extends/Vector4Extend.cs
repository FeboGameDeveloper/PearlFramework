using UnityEngine;

namespace Pearl
{
    public static class Vector4Extend
    {
        public static Vector4 LerpAngle(Vector4 vectorA, Vector4 vectorB, float t)
        {
            Vector4 result = default;
            result.x = Mathf.LerpAngle(vectorA.x, vectorB.x, t);
            result.y = Mathf.LerpAngle(vectorA.y, vectorB.y, t);
            result.z = Mathf.LerpAngle(vectorA.z, vectorB.z, t);
            result.w = Mathf.LerpAngle(vectorA.w, vectorB.w, t);
            return result;
        }

        public static Vector4 Lerp(Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ, Vector2 rangeW, float t, FunctionEnum function = FunctionEnum.Linear)
        {
            return Lerp(rangeX, rangeY, rangeZ, rangeW, Vector4.one * t, function);
        }

        public static Vector4 Lerp(Vector2 rangeX, Vector2 rangeY, Vector2 rangeZ, Vector2 rangeW, Vector4 t, FunctionEnum function = FunctionEnum.Linear)
        {
            Vector4 vector;
            vector.x = MathfExtend.Lerp(rangeX.x, rangeX.y, t.x, function);
            vector.y = MathfExtend.Lerp(rangeY.x, rangeY.y, t.y, function);
            vector.z = MathfExtend.Lerp(rangeZ.x, rangeZ.y, t.z, function);
            vector.w = MathfExtend.Lerp(rangeW.x, rangeW.y, t.w, function);
            return vector;
        }
    }
}