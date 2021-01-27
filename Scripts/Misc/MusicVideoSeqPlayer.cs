using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
class MusicVideoSeqPlayer : MonoBehaviour 
{
    public Animator animator;
    
    public void PlaySequence(string sequenceName)
    {
        animator.Play(sequenceName);
    }
}
