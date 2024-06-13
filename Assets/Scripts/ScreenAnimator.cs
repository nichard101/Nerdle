using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenAnimator : MonoBehaviour
{
    [SerializeField] private Animator transition;
    void Start(){
        
    }

    public void GoToStats(){
        transition.ResetTrigger("MoveToGame");
        transition.SetTrigger("MoveToStats");
    }

    public void GoToGame(){
        transition.ResetTrigger("MoveToStats");
        transition.SetTrigger("MoveToGame");
    }
}
