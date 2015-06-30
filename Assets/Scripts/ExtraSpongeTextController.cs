using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ExtraSpongeTextController : MonoBehaviour {
    private Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        float time = SpawnController.instance.time;
        int minutes = Mathf.FloorToInt(time) / 60;
        int sponges = minutes * 4;
        text.text = sponges.ToString();
	}
}
