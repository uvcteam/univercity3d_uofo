using System;
using UnityEngine;
using System.Collections;

public class NumberManipulator : MonoBehaviour
{
    public int min = 4;
    public int max = 99;
    public bool isMin = false;
    private UILabel number;

    void Awake()
    {
        number = GetComponent<UILabel>();
    }

    void OnIncrementClicked()
    {
        if (Convert.ToInt32(number.text) + 1 >= max)
            number.text = max.ToString();
        else
            number.text = (Convert.ToInt32(number.text) + 1).ToString();

        if (isMin) SendMessageUpwards("OnMinChanged");
        else SendMessageUpwards("OnMaxChanged");
    }

    void OnDecrementClicked()
    {
        if (Convert.ToInt32(number.text) - 1 <= min)
            number.text = min.ToString();
        else
            number.text = (Convert.ToInt32(number.text) - 1).ToString();

        if (isMin) SendMessageUpwards("OnMinChanged");
        else SendMessageUpwards("OnMaxChanged");
    }
}