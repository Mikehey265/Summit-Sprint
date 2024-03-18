using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Grabbable : MonoBehaviour
{
    public enum GrabbableState
    {
        OutOfRange,
        InRange,
        Selected
    }

    public GameObject MeshGameObject;
    public GameObject OutlineGameObject;
    public Renderer grabbableRenderer;
    public GrabbableState currentState = GrabbableState.OutOfRange;

    [Tooltip("Determines if this grabbable object should be the winning one")]
    [SerializeField] public bool isPeak;

    public void SetOutlineEnabled(bool enabled)
    {
        if (OutlineGameObject) OutlineGameObject.SetActive(enabled);
    }

    private void Awake()
    {
        if (isPeak)
        {
            gameObject.AddComponent<MountainPeak>();
        }
    }
    
    // Start is called before the first frame update
    void Start()
    {
        SetOutlineEnabled(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case GrabbableState.OutOfRange:
                grabbableRenderer.material.color = Color.white;
                break;
            case GrabbableState.InRange:
                grabbableRenderer.material.color = Color.cyan;
                break;
            case GrabbableState.Selected:
                grabbableRenderer.material.color = Color.green;
                break;
            default:
                grabbableRenderer.material.color = Color.white;
                break;
        }
    }
}
