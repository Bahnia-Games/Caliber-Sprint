using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RBMovementController : MonoBehaviour
{

    public float speed = 5f;
    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        //unused
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        float xAx = Input.GetAxisRaw("Horizontal");
        float zAx = Input.GetAxisRaw("Vertical");

        rb.velocity = new Vector3(xAx, rb.velocity.y, zAx) * speed * Time.deltaTime;

    }
}
