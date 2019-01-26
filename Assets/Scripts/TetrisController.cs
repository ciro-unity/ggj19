using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TetrisController : MonoBehaviour
{
    float fallCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Blocks move down by pressing the down key but also overtime
        if (Input.GetKeyDown(KeyCode.DownArrow) || Time.time - fallCounter >= 1)
        {
            // Modify position
            transform.position += new Vector3(0, -1, 0);

            fallCounter = Time.time;
        }

        else if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            // Modify position
            transform.position += new Vector3(1, 0, 0);
        }

        else if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            // Modify position
            transform.position += new Vector3(-1, 0, 0);
        }

        else if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            // Modify rotation
            transform.Rotate(0, 0, 90);
        }

    }
}
