using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControler : MonoBehaviour
{
    private Vector3 mouseStartGrabPos;
    void Update()
    {
        transform.position = transform.position + Vector3.up * Input.mouseScrollDelta.y * -5;
        if (transform.position.y > 200)
        {
            transform.position = new Vector3(transform.position.x, 200f, transform.position.z);
        }
        if (transform.position.y < 10)
        {
            transform.position = new Vector3(transform.position.x, 10f, transform.position.z);
        }

        transform.position = transform.position + (Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal")) * Time.deltaTime * transform.position.y * 1f;
    }
}
