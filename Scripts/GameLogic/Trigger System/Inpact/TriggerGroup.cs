using UnityEngine;

public class TriggerGroup : MonoBehaviour
{
    [SerializeField]
    private string groupName = string.Empty;

    private int hashCode;

    private void Awake()
    {
        hashCode = groupName.GetHashCode();
    }

    public int CodeID { get { return hashCode; } }

}
