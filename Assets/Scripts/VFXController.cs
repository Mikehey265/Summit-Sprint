using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.VFX;

public class VFXController : MonoBehaviour
{
    public GameObject dustVFXPrefab;
    private int _dustCount = 10;
    private GameObject[] _dusts;
    private int _index = 0;

    private GameObject _selectedDustVFXGameobject;

    private VisualEffect _dustVFX;

    // Start is called before the first frame update
    private void Awake()
    {
        transform.parent = null;
        transform.position = Vector3.zero;
        _dusts = new GameObject[_dustCount];
    }

    void Start()
    {
        for (int i = 0; i < _dustCount; i++)
        {
            GameObject newDust = Instantiate(dustVFXPrefab, Vector3.zero, quaternion.identity);
            newDust.transform.SetParent(transform);
            // newDust.SetActive(false);
            _dusts[i] = newDust;
        }

        // _dustVFX = _selectedDustVFXGameobject.GetComponent<VisualEffect>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void ActivateDustAt(Vector3 position)
    {
        _selectedDustVFXGameobject = _dusts[_index];
        // _selectedDustVFXGameobject.SetActive(false);
        _selectedDustVFXGameobject.transform.position = position;
        _selectedDustVFXGameobject.transform.forward = new Vector3(position.x, 0, position.z).normalized * -1;
        // _selectedDustVFXGameobject.SetActive(true);
        _selectedDustVFXGameobject.GetComponent<VisualEffect>().Play();
        _index++;
        _index %= _dustCount;

        // print($"VFX {_index} Activate!");
    }
}