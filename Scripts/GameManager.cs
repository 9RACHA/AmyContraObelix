using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour {
    public static GameManager instance;

    private int totalScore = 0;
    private int targetScore = 200;

    public GameObject fruitPrefab;
    public GameObject nextLevelDoor;
    public Light directionalLight;
    public List<Transform> fruitTransforms;
    public Transform playerTransform;

    private AudioSource audioSource;

    private bool levelLocked = false;
    public bool LevelLocked { get { return levelLocked; } }
    
    void Awake() {
        if(instance != null && instance != this) {
            Destroy(this);
        }
        instance = this;
    }
    
    // Start is called before the first frame update
    void Start() {
        fruitTransforms = new List<Transform>();
        StartCoroutine(SpawnFruit());
        audioSource = GetComponent<AudioSource>();

        DontDestroyOnLoad(this);
    }

    private void FindObjectReferences() {
        nextLevelDoor = GameObject.Find("NextLevelDoor");
        nextLevelDoor.SetActive(false);
        directionalLight = GameObject.Find("Directional Light").GetComponent<Light>();
        playerTransform = GameObject.Find("Amy").transform;     
    }

    // Update is called once per frame
    void Update() {
        if(nextLevelDoor == null) {
            FindObjectReferences();
        }

        foreach(Transform t in fruitTransforms) {
            //Debug.Log(t.position);
            //Comprobamos si la fruta está cerca de Amy y la recolectamos si lo está
            if(Vector3.Distance(playerTransform.position, t.position) < 1.5f) {
                totalScore += t.GetComponent<Fruit>().FruitPoints;
                audioSource.Play();
                Destroy(t.gameObject);
            }
        }

        if(totalScore >= targetScore) {
            nextLevelDoor.SetActive(true);
        }
        
    }

    public void ElPajaroEstaEnElNido() {
        levelLocked = true;
        StartCoroutine(FadeLight());
        playerTransform.GetComponent<Player>().ActivateAutoplay(nextLevelDoor.transform.position);
    }

    public void RemoveFruit(Transform fruit) {
        //Debug.Log("GameManager.RemoveFruit");
        fruitTransforms.Remove(fruit);
    }

    private IEnumerator SpawnFruit() {
        while( ! levelLocked) {
            if(Random.Range(0f, 1f) < 0.05f) {
                Vector3 position = new Vector3(Random.Range(-8f, 8f), 1, Random.Range(-8f, 8f));
                while(Vector3.Distance(position, playerTransform.position) < 4) {
                    position.x = Random.Range(-8f, 8f);
                    position.z = Random.Range(-8f, 8f);
                }
                fruitTransforms.Add(Instantiate(fruitPrefab, position, Quaternion.identity).transform);
            }
            yield return new WaitForSeconds(0.2f);
        }
    }

    public IEnumerator FadeLight() {
        while (directionalLight.intensity > 0.02f) {
            directionalLight.intensity -= 0.004f;
            yield return new WaitForSeconds(0.01f);
        }
        Invoke("LoadNextLevel", 1f);
    }

    private void LoadNextLevel() {
        SceneManager.LoadScene("AmyYLasPlataformas");
    }

    void OnGUI() {
        GUI.Label(new Rect(10, 10, 100, 30), totalScore+"");
    }
}
