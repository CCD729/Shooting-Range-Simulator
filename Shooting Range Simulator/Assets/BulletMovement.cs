using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletMovement : MonoBehaviour
{
    [SerializeField]
    private float timeoutLifetime = 2f;
    [SerializeField]
    private bool destroySelfAfterTimeout = true, destroySelfOnCollision = true;
    [SerializeField]
    private float speed = 80f;

    //Counter
    private float destroyTime;

    void Start()
    {   //Set timer
        destroyTime = Time.time + timeoutLifetime;
    }
    void FixedUpdate()
    {   //Make the bullet fly
        transform.Translate(Vector3.up * Time.fixedDeltaTime * speed);
        //Destroy the bullet when time out
        if (destroySelfAfterTimeout && destroyTime <= Time.time)
            this.Destroy();
    }
    void OnCollisionEnter(Collision collision)
    {   //Destroy the bullet on collision
        if (destroySelfOnCollision)
            //TODO: Particle/SFX
            this.Destroy();
    }

    void OnTriggerStay(Collider other)
    {   //Destroy the bullet on collision
        if (destroySelfOnCollision)
            //TODO: Particle/SFX
            this.Destroy();
    }
    public void Destroy()
    {
        Destroy(gameObject);
    }
}
