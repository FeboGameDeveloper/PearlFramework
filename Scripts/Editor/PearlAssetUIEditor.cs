using UnityEditor;
using UnityEngine;


namespace Pearl.Editor
{
    public class PearlAssetUIEditor : MonoBehaviour
    {
        [MenuItem("GameObject/UI/Pearl/PearlButton")]
        public static void CreatePearlButton()
        {

        }

        [MenuItem("GameObject/UI/Pearl/PearlScrollView")]
        public static void CreatePearlScrollView()
        {

        }

        [MenuItem("GameObject/UI/Pearl/PearlText")]
        public static void CreatePearlText()
        {
        }

        [MenuItem("GameObject/UI/Pearl/HorizontalNavbar")]
        public static void CreatePearlHorizontalNavbar()
        {
        }

        [MenuItem("GameObject/UI/Pearl/NavbarElement")]
        public static void CreatePearlNavbarElement()
        {
        }

        [MenuItem("GameObject/UI/Pearl/DynamicBarWidget")]
        public static void CreatePearlDynamicBarWidget()
        {
            CreateEventSystem();
        }

        [MenuItem("GameObject/UI/Pearl/InputElementWidget")]
        public static void CreatePearlInputElementWidget()
        {
            CreateEventSystem();
        }

        [MenuItem("GameObject/UI/Pearl/PearlSlider")]
        public static void CreatePearlSlider()
        {
            CreateEventSystem();
        }

        [MenuItem("GameObject/UI/Pearl/SwitchText")]
        public static void CreatePearlSwitchTextWidget()
        {
            CreateEventSystem();
        }

        [MenuItem("GameObject/UI/Pearl/SwitchImage")]
        public static void CreatePearlSwitchImageWidget()
        {
            CreateEventSystem();
        }


        [MenuItem("GameObject/UI/Pearl/Canvas")]
        public static void CreatePearlCanvas()
        {
            CreateEventSystem();
        }

        [MenuItem("GameObject/UI/Pearl/EventSystem")]
        public static void CreateEventSystem()
        {
        }
    }
}
