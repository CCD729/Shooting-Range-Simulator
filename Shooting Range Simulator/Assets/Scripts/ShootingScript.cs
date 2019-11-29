using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootingScript : MonoBehaviour
{
    //Scene management
    public GameObject sceneManmager;
    private bool levelEnded = false;
    private bool levelPaused = false;
    //Bullets per second
    public float firingRate = 10f;
    //Auto? Semi-auto?
    public bool autoTrigger;
    //Magazine size
    public int magSize = 30;
    //References
    public GameObject bullet;
    public Transform firePoint, detectPoint;
    public Text scoreText, ammoText, timeText;
    private int count;

    [SerializeField]
    private float timeLimit = 30.00f;
    private float timeLeft = 30.00f;

    private Ray ray;
    private RaycastHit raycastHit;

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

    //IMPORTANT: UPDATING PRESSINFO
    private bool EscPressed = false;
	
	public AudioClip shoot_sound;
	public AudioSource pewpew;


    void Start()
    {
        //Set firing interval according to input firing rate
        fRateInt = 1f / firingRate;
        count = 0;
        timeLeft = timeLimit;
        scoreText.text = "Score: " + count.ToString();
        timeText.text = "Time: " + timeLeft.ToString();
        Time.timeScale = 1;
		
		pewpew = this.GetComponent<AudioSource>();
		pewpew.clip = shoot_sound;
		pewpew.loop = false;
    }

    void Update()
    {
        if(count == 32)
        {
            this.Perfect();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            EscPressed = true;
        }
        if (Input.GetKeyUp(KeyCode.Escape))
        {
            EscPressed = false;
        }
        if (EscPressed && levelPaused == false)
        {
            EscPressed = false;
            this.Pause();
        }
        else if (EscPressed && levelPaused == true)
        {
            EscPressed = false;
            this.Resume();
        }
    }
    void FixedUpdate()
    {
        if (!levelPaused && !levelEnded)
        {
            if (!reloading)
                ammoText.text = currentMag.ToString() + "/30";
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
                        if (fRatePassed)
                            Shoot();
                    }
                    else
                    {
                        reloading = true;
                        ammoText.text = "Reloading";
                        Debug.Log("Reloading...");
                    }
                }
            }
            //Manual reload
            if (Input.GetKeyDown(KeyCode.R) && currentMag < magSize && !reloading)
            {
                reloading = true;
                ammoText.text = "Reloading";
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
                    ammoText.text = currentMag.ToString() + "/30";
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
            if (timeLeft > 0f)
            {
                timeLeft -= Time.fixedDeltaTime;
                timeText.text = "Time: " + timeLeft.ToString("F2");
            }
            else
            {
                timeLeft = 0.00f;
                timeText.text = "Time: 0.00";
                this.End();
            }
        }
    }


    void Shoot()
    {
		pewpew.Play();
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
                if (!target.GetComponent<TargetBehavior>().hit)
                {
                    count++;
                    scoreText.text = "Score: " + count.ToString();
                }
                target.GetComponent<TargetBehavior>().Hit(raycastHit.point, this.transform.forward);
            }
        }
    }
    public void Pause()
    {
        Time.timeScale = 0;
        sceneManmager.GetComponent<LevelSceneManager>().Pause();
        levelPaused = true;
    }
    public void Resume()
    {
        sceneManmager.GetComponent<LevelSceneManager>().Resume();
        levelPaused = false;
        Time.timeScale = 1;
    }
    public void End()
    {
        Time.timeScale = 0;
        levelEnded = true;
        sceneManmager.GetComponent<LevelSceneManager>().End(count);
    }
    public void Perfect()
    {
        Time.timeScale = 0;
        levelEnded = true;
        sceneManmager.GetComponent<LevelSceneManager>().Perfect();
    }
}
