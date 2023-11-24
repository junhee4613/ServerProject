using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActive;
    public Animator anim;   //��� �ִϸ��̼�

    [HideInInspector]   
    public CheckPointManager cpMan;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isActive == false)     //��Ȱ��ȭ �����ϋ� �÷��̾�� ������ ��� �ִϸ��̼� ����
        {
            cpMan.SetActiveCheckpoint(this);                //üũ����Ʈ SetActiveCheckpoint ����


            anim.SetBool("flagActive", true);

            isActive = true;
        }
    }

    public void DeactivateCheckpoint()      //üũ����Ʈ �ִϸ��̼� ���°���
    {
        anim.SetBool("flagActive", false);
        isActive = false;
    }
}
