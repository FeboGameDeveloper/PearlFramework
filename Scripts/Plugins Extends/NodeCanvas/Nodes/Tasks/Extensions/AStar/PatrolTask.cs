#if AStar && NODE_CANVAS

using NodeCanvas.Framework;
using ParadoxNotion.Design;
using Pathfinding;
using UnityEngine;

namespace Pearl.NodeCanvas.Tasks
{
    [Category("Pearl")]
    public class PatrolTask : ActionTask
    {
        public BBParameter<AIDestinationSetter> destinationSetter;
        public BBParameter<AIBase> AIpath;
        public BBParameter<BoxCollider2D> collider2D;
        public BBParameter<Transform> targetTransform;

        public BBParameter<float> timeForPatrol;

        private RectangleEdge edgeSquare = RectangleEdge.Right;

        protected override void OnExecute()
        {
            edgeSquare = EnumExtend.GetRandom<RectangleEdge>(edgeSquare);

            BoxCollider2D collider = collider2D.value;

            Vector2 newPosition = collider.GetPerimeterPoint(edgeSquare, Random.value);

            targetTransform.value.position = newPosition;

            EndAction();
        }
    }
}

#endif
