using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretAI : MonoBehaviour
{
    private Transform target;

    
    [SerializeField]
    private GameObject firePoint, projectile;
    [SerializeField] private float projectileSpeed = 500f;
    [SerializeField] private float shootRate = .3f;
    private float nextFire;
    [SerializeField] private float turretTurnAroundSpeed;
    [SerializeField] private float lookup; // value to manage turret AI, needs to be changed manually based on the detector range 
    Detector detector;

    private void Awake()
    {
        if (detector == null)
        {
            detector = GetComponent<Detector>();
        }
    }

    public void SetTarget(Transform targ)
    {
        target = targ;
    }

    // Turret behaviour
    // 1. if turret has no target, rotate around as IDLE
    // 2. if turret has target, look at enemy and shoot

    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Shoot()
    {
        if (target != null)
        {
            var proj = Instantiate(projectile, firePoint.transform.position, Quaternion.Euler(90f, 0.0f, 0.0f));
            proj.transform.rotation = Quaternion.Euler(proj.transform.rotation.x, transform.rotation.y, proj.transform.rotation.z);
            proj.GetComponent<Rigidbody>().AddForce(transform.forward * projectileSpeed,ForceMode.VelocityChange);
            //Debug.Log("Shoot Turret");
        }
        
        
        
    }
    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(firePoint.transform.position, firePoint.transform.forward, Color.red);
        if (target!= null)
        {
            StartCoroutine(TurnToTarget());
            
            transform.LookAt(target.position + Vector3.up * lookup);
            // after turret is all turned

            //Debug.Log(Time.time + " " + nextFire);
            if (Time.time > nextFire)
            {
                nextFire = Time.time + shootRate;
                
                Shoot();
            }    
        }
        
    }

    IEnumerator TurnToTarget()
    {
        Vector3 vect = Vector3.up * lookup;
        Quaternion targetRotation = Quaternion.identity;
        do
        {
            if (target ==  null) 
                yield break;
            Vector3 targetDirection = ((target.position+ vect)  - transform.position).normalized;
            targetRotation = Quaternion.LookRotation(targetDirection);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, turretTurnAroundSpeed * Time.deltaTime);

            yield return null;
        } while (target != null && Quaternion.Angle(transform.rotation, target.rotation) > 0.01f);
    }
}
