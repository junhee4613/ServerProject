using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckPoint : MonoBehaviour
{
    private bool isActive;
    public Animator anim;

    [HideInInspector]
    public CheckPointManager cpMan;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && isActive == false)
        {
            cpMan.SetActiveCheckpoint(this);


            anim.SetBool("flagActive", true);

            isActive = true;
        }
    }

    public void DeactivateCheckpoint()
    {
        anim.SetBool("flagActive", false);
        isActive = false;
    }
}
