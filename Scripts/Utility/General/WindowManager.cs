using Pearl;
using Pearl.Events;
using UnityEngine;

public static class WindowManager
{
    private static Vector2 _lastScreenSize;

    static WindowManager()
    {
        _lastScreenSize = new Vector2(Screen.width, Screen.height);
    }

    public static void OnUpdate()
    {
        Vector2 screenSize = new(Screen.width, Screen.height);

        if (_lastScreenSize != screenSize)
        {
            _lastScreenSize = screenSize;
            PearlEventsManager.CallEvent(ConstantStrings.ChangeResolution, PearlEventType.Normal);
        }
    }

}