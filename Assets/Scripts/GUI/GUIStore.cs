using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Soomla.Store;

public class GUIStore : GUIBaseClass 
{
    public static GUIStore instance;

    // GUI Components
    [SerializeField]
    private GameObject CurrencyObject;
    private Text CurrencyText;

    [SerializeField]
    private GameObject UnlockablesOcto;
    private Animator OctoAnimator;

    [SerializeField]
    private GameObject UnlockablesJellyfish;
    private Animator JellyAnimator;

    [SerializeField]
    private GameObject UnlockablesBoxFish;
    private Animator BoxFishAnimator;

    [SerializeField]
    private GameObject UnlockablesCuttlefish;
    private Animator CuttlefishAnimator;

    [SerializeField]
    private GameObject UnlockablesSeaHorse;
    private Animator SeaHorseAnimator;

    [SerializeField]
    private GameObject UnlockablesStingRay;
    private Animator StingRayAnimator;

    // List of locked characters
    private List<Unlockable> unlockables;

	void Awake()
    {
        // make sure there is only 1 instance of this class.
        if (instance == null)
        {
            DontDestroyOnLoad(gameObject);
            instance = this;
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }

        // Retreive transform components
        CurrencyText = CurrencyObject.GetComponent<Text>();
        OctoAnimator = UnlockablesOcto.GetComponent<Animator>();
        JellyAnimator = UnlockablesJellyfish.GetComponent<Animator>();
        BoxFishAnimator = UnlockablesBoxFish.GetComponent<Animator>();
        CuttlefishAnimator = UnlockablesCuttlefish.GetComponent<Animator>();
        SeaHorseAnimator = UnlockablesSeaHorse.GetComponent<Animator>();
        StingRayAnimator = UnlockablesStingRay.GetComponent<Animator>();
    }

    void Start()
    {
        UpdateUnlockables();
    }

    void Update()
    {
        // Update the interface
        UpdateCurrency();
        CheckUnlockables();
    }

    public void UpdateCurrency()
    {
        CurrencyText.text = StoreInventory.GetItemBalance(FranticFuguAssets.CURRENCY_SPONGE_ID).ToString();
    }

    public void CheckUnlockables()
    {
        if (StoreInventory.GetItemBalance(FranticFuguAssets.CHAR_OCTO_ID) > 0)
        {
            // Octopus unlocked
            OctoAnimator.SetBool("Locked", false);
        }
        else
        {
            // Octopus locked
            OctoAnimator.SetBool("Locked", true);
        }

        if (StoreInventory.GetItemBalance(FranticFuguAssets.CHAR_JELLYFISH_ID) > 0)
        {
            // Jellyfish unlocked
            JellyAnimator.SetBool("Locked", false);
        }
        else
        {
            // Jellyfish locked
            JellyAnimator.SetBool("Locked", true);
        }

        if (StoreInventory.GetItemBalance(FranticFuguAssets.CHAR_BOXFISH_ID) > 0)
        {
            // Boxfish unlocked
            BoxFishAnimator.SetBool("Locked", false);
        }
        else
        {
            // Boxfish locked
            BoxFishAnimator.SetBool("Locked", true);
        }

        if (StoreInventory.GetItemBalance(FranticFuguAssets.CHAR_CUTTLEFISH_ID) > 0)
        {
            // Cuttlefish unlocked
            CuttlefishAnimator.SetBool("Locked", false);
        }
        else
        {
            // Cuttlefish locked
            CuttlefishAnimator.SetBool("Locked", true);
        }

        if (StoreInventory.GetItemBalance(FranticFuguAssets.CHAR_SEAHORSE_ID) > 0)
        {
            // Seahorse unlocked
            SeaHorseAnimator.SetBool("Locked", false);
        }
        else
        {
            // Seahorse locked
            SeaHorseAnimator.SetBool("Locked", true);
        }
        if (StoreInventory.GetItemBalance(FranticFuguAssets.CHAR_STINGRAY_ID) > 0)
        {
            // Stingray unlocked
            StingRayAnimator.SetBool("Locked", false);
        }
        else
        {
            StingRayAnimator.SetBool("Locked", true);
        }
    }

    public void SelectCharacter(string characterID)
    {
        StoreInventory.EquipVirtualGood(characterID);
        SpawnController.instance.StartGame();
    }

    public void UnlockNewCharacter()
    {
        UpdateUnlockables();

        // Randomly unlock a character
        var maxNum = unlockables.Sum(x => x.weight);
        var rand = Random.Range(0,maxNum) + 1;
        
        foreach(var unlockable in unlockables)
        {
            rand -= unlockable.weight;
            if (rand <= 0)
            {
                StoreInventory.BuyItem(unlockable.itemID);
                break;
            }
        }
    }

    /// <summary>
    /// This function updates a list what unlockables are yet to be unlocked.
    /// </summary>
    private void UpdateUnlockables()
    {
        unlockables = new List<Unlockable>();

        // Retrieve a list of unlockable characters still available
        foreach (var good in StoreInfo.Goods)
        {
            if (good.GetBalance() <= 0)
            {
                Unlockable new_unlockable = new Unlockable();
                
                new_unlockable.itemID = good.ItemId;
                
                // Set the weight of each item
                switch (new_unlockable.itemID)
                {
                    case FranticFuguAssets.CHAR_JELLYFISH_ID:
                        new_unlockable.weight = 5;
                        break;
                    case FranticFuguAssets.CHAR_BOXFISH_ID:
                        new_unlockable.weight = 5;
                        break;
                    case FranticFuguAssets.CHAR_CUTTLEFISH_ID:
                        new_unlockable.weight = 5;
                        break;
                    case FranticFuguAssets.CHAR_SEAHORSE_ID:
                        new_unlockable.weight = 1;
                        break;
                    case FranticFuguAssets.CHAR_STINGRAY_ID:
                        new_unlockable.weight = 1;
                        break;
                }
                unlockables.Add(new_unlockable);
            }
        }
    }
}

public class Unlockable
{
    public string itemID;

    public int weight; // the higher the number the higher the chances of getting
}