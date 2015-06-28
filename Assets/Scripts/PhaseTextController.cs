using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Soomla.Store;

public class PhaseTextController : MonoBehaviour {
    private Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        int phase = SpawnController.instance.phase + 1;
        //text.text = phase.ToString();
        text.text = StoreInventory.GetItemBalance(FranticFuguAssets.CURRENCY_SPONGE_ID).ToString();
	}
}
