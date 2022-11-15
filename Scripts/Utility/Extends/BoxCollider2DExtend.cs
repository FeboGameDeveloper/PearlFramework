using UnityEngine;

namespace Pearl
{
    public enum RectangleEdge { Left, Right, Up, Down }

    public enum RectanglePoints { LeftUp, RightUp, RightDown, LeftDown }

    public static class BoxCollider2DExtend
    {
        public static Vector2 GetPerimeterPoint(this BoxCollider2D collider, RectangleEdge edge, float lerpValue)
        {
            float quaternionAngle = collider.transform.rotation.eulerAngles.z;

            Vector3 center = collider.bounds.center;
            Vector2 extens = collider.transform.lossyScale * 0.5f;
            extens.Scale(collider.size);

            Vector2 pointLeftUp = new Vector2(center.x - extens.x, center.y + extens.y);
            Vector2 pointRightUp = new Vector2(center.x + extens.x, center.y + extens.y);
            Vector2 pointRightDown = new Vector2(center.x + extens.x, center.y - extens.y);
            Vector2 pointLeftDown = new Vector2(center.x - extens.x, center.y - extens.y);

            if (quaternionAngle != 0)
            {
                pointLeftUp = QuaternionExtend.RotationByCenter(pointLeftUp, center, quaternionAngle);
                pointRightUp = QuaternionExtend.RotationByCenter(pointRightUp, center, quaternionAngle);
                pointRightDown = QuaternionExtend.RotationByCenter(pointRightDown, center, quaternionAngle);
                pointLeftDown = QuaternionExtend.RotationByCenter(pointLeftDown, center, quaternionAngle);
            }

            switch (edge)
            {
                case RectangleEdge.Up:
                    return Vector2.Lerp(pointLeftUp, pointRightUp, lerpValue);
                case RectangleEdge.Right:
                    return Vector2.Lerp(pointRightUp, pointRightDown, lerpValue);
                case RectangleEdge.Down:
                    return Vector2.Lerp(pointRightDown, pointLeftDown, lerpValue);
                case RectangleEdge.Left:
                    return Vector2.Lerp(pointLeftDown, pointLeftUp, lerpValue);
            }

            return Vector2.zero;
        }



    }
}
