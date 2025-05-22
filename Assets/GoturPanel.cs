using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class GoturPanel : MonoBehaviour
{
    public TMP_Text text;
    public GameData gameData;
    public float maxTime;
    public float timeLeft = 0f;
    public Slider slider;
    TimeSpan timeSpan;
    private void OnEnable()
    {
        maxTime = timeLeft;
        slider.maxValue = timeLeft;
        slider.value = 0f;

    }
    private void Update()
    {
        timeLeft -= Time.deltaTime;
        slider.value = maxTime - timeLeft;
        timeSpan = TimeSpan.FromSeconds(timeLeft);
        text.text = string.Format("{0:00}:{1:00}", timeSpan.Minutes, timeSpan.Seconds);
        if (timeLeft < 0f)
        {
            gameData.OrderArrived();
            gameObject.SetActive(false);
        }

    }
}
