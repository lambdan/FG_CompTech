using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerShip : MonoBehaviour
{
    private float MovementSpeed = 2f;
    private float RotationSpeed = 200f;
    
    void Update()
    {
        // input handling
        transform.position += transform.up * (Input.GetAxisRaw("Vertical") * Time.deltaTime * MovementSpeed);
        transform.Rotate(0, 0, Input.GetAxisRaw("Horizontal") * Time.deltaTime * RotationSpeed * -1);
    }
}
