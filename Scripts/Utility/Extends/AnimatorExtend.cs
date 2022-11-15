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
    }
}
