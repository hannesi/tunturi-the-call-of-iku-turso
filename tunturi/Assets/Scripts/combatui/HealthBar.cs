using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    public Sprite[] barImages;
    // ui elements
    private Image currentBarImage;
    private TextMeshProUGUI healthBarText;
    // Start is called before the first frame update
    void Start()
    {
        currentBarImage = gameObject.GetComponent<Image>();
        healthBarText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
    }

    public void SetHealth(int healthCurrent, int healthMax) {
        healthBarText.text = $"{healthCurrent} / {healthMax}";
        // TODO: SOS: Sisaltaa mysteerivakioita
        int imageIndex = Math.Max(Math.Min((int)Math.Ceiling((decimal)5 * healthCurrent / healthMax), 5), 0);
        currentBarImage.sprite = barImages[imageIndex];
    }


}
