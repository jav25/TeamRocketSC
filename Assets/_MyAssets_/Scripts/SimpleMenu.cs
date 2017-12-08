using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SimpleMenu : MonoBehaviour {

    //public Texture2D bg;
    public string nextScene;
    //public string btnText;
    public Button startButton;
    // Use this for initialization
    void Start () {
        Button btn = startButton.GetComponent<Button>();
        btn.onClick.AddListener(NextLevel);
    }
	
	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    void NextLevel()
    {
        Debug.Log("You have clicked the button!");
        SceneManager.LoadScene(nextScene);
    }

    //private void OnGUI()
    //{
    //GUI.DrawTexture(new Rect(0, 0, Screen.width, Screen.height), bg);
    //   if(GUI.Button(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 50, 100, 100), btnText))
    // {
    //   SceneManager.LoadScene(nextScene);//use scene manager to load next scene
    //}
    //}
}
