using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScorePanelController : Photon.PunBehaviour {

    public Text timerText;
    public Image guardScore, spyScore;

    private float initalScoreWidth;
    private static ScorePanelController mSingleton;

    public static ScorePanelController Singleton { get { return mSingleton; } }

    /**
     * Setup the ScorePanelController.
     */
    void Start()
    {
        // set the singleton
        if (mSingleton)
        {
            Debug.LogError("Two ScorePanelControllers in the scene");
        }
        mSingleton = this;

        // start the timer update coroutine
        StartCoroutine(TimerUpdate());

        // initialize scorebar variables
        initalScoreWidth = spyScore.rectTransform.sizeDelta.x;
        spyScore.rectTransform.sizeDelta = new Vector2(-1, spyScore.rectTransform.sizeDelta.y);
        guardScore.rectTransform.sizeDelta = new Vector2(-1, guardScore.rectTransform.sizeDelta.y);
    }

    /**
     * Update the score panel to reflect a new guard score
     */
    public void UpdateGuardScore(float progress)
    {
        StartCoroutine(IncreaseScoreBarAnimation(guardScore, progress));
    }

    /**
     * Update the score panel to reflect a new spy score
     */
    public void UpdateSpyScore(float progress)
    {
        StartCoroutine(IncreaseScoreBarAnimation(spyScore, progress));
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
            timerText.text = timeString;
        }
    }

    IEnumerator IncreaseScoreBarAnimation(Image scoreBar, float progress)
    {
        Color originalColor = scoreBar.color;
        var flashCoroutine = StartCoroutine(FlashScoreBar(scoreBar));
        float targetWidth = initalScoreWidth * progress;
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

        if (progress >= 1.0)
        {
            if (scoreBar == guardScore)
            {
                photonView.RPC("ShowWinScreen", PhotonTargets.All, true);
            }
            else
            {
                photonView.RPC("ShowWinScreen", PhotonTargets.All, false);
            }
        }

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
    #endregion

    #region PunRPC
    [PunRPC]
    void ShowWinScreen(bool guardsOrSpies)
    {
        WinAnimationController.ActiveController.PlayWinAnimation(guardsOrSpies);
    }
    #endregion
}
