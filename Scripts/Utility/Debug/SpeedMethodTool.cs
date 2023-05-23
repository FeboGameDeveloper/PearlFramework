using Pearl.Testing;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl
{
    public static class SpeedMethodTool
    {
        public class SpeedMethodInfo
        {
            #region Private Fields
            private DynamicVariance dynamicVariance = new DynamicVariance();
            private double minValueSpeed = Mathf.Infinity;
            private double maxValueSpeed = 0;
            #endregion

            #region Properties
            public double Variance { get { return dynamicVariance.Variance; } }
            public double Average { get { return dynamicVariance.Average; } }
            public double MinValueSpeed { get { return minValueSpeed; } }
            public double MaxValueSpeed { get { return maxValueSpeed; } }
            #endregion

            #region Public Methods
            public void AddElementInPopolation(double newElement)
            {
                if (dynamicVariance != null)
                {
                    dynamicVariance.AddElementInPopolation(newElement);

                    if (newElement < minValueSpeed)
                    {
                        minValueSpeed = newElement;
                    }

                    if (newElement > maxValueSpeed)
                    {
                        maxValueSpeed = newElement;
                    }
                }
            }
            #endregion
        }

        #region Private  Fields
        private static readonly Dictionary<string, SpeedMethodInfo> mapAverageMethod;
        private static readonly Dictionary<string, float> startTimeMethod;
        #endregion

        #region Constructors
        static SpeedMethodTool()
        {
            mapAverageMethod = new Dictionary<string, SpeedMethodInfo>();
            startTimeMethod = new Dictionary<string, float>();
        }
        #endregion

        #region Public Methods
        public static void StartCalculateMethod(string nameMethod)
        {
            if (nameMethod != null)
            {
                if (mapAverageMethod != null && !mapAverageMethod.ContainsKey(nameMethod))
                {
                    mapAverageMethod.Add(nameMethod, new SpeedMethodInfo());
                }

                if (startTimeMethod != null)
                {
                    startTimeMethod.Update(nameMethod, Time.realtimeSinceStartup);
                }
            }
        }

        public static void FinishCalculateMethod(string nameMethod, bool log = false)
        {
            if (nameMethod != null && startTimeMethod != null && mapAverageMethod != null &&
                startTimeMethod.TryGetValue(nameMethod, out float startTime) &&
                mapAverageMethod.TryGetValue(nameMethod, out SpeedMethodInfo infoMethod))
            {
                double currentTime = Time.realtimeSinceStartup;
                double newElement = currentTime - startTime;

                infoMethod.AddElementInPopolation(newElement);

                if (log)
                {
                    string auxString = string.Format("{0:.00000000000}", newElement);
                    LogManager.Log(auxString);
                }
            }
        }

        public static void ReadMethodSpeed(string nameMethod)
        {
            if (mapAverageMethod != null && mapAverageMethod.TryGetValue(nameMethod, out SpeedMethodInfo infoMethod))
            {
                infoMethod = mapAverageMethod[nameMethod];
                string auxString = string.Format("{0} => New Istance; Average: {1:.00000000000}, " +
                    "Variance: {2:.00000000000}, Max: {3:.00000000000}, Min: {4:.00000000000}", nameMethod,
                    infoMethod.Average, infoMethod.Variance, infoMethod.MaxValueSpeed, infoMethod.MinValueSpeed);
                LogManager.Log(auxString);
            }
        }
        #endregion
    }
}
