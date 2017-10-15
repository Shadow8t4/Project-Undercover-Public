using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class WinAnimationController : MonoBehaviour {

    [SerializeField]
    private Image _spySprite, _guardSprite, _starSprite;

    [SerializeField]
    private Text _winText;

    [SerializeField]
    private Image _winAnimationPanel;

    delegate T GetDelegate<T>();
    delegate void SetDelegate<T>(T value);


    public class Vector3Ref
    {
        public Vector3 refVar;
        public Vector3Ref(ref Vector3 refVar)
        {
            this.refVar = refVar;
        }
    }

    void Start () {
        StartCoroutine(WinAnimation(false));	
	}

    IEnumerator WinAnimation(bool guardsOrSpies)
    {
        _winText.gameObject.SetActive(true);
        _starSprite.gameObject.SetActive(true);
        RectTransform winnerTrans = _guardSprite.rectTransform;
        if (guardsOrSpies)
        {
            _guardSprite.gameObject.SetActive(true);
            _spySprite.gameObject.SetActive(false);
            _winText.text = "Guards\nWin!";
        }
        else
        {
            winnerTrans = _spySprite.rectTransform;
            _spySprite.gameObject.SetActive(true);
            _guardSprite.gameObject.SetActive(false);
            _winText.text = "Spies\nWin!";
        }

        StartCoroutine(FadeOutBackground(1));

        // Initialize positions
        Vector2 initialPos = winnerTrans.localPosition;
        RectTransform starTrans = _starSprite.rectTransform;
        Vector2 starSize = starTrans.sizeDelta;
        starTrans.sizeDelta = new Vector2(0, 0);
        winnerTrans.localPosition = new Vector2(0, Screen.height/2 + winnerTrans.sizeDelta.y / 2.0f);

        // Smoothly animate drop down for winnerTrans
        GetDelegate<Vector2> getter = () => { return winnerTrans.localPosition; };
        SetDelegate<Vector2> setter = v => { winnerTrans.localPosition = v; };
        yield return StartCoroutine(SmoothVector2Lerp(getter, setter, winnerTrans.localPosition, initialPos, 2.0f));

        // Spin the star simultaneously
        StartCoroutine(SpinStar(20.0f));

        // Smoothly animate star reveal
        getter = () => { return starTrans.sizeDelta; };
        setter = v => { starTrans.sizeDelta = v; };
        yield return StartCoroutine(SmoothVector2Lerp(getter, setter, starTrans.sizeDelta, starSize, 4.0f));

        // Reveal Win Text
        yield return StartCoroutine(RevealWinText(1));
        yield return null;
    }

    IEnumerator SmoothVector2Lerp(GetDelegate<Vector2> getter, SetDelegate<Vector2> setter, Vector2 initial, Vector2 final, float speed)
    {
        setter(initial);
        Vector2 yVelocity = final - initial;
        float magnitude = yVelocity.magnitude;
        yVelocity = yVelocity.normalized;
        Vector2 overShotFinal = final + (yVelocity * magnitude * 0.1f);
        while (true)
        {
            Vector2 newVec = Vector2.Lerp(getter(), overShotFinal, Time.deltaTime * speed);
            Vector2 checkDireciton = (final - newVec).normalized;
            if (checkDireciton != yVelocity)
            {
                newVec = final;
                setter(newVec);
                break;
            }
            setter(newVec);
            yield return new WaitForEndOfFrame();
        }
        yield return null;
    }

    IEnumerator SpinStar(float speed)
    {
        float spin = 0.0f;
        while (true)
        {
            spin += Time.deltaTime * speed;
            _starSprite.rectTransform.localRotation = Quaternion.Euler(0, 0, spin);
            yield return new WaitForEndOfFrame();
        }
    }

    IEnumerator FadeOutBackground(float speed)
    {
        float time = 0.0f;
        float limit = 0.5f;
        while (time < limit)
        {
            _winAnimationPanel.color = new Color(0, 0, 0, time);
            float elapsedTime = Time.deltaTime * speed;
            time += elapsedTime;
            yield return new WaitForEndOfFrame();
        }
        _winAnimationPanel.color = new Color(0, 0, 0, limit);
        yield return null;
    }

    IEnumerator RevealWinText(float speed)
    {
        float time = 0.0f;
        float limit = 1.0f;
        while (time < limit)
        {
            _winText.color = new Color(1, 1, 1, time);
            float elapsedTime = Time.deltaTime * speed;
            time += elapsedTime;
            yield return new WaitForEndOfFrame();
        }
        _winText.color = new Color(1, 1, 1, limit);
        yield return null;
    }
}
