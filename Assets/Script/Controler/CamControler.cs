using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamControler : MonoBehaviour
{
    void Update()
    {
        if (!PlayerControler.PointerIsOverUI())
        {
            transform.position = transform.position + Vector3.up * Input.mouseScrollDelta.y * -5;
        }
        if (transform.position.y > 200)
        {
            transform.position = new Vector3(transform.position.x, 200f, transform.position.z);
        }
        if (transform.position.y < 3)
        {
            transform.position = new Vector3(transform.position.x, 3f, transform.position.z);
        }
        transform.position = transform.position + (Vector3.forward * Input.GetAxis("Vertical") + Vector3.right * Input.GetAxis("Horizontal")) * Time.deltaTime * transform.position.y * 1f;
    }
}
