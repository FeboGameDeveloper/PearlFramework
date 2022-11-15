using Pearl.AI;
using UnityEditor;
using UnityEngine;

namespace Pearl
{
    [CustomEditor(typeof(FieldOfView))]
    public class FieldOfViewEditor : UnityEditor.Editor
    {
        private void OnSceneGUI()
        {
            FieldOfView fow = (FieldOfView)target;

            float viewAngle = fow.ViewAngle;
            float viewRadius = fow.ViewRadius;
            Transform targetTransform = fow.Target;

            if (!target)
            {
                return;
            }

            Handles.color = Color.white;
            Handles.DrawWireArc(targetTransform.position, Vector3.forward, Vector3.right, 360, viewRadius);

            Vector3 viewAngleA = targetTransform.DirFromAngle(-viewAngle / 2, false);
            Vector3 viewAngleB = targetTransform.DirFromAngle(viewAngle / 2, false);

            float sign = fow.CurrentDirection.x > 0 ? 1 : -1;

            Handles.DrawLine(targetTransform.position, targetTransform.position + (viewAngleA * viewRadius * sign));
            Handles.DrawLine(targetTransform.position, targetTransform.position + (viewAngleB * viewRadius * sign));


            Handles.color = Color.red;
            foreach (Transform visibleTarget in fow.Targets)
            {
                Handles.DrawLine(targetTransform.position, visibleTarget.position);
            }
        }

    }
}
