using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyCtrl : MonoBehaviour
{
    private float move=-1;
    public float speed=1;
    Rigidbody2D rb;
    private enum State
    {
        None,
        Run,
        Explosion,
    };
    public float appleposition;
    // Start is called before the first frame update
    void Start()
    {
        rb = this.GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(move,rb.velocity.y);
        
    }
}
