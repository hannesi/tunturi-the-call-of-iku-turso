using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CombatUIManager : MonoBehaviour
{
    public GameObject canvas;
    public HealthBar healthBar;
    public TextMeshProUGUI actionPoints;

    // Start is called before the first frame update
    // void Start()
    // {
    // }

    // Update is called once per frame
    // void Update()
    // {
    // }
    public void SetHealthBarValue(int healthCurrent, int healthMax) {
        healthBar.SetHealth(healthCurrent, healthMax);
    }
    public void SetAP(int apCurrent, int apMax = 0) {
        Debug.Log("TODO: Proper implementation for combatUI ActionPoints");
        actionPoints.text = $"AP: {apCurrent}";
    }

    public void Show() {
        canvas.SetActive(true);
    }

    public void Hide() {
        canvas.SetActive(false);
    }
}
