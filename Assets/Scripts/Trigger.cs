using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private Transform theDoor; // this will get the transform position of the door.
    private float startZ; // this will be the dorrs starting postion.
    private bool isOpen; // this is the action we are trying to achieve. A true or false value

    // Start is called before the first frame update
    private void Start()
    {
        startZ = theDoor.position.z;  // this aquires the door's starting position on start up and asign that value to "startX"
    }

    // Update is called once per frame
    private void Update()
    {
        if (isOpen && theDoor.position.z < startZ + 2.5f) // this is the instruction... if "isOpen" is true and the door position is less that 2.5 units from its starting postion.. complete the instruction(move the door)
        {
            theDoor.position += theDoor.forward * Time.deltaTime; // this will move the door to the right 1 unit. Make Note that if door was alined with the x axis it would be "theDoor.right"/ Time.deltatime allows for smooth motion and not an A to B movement
        }

        if (!isOpen && theDoor.position.z > startZ) // this instruction is saying, if the IsOpen is false and the door postion is greater than its starting position, close the door
        {
            theDoor.position -= theDoor.forward * Time.deltaTime; // this will close the door and return it to its starting tranform position
        }
    }

    private void OnTriggerEnter(Collider other) // this is "open"
    {
        if (other.gameObject.tag =="Player") // this only allows a specific object with a "Key" tag attached to only open the door.
        isOpen = true;
    }

    private void OnTriggerExit(Collider other) // this is "closed"
    {
        if (other.gameObject.tag == "Player")
        isOpen = false;    
    }
}

