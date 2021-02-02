using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    Animator animator;
    [HideInInspector]
    public string currentClip;
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayAnim(string clipName)
    {
        if (currentClip == clipName) return;
        animator.Play(clipName);
        currentClip = clipName;
    }
}
