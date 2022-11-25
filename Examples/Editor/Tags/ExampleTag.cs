using Pearl.Multitags;
using UnityEngine;

namespace Pearl.Examples.TagsExample
{
    public class ExampleTag : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var a = MultiTagsManager.FindGameObjectWithMultiTags(true, "enemy");
            UnityEngine.Debug.Log(a);
            a = MultiTagsManager.FindGameObjectWithMultiTags(false, "enemy");
            UnityEngine.Debug.Log(a);
            a = MultiTagsManager.FindGameObjectWithMultiTags(true, "enemy", "player");
            UnityEngine.Debug.Log(a);
            UnityEngine.Debug.Log(a.GetTags().PrintElements());
            UnityEngine.Debug.Log(a.HasTags(true, "player"));
            UnityEngine.Debug.Log(a.HasTags(false, "player"));
            a.RemoveTags("player");
            UnityEngine.Debug.Log(a.HasTags(false, "player"));
        }
    }
}
