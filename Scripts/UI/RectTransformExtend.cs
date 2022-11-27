using UnityEngine;

namespace Pearl.UI
{
    public static class RectTransformExtend
    {
        #region Public

        public static void AnchorsToCorners(this Transform @this)
        {
            if (@this != null)
            {
                if (@this.TryGetComponent<RectTransform>(out var rect))
                {
                    rect.AnchorsToCorners();
                }
            }

        }

        public static void AnchorsToCorners(this RectTransform @this)
        {
            if (@this == null)
            {
                return;
            }

            RectTransform pt = @this.parent.GetComponent<RectTransform>();

            if (pt == null)
            {
                return;
            }

            Vector2 newAnchorsMin = new (@this.anchorMin.x + @this.offsetMin.x / pt.rect.width,
                                                @this.anchorMin.y + @this.offsetMin.y / pt.rect.height);
            Vector2 newAnchorsMax = new (@this.anchorMax.x + @this.offsetMax.x / pt.rect.width,
                                                @this.anchorMax.y + @this.offsetMax.y / pt.rect.height);

            @this.anchorMin = newAnchorsMin;
            @this.anchorMax = newAnchorsMax;
            @this.offsetMin = @this.offsetMax = Vector2.zero;
        }



        /// <summary>
        /// The screen space of rectTransform pivot
        /// </summary>
        /// <param name="@this"></param>
        public static Vector2 CanvasOverlayToScreenPoint(this RectTransform @this)
        {
            return @this != null ? RectTransformUtility.WorldToScreenPoint(null, @this.position) : Vector2.zero;
        }

        /// <summary>
        /// Sets the left offset of a rect transform to the specified value
        /// </summary>
        /// <param name="@this"></param>
        /// <param name="left"></param>
        public static void SetLeft(this RectTransform @this, float left)
        {
            AssertExtend.PreConditions("rectTransform", @this);
            if (@this == null)
            {
                return;
            }

            @this.offsetMin = new Vector2(left, @this.offsetMin.y);
        }

        /// <summary>
        /// Sets the right offset of a rect transform to the specified value
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="right"></param>
        public static void SetRight(this RectTransform @this, float right)
        {
            AssertExtend.PreConditions("rectTransform", @this);
            if (@this == null)
            {
                return;
            }

            @this.offsetMax = new Vector2(-right, @this.offsetMax.y);
        }

        /// <summary>
        /// Sets the top offset of a rect transform to the specified value
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="top"></param>
        public static void SetTop(this RectTransform @this, float top)
        {
            AssertExtend.PreConditions("rectTransform", @this);
            if (@this == null)
            {
                return;
            }

            @this.offsetMax = new Vector2(@this.offsetMax.x, -top);
        }

        /// <summary>
        /// Sets the bottom offset of a rect transform to the specified value
        /// </summary>
        /// <param name="rt"></param>
        /// <param name="bottom"></param>
        public static void SetBottom(this RectTransform @this, float bottom)
        {
            AssertExtend.PreConditions("rectTransform", @this);
            if (@this == null)
            {
                return;
            }

            @this.offsetMin = new Vector2(@this.offsetMin.x, bottom);
        }

        /// <summary>
        /// Determines if this RectTransform is fully visible.
        /// Works by checking if each bounding box corner of this RectTransform is inside the screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is fully visible; otherwise, <c>false</c>.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera. Leave it null for Overlay Canvasses.</param>
        public static bool IsFullyVisibleFrom(this RectTransform @this, Camera camera = null)
        {
            return CountCornersVisibleFrom(@this, camera) == 4; // True if all 4 corners are visible
        }

        /// <summary>
        /// Determines if this RectTransform is at least partially visible.
        /// Works by checking if any bounding box corner of this RectTransform is inside the screen space view frustrum.
        /// </summary>
        /// <returns><c>true</c> if is at least partially visible; otherwise, <c>false</c>.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera. Leave it null for Overlay Canvasses.</param>
        public static bool IsVisibleFrom(this RectTransform @this, Camera camera = null)
        {
            return CountCornersVisibleFrom(@this, camera) > 0; // True if any corners are visible
        }
        #endregion

        /// <summary>
        /// Counts the bounding box corners of the given RectTransform that are visible in screen space.
        /// </summary>
        /// <returns>The amount of bounding box corners that are visible.</returns>
        /// <param name="rectTransform">Rect transform.</param>
        /// <param name="camera">Camera. Leave it null for Overlay Canvasses.</param>
        private static int CountCornersVisibleFrom(this RectTransform @this, Camera camera = null)
        {
            if (@this == null && !@this.gameObject.activeInHierarchy)
            {
                return -1;
            }

            Rect screenBounds = new(0f, 0f, Screen.width, Screen.height); // Screen space bounds (assumes camera renders across the entire screen)
            Vector3[] objectCorners = new Vector3[4];
            @this.GetWorldCorners(objectCorners);

            int visibleCorners = 0;
            Vector3 tempScreenSpaceCorner; // Cached
            for (var i = 0; i < objectCorners.Length; i++) // For each corner in rectTransform
            {
                tempScreenSpaceCorner = camera != null ? camera.WorldToScreenPoint(objectCorners[i]) : objectCorners[i];
                // Transform world space position of corner to screen space,
                // no camera is provided we assume the canvas is Overlay and world space == screen space

                if (screenBounds.Contains(tempScreenSpaceCorner)) // If the corner is inside the screen
                {
                    visibleCorners++;
                }
            }
            return visibleCorners;
        }

        public static Rect RectTransformToScreenSpace(this RectTransform @this)
        {
            Vector2 size = Vector2.Scale(@this.rect.size, @this.lossyScale);
            float x = @this.position.x + @this.anchoredPosition.x;
            float y = Screen.height - @this.position.y - @this.anchoredPosition.y;

            return new Rect(x, y, size.x, size.y);
        }
    }
}

