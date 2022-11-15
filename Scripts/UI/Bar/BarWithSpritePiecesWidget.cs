using UnityEngine;

namespace Pearl
{
    public class BarWithSpritePiecesWidget : MonoBehaviour
    {

        [SerializeField]
        private Sprite[] pieces = null;

        [SerializeField]
        private GameObject barImage = null;

        [SerializeField]
        private bool circleBar = false;

        private int currentPiece = 0;
        private SpriteManager spriteManagr;


        private void Awake()
        {
            spriteManagr = new SpriteManager(barImage);
            SetBar();
        }

        public void AddPiece()
        {
            currentPiece = Mathf.Min(++currentPiece, pieces.Length - 1);
            SetBar();
        }

        public void RemovePiece()
        {
            currentPiece = Mathf.Max(--currentPiece, 0);
            SetBar();
        }

        public void Reset()
        {
            currentPiece = 0;
            SetBar();
        }

        private void SetBar()
        {
            if (circleBar)
            {
                currentPiece = MathfExtend.ChangeInCircle(currentPiece, 1, pieces.Length);
            }

            if (pieces != null && barImage != null && currentPiece < pieces.Length)
            {
                spriteManagr.SetSprite(pieces[currentPiece]);
            }
        }

    }
}
