using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingScript : MonoBehaviour
{
    //Bullets per second
    public float firingRate = 10f;
    //Auto? Semi-auto?
    public bool autoTrigger;
    //Magazine size
    public int magSize = 30;
    //References
    public GameObject bullet;
    public Transform firePoint, detectPoint;

    //Current bullets left in mag
    private int currentMag = 30;
    //Reload interval
    private float reloadTime = 3f;
    //Reload counter
    private float reloadingTime = 0f;
    //Reload status
    private bool reloading = false;
    //Check if bullets are enough to shoot
    private bool bulletEnough = true;

    //Firing rate interval
    private float fRateInt;
    //Firing gap counter
    private float fGapCount = 0f;
    //able to shoot status
    private bool fRatePassed = true;


    private Ray ray;
    private RaycastHit raycastHit;
    
    void Start()
    {
        //Set firing interval according to input firing rate
        fRateInt = 1f / firingRate;
    }

    

    void FixedUpdate()
    {
        bulletEnough = currentMag > 0;
        // Get a ray from the camera pointing forwards
        ray = new Ray(this.transform.position, this.transform.forward);
        if (Input.GetMouseButton(0))
        {
            //Do nothing if reloading (TODO: SHOW RELOAD UI)
            if (!reloading)
            {
                //Start shooting if mag has bullets
                if (bulletEnough)
                {
                    if(fRatePassed)
                        Shoot();
                }  
                else
                {
                    reloading = true;
                    Debug.Log("Reloading...");
                }


            }
        }
        //Manual reload
        if (Input.GetKeyDown(KeyCode.R) && currentMag < magSize && !reloading)
        {
            reloading = true;
            Debug.Log("Reloading...");
        }

        //Reload logic
        if (reloading)
        {
            if (reloadingTime < reloadTime)
            {
                reloadingTime += Time.fixedDeltaTime;
            }
            else
            {
                reloadingTime = 0f;
                currentMag = magSize;
                reloading = false;
                Debug.Log("Reloaded");
            }
        }
        //Firing rate recover logic
        if (!fRatePassed)
        {
            if (fGapCount < fRateInt)
            {
                fGapCount += Time.fixedDeltaTime;
            }
            else
            {
                fGapCount = 0f;
                fRatePassed = true;
            }
        }
    }
    void Shoot()
    {
        fRatePassed = false;
        currentMag--;
        // Check if we hit anything
        bool hit = Physics.Raycast(ray, out raycastHit);
        // If we did...Shoot to the hitposition
        if (!hit)
        {
            //Modify The angle a bit if not hitting anything
            var straightQ = Quaternion.LookRotation(-this.transform.up, this.transform.forward);
            Vector3 straightV = straightQ.eulerAngles;
            Vector3 modifiedV = straightV + new Vector3(0f,-0.4f,0f);
            //Shoot
            //TODO: Destroy hitted object if target, Score, Recoil, HittingSound, and HittingSparkles
            var clone = Instantiate(bullet, firePoint.position, Quaternion.Euler(modifiedV));
        }
        else
        {
            //Shoot to hit pos
            Vector3 relativePos = raycastHit.point - detectPoint.position;
            Quaternion preQ = Quaternion.LookRotation(relativePos);
            //Modify The angle a bit
            Vector3 preV = preQ.eulerAngles;
            Vector3 modifiedV = preV + new Vector3(90f, 0f, 0f);
            //TODO: Recoil, HittingSound, and HittingSparkles
            var clone = Instantiate(bullet, firePoint.position, Quaternion.Euler(modifiedV));

            //Looks like the target is hit
            var target = raycastHit.collider.gameObject;
            if (target.CompareTag("Target"))
            {
                Debug.Log("TargetHit");
                target.GetComponent<TargetBehavior>().Hit(raycastHit.point, this.transform.forward);
            }
        }
    }
}
