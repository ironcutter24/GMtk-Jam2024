using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformTrapController : MonoBehaviour
{
    [SerializeField] GameObject platform_right;
    [SerializeField] GameObject platform_left;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            Debug.Log("Collision enter");
            platform_right.GetComponent<HingeJoint2D>().useMotor = false;
            platform_left.GetComponent<HingeJoint2D>().useMotor = false;
        }
    }
}