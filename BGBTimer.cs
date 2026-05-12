using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BGBTimer : MonoBehaviour
{
    public TextMeshProUGUI timerDisplay;
    public float timer;
    public BGBlitzManager bgbScript;
    public GameObject gameReport;
    public GameObject highScoreImage;

    public AudioSource applause;
    bool applausePlayed;
    // Start is called before the first frame update
    void Start()
    {
        timer = 600f;
        timerDisplay.enabled = false;
        gameReport.SetActive(false);
        highScoreImage.SetActive(false);
        applausePlayed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (bgbScript.gameActive)
        {
            timerDisplay.enabled = true;
            timer -= Time.deltaTime;
        }

        DisplayTime(timer);

        if (timer <= 0f)
        {
            bgbScript.gameActive = false;
            if (bgbScript.score > PlayerPrefs.GetInt("HighScore", 0))
            {
                bgbScript.bestScore = bgbScript.score;
                highScoreImage.SetActive(true);
                PlayerPrefs.SetInt("HighScore", bgbScript.score);
                PlayerPrefs.Save();
            }

            if (!applausePlayed)
            {
                applause.Play();
                applausePlayed = true;
            }
            PlayerPrefs.SetInt("HasPlayedBefore", 1);
            gameReport.SetActive(true);
            bgbScript.gameCursorImage.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0f;
        }
    }

    void DisplayTime(float time)
    {
        int minutes = Mathf.FloorToInt(time / 60f);
        int seconds = Mathf.FloorToInt(time % 60f);

        timerDisplay.text = string.Format("{0:00}:{1:00}", minutes, seconds);
    }
}
