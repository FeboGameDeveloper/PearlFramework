using UnityEngine;

namespace Pearl
{
    public interface IMakeSound
    {
        float CurrentLevelSound { get; }

        void ChangeLevelSound(float sound);

        Transform Origin { get; }
    }
}
