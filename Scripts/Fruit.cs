using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fruit : MonoBehaviour {
    public List<Material> fruitMaterials;
    public List<int> fruitPoints;

    public int FruitPoints { get { return fruitPoints[fruitIndex]; } }
    private int fruitIndex;

    private float ttl = 60;
    // Start is called before the first frame update
    void Start()  {
        Destroy(gameObject, ttl);

        fruitIndex = Random.Range(0, fruitMaterials.Count);
        GetComponent<MeshRenderer>().material = fruitMaterials[fruitIndex];
    }


    void OnDestroy() {
        GameManager.instance.RemoveFruit(transform);
    }

    
}
