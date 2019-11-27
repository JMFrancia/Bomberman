using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CountdownTimer : MonoBehaviour
{
    [SerializeField] bool noTimer = false;
    [SerializeField] float matchLength = 180f;

    float timeLeft;
    TextMeshProUGUI tm;

    private void Awake()
    {
        tm = GetComponent<TextMeshProUGUI>();
        if(noTimer) {
            tm.gameObject.SetActive(false);
        } else {
            timeLeft = matchLength;
        }
    }


    private void Update()
    {
        if (noTimer)
            return;

        if (timeLeft > 0)
        {
            timeLeft = Mathf.Max(0f, timeLeft - Time.deltaTime);
            tm.text = FormatTime(timeLeft);
        } else {
            EventManager.TriggerEvent(EventName.TIME_UP);
        }
    }

    string FormatTime(float time) {
        int minutes = Mathf.FloorToInt(time / 60F);
        int seconds = Mathf.FloorToInt(time - minutes * 60);
        return string.Format("{0:0}:{1:00}", minutes, seconds);
    }

}
