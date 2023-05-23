using System;
using UnityEngine;

namespace Pearl
{
    public static class PhysicsExtend
    {
        public static void SetVelocity(this Rigidbody @this, Vector2 velocity, Transform finalTransform, float neighbourhood = 0)
        {
            if (finalTransform != null)
            {
                SetVelocity(@this, velocity, finalTransform.position, neighbourhood);
            }
        }

        public static int RaycastNonAlloc(Ray ray, RaycastHit[] result, float distance, bool clearArrayBeforeSearch = false)
        {
            if (clearArrayBeforeSearch)
            {
                result.ClearAll();
            }
            return Physics.RaycastNonAlloc(ray, result, distance);
        }

        public static void SetVelocity(this Rigidbody @this, Vector3 velocity, Vector3 finalPosition, float neighbourhood = 0)
        {
            if (@this != null)
            {
                Transform tr = @this.transform;
                Vector3 result = velocity;
                if (tr != null)
                {
                    Vector3 currentPoition = tr.position;
                    Vector3 delta = finalPosition - currentPoition;

                    if (delta.ApproxZero())
                    {
                        result = Vector3.zero;
                    }
                    else
                    {
                        Vector3 translate = velocity * Time.fixedDeltaTime;

                        if (delta == Vector3.zero || (delta.normalized == translate.normalized && delta.magnitude < translate.magnitude))
                        {
                            @this.MovePosition(finalPosition);
                            return;
                        }
                        else if (neighbourhood != 0)
                        {
                            neighbourhood = Mathf.Abs(neighbourhood);
                            Vector3 futureDestination = currentPoition + translate;
                            if (Vector3.Distance(finalPosition, futureDestination) <= neighbourhood)
                            {
                                @this.MovePosition(finalPosition);
                                return;
                            }
                        }
                    }
                }

                @this.velocity = result;
            }
        }
    }
}
