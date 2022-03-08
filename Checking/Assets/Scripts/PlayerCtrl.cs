using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.SceneManagement;

public class PlayerCtrl : MonoBehaviour
{

    

    public bool isGround = false;   
    private Rigidbody2D rb;    
    public Animator animator;

    //Run
    private float move;
    public float runSpeed;

    //Jump
    public float jumpForce = 10;
    private bool isJumping = false;
    private float jumpTime;
    public float jumpStartTime;
    private bool facingRight = true;


    //ゴール演出  
    public GameObject goalObj;
    public GameObject fade;
    public float countTime = 1;
    public float countForGoalObj = 0.5f;

    //SFX
    public int soundToPlay;



    


    void ChangeAnimationState(string newState)//アニメーション遷移の関数 引数newStateには上記の""内のコードが入る
    {
        animator.Play(newState);
    }


    private enum State 
    {
        None,
        Idle,
        Run,
        Jumping,
        WinningWalk,
        Goal,
    };
    private State currentState = State.None;

    



    private void Awake()
    {
        currentState = State.Idle;
        rb = this.GetComponent<Rigidbody2D>();

    }

    private bool CanMove()
    {
        if (currentState == State.Goal)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    private bool StartJumping()
    {
        if (Input.GetButtonDown("Jump") & isGround==true)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void GoToJumping()
    {
        Jump();
        AudioManager.instance.PlaySfx("PlayerJump");
        jumpTime = jumpStartTime;
        isJumping = true;
        ChangeAnimationState("Player_jump");
        // ステートをJumpに切り替え
        currentState = State.Jumping;
    }

    private void FixedUpdate()
    {
        float currentMove = CanMove() ? move : 0f;
        move = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(currentMove * runSpeed, rb.velocity.y);
        animator.SetFloat("Velocity", rb.velocity.y); //Animatorのパラメーターfloat Velocityにrb.velocity.yを反映させる
    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.W))
        {
            FindObjectOfType<SEManager>().Play("Player_Jump");
        }


        if (Input.GetKeyDown(KeyCode.A))
        {
            FindObjectOfType<SEManager>().Play("Player_Hurt");
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
            StartCoroutine(Goal());
            currentState = State.Goal;
        }

        if (move < 0 && facingRight)
        {
            Flip();
        }
        else if (move > 0 && !facingRight)
        {
            Flip();
        }

        switch (currentState)
        {
            case State.Idle:
                // 左右に動いた
                if (move != 0 && isGround==true)
                {
                    ChangeAnimationState("Player_run");
                    // ステートをRunに切り替え
                    currentState = State.Run;
                }
                // ジャンプ開始操作をした
                else if (StartJumping()&&isGround == true)
                {
                    // ステートをJumpに切り替え
                    GoToJumping();
                }
                break;
            case State.Run:
                float currentMove = CanMove() ? move : 0f;
                if (move ==0)
                {
                    ChangeAnimationState("Player_idle");
                    // ステートをIdleに切り替え
                    currentState = State.Idle;
                }
               
                // ジャンプ開始操作をした
                else if (StartJumping())
                {
                    // ステートをJumpに切り替え
                    GoToJumping();
                }
                break;
            case State.Jumping:
                if (isJumping)
                {
                    if (Input.GetButton("Jump"))
                    {
                        if (jumpTime > 0f)
                        {
                            rb.velocity = Vector2.up * jumpForce;
                            jumpTime -= Time.deltaTime;
                        }
                        else if (jumpTime <= 0f)
                        {
                            jumpTime = 0f;
                            isJumping = false;
                        }
                    }
                    else if (Input.GetButtonUp("Jump"))
                    {
                        jumpTime = 0f;
                        isJumping = false;
                    }
                }
                else
                {
                    if (isGround)
                    {
                        if (Mathf.Abs(move) > 0f)
                        {
                            ChangeAnimationState("Player_run");
                            currentState = State.Idle;
                        }
                        else
                        {
                            ChangeAnimationState("Player_idle");
                            currentState = State.Run;
                        }
                    }
                }
                break;

            case State.WinningWalk:
                WinningWalk();


                break;

            case State.Goal:
               
                               
                //CanWalk();
                break;


            default:
                break;
        }

    }



    public void IsGroundToTrue()
    {
        isGround = true;
    }
    public void IsGroundToFalse()
    {
        isGround = false;
    }

    public void Jump()
    {
        //FindObjectOfType<SEManager>().Play("Player_Jump");
        rb.velocity = new Vector3(rb.velocity.x, jumpForce, 0);        
        
    }

    public void Flip()
    {
        if (CanMove())
        {
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
        
    }

    public void WinningWalk()
    {
        move = 0.16f;
        ChangeAnimationState("WinningWalk");
    }

    public void GoalAction()
    {
        ChangeAnimationState("Player_Goal");
    }

    
    IEnumerator Goal()
    {
        //bgmCtrl.StopBgm();
        Instantiate(fade, transform.position, transform.rotation);
        yield return new WaitForSeconds(0.5f);
        // bgmCtrl.StageClearBgm();
        currentState = State.WinningWalk;
        yield return new WaitForSeconds(countTime);
        move = 0;
        currentState = State.Goal;
        ChangeAnimationState("Player_goal");
        yield return new WaitForSeconds(countForGoalObj);
        Instantiate(goalObj, transform.position, transform.rotation);        
    }
    
}
