using EzySlice;
using UnityEngine;
using TMPro;

public class SliceObject : MonoBehaviour
{
    public Transform         startSlicePoint;
    public Transform         endSlicePoint;
    public VelocityEstimator velocityEstimator;
    public LayerMask         canSlice;

    public Material        crossSectionMaterial;
    public float           cutForce = 2000;
    public TextMeshProUGUI score;
    public int             scoreValue;

    public  AudioClip   clip;
    private AudioSource source;

    private void Start()
    {
        source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        var hasHit = Physics.Linecast(startSlicePoint.position, endSlicePoint.position, out var hit, canSlice);

        if (!hasHit)
            return;

        var target = hit.transform.gameObject;

        Slice(target);
    }

    private void Slice(GameObject target)
    {
        var velocity    = velocityEstimator.GetVelocityEstimate();
        var planeNormal = Vector3.Cross(endSlicePoint.position - startSlicePoint.position, velocity);
        planeNormal.Normalize();

        var hull = target.Slice(endSlicePoint.position, planeNormal);

        if (hull == null)
            return;

        var upperHull = hull.CreateUpperHull(target, crossSectionMaterial);
        var lowerHull = hull.CreateLowerHull(target, crossSectionMaterial);

        SetupSliceComponent(upperHull);
        SetupSliceComponent(lowerHull);

        Destroy(target);
        scoreValue++;
        score.text = "score = " + scoreValue;
        source.PlayOneShot(clip);
    }

    private void SetupSliceComponent(GameObject slicedObject)
    {
        var rb       = slicedObject.AddComponent<Rigidbody>();
        var collider = slicedObject.AddComponent<MeshCollider>();
        
        collider.convex = true;
        rb.AddExplosionForce(cutForce, slicedObject.transform.position, 1);
    }
}