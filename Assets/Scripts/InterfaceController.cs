using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class InterfaceController : MonoBehaviour
{
    private MovementPlayer player_controller;
    public Slider slider_playerHealth;
    public GameObject gameoverPanel;
    public Text text_SurviveTime;
    public Text text_SurviveTimeTop;
    private float bestScore;

    // Start is called before the first frame update
    void Start() {
        player_controller = GameObject.FindWithTag("Jogador").GetComponent<MovementPlayer>();

        slider_playerHealth.maxValue = player_controller.status_controller.life;
        UpdateSliderPlayerHealth();
        bestScore = PlayerPrefs.GetFloat("BestScore");
    }

    // Update is called once per frame
    void Update() {
    }

    public void UpdateSliderPlayerHealth(){
        slider_playerHealth.value = player_controller.status_controller.life;
    }

    public void GameOver(){
        gameoverPanel.SetActive(true);
        Time.timeScale = 0;

        int min = (int)(Time.timeSinceLevelLoad / 60); // time in minutes, convert to int
        int seg = (int)(Time.timeSinceLevelLoad % 60);
        text_SurviveTime.text = "Você sobreviveu " + min + "min e " + seg + "seg";

        UpdateBestScore(min, seg);
    }

    void UpdateBestScore(int min, int seg){
        if(Time.timeSinceLevelLoad > bestScore){
            bestScore = Time.timeSinceLevelLoad;
            text_SurviveTimeTop.text = string.Format("Seu melhor tempo é {0}min e {1}seg", min, seg);
            PlayerPrefs.SetFloat("BestScore", bestScore);
        }

        if(text_SurviveTimeTop.text == ""){
            min = (int)bestScore / 60;
            seg = (int)bestScore % 60;
            text_SurviveTimeTop.text = string.Format("Seu melhor tempo é {0}min e {1}seg", min, seg);
        }
    }

    public void Restart(){
        SceneManager.LoadScene("MainScene");
    }
}
