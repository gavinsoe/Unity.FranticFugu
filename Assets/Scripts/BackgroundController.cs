using UnityEngine;
using System.Collections;

public class BackgroundController : MonoBehaviour {
    private GameObject background;
    private GameObject playArea;

	// Use this for initialization
	void Start () {
	
	}

    void Awake()
    {
        background = GameObject.Find("BackgroundQuad");
        playArea = GameObject.Find("PlayAreaQuad");

        float quadHeight = Camera.main.orthographicSize * 2f;
        float quadWidht = quadHeight * Screen.width / Screen.height;
        background.transform.localScale.Set(quadWidht, quadHeight, 1f);
        playArea.transform.localScale.Set(quadWidht * 0.75f, quadHeight * 0.75f, 1f);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
