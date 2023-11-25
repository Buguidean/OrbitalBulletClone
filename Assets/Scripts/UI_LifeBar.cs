using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LifeBar : MonoBehaviour
{
    public Image lifeBar;
    public float ActualLife;
    public float MaximumLife; 

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lifeBar.fillAmount = ActualLife / MaximumLife;
    }
}
