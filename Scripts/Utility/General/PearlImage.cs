using UnityEngine.UI;

namespace Pearl
{
    public class PearlImage : Image, IFill
    {
        public void Fill(float percent)
        {
            fillAmount = percent;
        }
    }
}
