
using System;
using UnityEngine;

namespace Pearl
{
    public enum ColorEnum
    {
        ReunoYellow,
        BestRed,
        AliceBlue,
        AntiqueWhite,
        Aqua,
        Aquamarine,
        Azure,
        Beige,
        Bisque,
        Black,
        BlanchedAlmond,
        Blue,
        BlueViolet,
        Brown,
        Burlywood,
        CadetBlue,
        Chartreuse,
        Chocolate,
        Coral,
        CornflowerBlue,
        Cornsilk,
        Crimson,
        Cyan,
        DarkBlue,
        DarkCyan,
        DarkGoldenrod,
        DarkGray,
        DarkGreen,
        DarkKhaki,
        DarkMagenta,
        DarkOliveGreen,
        DarkOrange,
        DarkOrchid,
        DarkRed,
        DarkSalmon,
        DarkSeaGreen,
        DarkSlateBlue,
        DarkSlateGray,
        DarkTurquoise,
        DarkViolet,
        DeepPink,
        DeepSkyBlue,
        DimGray,
        DodgerBlue,
        FireBrick,
        FloralWhite,
        ForestGreen,
        Fuchsia,
        Gainsboro,
        GhostWhite,
        Gold,
        Goldenrod,
        Gray,
        Green,
        GreenYellow,
        Honeydew,
        HotPink,
        IndianRed,
        Indigo,
        Ivory,
        Khaki,
        Lavender,
        Lavenderblush,
        LawnGreen,
        LemonChiffon,
        LightBlue,
        LightCoral,
        LightCyan,
        LightGoldenodYellow,
        LightGray,
        LightGreen,
        LightPink,
        LightSalmon,
        LightSeaGreen,
        LightSkyBlue,
        LightSlateGray,
        LightSteelBlue,
        LightYellow,
        Lime,
        LimeGreen,
        Linen,
        Magenta,
        Maroon,
        MediumAquamarine,
        MediumBlue,
        MediumOrchid,
        MediumPurple,
        MediumSeaGreen,
        MediumSlateBlue,
        MediumSpringGreen,
        MediumTurquoise,
        MediumVioletRed,
        MidnightBlue,
        Mintcream,
        MistyRose,
        Moccasin,
        NavajoWhite,
        Navy,
        OldLace,
        Olive,
        Olivedrab,
        Orange,
        Orangered,
        Orchid,
        PaleGoldenrod,
        PaleGreen,
        PaleTurquoise,
        PaleVioletred,
        PapayaWhip,
        PeachPuff,
        Peru,
        Pink,
        Plum,
        PowderBlue,
        Purple,
        Red,
        RosyBrown,
        RoyalBlue,
        SaddleBrown,
        Salmon,
        SandyBrown,
        SeaGreen,
        Seashell,
        Sienna,
        Silver,
        SkyBlue,
        SlateBlue,
        SlateGray,
        Snow,
        SpringGreen,
        SteelBlue,
        Tan,
        Teal,
        Thistle,
        Tomato,
        Turquoise,
        Violet,
        Wheat,
        White,
        WhiteSmoke,
        Yellow,
        YellowGreen,
    }


    public static class ColorExtend
    {
        public static Color RandomColor()
        {
            ColorEnum randomColor = EnumExtend.GetRandom<ColorEnum>();
            return GetColor(randomColor);
        }

        public static Color GetColor(ColorEnum color, float alpha = 1)
        {
            byte a = (byte)(byte.MaxValue * Mathf.Clamp01(alpha));

            switch (color)
            {
                case ColorEnum.YellowGreen: return new Color32(154, 205, 50, a);
                case ColorEnum.Yellow: return new Color32(255, 255, 0, a);
                case ColorEnum.WhiteSmoke: return new Color32(245, 245, 245, a);
                case ColorEnum.White: return new Color32(255, 255, 255, a);
                case ColorEnum.Wheat: return new Color32(245, 222, 179, a);
                case ColorEnum.Violet: return new Color32(238, 130, 238, a);
                case ColorEnum.Turquoise: return new Color32(64, 224, 208, a);
                case ColorEnum.Tomato: return new Color32(255, 99, 71, a);
                case ColorEnum.Thistle: return new Color32(216, 191, 216, a);
                case ColorEnum.Teal: return new Color32(0, 128, 128, a);
                case ColorEnum.Tan: return new Color32(210, 180, 140, a);
                case ColorEnum.ReunoYellow: return new Color32(255, 196, 0, a);
                case ColorEnum.BestRed: return new Color32(255, 24, 0, a);
                case ColorEnum.AliceBlue: return new Color32(240, 248, 255, a);
                case ColorEnum.AntiqueWhite: return new Color32(250, 235, 215, a);
                case ColorEnum.Aqua: return new Color32(0, 255, 255, a);
                case ColorEnum.Aquamarine: return new Color32(127, 255, 212, a);
                case ColorEnum.Azure: return new Color32(240, 255, 255, a);
                case ColorEnum.Beige: return new Color32(245, 245, 220, a);
                case ColorEnum.Bisque: return new Color32(255, 228, 196, a);
                case ColorEnum.Black: return new Color32(0, 0, 0, a);
                case ColorEnum.BlanchedAlmond: return new Color32(255, 235, 205, a);
                case ColorEnum.Blue: return new Color32(0, 0, 255, a);
                case ColorEnum.BlueViolet: return new Color32(138, 43, 226, a);
                case ColorEnum.Brown: return new Color32(165, 42, 42, a);
                case ColorEnum.Burlywood: return new Color32(222, 184, 135, a);
                case ColorEnum.CadetBlue: return new Color32(95, 158, 160, a);
                case ColorEnum.Chartreuse: return new Color32(127, 255, 0, a);
                case ColorEnum.Chocolate: return new Color32(210, 105, 30, a);
                case ColorEnum.Coral: return new Color32(255, 127, 80, a);
                case ColorEnum.CornflowerBlue: return new Color32(100, 149, 237, a);
                case ColorEnum.Cornsilk: return new Color32(255, 248, 220, a);
                case ColorEnum.Crimson: return new Color32(220, 20, 60, a);
                case ColorEnum.Cyan: return new Color32(0, 255, 255, a);
                case ColorEnum.DarkBlue: return new Color32(0, 0, 139, a);
                case ColorEnum.DarkCyan: return new Color32(0, 139, 139, a);
                case ColorEnum.DarkGoldenrod: return new Color32(184, 134, 11, a);
                case ColorEnum.DarkGray: return new Color32(169, 169, 169, a);
                case ColorEnum.DarkGreen: return new Color32(0, 100, 0, a);
                case ColorEnum.DarkKhaki: return new Color32(189, 183, 107, a);
                case ColorEnum.DarkMagenta: return new Color32(139, 0, 139, a);
                case ColorEnum.DarkOliveGreen: return new Color32(85, 107, 47, a);
                case ColorEnum.DarkOrange: return new Color32(255, 140, 0, a);
                case ColorEnum.DarkOrchid: return new Color32(153, 50, 204, a);
                case ColorEnum.DarkRed: return new Color32(139, 0, 0, a);
                case ColorEnum.DarkSalmon: return new Color32(233, 150, 122, a);
                case ColorEnum.DarkSeaGreen: return new Color32(143, 188, 143, a);
                case ColorEnum.DarkSlateBlue: return new Color32(72, 61, 139, a);
                case ColorEnum.DarkSlateGray: return new Color32(47, 79, 79, a);
                case ColorEnum.DarkTurquoise: return new Color32(0, 206, 209, a);
                case ColorEnum.DarkViolet: return new Color32(148, 0, 211, a);
                case ColorEnum.DeepPink: return new Color32(255, 20, 147, a);
                case ColorEnum.DeepSkyBlue: return new Color32(0, 191, 255, a);
                case ColorEnum.DimGray: return new Color32(105, 105, 105, a);
                case ColorEnum.DodgerBlue: return new Color32(30, 144, 255, a);
                case ColorEnum.FireBrick: return new Color32(178, 34, 34, a);
                case ColorEnum.FloralWhite: return new Color32(255, 250, 240, a);
                case ColorEnum.ForestGreen: return new Color32(34, 139, 34, a);
                case ColorEnum.Fuchsia: return new Color32(255, 0, 255, a);
                case ColorEnum.Gainsboro: return new Color32(220, 220, 220, a);
                case ColorEnum.GhostWhite: return new Color32(248, 248, 255, a);
                case ColorEnum.Gold: return new Color32(255, 215, 0, a);
                case ColorEnum.Goldenrod: return new Color32(218, 165, 32, a);
                case ColorEnum.Gray: return new Color32(128, 128, 128, a);
                case ColorEnum.Green: return new Color32(0, 128, 0, a);
                case ColorEnum.GreenYellow: return new Color32(173, 255, 47, a);
                case ColorEnum.Honeydew: return new Color32(240, 255, 240, a);
                case ColorEnum.HotPink: return new Color32(255, 105, 180, a);
                case ColorEnum.IndianRed: return new Color32(205, 92, 92, a);
                case ColorEnum.Indigo: return new Color32(75, 0, 130, a);
                case ColorEnum.Ivory: return new Color32(255, 255, 240, a);
                case ColorEnum.Khaki: return new Color32(240, 230, 140, a);
                case ColorEnum.Lavender: return new Color32(230, 230, 250, a);
                case ColorEnum.Lavenderblush: return new Color32(255, 240, 245, a);
                case ColorEnum.LawnGreen: return new Color32(124, 252, 0, a);
                case ColorEnum.LemonChiffon: return new Color32(255, 250, 205, a);
                case ColorEnum.LightBlue: return new Color32(173, 216, 230, a);
                case ColorEnum.LightCoral: return new Color32(240, 128, 128, a);
                case ColorEnum.LightCyan: return new Color32(224, 255, 255, a);
                case ColorEnum.LightGoldenodYellow: return new Color32(250, 250, 210, a);
                case ColorEnum.LightGray: return new Color32(211, 211, 211, a);
                case ColorEnum.LightGreen: return new Color32(144, 238, 144, a);
                case ColorEnum.LightPink: return new Color32(255, 182, 193, a);
                case ColorEnum.LightSalmon: return new Color32(255, 160, 122, a);
                case ColorEnum.LightSeaGreen: return new Color32(32, 178, 170, a);
                case ColorEnum.LightSkyBlue: return new Color32(135, 206, 250, a);
                case ColorEnum.LightSlateGray: return new Color32(119, 136, 153, a);
                case ColorEnum.LightSteelBlue: return new Color32(176, 196, 222, a);
                case ColorEnum.LightYellow: return new Color32(255, 255, 224, a);
                case ColorEnum.Lime: return new Color32(0, 255, 0, a);
                case ColorEnum.LimeGreen: return new Color32(50, 205, 50, a);
                case ColorEnum.Linen: return new Color32(250, 240, 230, a);
                case ColorEnum.Magenta: return new Color32(255, 0, 255, a);
                case ColorEnum.Maroon: return new Color32(128, 0, 0, a);
                case ColorEnum.MediumAquamarine: return new Color32(102, 205, 170, a);
                case ColorEnum.MediumBlue: return new Color32(0, 0, 205, a);
                case ColorEnum.MediumOrchid: return new Color32(186, 85, 211, a);
                case ColorEnum.MediumPurple: return new Color32(147, 112, 219, a);
                case ColorEnum.MediumSeaGreen: return new Color32(60, 179, 113, a);
                case ColorEnum.MediumSlateBlue: return new Color32(123, 104, 238, a);
                case ColorEnum.MediumSpringGreen: return new Color32(0, 250, 154, a);
                case ColorEnum.MediumTurquoise: return new Color32(72, 209, 204, a);
                case ColorEnum.MediumVioletRed: return new Color32(199, 21, 133, a);
                case ColorEnum.MidnightBlue: return new Color32(25, 25, 112, a);
                case ColorEnum.Mintcream: return new Color32(245, 255, 250, a);
                case ColorEnum.MistyRose: return new Color32(255, 228, 225, a);
                case ColorEnum.Moccasin: return new Color32(255, 228, 181, a);
                case ColorEnum.NavajoWhite: return new Color32(255, 222, 173, a);
                case ColorEnum.Navy: return new Color32(0, 0, 128, a);
                case ColorEnum.OldLace: return new Color32(253, 245, 230, a);
                case ColorEnum.Olive: return new Color32(128, 128, 0, a);
                case ColorEnum.Olivedrab: return new Color32(107, 142, 35, a);
                case ColorEnum.Orange: return new Color32(255, 165, 0, a);
                case ColorEnum.Orangered: return new Color32(255, 69, 0, a);
                case ColorEnum.Orchid: return new Color32(218, 112, 214, a);
                case ColorEnum.PaleGoldenrod: return new Color32(238, 232, 170, a);
                case ColorEnum.PaleGreen: return new Color32(152, 251, 152, a);
                case ColorEnum.PaleTurquoise: return new Color32(175, 238, 238, a);
                case ColorEnum.PaleVioletred: return new Color32(219, 112, 147, a);
                case ColorEnum.PapayaWhip: return new Color32(255, 239, 213, a);
                case ColorEnum.PeachPuff: return new Color32(255, 218, 185, a);
                case ColorEnum.Peru: return new Color32(205, 133, 63, a);
                case ColorEnum.Pink: return new Color32(255, 192, 203, a);
                case ColorEnum.Plum: return new Color32(221, 160, 221, a);
                case ColorEnum.PowderBlue: return new Color32(176, 224, 230, a);
                case ColorEnum.Purple: return new Color32(128, 0, 128, a);
                case ColorEnum.Red: return new Color32(255, 0, 0, a);
                case ColorEnum.RosyBrown: return new Color32(188, 143, 143, a);
                case ColorEnum.RoyalBlue: return new Color32(65, 105, 225, a);
                case ColorEnum.SaddleBrown: return new Color32(139, 69, 19, a);
                case ColorEnum.Salmon: return new Color32(250, 128, 114, a);
                case ColorEnum.SandyBrown: return new Color32(244, 164, 96, a);
                case ColorEnum.SeaGreen: return new Color32(46, 139, 87, a);
                case ColorEnum.Seashell: return new Color32(255, 245, 238, a);
                case ColorEnum.Sienna: return new Color32(160, 82, 45, a);
                case ColorEnum.Silver: return new Color32(192, 192, 192, a);
                case ColorEnum.SkyBlue: return new Color32(135, 206, 235, a);
                case ColorEnum.SlateBlue: return new Color32(106, 90, 205, a);
                case ColorEnum.SlateGray: return new Color32(112, 128, 144, a);
                case ColorEnum.Snow: return new Color32(255, 250, 250, a);
                case ColorEnum.SpringGreen: return new Color32(0, 255, 127, a);
                case ColorEnum.SteelBlue: return new Color32(70, 130, 180, a);
            }

            return new Color(255, 255, 255);
        }

        public static Color GetColor(string hexValues, float transparent = 1)
        {
            if (hexValues == null)
            {
                return default;
            }

            hexValues.Replace("#", "");
            hexValues.Replace(" ", "");
            transparent = Mathf.Clamp01(transparent);
            string hex;
            if (hexValues.Length == 6)
            {
                hex = hexValues.Substring(0, 2);
                int value1 = Convert.ToInt32(hex, 16);

                hex = hexValues.Substring(2, 2);
                int value2 = Convert.ToInt32(hex, 16);

                hex = hexValues.Substring(4, 2);
                int value3 = Convert.ToInt32(hex, 16);

                return new Color(value1, value2, value3, transparent);
            }
            return default;
        }

        /// <summary>
        /// Returns a random color between the two min/max specified
        /// </summary>
        /// <param name="color"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Color RandomColor(this Color color, Color min, Color max)
        {
            Color c = new Color()
            {
                r = UnityEngine.Random.Range(min.r, max.r),
                g = UnityEngine.Random.Range(min.g, max.g),
                b = UnityEngine.Random.Range(min.b, max.b),
                a = UnityEngine.Random.Range(min.a, max.a)
            };

            return c;
        }

        /// <summary>
        /// Returns a random color between the two min/max specified
        /// </summary>
        /// <param name="color"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static Color RandomColor(this Color color)
        {
            return RandomColor(color, new Color(0, 0, 0), new Color(255, 255, 255));
        }


        /// <summary>
        /// Tint : Uses HSV color conversions, keeps the original values, multiplies alpha
        /// Multiply : The whole color, including alpha, is multiplied over the original 
        /// Replace : completely replaces the original with the target color
        /// ReplaceKeepAlpha : color is replaced but the original alpha channel is ignored
        /// Add : target color gets added (including its alpha)
        /// </summary>
        public enum ColoringMode { Tint, Multiply, Replace, ReplaceKeepAlpha, Add }

        public static Color Colorize(this Color originalColor, Color targetColor, ColoringMode coloringMode, float lerpAmount = 1.0f)
        {
            Color resultColor = Color.white;
            switch (coloringMode)
            {
                case ColoringMode.Tint:
                    {
                        Color.RGBToHSV(originalColor, out float s_h, out float s_s, out float s_v);
                        Color.RGBToHSV(targetColor, out float t_h, out float t_s, out float t_v);
                        resultColor = Color.HSVToRGB(t_h, t_s, s_v * t_v);
                        resultColor.a = originalColor.a * targetColor.a;
                    }
                    break;
                case ColoringMode.Multiply:
                    resultColor = originalColor * targetColor;
                    break;
                case ColoringMode.Replace:
                    resultColor = targetColor;
                    break;
                case ColoringMode.ReplaceKeepAlpha:
                    resultColor = targetColor;
                    resultColor.a = originalColor.a;
                    break;
                case ColoringMode.Add:
                    resultColor = originalColor + targetColor;
                    break;
                default:
                    break;
            }
            return Color.Lerp(originalColor, resultColor, lerpAmount);
        }
    }
}
