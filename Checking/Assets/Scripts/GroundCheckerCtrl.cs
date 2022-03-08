using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundCheckerCtrl : MonoBehaviour
{
    [SerializeField] PlayerCtrl playerCtrl;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            playerCtrl.IsGroundToTrue();
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground")&&playerCtrl.isGround == false)
        {
            playerCtrl.IsGroundToTrue();
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ground"))
        {
            playerCtrl.IsGroundToFalse();
        }

    }
}
