using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed; //플레이어 속도
    public Rigidbody2D theRB;
    public float jumpForce; //플레이어 점프
    public float runSpeed;  //플레이어 달리기
    private float activeSpeed;

    private bool isGrounded;
    public Transform groundCheckPoint;  //플레이어 땅인식
    public float groundCheckRadius;     //플레이어 땅인식 범위
    public LayerMask whatIsGround;  // 그라운드 레이어 감지
    private bool canDoubleJump;     //플레이어 더블점프

    public Animator anim;       //플레이어 애니메이션

    public float knockbackLength, knockbackSpeed;   //넉백 거리, 넉백 스피드

    private float knockbackCounter;                 //넉백 체크용



    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        isGrounded = Physics2D.OverlapCircle(groundCheckPoint.position, groundCheckRadius, whatIsGround); //플레이어 땅인식


        if (knockbackCounter <= 0)
        {



            activeSpeed = moveSpeed;
            if (Input.GetKey(KeyCode.LeftShift))  //플레이어 달리기
            {
                activeSpeed = runSpeed;
            }

            theRB.velocity = new Vector2(Input.GetAxisRaw("Horizontal") * activeSpeed, theRB.velocity.y);  //플레이어 이동


            if (Input.GetButtonDown("Jump")) //플레이어 점프
            {
                if (isGrounded == true)
                {
                    Jump(); //점프
                    canDoubleJump = true; //플레이어 더블점프

                    anim.SetBool("isDoubleJumping", false); //더블점프 애니메이션
                }
                else
                {
                    if (canDoubleJump == true)
                    {
                        Jump(); //점프
                        canDoubleJump = false;

                        anim.SetTrigger("doDoubleJump");    //더블점프 애니메이션
                    }
                }
            }
            //방향키 누르는 방향대로 플레이어 좌우반전
            if (theRB.velocity.x > 0)
            {
                transform.localScale = Vector3.one;
            }
            if (theRB.velocity.x < 0)
            {
                transform.localScale = new Vector3(-1f, 1f, 1f);
            }
        }
        else
        {
            knockbackCounter -= Time.deltaTime;

            theRB.velocity = new Vector2(knockbackSpeed * -transform.localScale.x, theRB.velocity.y);
        }
        //핸들 애니메이션
        anim.SetFloat("speed", Mathf.Abs(theRB.velocity.x)); //플레이어 수평속도 제어
        anim.SetBool("isGrounded", isGrounded);             //플레이어가 땅에 닿아있는 동안의 제어
        anim.SetFloat("ySpeed", theRB.velocity.y);          // 플레이어 수직속도 제어(점프 또는 낙하)
    }
    public void Jump()  //플레이어 점프
    {
        theRB.velocity = new Vector2(theRB.velocity.x, jumpForce);

    }

    public void KnockBack() 
    {
        theRB.velocity = new Vector2(0f, jumpForce * .5f);      //플레이어 넉백
        anim.SetTrigger("isKnockingBack");                      //넉백 애니메이션

        knockbackCounter = knockbackLength;                         //넉백 거리
    }
}

