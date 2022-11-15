#if SPLINE

using Pearl.Paths;
using UnityEngine;

namespace Pearl.Examples.PathExamples
{
    /*
     * In this example there are four paths: a line, a curve (Bezier), 
     * a merge (a merger of several paths, see the "mergePathsParameter" parameter) and 
     * a canvas curve (where the handles are objects of the canvas)
     */
    public class PathTest : MonoBehaviour
    {
        public PathManager newPathManager;

        [Range(0, 1)]
        public float puntator;


        [InspectorButton("Stop")]
        public bool stop;

        [InspectorButton("Pause")]
        public bool pause;

        [InspectorButton("Unpause")]
        public bool unpause;

        [InspectorButton("ResetPath")]
        public bool resetPath;

        [InspectorButton("ForceFinish")]
        public bool forceFinish;

        [InspectorButton("Play")]
        public bool play;

        [InspectorButton("Force")]
        public bool force;

        public void Stop()
        {
            newPathManager.Stop();
        }

        public void Force()
        {
            newPathManager.Force(puntator);
        }

        public void Pause()
        {
            newPathManager.Pause(true);
        }

        public void Unpause()
        {
            newPathManager.Pause(false);
        }

        public void ResetPath()
        {
            newPathManager.ResetPath();
        }

        public void ForceFinish()
        {
            newPathManager.ForceFinish();
        }

        public void Play()
        {
            newPathManager.Play();
        }
    }
}

#endif