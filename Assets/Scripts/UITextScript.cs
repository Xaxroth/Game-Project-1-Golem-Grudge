using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UITextScript : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI countDownText;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        countDownText.text = GameManager.turnCountdown.ToString();
    }
}
