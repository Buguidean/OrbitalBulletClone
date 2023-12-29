using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_ShieldBar : MonoBehaviour
{
    //public GameObject canvasLifeBar;
    public float actualHealth;
    public float maxHealth;
    public new Transform camera;
    public Vector3 posEnemy;
    public float orientation;

    private Image background;
    private Image lifeBar;

    private float offset_y = 2f;

    // Start is called before the first frame update
    void Start()
    {
        background = gameObject.transform.GetChild(0).GetComponent<Image>();
        lifeBar = gameObject.transform.GetChild(1).GetComponent<Image>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        lifeBar.fillAmount = actualHealth / maxHealth;

        transform.rotation = camera.rotation;

        Vector3 delta = posEnemy - camera.position;
        Vector3.Normalize(delta);

        transform.position = new Vector3(posEnemy.x - delta.x*0.001f, posEnemy.y + offset_y, posEnemy.z - delta.z*0.001f);
    }
}
