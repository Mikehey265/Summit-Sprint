using UnityEngine;

public class GhostTransparency : MonoBehaviour
{
    [SerializeField] private SkinnedMeshRenderer ghostMaterial;
    [SerializeField] private Transform target;
    [SerializeField] private float maxDistance = 10f;
    [SerializeField] private float minDistance = 1f;
    [SerializeField] private Color closeColor = new Color(1, 1, 1, 0.1f); 
    [SerializeField] private Color farColor = new Color(1, 1, 1, 1f);
    [SerializeField] private float opacity;

    void Start()
    {
        if (target == null)
        {
            target = GameObject.FindGameObjectWithTag("Player").transform;
        }
    }

    void Update()
    {
        float distance = Vector3.Distance(transform.position, target.position);
        distance = Mathf.Clamp(distance, minDistance, maxDistance);
        float normalizedDistance = (distance - minDistance) / (maxDistance - minDistance);
       
        opacity = Mathf.Lerp(closeColor.a, farColor.a, normalizedDistance);
        
        Color baseColor = ghostMaterial.material.color;
        ghostMaterial.material.color = new Color(baseColor.r, baseColor.g, baseColor.b, opacity);
    }

}