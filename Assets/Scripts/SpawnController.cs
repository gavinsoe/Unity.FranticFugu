using UnityEngine;
using System.Collections;

public class SpawnController : MonoBehaviour {
    public static SpawnController instance;

    private GameObject startCanvas, gameCanvas, pauseCanvas, endCanvas;
    private GameObject videoCanvas;

    public GameObject player;
    public GameObject[] enemy;
    public Vector3[] spawnPoints;
    public float time;
    private float tick;
    public int points;
    public bool paused;
    private float tickSetter;
    private int enemySetter;
    public int phase;
    private double phaseTimer;
    public int phasePicker;
    private bool videoWatched;
    private bool invincibility;
    private float invincCounter;

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
            videoCanvas = GameObject.Find("VideoCanvas");
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
        tickSetter = 0.4f;
        enemySetter = 1;
        phase = 0;
        videoWatched = false;
        invincibility = false;

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
        phaseTimer = 30f;
        phasePicker = Random.Range(0, 18);

        startCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
        videoCanvas.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            if (invincibility)
            {
                if (invincCounter > 3f)
                {
                    GameObject temp = GameObject.FindGameObjectWithTag("Player");
                    temp.GetComponent<PolygonCollider2D>().enabled = true;
                    invincibility = false;
                }
                invincCounter += Time.deltaTime;
            }
            if (time > phaseTimer)
            {
                phaseTimer += 30f;
                phasePicker = Random.Range(0, 18);
            }
            time += Time.deltaTime;
            if (tick > tickSetter)
            {
                tick = 0;
                SpawnControl();
                //GameObject newEnemy = Instantiate(enemy[Random.Range(0,3)]);
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
        phasePicker = Random.Range(0, 18);
        paused = false;
        time = 0f;
        tick = 0f;
        videoWatched = false;
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
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(true);

        if (!videoWatched)
        {
            videoWatched = true;
            videoCanvas.SetActive(true);
        }
        else
        {
            GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (GameObject item in list)
            {
                Destroy(item);
            }
            GameObject temp = GameObject.FindGameObjectWithTag("Player");
            Destroy(temp);
        }

        // Do Highest score check and save
        float highScore = PlayerPrefs.GetFloat("HighScore");
        if (time > highScore)
        {
            PlayerPrefs.SetFloat("HighScore", time);
            PlayerPrefs.Save();
        }
    }

    public void WatchVideo()
    {
        //Watch Video

        invincibility = true;
        invincCounter = 0f;
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        temp.GetComponent<PolygonCollider2D>().enabled = false;

        paused = false;
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
        videoCanvas.SetActive(false);
    }

    public void DontWatchVideo()
    {
        GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject item in list)
        {
            Destroy(item);
        }
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        Destroy(temp);
        videoCanvas.SetActive(false);
    }

    public void SpawnControl()
    {
        int picker;
        switch(phasePicker)
        {
            // Medium speed medium amount Fugus
            case 0:
                phase = 0;
                tickSetter = 0.6f;
                enemySetter = 1;
                break;
            // Army of slow moving babies
            case 1:
                phase = 1;
                tickSetter = 0.3f;
                enemySetter = 0;
                break;
            // Medium speed medium amount of babies
            case 2:
                phase = 2;
                tickSetter = 0.6f;
                enemySetter = 0;
                break;
            // Fast speed less amount of babies
            case 3:
                phase = 3;
                tickSetter = 0.7f;
                enemySetter = 0;
                break;
            // Medium speed medium amount of spikies
            case 4:
                phase = 4;
                tickSetter = 0.6f;
                enemySetter = 2;
                break;
            // Slow speed more amount of babies and fast speed less amount of fugus
            case 5:
                phase = 5;
                tickSetter = 0.4f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 0;
                else
                    enemySetter = 1;
                break;
            // Slow speed more amount of babies and fast speed less amount of spikies
            case 6:
                phase = 6;
                tickSetter = 0.4f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 0;
                else
                    enemySetter = 2;
                break;
            // Medium speed medium amount of all types
            case 7:
                phase = 7;
                tickSetter = 0.6f;
                enemySetter = Random.Range(0, 2);
                break;
            // Medium speed more amount of all types with lesser spikies
            case 8:
                phase = 8;
                tickSetter = 0.4f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = Random.Range(0,1);
                else
                    enemySetter = 2;
                break;
            // Fast speed less amount of fugus
            case 9:
                phase = 9;
                tickSetter = 0.7f;
                enemySetter = 1;
                break;
            // Fast speed less amount of spikies
            case 10:
                phase = 10;
                tickSetter = 0.7f;
                enemySetter = 2;
                break;
            // Slow speed more amount of babies
            case 11:
                phase = 11;
                tickSetter = 0.4f;
                enemySetter = 0;
                break;
            // Slow speed more amount of fugus
            case 12:
                phase = 12;
                tickSetter = 0.4f;
                enemySetter = 1;
                break;
            // Slow speed more amount of spikies
            case 13:
                phase = 13;
                tickSetter = 0.4f;
                enemySetter = 2;
                break;
            // Slow speed more amount of fugus and fast speed less amount of spikies
            case 14:
                phase = 14;
                tickSetter = 0.4f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 1;
                else
                    enemySetter = 2;
                break;
            // Slow speed more amount of fugus and fast speed less amount of babies
            case 15:
                phase = 15;
                tickSetter = 0.4f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 1;
                else
                    enemySetter = 0;
                break;
            // Slow speed more amount of spikies and fast speed less amount of babies
            case 16:
                phase = 16;
                tickSetter = 0.4f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 2;
                else
                    enemySetter = 0;
                break;
            // Slow speed more amount of spikies and fast speed less amount of fugus
            case 17:
                phase = 17;
                tickSetter = 0.4f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 2;
                else
                    enemySetter = 1;
                break;
            // Medium speed more amount of all types
            default:
                tickSetter = 0.4f - ((time - 400f) / 10000);
                phase = 18;
                enemySetter = Random.Range(0, 2);
                break;
        }
        GameObject newEnemy = Instantiate(enemy[enemySetter]);
    }
}
