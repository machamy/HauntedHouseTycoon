
using UnityEngine;

public class GuestVisualController : MonoBehaviour
{
    [Header("Refereces")]
    [SerializeField] private GuestObject guestObject;
    [SerializeField] private Animator guestAnimator;
    [SerializeField] private bool isSpum = true;
    [SerializeField] private SPUM_Prefabs spumPrefab;
    
    private AnimatorOverrideController animatorOverrideController;
    
    private void Awake()
    {
        if (guestObject == null)
        {
            guestObject = GetComponent<GuestObject>();
        }
        if (isSpum)
        {
            if (spumPrefab == null)
            {
                spumPrefab = GetComponentInChildren<SPUM_Prefabs>();
                if (!spumPrefab.allListsHaveItemsExist())
                {
                    spumPrefab.PopulateAnimationLists();
                }
            }
            spumPrefab.OverrideControllerInit();
        }
        else
        {
            animatorOverrideController = new AnimatorOverrideController(guestAnimator.runtimeAnimatorController);
            AnimationClip[] clips = guestAnimator.runtimeAnimatorController.animationClips;

            foreach (AnimationClip clip in clips)
            {
                animatorOverrideController[clip.name] = clip;
            }
            guestAnimator.runtimeAnimatorController = animatorOverrideController;
        }
    }
    
    
    public void PlayAnimation(AnimationType state, int index = 0)
    {
        if (isSpum)
        {
            spumPrefab.PlayAnimation(state.ToSpumState(ref index), index);
        }
        else
        {
            bool isMove = state == AnimationType.MOVE;
            bool isDebuff = state == AnimationType.FEAR;
            bool isDeath = state == AnimationType.PANIC;
            guestAnimator.SetBool("1_Move", isMove);
            guestAnimator.SetBool("5_Debuff", isDebuff);
            guestAnimator.SetBool("isDeath", isDeath);
            
            if (state == AnimationType.SCREAM)
            {
                guestAnimator.SetTrigger("2_Attack");
            }
            else if (state == AnimationType.PANIC)
            {
                guestAnimator.SetTrigger("6_Death");
            }
        }
    }
}


public enum AnimationType
{
    IDLE,
    MOVE,
    FEAR,
    SCREAM,
    PANIC,
}


public static class AnimationTypeExtension
{
    public static PlayerState ToSpumState(this AnimationType type, ref int index)
    {
        switch (type)
        {
            case AnimationType.IDLE:
                return PlayerState.IDLE;
            case AnimationType.MOVE:
                return PlayerState.MOVE;
            case AnimationType.FEAR:
                return PlayerState.DAMAGED;
            case AnimationType.SCREAM:
                index = 1;
                return PlayerState.ATTACK;
            case AnimationType.PANIC:
                return PlayerState.DEATH;
        }
        
        return PlayerState.IDLE;
    }
}