using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public enum Axis2DCombined
    {
        X,
        Y,
        XY,
    }

    public enum AxisCombined
    {
        X,
        Y,
        Z,
        XY,
        XZ,
        YZ,
        XYZ,
    }

    public enum Axis2DEnum
    {
        X,
        Y,
    }

    public enum SemiAxis2DEnum
    {
        Left,
        Right,
        Up,
        Down,
    }

    public enum SemiAxisX
    {
        Left,
        Right,
    }

    public enum SemiAxisY
    {
        Up,
        Down,
    }

    public enum SemiAxisZ
    {
        Forward,
        Back,
    }

    public enum Axis3DEnum
    {
        X,
        Y,
        Z,
    }

    public enum SemiAxis3DEnum
    {
        Left,
        Right,
        Up,
        Down,
        Forward,
        Back,
    }

    public static class AxisDictonary
    {
        #region Private Fields
        private static readonly Dictionary<AxisCombined, Vector3> axisDictonary;
        #endregion

        #region Constructors
        static AxisDictonary()
        {
            axisDictonary = new Dictionary<AxisCombined, Vector3>
            {
                { AxisCombined.X, Vector3.right },
                { AxisCombined.Y, Vector3.up },
                { AxisCombined.Z, Vector3.forward },
                { AxisCombined.XY, new Vector3(1, 1, 0).normalized },
                { AxisCombined.XYZ, new Vector3(1, 1, 1).normalized },
                { AxisCombined.XZ, new Vector3(1, 0, 1).normalized },
                { AxisCombined.YZ, new Vector3(0, 1, 1).normalized }
            };
        }
        #endregion

        #region Public Methods
        public static Vector3 Get(AxisCombined axis)
        {
            return axisDictonary != null ? axisDictonary[axis] : default;
        }

        public static AxisCombined Get(Vector3 axis)
        {
            return axisDictonary != null && axisDictonary.TryGetKey(axis, out AxisCombined result) ? result : default;
        }
        #endregion
    }
}
