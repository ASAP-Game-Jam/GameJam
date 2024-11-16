using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    public Vector3 FinelDestination;
    private bool isStopped;

    public int Health;
    public int Damage;
    public float MovementSpeed;
    // Update is called once per frame
    void Update()
    {
        if (!isStopped)
        { 
            transform.Translate(new Vector3(MovementSpeed * -1, 0, 0));
        }
        
    }

    public void OnTriggerEnter2D (Collider2D collision) {
        if (collision.gameObject.layer == 9)
        {
            isStopped = true;
        }
    }

    public void OnTriggerExit2D (Collider2D collision) {
        if (collision.gameObject.layer == 9)
        {
            isStopped = false;
        }
    }


}
