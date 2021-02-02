using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BeautifulMorning : MonoBehaviour
{
    public float cumForce;
    private Rigidbody rb;
    private string message;
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        message = "Beautiful Morning...";
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            rb.AddForce(Vector3.up * cumForce);
            MasterTrigger.UpdateDRP(message);
        }
            
    }
}
