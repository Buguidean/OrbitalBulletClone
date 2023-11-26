using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_LifeBar : MonoBehaviour
{
    public float actualHealth = 50f;
    public float maxHealth = 50f;

    //private Image background;
    private Image lifeBar;

    // Start is called before the first frame update
    void Start()
    {
        //background = gameObject.transform.GetChild(0).gameObject.GetComponent<Image>();
        lifeBar = gameObject.GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!lifeBar.Equals(null))
        {
            lifeBar.fillAmount = actualHealth / maxHealth;
        }
    }
}
