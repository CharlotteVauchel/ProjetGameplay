using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollision : MonoBehaviour
{
    [SerializeField] private float rayDistance = 10.0f;
    [SerializeField] private GameObject cam;

    // Update is called once per frame
    void Update()
    {
        LayerMask layerToDetect = LayerMask.GetMask("Wall");
        // Créer un ray depuis la position actuelle vers l'avant
        Ray ray = new Ray(transform.position, transform.forward);
        RaycastHit hit;

        // Lancer le raycast et filtrer avec le LayerMask
        if (Physics.Raycast(ray, out hit, rayDistance, layerToDetect))
        {
            float distance = Vector3.Distance(transform.position, hit.point);

            if (distance < 4.5)
            {
                Vector3 localPos = cam.transform.localPosition;

                localPos.z = distance;

                cam.transform.localPosition = localPos;
                return;
            }
        }

        // Reset la camera à la distance normale
        cam.transform.localPosition = Vector3.zero;
        
    }
}
