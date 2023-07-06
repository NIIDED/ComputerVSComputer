using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrictionManager : MonoBehaviour
{
    float friction;
    public float terminalVelocityaKeisu;
    Rigidbody2D rb;

    void Start(){
       rb= gameObject.GetComponent<Rigidbody2D>();
    }

    /* public void TerminalVelocityFriction(){
        rb.AddForce(-rb.velocity/Time.fixedDeltaTime)
    }
    */

    public void TerminalVelocityFriction(){
        friction = rb.velocity.magnitude * terminalVelocityaKeisu;              //プレイヤーの速さ（大きさ）の１/2の大きさ　
        rb.AddForce(Vector2.left*GetVelocityDirection()*friction);　
    }
    

    public float GetVelocityDirection(){
        if(rb.velocity.x>0){
            return 1.0f;
        }else{
            return -1.0f;
        }
    }




}
