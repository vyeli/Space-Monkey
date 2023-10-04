using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotatingSkybox : MonoBehaviour
{
    public float rotateSpeed = 1.5f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        RenderSettings.skybox.SetFloat("_Rotation", Time.time * rotateSpeed);
    }
}
