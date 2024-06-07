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
    public Text text_enemiesKilled;
    public Text text_bossWarning;
    private float bestScore;
    private int enemiesKilledQuantity;

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

    public void UpdateQuantityEnemiesKilled(){
        enemiesKilledQuantity++;
        text_enemiesKilled.text = string.Format("X {0}", enemiesKilledQuantity);
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

    public void BossWarning() {
        StartCoroutine(FadeText(1, text_bossWarning));
    }

    IEnumerator FadeText(float time, Text textToFade){

        textToFade.gameObject.SetActive(true);
        Color textColor = textToFade.color;
        textColor.a = 1;
        textToFade.color = textColor;
        yield return new WaitForSeconds(time);
        float counter = 0;
        while(textToFade.color.a > 0)
        {
            counter += Time.deltaTime / time;
            textColor.a = Mathf.Lerp(1, 0, counter);
            textToFade.color = textColor;
            if (textToFade.color.a <= 0) {
                textToFade.gameObject.SetActive(false);
            }
            yield return null;
        }
    }
}
