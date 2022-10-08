using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.5f;

    // Start is called before the first frame update
    void Start()
    {
        //take the current position = new position (0,0,0)//
       transform.position = new Vector3(0, 0, 0);
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float verticalInput = Input.GetAxis("Vertical");

        // transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);
        // transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);

        Vector3 direction = new Vector3(horizontalInput, verticalInput);
        transform.Translate(new Vector3(horizontalInput, verticalInput, 0) * speed * Time.deltaTime);

        if(transform.position.y >= 0){
            transform.position = new Vector3(transform.position.x,0,transform.position.z);
        }
        else if(transform.position.y <= -4f){
            transform.position = new Vector3(transform.position.x, -4f, transform.position.z);
        }
        
        if(transform.position.x >= 11f){
            transform.position = new Vector3(-11, transform.position.y, transform.position.z);
        }
        else if(transform.position.x <= -11f){
            transform.position = new Vector3(11, transform.position.y, transform.position.z);
        }
    }
}
