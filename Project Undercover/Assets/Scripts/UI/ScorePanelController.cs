using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanelController : Photon.PunBehaviour {

    public Text _timerText, winText;
    public Image _guardScore, _spyScore;
    public Text missionsCompleteText;
    public GameObject winPanel;
    public GameObject guardPanel;
    public GameObject spyPanel;

    private int _numOfMissions = 3;
    private int _missionsComplete = 0;
    private float waitBetweenMissions = 5.0f;
    private bool onMissionCooldown = false;

    private float _initalScoreWidth;
    private static ScorePanelController ActivePanel;

    void Start () {
        if (!ActivePanel)
            ActivePanel = this;
        else
            Debug.LogError("Two ScorePanelControllers in the scene");
        StartCoroutine(TimerUpdate());

        // Initialize scorebar variables
        _initalScoreWidth = _spyScore.rectTransform.sizeDelta.x;
        _spyScore.rectTransform.sizeDelta = new Vector2(-1, _spyScore.rectTransform.sizeDelta.y);
        _guardScore.rectTransform.sizeDelta = new Vector2(-1, _guardScore.rectTransform.sizeDelta.y);
    }

    public static void CompleteMission()
    {
        if (ActivePanel.onMissionCooldown)
            return;

        ActivePanel.photonView.RPC("CompleteMissionRPC", PhotonTargets.All);
    }

    #region Coroutines
    IEnumerator TimerUpdate()
    {
        while (true)
        {
            yield return new WaitForEndOfFrame();
            int seconds = (int)Time.timeSinceLevelLoad;
            int minutes = seconds / 60;
            seconds = seconds % 60;
            string timeString = "";
            if (seconds < 10)
                timeString = minutes.ToString() + ":0" + seconds.ToString();
            else
                timeString = minutes.ToString() + ":" + seconds.ToString();
            _timerText.text = timeString;
        }
    }

    IEnumerator IncreaseScoreBarAnimation(Image scoreBar, float progress)
    {
        Color originalColor = scoreBar.color;
        var flashCoroutine = StartCoroutine(FlashScoreBar(scoreBar));
        float targetWidth = _initalScoreWidth * progress;
        float overshotWidth = targetWidth * 1.2f;
        while (true)
        {
            Vector2 sizeDelta = scoreBar.rectTransform.sizeDelta;
            float newWidth = Mathf.Lerp(sizeDelta.x, overshotWidth, Time.deltaTime * 0.8f);
            if (sizeDelta.x < targetWidth)
            {
                scoreBar.rectTransform.sizeDelta = new Vector2(newWidth, sizeDelta.y);
                yield return new WaitForEndOfFrame();
            }
            else
            {
                scoreBar.rectTransform.sizeDelta = new Vector2(targetWidth, sizeDelta.y);
                break;
            }
        }
        StopCoroutine(flashCoroutine);
        StartCoroutine(ResetScoreBarColor(scoreBar, originalColor));
        yield return null;
    }

    IEnumerator FlashScoreBar(Image scoreBar)
    {
        Color darkerColor = scoreBar.color;
        darkerColor.r *= 0.5f;
        darkerColor.g *= 0.5f;
        darkerColor.b *= 0.5f;
        HSBColor darkColor = HSBColor.FromColor(darkerColor);
        HSBColor flashColor = HSBColor.FromColor(Color.yellow);
        HSBColor currentColor = darkColor;
        bool pingPong = true;
        float time = 0.0f;
        while (true)
        {
            float elapsedTime = Time.deltaTime * 2.0f;
            if (pingPong)
                time += elapsedTime;
            else
                time -= elapsedTime;
            time = Mathf.Clamp01(time);
            if (time == 0.0f)
                pingPong = true;
            else if (time == 1.0f)
                pingPong = false;

            currentColor = HSBColor.Lerp(darkColor, flashColor, time);
            scoreBar.color = currentColor.ToColor();
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator ResetScoreBarColor(Image scoreBar, Color originalColor)
    {
        float time = 0.0f;
        HSBColor startColor = HSBColor.FromColor(scoreBar.color);
        HSBColor endColor = HSBColor.FromColor(originalColor);
        HSBColor currentColor = startColor;
        while (true)
        {
            time += Time.deltaTime * 2.0f;
            time = Mathf.Clamp01(time);
            currentColor = HSBColor.Lerp(startColor, endColor, time);
            scoreBar.color = currentColor.ToColor();
            if (time >= 0.90f)
            {
                scoreBar.color = originalColor;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator MissionCooldown()
    {
        onMissionCooldown = true;
        yield return new WaitForSeconds(waitBetweenMissions);
        onMissionCooldown = false;
    }
    #endregion

    #region PunRPC
    [PunRPC]
    void CompleteMissionRPC()
    {
        Debug.Log("Mission Completed!");
        _missionsComplete++;
        StartCoroutine(MissionCooldown());
        StartCoroutine(IncreaseScoreBarAnimation(_spyScore, (float)_missionsComplete / _numOfMissions));
        /*if (_missionsComplete >= _numOfMissions)
            photonView.RPC("ShowSpiesWinScreen", PhotonTargets.All);*/
    }

    [PunRPC]
    void ShowSpiesWinScreen()
    {
        winPanel.SetActive(true);
        winText.text = "SPIES WIN!";
    }

    [PunRPC]
    void ShowGuardsWinScreen()
    {
        winPanel.SetActive(true);
        winText.text = "GUARDS WIN!";
    }
    #endregion

}
