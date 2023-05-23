using System.Runtime.CompilerServices;
using UnityEngine;

namespace Pearl
{
    public static class AnimatorExtend
    {
        public static void Repeat(this Animator animator)
        {
            if (animator == null)
            {
                return;
            }

            animator.Rebind();
            animator.Update(0f);
        }

        public static AnimationClip GetClip(this Animator anim, in string name)
        {
            if (anim != null)
            {
                AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
                foreach (AnimationClip clip in clips)
                {
                    if (clip.name == name)
                    {
                        return clip;
                    }
                }
            }
            return null;
        }
    }
}
