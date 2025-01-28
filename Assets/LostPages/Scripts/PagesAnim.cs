using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PagesAnim : MonoBehaviour
{
    public Vector3 dir;


    // Update is called once per frame
    void Update()
    {
        transform.Rotate(dir * Time.deltaTime);
    }

}
