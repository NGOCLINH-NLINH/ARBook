using UnityEngine;

public class TrexRoarState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        TrexRoar soundFX = animator.GetComponent<TrexRoar>();
        if (soundFX != null)
        {
            soundFX.Roar();  // Or Growl, Bark, etc.
        }
    }
}
