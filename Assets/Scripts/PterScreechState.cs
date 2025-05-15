using UnityEngine;

public class PterScreechState : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        PterScreech soundFX = animator.GetComponent<PterScreech>();
        if (soundFX != null)
        {
            soundFX.Screech();  // Or Growl, Bark, etc.
        }
    }
}
