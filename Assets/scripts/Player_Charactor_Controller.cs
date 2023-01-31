using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Player_Charactor_Controller : MonoBehaviour
{
    public Text keyText;
    
    public float walkingSpeed = 4;
    public float xDirection;
    public float yDirection;
    public float xVector;
    public float yVector;
    public int keys = 0;
    // Start is called before the first frame update
    void Start()
    {
        SetKeyText();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        xDirection = Input.GetAxis("Horizontal");
        xVector = xDirection * walkingSpeed * Time.deltaTime;
        yDirection = Input.GetAxis("Vertical");
        yVector = yDirection * walkingSpeed * Time.deltaTime;

        transform.position = transform.position + new Vector3(xVector, yVector, 0);

        

        

    }


    //code that runs once when i collide with a key

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

    void SetKeyText()
    {

        keyText.text = "Keys: " + keys.ToString();

    }

}

