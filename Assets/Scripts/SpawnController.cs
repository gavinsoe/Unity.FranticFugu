using UnityEngine;
using System.Collections;
using Soomla.Highway;
using Soomla.Store;
using GooglePlayGames;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;

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
    public bool phaseChange;

    private BannerView bannerView;
    private InterstitialAd interstitial;

    public GameObject loot;
    public float lootTimer;
    private float timeTillNextLoot;
    private bool lootSpawned;

    public Character[] characters;
    
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
	void Start () 
    {
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
        phaseTimer = 20f;
        phasePicker = Random.Range(0, 7);
        phaseChange = false;

        // Initialise Soomla Highway (Online Statistics)
        SoomlaHighway.Initialize();

        // Initialise Soomla Store
        SoomlaStore.Initialize(new FranticFuguAssets());

        /*
        // Sign in to Google Play Game Services
        Social.localUser.Authenticate((bool success) =>
        {
            // handle success or failure
        });
         */

        // Admob Banner Request
        RequestBanner();
        RequestInterstitial();

        // Temporary for testing
        /*StoreInventory.GiveItem(FranticFuguAssets.CURRENCY_SPONGE_ID, 450);
        StoreInventory.TakeItem(FranticFuguAssets.CHAR_JELLYFISH_ID, 1);
        StoreInventory.TakeItem(FranticFuguAssets.CHAR_CUTTLEFISH_ID, 1);
        StoreInventory.TakeItem(FranticFuguAssets.CHAR_BOXFISH_ID, 1);
        StoreInventory.TakeItem(FranticFuguAssets.CHAR_SEAHORSE_ID, 1);
        StoreInventory.TakeItem(FranticFuguAssets.CHAR_STINGRAY_ID, 1);*/

        // Make sure the octo is unlocked
        if (StoreInventory.GetItemBalance(FranticFuguAssets.CHAR_OCTO_ID) <= 0)
        {
            StoreInventory.GiveItem(FranticFuguAssets.CHAR_OCTO_ID, 1);
            StoreInventory.EquipVirtualGood(FranticFuguAssets.CHAR_OCTO_ID);
        }

        // Load the home screen
        Home();

        // Reset loot timer
        ResetLootTimer();
	}
	
	// Update is called once per frame
	void Update () {
        if (!paused)
        {
            if (invincibility)
            {
                if (invincCounter > 3f)
                {
                    //GameObject temp = GameObject.FindGameObjectWithTag("Player");
                    //temp.GetComponent<PolygonCollider2D>().enabled = true;
                    invincibility = false;
                }
                invincCounter += Time.deltaTime;
            }
            if (time > phaseTimer)
            {
                phaseChange = true;
                if (time <= 60f)
                {
                    phasePicker = Random.Range(0, 7);
                }
                else if (time <= 120f) 
                {
                    phasePicker = Random.Range(8, 11);
                }
                else if (time <= 180f)
                {
                    phasePicker = Random.Range(12, 17);
                }
                else if (time <= 240f)
                {
                    phasePicker = Random.Range(18, 24);
                }
                else
                {
                    phasePicker = Random.Range(25, 28);
                }
            }
            time += Time.deltaTime;
            timeTillNextLoot -= Time.deltaTime;
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

            if (!lootSpawned && time > 30 && timeTillNextLoot < 0)
            {
                SpawnLoot();
            }
        }
	}

    public void StartGame()
    {
        if (PCController.instance != null)
        {
            PCController.instance.gameObject.SetActive(true);
        }
        ResetField();

        phasePicker = Random.Range(0, 7);
        paused = false;
        time = 0f;
        tick = 0f;
        phaseTimer = 20f;
        videoWatched = false;
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
        GUIStore.instance.Hide();

        // Admob Banner
        bannerView.Hide();
    }

    public void Store()
    {
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
        GUIStore.instance.Show();
        PCController.instance.gameObject.SetActive(false);

        // Admob Banner
        bannerView.Hide();
    }

    public void Home()
    {
        ResetField();
        startCanvas.SetActive(true);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
        videoCanvas.SetActive(false);
        GUIStore.instance.Hide();
        PCController.instance.gameObject.SetActive(true);

        // Admob Banner
        bannerView.Show();
    }

    public void PauseGame()
    {
        paused = true;
        startCanvas.SetActive(false);
        gameCanvas.SetActive(false);
        pauseCanvas.SetActive(true);
        endCanvas.SetActive(false);
        GUIStore.instance.Hide();
        PCController.instance.gameObject.SetActive(true);

        // Admob Banner
        bannerView.Show();
    }

    public void UnpauseGame()
    {
        paused = false;
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
        GUIStore.instance.Hide();
        PCController.instance.gameObject.SetActive(true);

        // Admob Banner
        bannerView.Hide();
    }

    public void EndGame()
    {
        if (!invincibility)
        {
            paused = true;
            startCanvas.SetActive(false);
            gameCanvas.SetActive(false);
            pauseCanvas.SetActive(false);
            endCanvas.SetActive(true);
            GUIStore.instance.Hide();
            PCController.instance.gameObject.SetActive(true);

            if (!videoWatched)
            {
                //videoWatched = true;
                videoCanvas.SetActive(true);
            }
            else
            {
                // Admob Banner
                bannerView.Show();

                CleanGameField();
            }

            // Do Highest score check and save
            float highScore = PlayerPrefs.GetFloat("HighScore");
            if (time > highScore)
            {
                PlayerPrefs.SetFloat("HighScore", time);
                PlayerPrefs.Save();

                /*
                // Post to leaderboard
                Social.ReportScore((long)time, "LEADERBOARD_ID", (bool success) => { });
                 */
            }
        }
    }

    /// <summary>
    /// Simply destroys all the objects in the game
    /// </summary>
    private void CleanGameField()
    {
        // Destroy enemies
        GameObject[] list = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (GameObject item in list)
        {
            Destroy(item);
        }

        // Destroy loot
        GameObject[] loot = GameObject.FindGameObjectsWithTag("Loot");
        foreach (GameObject item in loot)
        {
            Destroy(item);
        }

        // Destroy character
        GameObject temp = GameObject.FindGameObjectWithTag("Player");
        Destroy(temp);
    }

    private void ResetField()
    {
        CleanGameField();
        SpawnCharacter();
    }

    private void SpawnCharacter()
    {
        // Check which character is equipped
        foreach (var character in characters)
        {
            if (StoreInventory.IsVirtualGoodEquipped(character.id))
            {
                Instantiate(character.prefab);
                return;
            }
        }

        // if you reach this point no character is equipped, hence equip the octo
        StoreInventory.EquipVirtualGood(FranticFuguAssets.CHAR_OCTO_ID);
        // Try to equip again
        foreach (var character in characters)
        {
            if (StoreInventory.IsVirtualGoodEquipped(character.id))
            {
                Instantiate(character.prefab);
                return;
            }
        }
    }

    public void WatchVideo()
    {
        //Watch Video
        if (interstitial.IsLoaded())
        {
            interstitial.Show();
        }
        
        videoWatched = true;
        invincibility = true;
        invincCounter = 0f;
        //GameObject temp = GameObject.FindGameObjectWithTag("Player");
        //temp.GetComponent<PolygonCollider2D>().enabled = false;

        int chance = Random.Range(0, 1);
        if (chance == 1)
        {
            int minutes = Mathf.FloorToInt(time) / 60;
            StoreInventory.GiveItem(FranticFuguAssets.CURRENCY_SPONGE_ID, minutes * 4);
        }

        RequestInterstitial();

        paused = false;
        startCanvas.SetActive(false);
        gameCanvas.SetActive(true);
        pauseCanvas.SetActive(false);
        endCanvas.SetActive(false);
        videoCanvas.SetActive(false);
        GUIStore.instance.Hide();
        PCController.instance.gameObject.SetActive(true);
    }

    public void DontWatchVideo()
    {
        videoWatched = true;
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
        double phaseLength = 20f;
        switch(phasePicker)
        {
            // Medium speed medium amount of babies | Tier 1
            case 0:
                phase = 0;
                tickSetter = 0.6f;
                enemySetter = 0;
                phaseLength = 20f;
                break;

            // Slow speed more amount of babies | Tier 1
            case 1:
                phase = 1;
                tickSetter = 0.4f;
                enemySetter = 0;
                phaseLength = 20f;
                break;

            // Slow speed medium amount Fugus | Tier 1
            case 2:
                phase = 2;
                tickSetter = 0.6f;
                enemySetter = 1;
                phaseLength = 20f;
                break;

            // Slow speed medium amount of babies | Tier 1
            case 3:
                phase = 3;
                tickSetter = 0.6f;
                enemySetter = 0;
                phaseLength = 20f;
                break;

            // Fast speed very low amount of babies | Tier 1
            case 4:
                phase = 4;
                tickSetter = 0.9f;
                enemySetter = 0;
                phaseLength = 20f;
                break;

            // Slow speed less amount of spikies | Tier 1
            case 5:
                phase = 5;
                tickSetter = 0.7f;
                enemySetter = 2;
                phaseLength = 20f;
                break;

            // Medium speed less amount of spikies | Tier 1
            case 6:
                phase = 6;
                tickSetter = 0.7f;
                enemySetter = 2;
                phaseLength = 20f;
                break;

            // Fast speed very low amount of spikies | Tier 1
            case 7:
                phase = 7;
                tickSetter = 0.9f;
                enemySetter = 2;
                phaseLength = 20f;
                break;

            // Fast speed medium amount of fugus | Tier 2
            case 8:
                phase = 8;
                tickSetter = 0.6f;
                enemySetter = 1;
                phaseLength = 20f;
                break;

            // Medium speed medium amount Fugus | Tier 2
            case 9:
                phase = 9;
                tickSetter = 0.6f;
                enemySetter = 1;
                phaseLength = 20f;
                break;

            // Army of slow moving babies | Tier 2
            case 10:
                phase = 10;
                tickSetter = 0.3f;
                enemySetter = 0;
                phaseLength = 20f;
                break;

            // Slow speed medium amount of spikies | Tier 2
            case 11:
                phase = 11;
                tickSetter = 0.6f;
                enemySetter = 2;
                phaseLength = 20f;
                break;

            // Fast speed less amount of babies | Tier 3
            case 12:
                phase = 12;
                tickSetter = 0.7f;
                enemySetter = 0;
                phaseLength = 20f;
                break;

            // Medium speed medium amount of spikies | Tier 3
            case 13:
                phase = 13;
                tickSetter = 0.6f;
                enemySetter = 2;
                phaseLength = 20f;
                break;

            // Medium speed medium amount of all types | Tier 3
            case 14:
                phase = 14;
                tickSetter = 0.6f;
                enemySetter = Random.Range(0, 2);
                phaseLength = 20f;
                break;

            // Fast speed less amount of fugus | Tier 3
            case 15:
                phase = 15;
                tickSetter = 0.7f;
                enemySetter = 1;
                phaseLength = 20f;
                break;

            // Slow speed more amount of fugus | Tier 3
            case 16:
                phase = 16;
                tickSetter = 0.4f;
                enemySetter = 1;
                phaseLength = 20f;
                break;

            // Slow speed more amount of spikies | Tier 3
            case 17:
                phase = 17;
                tickSetter = 0.4f;
                enemySetter = 2;
                phaseLength = 20f;
                break;

            // Slow speed more amount of babies and fast speed less amount of fugus | Tier 4
            case 18:
                phase = 18;
                tickSetter = 0.4f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 0;
                else
                    enemySetter = 1;
                break;

            // Slow speed more amount of babies and fast speed less amount of spikies | Tier 4
            case 19:
                phase = 19;
                tickSetter = 0.4f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 0;
                else
                    enemySetter = 2;
                break;
            
            // Medium speed more amount of all types with lesser spikies | Tier 4
            case 20:
                phase = 20;
                tickSetter = 0.4f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 8)
                    enemySetter = Random.Range(0,1);
                else
                    enemySetter = 2;
                break;
            
            // Fast speed less amount of spikies | Tier 4
            case 21:
                phase = 21;
                tickSetter = 0.7f;
                enemySetter = 2;
                phaseLength = 20f;
                break;
            
            // Slow speed more amount of fugus and fast speed less amount of spikies | Tier 4
            case 22:
                phase = 22;
                tickSetter = 0.4f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 1;
                else
                    enemySetter = 2;
                break;

            // Slow speed more amount of fugus and fast speed less amount of babies | Tier 4
            case 23:
                phase = 23;
                tickSetter = 0.4f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 9)
                    enemySetter = 1;
                else
                    enemySetter = 0;
                break;

            // Medium speed less amount of babies and more amount of fugus and spikies | Tier 4
            case 24:
                phase = 24;
                tickSetter = 0.6f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 2)
                    enemySetter = 0;
                else
                {
                    enemySetter = Random.Range(1, 2);
                }
                break;
            
            // Slow speed more amount of spikies and fast speed less amount of babies | Tier 5
            case 25:
                phase = 25;
                tickSetter = 0.5f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 8)
                    enemySetter = 2;
                else
                    enemySetter = 0;
                break;
            // Slow speed more amount of spikies and fast speed less amount of fugus | Tier 5
            case 26:
                phase = 26;
                tickSetter = 0.5f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 7)
                    enemySetter = 2;
                else
                    enemySetter = 1;
                break;
            
            // Slow speed slightly more amount of babies and fast speed slightly less amount of fugus | Tier 5
            case 27:
                phase = 27;
                tickSetter = 0.4f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 6)
                    enemySetter = 0;
                else
                    enemySetter = 1;
                break;
            
            // Slow speed medium amount of fugus and fast speed slightly less amount of babies | Tier 5
            case 28:
                phase = 28;
                tickSetter = 0.4f;
                phaseLength = 20f;
                picker = Random.Range(0, 10);
                if (picker < 7)
                    enemySetter = 1;
                else
                    enemySetter = 0;
                break;
            
            // Medium speed more amount of all types
            default:
                tickSetter = 0.4f - ((time - 400f) / 10000);
                phase = 18;
                enemySetter = Random.Range(0, 2);
                phaseLength = 20f;
                break;
        }
        if (phaseChange)
        {
            phaseChange = false;
            phaseTimer += phaseLength;
        }
        GameObject newEnemy = Instantiate(enemy[enemySetter]);
    }

    public void SpawnLoot()
    {
        Instantiate(loot);
        lootSpawned = true;
    }

    public void ResetLootTimer()
    {
        timeTillNextLoot = lootTimer;
        lootSpawned = false;
    }

    public void ShowLeaderboard()
    {
        PlayGamesPlatform.Instance.ShowLeaderboardUI("LEADERBOARD_ID");
    }

    private void RequestBanner()
    {
    #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1741811527316190/7681476465";
    #elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_BANNER_AD_UNIT_ID_HERE";
    #else
        string adUnitId = "unexpected_platform";
    #endif

        // Create a 320x50 banner at the bottom of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request
        //AdRequest request = new AdRequest.Builder().Build();
        AdRequest request = new AdRequest.Builder()
               .AddTestDevice(AdRequest.TestDeviceSimulator)   // Simulator
               .AddTestDevice("3de2c1414b9811c9")                     // can only be found in the logs
               .Build();
        // Load the banner with the request
        bannerView.LoadAd(request);

        // For Custom ad size
        /*
         * AdSize adSize = new AdSize(250, 250);
         * BannerView bannerView = new BannerView(adUnitId, adSize, AdPosition.Bottom);
         */

        // To Test Ads without generating false impressions
        /*
         * AdRequest request = new AdRequest.Builder()
         *      .AddTestDevice(AdRequest.TestDeviceSimulator)   // Simulator
         *      .AddTestDevice("DEVICE_ID")                     // can only be found in the logs
         *      .Build();
         */
    }

    private void RequestInterstitial()
    {
    #if UNITY_ANDROID
        string adUnitId = "ca-app-pub-1741811527316190/9158209664";
    #elif UNITY_IPHONE
        string adUnitId = "INSERT_IOS_INTERSTITIAL_AD_UNIT_ID_HERE";
    #else
        string adUnitId = "unexpected_platform";
    #endif

        // Initialize an InterstitialAd.
        interstitial = new InterstitialAd(adUnitId);
        // Create an empty ad request;
        //AdRequest request = new AdRequest.Builder().Build();
        AdRequest request = new AdRequest.Builder()
               .AddTestDevice(AdRequest.TestDeviceSimulator)   // Simulator
               .AddTestDevice("3de2c1414b9811c9")                     // can only be found in the logs
               .Build();
        // Load the interstitial with the request.
        interstitial.LoadAd(request);
    }

    void OnApplicationQuit()
    {
        bannerView.Destroy();
        interstitial.Destroy();
    }
}

[System.Serializable]
public class Character : System.Object
{
    public string id;
    public GameObject prefab;
}
