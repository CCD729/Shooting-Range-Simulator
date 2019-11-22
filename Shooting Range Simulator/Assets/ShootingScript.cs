using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    public float firingRate = 10f;
    public bool autoTrigger;
    public GameObject bullet;
    public Transform firePoint, detectPoint;
    private Ray ray;
    RaycastHit raycastHit;
    
    void Start()
    {
        // Get a ray from the camera pointing forwards
        
    }

    

    void FixedUpdate()
    {
        ray = new Ray(this.transform.position, this.transform.forward);
        if (Input.GetMouseButton(0))
        {
            Shoot();
        }
        
    }
    void Shoot()
    {
        // Check if we hit anything
        bool hit = Physics.Raycast(ray, out raycastHit);

        // If we did...Shoot to the hitposition
        if (!hit)
        {
            //Modify The angle a bit if not hitting anything
            var straightQ = Quaternion.LookRotation(-this.transform.up, this.transform.forward);
            Vector3 straightV = straightQ.eulerAngles;
            Vector3 modifiedV = straightV + new Vector3(0f,-0.6f,0f);
            //Shoot
            var clone = Instantiate(bullet, firePoint.position, Quaternion.Euler(modifiedV));
            //raycastHit.collider.gameObject.transform.position;
        }
        else
        {
            //Shoot to hit pos
            Vector3 relativePos = raycastHit.point - detectPoint.position;
            Quaternion preQ = Quaternion.LookRotation(relativePos);
            //Modify The angle a bit
            Vector3 preV = preQ.eulerAngles;
            Vector3 modifiedV = preV + new Vector3(90f, 0f, 0f);
            var clone = Instantiate(bullet, firePoint.position, Quaternion.Euler(modifiedV));
        }
    }
}
