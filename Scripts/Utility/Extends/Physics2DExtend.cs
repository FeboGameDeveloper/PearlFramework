﻿using UnityEngine;


namespace Pearl
{
    public static class Physics2DExtend
    {
        private static readonly int myLayer;
        private static readonly LayerMask maskInverse;

        static Physics2DExtend()
        {
            maskInverse = LayerExtend.CreateLayerMask("AuxLayer").Inverse();
            myLayer = LayerMask.NameToLayer("AuxLayer");
        }

        #region Velocity
        public static void SetVelocity(this Rigidbody2D @this, Vector2 newValue, ChangeTypeEnum changeTypeTransform, Axis2DCombined axisCombined = Axis2DCombined.XY)
        {
            if (@this)
            {
                Vector3 velocity = @this.velocity;
                @this.velocity = Vector2Extend.ChangeVector(velocity, newValue, changeTypeTransform, axisCombined);
            }
        }

        public static void SetXVelocity(this Rigidbody2D @this, float xValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            if (@this)
            {
                Vector3 velocity = @this.velocity;
                velocity = Vector2Extend.ChangeVector(velocity, Axis2DEnum.X, xValue, changeTypeTransform, range);
                @this.velocity = velocity;
            }
        }

        public static void SetYVelocity(this Rigidbody2D @this, float yValue, ChangeTypeEnum changeTypeTransform, Vector2 range = default)
        {
            if (@this)
            {
                Vector3 velocity = @this.velocity;
                velocity = Vector2Extend.ChangeVector(velocity, Axis2DEnum.Y, yValue, changeTypeTransform, range);
                @this.velocity = velocity;
            }
        }
        #endregion

        public static void SetVelocity(this Rigidbody2D @this, out bool isFinish, Vector2 velocity, Transform finalTransform, float neighbourhood = 0)
        {
            isFinish = false;
            if (finalTransform != null)
            {
                SetVelocity(@this, out isFinish, velocity, finalTransform.position, neighbourhood);
            }
        }

        public static void Translate(this Rigidbody2D @this, Vector2 traslateVector)
        {
            if (@this != null)
            {
                @this.MovePosition(@this.position + traslateVector);
            }
        }

        public static void SetVelocity(this Rigidbody2D @this, out bool isFinish, Vector3 translate, Vector3 finalPosition, float neighbourhood = 0)
        {
            isFinish = false;
            if (@this != null)
            {
                Vector3 currentPoition = @this.position;
                Vector3 delta = finalPosition - currentPoition;

                if (delta.ApproxZero() || (delta.normalized == translate.normalized && delta.magnitude < translate.magnitude))
                {
                    isFinish = true;
                }
                else if (neighbourhood != 0)
                {
                    neighbourhood = Mathf.Abs(neighbourhood);
                    Vector3 futureDestination = currentPoition + translate;
                    if (Vector3.Distance(finalPosition, futureDestination) <= neighbourhood)
                    {
                        isFinish = true;
                    }
                }

                if (isFinish)
                {
                    @this.position = finalPosition;
                }
                else
                {
                    @this.MovePosition(currentPoition + translate);
                }
            }
        }


        /// <summary>
        /// Draws a debug ray in 2D and does the actual raycast
        /// </summary>
        /// <returns>The raycast hit.</returns>
        /// <param name="rayOriginPoint">Ray origin point.</param>
        /// <param name="rayDirection">Ray direction.</param>
        /// <param name="rayDistance">Ray distance.</param>
        /// <param name="mask">Mask.</param>
        /// <param name="debug">If set to <c>true</c> debug.</param>
        /// <param name="color">Color.</param>
        public static RaycastHit2D RayCast(Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
        {
            if (drawGizmo)
            {
                UnityEngine.Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
            }
            return Physics2D.Raycast(rayOriginPoint, rayDirection, rayDistance, mask);
        }

        /// <summary>
        /// Does a boxcast and draws a box gizmo
        /// </summary>
        /// <param name="origin"></param>
        /// <param name="size"></param>
        /// <param name="angle"></param>
        /// <param name="direction"></param>
        /// <param name="length"></param>
        /// <param name="mask"></param>
        /// <param name="color"></param>
        /// <param name="drawGizmo"></param>
        /// <returns></returns>
        public static RaycastHit2D BoxCast(Vector2 origin, Vector2 size, float angle, Vector2 direction, float length, LayerMask mask, Color color, bool drawGizmo = false)
        {
            if (drawGizmo)
            {
                Quaternion rotation = Quaternion.Euler(0f, 0f, angle);

                Vector3[] points = new Vector3[8];

                float halfSizeX = size.x / 2f;
                float halfSizeY = size.y / 2f;

                points[0] = rotation * (origin + (Vector2.left * halfSizeX) + (Vector2.up * halfSizeY)); // top left
                points[1] = rotation * (origin + (Vector2.right * halfSizeX) + (Vector2.up * halfSizeY)); // top right
                points[2] = rotation * (origin + (Vector2.right * halfSizeX) - (Vector2.up * halfSizeY)); // bottom right
                points[3] = rotation * (origin + (Vector2.left * halfSizeX) - (Vector2.up * halfSizeY)); // bottom left

                points[4] = rotation * ((origin + Vector2.left * halfSizeX + Vector2.up * halfSizeY) + length * direction); // top left
                points[5] = rotation * ((origin + Vector2.right * halfSizeX + Vector2.up * halfSizeY) + length * direction); // top right
                points[6] = rotation * ((origin + Vector2.right * halfSizeX - Vector2.up * halfSizeY) + length * direction); // bottom right
                points[7] = rotation * ((origin + Vector2.left * halfSizeX - Vector2.up * halfSizeY) + length * direction); // bottom left

                UnityEngine.Debug.DrawLine(points[0], points[1], color);
                UnityEngine.Debug.DrawLine(points[1], points[2], color);
                UnityEngine.Debug.DrawLine(points[2], points[3], color);
                UnityEngine.Debug.DrawLine(points[3], points[0], color);

                UnityEngine.Debug.DrawLine(points[4], points[5], color);
                UnityEngine.Debug.DrawLine(points[5], points[6], color);
                UnityEngine.Debug.DrawLine(points[6], points[7], color);
                UnityEngine.Debug.DrawLine(points[7], points[4], color);

                UnityEngine.Debug.DrawLine(points[0], points[4], color);
                UnityEngine.Debug.DrawLine(points[1], points[5], color);
                UnityEngine.Debug.DrawLine(points[2], points[6], color);
                UnityEngine.Debug.DrawLine(points[3], points[7], color);

            }
            return Physics2D.BoxCast(origin, size, angle, direction, length, mask);
        }

        /// <summary>
        /// Draws a debug ray without allocating memory
        /// </summary>
        /// <returns>The ray cast non alloc.</returns>
        /// <param name="array">Array.</param>
        /// <param name="rayOriginPoint">Ray origin point.</param>
        /// <param name="rayDirection">Ray direction.</param>
        /// <param name="rayDistance">Ray distance.</param>
        /// <param name="mask">Mask.</param>
        /// <param name="color">Color.</param>
        /// <param name="drawGizmo">If set to <c>true</c> draw gizmo.</param>
        public static RaycastHit2D MonoRayCastNonAlloc(RaycastHit2D[] array, Vector2 rayOriginPoint, Vector2 rayDirection, float rayDistance, LayerMask mask, Color color, bool drawGizmo = false)
        {
            if (drawGizmo)
            {
                UnityEngine.Debug.DrawRay(rayOriginPoint, rayDirection * rayDistance, color);
            }
            if (Physics2D.RaycastNonAlloc(rayOriginPoint, rayDirection, array, rayDistance, mask) > 0)
            {
                return array[0];
            }
            return new RaycastHit2D();
        }

        public static void BoxCastAllWithoutGameObject(GameObject gameObject, out RaycastHit2D[] otherHits, in Vector2 origin, in Vector2 size, in float angle, in Vector2 direction, in float distance)
        {
            int auxLayer = gameObject.layer;
            gameObject.layer = myLayer;
            otherHits = UnityEngine.Physics2D.BoxCastAll(origin, size, angle, direction, distance, maskInverse);
            gameObject.layer = auxLayer;
        }

        public static void RayCastWithoutGameObject(GameObject gameObject, Vector2 origin, Vector2 direction, out RaycastHit2D otherHit, in float distance)
        {
            int auxLayer = gameObject.layer;
            gameObject.layer = myLayer;
            otherHit = UnityEngine.Physics2D.Raycast(origin, direction, distance, maskInverse);
            gameObject.layer = auxLayer;
        }

        public static bool LineRayIntersection(out Vector3 intersection, Vector3 rayPoint, Vector3 rayDir, Vector3 linePoint, Vector3 lineDir)
        {
            bool ThereIsContact = Math3D.LineLineIntersection(out intersection, linePoint, lineDir, rayPoint, rayDir);
            if (ThereIsContact)
            {
                if (rayDir.x == 0 && rayDir.y > 0)
                {
                    return intersection.y >= rayPoint.y;
                }
                if (rayDir.x == 0 && rayDir.y < 0)
                {
                    return intersection.y <= rayPoint.y;
                }
                if (rayDir.x > 0 && rayDir.y == 0)
                {
                    return intersection.x >= rayPoint.x;
                }
                if (rayDir.x < 0 && rayDir.y == 0)
                {
                    return intersection.x <= rayPoint.x;
                }


                if (rayDir.x > 0 && rayDir.y > 0)
                {
                    return intersection.x >= rayPoint.x && intersection.y >= rayPoint.y;
                }
                if (rayDir.x > 0 && rayDir.y < 0)
                {
                    return intersection.x >= rayPoint.x && intersection.y <= rayPoint.y;
                }
                if (rayDir.x < 0 && rayDir.y > 0)
                {
                    return intersection.x <= rayPoint.x && intersection.y >= rayPoint.y;
                }
                if (rayDir.x < 0 && rayDir.y < 0)
                {
                    return intersection.x <= rayPoint.x && intersection.y <= rayPoint.y;
                }

            }
            return false;
        }

        public static bool RayCasterInsideColliderWithBoundsWithoutGameObject(GameObject gameObject, float angle, ref int index, ref Vector3 interactPoint, in Vector2 initPosition, in Vector2 sizeBox, in Vector2 translationVector)
        {
            int auxIndex = -1;
            Vector3 auxVector1 = Vector3.zero;
            Vector3 auxVector2 = Vector3.zero;
            RaycastHit2D[] hits;

            Physics2DExtend.BoxCastAllWithoutGameObject(gameObject, out hits, initPosition, sizeBox, angle, Vector2.zero, 0);
            for (int i = 0; i < hits.Length; i++)
            {
                if (hits[i].collider.isTrigger)
                {
                    auxIndex = i;
                    break;
                }
            }

            if (auxIndex == -1)
            {
                return false;
            }
            else
            {
                index = auxIndex;
                if (translationVector.x > 0)
                    auxVector1 = new Vector2(hits[auxIndex].collider.bounds.max.x, hits[0].collider.bounds.min.y);
                else if (translationVector.x < 0)
                    auxVector1 = new Vector2(hits[auxIndex].collider.bounds.min.x, hits[0].collider.bounds.min.y);

                if (translationVector.y > 0)
                    auxVector2 = new Vector2(hits[auxIndex].collider.bounds.min.x, hits[0].collider.bounds.max.y);
                else if (translationVector.y < 0)
                    auxVector2 = new Vector2(hits[auxIndex].collider.bounds.min.x, hits[0].collider.bounds.min.y);
            }

            Physics2DExtend.BoxCastAllWithoutGameObject(gameObject, out hits, initPosition + translationVector, sizeBox, angle, Vector2.zero, 0);
            if (hits.Length != 0)
            {
                return false;
            }
            if (translationVector.y == 0)
            {
                Math3D.LineLineIntersection(out interactPoint, initPosition, translationVector, auxVector1, Vector2.up);

            }
            else if (translationVector.x == 0)
            {
                Math3D.LineLineIntersection(out interactPoint, initPosition, translationVector, auxVector2, Vector2.right);
            }
            else
            {
                Math3D.LineLineIntersection(out auxVector1, initPosition, translationVector, auxVector1, Vector2.up);
                Math3D.LineLineIntersection(out auxVector2, initPosition, translationVector, auxVector2, Vector2.right);
                if (Vector3.Distance(initPosition, auxVector1) > Vector3.Distance(initPosition, auxVector2))
                {
                    interactPoint = auxVector2;
                }
                else
                {
                    interactPoint = auxVector1;
                }
            }
            return true;
        }
    }
}
