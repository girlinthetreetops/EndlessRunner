using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RunningParticles : MonoBehaviour
{
    private Renderer particleRenderer; 

    private void Start()
    {
        particleRenderer = GetComponent<ParticleSystemRenderer>();
    }

    private void Update()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out RaycastHit raycastHit, 10f))
        {
            // if (raycastHit.collider.CompareTag("Ground"))
            //{
                Renderer groundRenderer = raycastHit.collider.GetComponent<Renderer>();

                if (groundRenderer != null)
                { 
                    Material groundMaterial = groundRenderer.material;
                    particleRenderer.material = groundMaterial;
                }
            //}
        }
    }

}
