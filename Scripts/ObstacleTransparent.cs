using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObstacleTransparent : MonoBehaviour {
    public Material transparentMaterial;
    private Material originalMaterial;
    private MeshRenderer meshRenderer;
    // Start is called before the first frame update
    void Start() {
        meshRenderer = GetComponent<MeshRenderer>();
        originalMaterial = meshRenderer.material;        
    }

    public void GoTransparent(bool transparent) {
        if(transparent) {
            meshRenderer.material = transparentMaterial;
        } else {
            meshRenderer.material = originalMaterial;
        }
    }
}
