using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HighScoreGUIController : MonoBehaviour {
    private Text text;

	// Use this for initialization
	void Start () {
        text = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        text.text = "High Score: " + PlayerPrefs.GetFloat("HighScore").ToString("F2");
	}
}
