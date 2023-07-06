using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerManager : MonoBehaviour
{

    // 速度を与えたパターン　最初加速度無限大ではなく！！　力だけで統一している！！

    Rigidbody2D rb;
    float direction;
    public float power;
    public float jumpPower;

    FrictionManager frictionManager;
    [SerializeField] LayerMask blockLayer;
    [SerializeField] GameManager gameManager;

    public enum MOVE_DIRECTION{
        STOP,
        LEFT,
        RIGHT,
    }
    MOVE_DIRECTION moveDirection = MOVE_DIRECTION.STOP;

    void Start(){
        frictionManager = GetComponent<FrictionManager>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        float x = Input.GetAxis("Horizontal");
        if(x==0){
            moveDirection = MOVE_DIRECTION.STOP;
        }else if(x>0){
            moveDirection = MOVE_DIRECTION.RIGHT;
        }else {
            moveDirection = MOVE_DIRECTION.LEFT;
        }

        if(IsGround()){
            if(Input.GetKeyDown("space")) {
                Jump();
            }
        }

        frictionManager.TerminalVelocityFriction();
    }

    private void FixedUpdate(){
        Movement();
    }

    public void Movement(){
        switch (moveDirection){
            case MOVE_DIRECTION.STOP:
                direction =0;
                break;
            case MOVE_DIRECTION.RIGHT:
                direction =1;
                transform.localScale= new Vector3 (1,1,1);
                break;
            case MOVE_DIRECTION.LEFT:
                direction = -1;
                transform.localScale= new Vector3 (-1,1,1);
                break;
        }
        // 方向を進める！
        rb.AddForce(Vector2.right*direction*power);
        
    }

    public void Jump(){
            // ジャンプさせる！
            //rb.Add(Vector2.up*jumpPower);
            rb.velocity = new Vector2 (rb.velocity.x,10.0f);
        }

    bool IsGround(){
        Debug.DrawLine(transform.position - transform.right*0.2f,transform.position - transform.up*0.1f,Color.red);
        Debug.DrawLine(transform.position + transform.right*0.2f,transform.position - transform.up*0.1f,Color.red);
        return Physics2D.Linecast(transform.position - transform.right*0.2f,transform.position - transform.up*0.1f,blockLayer)
            || Physics2D.Linecast(transform.position + transform.right*0.2f,transform.position - transform.up*0.1f,blockLayer);
    }
    
    public void DestroyEnemy(){
       Destroy(this.gameObject);
   }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.tag == "Goal"){
            gameManager.GameClear();
        }

        if(collision.tag == "Enemy"){
            EnemyManager enemy = collision.GetComponent<EnemyManager>();
            if(this.transform.position.y +0.3f > enemy.transform.position.y )
            {
                Jump();
                enemy.DestroyEnemy();
            }
            else    // エネミーに当たったとき
            {
                Destroy(this.gameObject);
                gameManager.GameOver();
            }
        }
    }

}
