using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class Character_EnumStateMachine : MonoBehaviour
{

    Animator m_Animator;

    enum States
    {
        idle,
        walking,
        attackOne,
        attackTwo,
        sprint

    }
    //on lesson 14!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
    //search up raycasting to help with bouncing against walls
    //https://gamedevelopertips.com/unity-raycast-2d-what-is-it-and-how-to-use-it/
    //use the cloud code on all sides?
    //https://gamedev.stackexchange.com/questions/202223/using-a-raycast-to-stop-the-player-from-going-through-walls
    States state;
    
    public float walkingSpeed = 4;
    public float sprintSpeed = 5;
    public float hp = 5;
    public Rigidbody2D rb;
    public Text keyText;
    public float xDirection;
    public float yDirection;
    public float xVector;
    public float yVector;
    public int keys = 0;
    public bool enemyCollision = false;
    public bool isInvincible = false;
    public bool sprint = false;
    public float invTimer = 3;
    public float meleeTimer = 2;
    public bool aOne = false;
    public bool aTwo = false;
    public float attackTimer = 2;

    public float floatHeight;     // Desired floating height.
    public float liftForce;       // Force to apply when lifting the rigidbody.
    public float damping;         // Force reduction proportional to speed (reduces bouncing).




    // Start is called before the first frame update
    void Start()
    {
        m_Animator = gameObject.GetComponent<Animator>();
        SetKeyText();
        state = States.idle;
        enemyCollision = false;

        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "enemy")
        {


            if (aOne || aTwo)
            {

                collision.gameObject.SetActive(false);

            }


            else
            {
                //collision.gameObject.SetActive(false);
                //keys++;
                print("we have collided with enemy");

                PlayerHurt();
            }
        }

    }

    void PlayerHurt()
    {
        if (!isInvincible)
        {
            isInvincible = true;
            hp--;
            if (hp < 1)
            {
                PlayerDeath();

            }

        }



    }

    void PlayerDeath()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }


    void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "key")
        {
            collision.gameObject.SetActive(false);
            keys++;
            print("we have " + keys + " keys!");
            SetKeyText();
        }
    }
    /*
    void PlayerSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift)) // while player holds shift he can sprint
        {
            if (!sprint)
            {
                walkingSpeed += sprintSpeed;
                sprint = true; // right after we apply the double speed or whatever, we set the bool to true so it can't do it over and over again.
            }
        }
        if (Input.GetKeyUp(KeyCode.LeftShift)) // as soon as he lets go, the bool turns false and the speed is reset
        {
            sprintSpeed = walkingSpeed;
            sprint = false;
        }
    }
    */
    // Update is called once per frame
    void Update()
    {


       
       

        xDirection = Input.GetAxis("Horizontal");
        yDirection = Input.GetAxis("Vertical");
        xVector = xDirection * walkingSpeed * Time.deltaTime;
        yVector = yDirection * walkingSpeed * Time.deltaTime;

        


        if(Input.GetKey(KeyCode.RightShift) && !Input.GetKey(KeyCode.RightControl))
        {
            aOne = true;
        }


        if (Input.GetKey(KeyCode.RightControl) && !Input.GetKey(KeyCode.RightShift))
        {
            aTwo = true;
        }



        
        if (Input.GetKeyDown("space"))
        {
            sprint = true;
            state = States.sprint;
        }
        
        if (sprint)
        {
            state = States.sprint;
        }
        
        if (state == States.idle)
        {
            IdleState();
        }

        else if(state == States.walking)
        {
            WalkingState();
        }

        else if (state == States.attackOne)
        {
            AttackOneState();
        }

        else if (state == States.attackTwo)
        {
            AttackTwoState();
        }

        else if (state == States.sprint)
        {
            SprintState();
        }

        if (isInvincible == true)
        {
            invTimer -= (1 * Time.deltaTime);
            if (invTimer <= 0)
            {
                isInvincible = false;
                invTimer = 3;

            }
        }
      
    }

    void IdleState()
    {
        if ((xDirection != 0f || yDirection != 0f) && !aOne && !aTwo)
        {
            state = States.walking;
        }

        else if (aOne)
        {
            state = States.attackOne;
        }

        else if (aTwo)
        {
            state = States.attackTwo;
        }
        else if (sprint)
        {
            state = States.sprint;

        }
       
        else 
        {
            Debug.Log("I am Idle");
            walkingSpeed = 4;
            //reset other triggers
            m_Animator.ResetTrigger("backIdle");
                m_Animator.ResetTrigger("walkingLeft");
                m_Animator.ResetTrigger("walkingRight");
                m_Animator.SetTrigger("frontIdle");
                m_Animator.ResetTrigger("attack1");
                m_Animator.ResetTrigger("sprint");
        }
    }

    void WalkingState()
    {
        if ((xDirection == 0f && yDirection == 0f) && !aOne && !aTwo)
        {
            state = States.idle;
        }

        else
        {

            transform.position = transform.position + new Vector3(xVector, yVector, 0);
            
            Debug.Log("I am walking");
            





            if (Input.GetKey(KeyCode.UpArrow))
            {
                //reset other triggers
                m_Animator.ResetTrigger("frontIdle");
                m_Animator.ResetTrigger("walkingLeft");
                m_Animator.ResetTrigger("walkingRight");
                m_Animator.SetTrigger("backIdle");
                m_Animator.ResetTrigger("attack1");
                m_Animator.ResetTrigger("sprint");
            }

            if (Input.GetKey(KeyCode.DownArrow))
            {
                //reset other triggers
                m_Animator.ResetTrigger("backIdle");
                m_Animator.ResetTrigger("walkingLeft");
                m_Animator.ResetTrigger("walkingRight");
                m_Animator.SetTrigger("frontIdle");
                m_Animator.ResetTrigger("attack1");
                m_Animator.ResetTrigger("sprint");
            }
            if (Input.GetKey(KeyCode.RightArrow))
            {
                //reset other triggers
                m_Animator.ResetTrigger("frontIdle");
                m_Animator.ResetTrigger("walkingLeft");
                m_Animator.ResetTrigger("backIdle");
                m_Animator.SetTrigger("walkingRight");
                m_Animator.ResetTrigger("attack1");
                m_Animator.ResetTrigger("sprint");
            }
            if (Input.GetKey(KeyCode.LeftArrow))
            {
                //reset other triggers
                m_Animator.ResetTrigger("frontIdle");
                m_Animator.ResetTrigger("backIdle");
                m_Animator.ResetTrigger("walkingRight");
                m_Animator.SetTrigger("walkingLeft");
                m_Animator.ResetTrigger("attack1");
                m_Animator.ResetTrigger("sprint");
            }
            if (Input.GetKeyDown("space"))
                {
                state = States.sprint;
                }
        }
    }
    void AttackOneState()
    {
        if (!aOne)
        {
            state = States.idle;
            
        }
        else
        {
            xVector = xDirection * walkingSpeed * Time.deltaTime;
            yVector = yDirection * walkingSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(xVector, yVector, 0);
            m_Animator.ResetTrigger("frontIdle");
            m_Animator.ResetTrigger("backIdle");
            m_Animator.ResetTrigger("walkingRight");
            m_Animator.ResetTrigger("walkingLeft");
            m_Animator.SetTrigger("attack1");
            m_Animator.ResetTrigger("sprint");

            meleeTimer -= (1 * Time.deltaTime);
            if (meleeTimer <= 0)
            {
                aOne = false;
                meleeTimer = 2;
            }
        }
    }


    void AttackTwoState()
    {
        //fill
    }
    
    void SprintState()
    {
        if (sprint == false)
            {

            state = States.walking;
            
        }

        else if (sprint)
        {
            
            walkingSpeed = sprintSpeed;
            transform.position = transform.position + new Vector3(xVector, yVector, 0);

            Debug.Log("i am sprinting");
            xVector = xDirection * walkingSpeed * Time.deltaTime;
            yVector = yDirection * walkingSpeed * Time.deltaTime;
            transform.position = transform.position + new Vector3(xVector, yVector, 0);
            m_Animator.ResetTrigger("frontIdle");
            m_Animator.ResetTrigger("backIdle");
            m_Animator.ResetTrigger("walkingRight");
            m_Animator.ResetTrigger("walkingLeft");
            m_Animator.ResetTrigger("attack1");
            m_Animator.SetTrigger("sprint");

            if (Input.GetKeyUp("space"))
            {
                sprint = false;
                walkingSpeed = 4;
            }
        }
    }

    

   

    

    void SetKeyText()
    {

        keyText.text = "Keys: " + keys.ToString();

    }


    /*

    RaycastHit2D hit1 = Physics2D.Raycast(transform.position, -Vector2.up);
    RaycastHit2D hit2 = Physics2D.Raycast(transform.position, -Vector2.down);
    RaycastHit2D hit3 = Physics2D.Raycast(transform.position, -Vector2.left);
    RaycastHit2D hit4 = Physics2D.Raycast(transform.position, -Vector2.right);



        




    
        if (hit1.collider != null)
        {
            // Calculate the distance from the surface and the "error" relative
            // to the floating height.
            float distance1 = Mathf.Abs(hit1.point.y - transform.position.y);
    float heightError1 = floatHeight - distance1;

    // The force is proportional to the height error, but we remove a part of it
    // according to the object's speed.
    float force = liftForce * heightError1 - rb.velocity.y * damping;

    // Apply the force to the rigidbody.
    rb.AddForce(Vector3.up* force);
        }

if (hit2.collider != null)
{
    // Calculate the distance from the surface and the "error" relative
    // to the floating height.
    float distance2 = Mathf.Abs(hit2.point.y - transform.position.y);
    float heightError2 = floatHeight - distance2;

    // The force is proportional to the height error, but we remove a part of it
    // according to the object's speed.
    float force = liftForce * heightError2 - rb.velocity.y * damping;

    // Apply the force to the rigidbody.
    rb.AddForce(Vector3.down * force);
}

// if (hit3.collider != null)
{
    // Calculate the distance from the surface and the "error" relative
    // to the floating height.
    float distance3 = Mathf.Abs(hit3.point.x - transform.position.x);
    float heightError3 = floatHeight - distance3;

    // The force is proportional to the height error, but we remove a part of it
    // according to the object's speed.
    float force = liftForce * heightError3 - rb.velocity.x * damping;

    // Apply the force to the rigidbody.
    rb.AddForce(Vector3.left * force);
}

//if (hit4.collider != null)
{
    // Calculate the distance from the surface and the "error" relative
    // to the floating height.
    float distance4 = Mathf.Abs(hit4.point.x - transform.position.x);
    float heightError4 = floatHeight - distance4;

    // The force is proportional to the height error, but we remove a part of it
    // according to the object's speed.
    float force = liftForce * heightError4 - rb.velocity.x * damping;

    // Apply the force to the rigidbody.
    rb.AddForce(Vector3.right * force);
}


    */


}
