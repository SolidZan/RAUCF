using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{

    private RubyController rubyController;
    public GameObject youWin;

    // Start is called before the first frame update
    void Start()
    {
        //youWin.SetActive(true);
        //print("gamestart");
    }
    // Update is called once per frame
    void Update()
    {

    }
    public void Win(bool winVar)
    {
        if (winVar == true)
        {
           youWin.SetActive(true);
        }
        
    }
}
