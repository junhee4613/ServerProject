using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActive;
    public Animator anim;   //깃발 애니메이션

    [HideInInspector]   
    public CheckPointManager cpMan;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isActive == false)     //비활성화 상태일떄 플레이어와 닿으면 깃발 애니메이션 실행
        {
            cpMan.SetActiveCheckpoint(this);                //체크포인트 SetActiveCheckpoint 실행


            anim.SetBool("flagActive", true);

            isActive = true;
        }
    }

    public void DeactivateCheckpoint()      //체크포인트 애니메이션 상태관리
    {
        anim.SetBool("flagActive", false);
        isActive = false;
    }
}
