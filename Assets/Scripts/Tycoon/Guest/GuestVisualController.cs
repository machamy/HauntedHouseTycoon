
using System.Collections;
using UnityEngine;

public class GuestVisualController : MonoBehaviour
{
    [Header("Refereces")]
    [SerializeField] private GuestObject guestObject;
    [SerializeField] private GameObject guestVisual;
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

    private bool isMoving = false;
    private bool isDirectingLeft = true;

    public bool IsDirectingLeft
    {
        get => isDirectingLeft;
        set
        {
            if(isDirectingLeft == value) return;
            isDirectingLeft = value;
            Vector3 localScale = guestVisual.transform.localScale;
            // Debug.Log($"{isDirectingLeft} localPosition: {localPosition.x} => {localPosition.x * -1}");
            localScale.x *= -1;
            guestVisual.transform.localScale = localScale;
        }
    }
    public IEnumerator MoveDirectionCheckRoutine()
    {
        float prevX = guestObject.transform.position.x;
        while (isMoving)
        {
            float currentX = guestObject.transform.position.x;
            if (currentX > prevX)
            {
                IsDirectingLeft = false;
            }
            else if (currentX < prevX)
            {
                IsDirectingLeft = true;
            }
            prevX = currentX;
            yield return null;
        }
    }
    
    

    public void SetIsMoving(bool isMoving = true)
    {
        guestAnimator.SetBool("1_Move", isMoving);
        this.isMoving = isMoving;
        if (isMoving)
        {
            StartCoroutine(MoveDirectionCheckRoutine());
        }
    }

    public void SetIdle()
    {
        guestAnimator.SetBool("1_Move", false);
        guestAnimator.SetBool("5_Debuff", false);
    }
    
    
    public void PlayAnimation(AnimationType state, int index = 0)
    {
        if (isSpum)
        {
            if (state == AnimationType.MOVE)
            {
                isMoving = true;
            }
            spumPrefab.PlayAnimation(state.ToSpumState(ref index), index);
        }
        else
        {
            bool isMove = state == AnimationType.MOVE;
            this.isMoving = isMove; // 단순화 가능, 일단 이렇게
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
        StartCoroutine(MoveDirectionCheckRoutine());
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