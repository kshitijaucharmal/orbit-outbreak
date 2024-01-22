using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Data;

public class GameStatus : MonoBehaviour
{

    public string status = "";
    public TMP_Text statusText;

    // Start is called before the first frame update
    void Start()
    {
        statusText.text = status;
    }

    public void UpdateStatus(string newStatus){
        Time.timeScale = 0;
        status = newStatus;
        statusText.text = status;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
