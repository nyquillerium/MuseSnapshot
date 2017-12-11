using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSwap : MonoBehaviour {

    public int FaceState;
    public Sprite smileTex;
    public Sprite notSmileTex;
    private OSCConnection manager;

    IEnumerator NumberGen()
    {
        while (true)
        {
            FaceState = Random.Range(0, 2);
            Debug.Log("New face: " + FaceState);
            yield return new WaitForSeconds(3);
        }
    }

    void FinishRound()
    {
        manager.Calibrating = false;
    }

    // Use this for initialization
    void Start () {
        manager = GameObject.FindObjectOfType<OSCConnection>();
        FaceState = 0;
        StartCoroutine(NumberGen());
    }
	
	// Update is called once per frame
	void Update () {
		switch (FaceState)
        {
            case 0:
                gameObject.GetComponent<Image>().sprite = smileTex;
                break;
            case 1:
                gameObject.GetComponent<Image>().sprite = notSmileTex;
                break;
        }

	}
}
