using System.Collections;
using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Script is made for testing whether collision between player and river works properly.
/// It does so by showing debug messages whenever player enters or exits a collision or trigger.
/// </summary>
public class TestPlayerRiverCollision : MonoBehaviour
{
    void Start() => Debug.Log("Start player-river test"); 

    private void OnCollisionEnter(Collision collision) => Debug.Log("Enter collision");
    private void OnCollisionExit(Collision collision) => Debug.Log("Exit collision");
    private void OnTriggerEnter(Collider other) => Debug.Log("Enter trigger");
    private void OnTriggerExit(Collider other) => Debug.Log("Exit trigger");
}
