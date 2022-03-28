using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Detector : MonoBehaviour
{
    [field: SerializeField]
    public bool isDetected { get; private set; }

    public Vector3 DirectionToTarget => (target.transform.position - detectorOrigin.position).normalized;

    [Header("OverlapBox Parameters")]
    public Transform detectorOrigin;
    public float detectorSize = 20;
    public Vector3 detectorOriginOffset = Vector3.zero;

    private float DetectionDelay;
    public float detectionDelay
    {
        get
        {
            return DetectionDelay;
        }
        set
        {
            if (DetectionDelay != value)
            {
                delayTime = new WaitForSeconds(value);
            }
            DetectionDelay = value;
        }
    }

    private WaitForSeconds delayTime;

    public LayerMask detectorLayerMask;

    [Header("Gizmo parameters")]
    public bool showGizmos;

    public Color gizmoIdleColor = Color.green;    
    public Color gizmoDetectedColor = Color.red;

    [SerializeField]
    Collider[] colliders;
    [SerializeField]
    List<Collider> colliderList;

    [SerializeField]
    private GameObject target;

    public GameObject Target
    {
        get => target;
        private set
        {
            target = value;
            //isDetected = target != null;
        }
    }

    TurretAI turretAI;

    private void Awake()
    {
        if (turretAI == null)
            turretAI = GetComponent<TurretAI>();
    }

    void Start()
    {
        
        detectorOrigin = transform;
        StartCoroutine(DetectionCoroutine());
    }

    IEnumerator DetectionCoroutine()
    {
        yield return delayTime;
        Detection();
        StartCoroutine(DetectionCoroutine());
    }

    // Object searches for certain layer object. 
    // if certain layer object is found, increases search range
    // if not found, continue searching with default search range
    public bool Detection()
    {
        colliderList.Clear();

        if (Target != null) // when Target is not null
        {
            colliders = Physics.OverlapSphere(detectorOrigin.position + detectorOriginOffset,
                                    detectorSize,
                                    detectorLayerMask);
        }
        else
        {
            colliders = Physics.OverlapSphere(detectorOrigin.position + detectorOriginOffset,
                                    detectorSize,
                                    detectorLayerMask);
        }
        for (int i = 0; i < colliders.Length; i++)
        {
            colliderList.Add(colliders[i]);
        }
        SortObjectsOnDistance(colliderList);

        if (colliders != null && colliders.Length > 0)
        {
            // Set Target
            // animation to shoot
            target = colliderList[0].gameObject;
            if (target != null)
            {
                isDetected = true;
                turretAI.SetTarget(target.transform);
            }
        }
        else
        { // set animation to null
            target = null;
            isDetected = false;
            if (turretAI != null)
                turretAI.SetTarget(null);
        }

        return Target != null;
    }

    // Sorts players from closest from furthest
    public void SortObjectsOnDistance(List<Collider> objects)
    {
        objects.Sort(SortByDistanceToMe);
    }

    int SortByDistanceToMe(Collider a, Collider b)
    {
        float squaredRangeA = (a.transform.position - transform.position).magnitude;
        float squaredRangeB = (b.transform.position - transform.position).magnitude;
        return squaredRangeA.CompareTo(squaredRangeB);
    }

    // builtin function that only works in the editor 
    private void OnValidate()
    {
        if (detectorOrigin == null)
            detectorOrigin = transform;
    }
    private void OnDrawGizmos()
    {
        if (showGizmos)
        {
            Gizmos.color = gizmoIdleColor;
            if (isDetected)
                Gizmos.color = gizmoDetectedColor;
            Gizmos.DrawSphere(detectorOrigin.position + detectorOriginOffset, detectorSize);
        }

    }

}