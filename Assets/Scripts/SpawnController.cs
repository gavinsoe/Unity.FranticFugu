using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {
    public static SpawnController instance;

    private GameObject startCanvas, gameCanvas, pauseCanvas, endCanvas;

    public GameObject player;
    public GameObject[] enemy;
    public Vector3[] spawnPoints;
    public float time;
    private float tick;
    public int points;
    public bool paused;

    void Awake()
    {
        // Forces a different code path in the BinaryFormatter that doesn't rely on run-time code generation (which would break on iOS).
        System.Environment.SetEnvironmentVariable("MONO_REFLECTION_SERIALIZER", "yes");

        Debug.Log(Application.persistentDataPath);

        // Make sure there is only 1 instance of this class.
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
            startCanvas = GameObject.Find("StartCanvas");
            gameCanvas = GameObject.Find("GameCanvas");
            pauseCanvas = GameObject.Find("PauseCanvas");
            endCanvas = GameObject.Find("EndCanvas");
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

	// Use this for initialization
	void Start () {
        spawnPoints = new Vector3[14];
        Vector3 topRight = Camera.main.ViewportToWorldPoint(new Vector3(1, 1, 0));
        Vector3 bottomLeft = Camera.main.ViewportToWorldPoint(new Vector3(0, 0, 0));
        float height = topRight.y - bottomLeft.y;
        float width = topRight.x - bottomLeft.x;
        float heightDiff = height / 4f;
        float widthDiff = width / 3f;

        spawnPoints[0] = new Vector3(bottomLeft.x - 2f, bottomLeft.y - 2f, 0);
        spawnPoints[1] = new Vector3(bottomLeft.x - 2f, bottomLeft.y + heightDiff, 0);
        spawnPoints[2] = new Vector3(bottomLeft.x - 2f, bottomLeft.y + heightDiff * 2f, 0);
        spawnPoints[3] = new Vector3(bottomLeft.x - 2f, topRight.y - heightDiff, 0);
        spawnPoints[4] = new Vector3(bottomLeft.x - 2f, topRight.y + 2f, 0);
        spawnPoints[5] = new Vector3(bottomLeft.x + widthDiff, topRight.y + 2f, 0);
        spawnPoints[6] = new Vector3(topRight.x - widthDiff, topRight.y + 2f, 0);
        spawnPoints[7] = new Vector3(topRight.x + 2f, topRight.y + 2f, 0);
        spawnPoints[8] = new Vector3(topRight.x + 2f, topRight.y - heightDiff, 0);
        spawnPoints[9] = new Vector3(topRight.x + 2f, bottomLeft.y + heightDiff * 2f, 0);
        spawnPoints[10] = new Vector3(topRight.x + 2f, bottomLeft.y + heightDiff, 0);
        spawnPoints[11] = new Vector3(topRight.x + 2f, bottomLeft.y - 2f, 0);
        spawnPoints[12] = new Vector3(topRight.x - widthDiff, bottomLeft.y - 2f, 0);
        spawnPoints[13] = new Vector3(bottomLeft.x + widthDiff, bottomLeft.y - 2f, 0);

        time = 0f;
        tick = 0f;
        points = 0;
        paused = true;

        startCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            time += Time.deltaTime;
            if (tick > 0.4f)
            {
                tick = 0;
                GameObject newEnemy = Instantiate(enemy[Random.Range(0,3)]);
                //newEnemy.GetComponent<EnemyController>().setStartingPoint(spawnPoints[start]);
                //newEnemy.GetComponent<EnemyController>().setEndPoint(spawnPoints[end]);
                //newEnemy.GetComponent<EnemyController>().setSpeed(5f);
            }
            else
            {
                tick += Time.deltaTime;
            }
        }
	}

    public void StartGame()
    {
        if (GameObject.FindGameObjectWithTag("Player") == null)
        {
            Instantiate(player);
        }
        paused = false;
        time = 0f;
        tick = 0f;
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
    }

    public void PauseGame()
    {
        paused = true;
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        endCanvas.SetActive(false);
    }

    public void UnpauseGame()
    {
        paused = false;
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
    }

    public void EndGame()
    {
        paused = true;
        GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject item in list)
        {
            Destroy(item);
        }
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        Destroy(temp);
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(true);
        // Do Highest score check and save
    }
}
