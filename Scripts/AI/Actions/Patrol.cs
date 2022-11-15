using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pearl.AI
{
    [Serializable]
    public struct WaypointInfo
    {
        public int ID;
        public float waitAtWaypoint;

        public bool isNext;
        [ConditionalField("@isNext")]
        public int nextID;

        public bool isPrevious;
        [ConditionalField("@isPrevious")]
        public int previousID;
        public Transform target;
    }

    public class Patrol : AbstractAction
    {
        [SerializeField]
        private WaypointInfo[] waypoints = null;
        [SerializeField]
        private TypePathEnum typePath = TypePathEnum.PingPong;
        [SerializeField]
        private bool useFirstwayPoint = false;
        [SerializeField]
        [ConditionalField("@useFirstwayPoint")]
        private int firstWayPoint = 0;
        [SerializeField]
        private bool initAtStart = false;
        [SerializeField]
        private bool inverse = false;
        [SerializeField]
        private Seeker seeker = null;
        [SerializeField]
        private Transform target = null;

        private Dictionary<int, WaypointInfo> _waypointsDict = new Dictionary<int, WaypointInfo>();
        private WaypointInfo _nextWaypont;


        protected void Awake()
        {
            if (_waypointsDict != null)
            {
                foreach (var waypoint in waypoints)
                {
                    _waypointsDict.Add(waypoint.ID, waypoint);
                }
            }
        }

        // Start is called before the first frame update
        protected void Start()
        {
            if (seeker && initAtStart)
            {
                Execute();
            }
        }

        protected void OnDestroy()
        {
            PearlInvoke.StopTimer(NextWayPoint);
        }

        public override void Execute()
        {
            if (seeker)
            {
                seeker.OnFinishSeek += PreNextWayPoint;

                int _nextIndex = useFirstwayPoint ? firstWayPoint : ChooseWaypointNearest();
                SetNextWaypoint(_nextIndex);
                seeker.Execute(_nextWaypont.target);
            }
        }

        public override void Stop()
        {
            if (seeker)
            {
                seeker.OnFinishSeek -= PreNextWayPoint;
                seeker.Stop();
                PearlInvoke.StopTimer(NextWayPoint);
            }
        }

        private void PreNextWayPoint()
        {
            float time = _nextWaypont.waitAtWaypoint;
            if (time != 0)
            {
                PearlInvoke.WaitForMethod(time, NextWayPoint);
            }
            else
            {
                NextWayPoint();
            }
        }

        private void NextWayPoint()
        {
            if (seeker)
            {
                if (typePath == TypePathEnum.PingPong && (!inverse && !_nextWaypont.isNext) || (inverse && !_nextWaypont.isPrevious))
                {
                    inverse = !inverse;
                }


                if ((!inverse && _nextWaypont.isNext) || (inverse && _nextWaypont.isPrevious))
                {
                    if (inverse)
                    {
                        SetNextWaypoint(_nextWaypont.previousID);
                    }
                    else
                    {
                        SetNextWaypoint(_nextWaypont.nextID);
                    }

                    seeker.Execute(_nextWaypont.target);
                }
            }
        }

        private int ChooseWaypointNearest()
        {
            if (target != null)
            {
                WaypointInfo aux = waypoints.MinBy(x => Vector2Extend.DistancePow2(target.position, x.target.position));
                return aux.ID;
            }
            return -1;
        }

        private void SetNextWaypoint(int index)
        {
            _waypointsDict.IsNotNullAndTryGetValue(index, out _nextWaypont);
        }
    }
}
