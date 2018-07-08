using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lighting : MonoBehaviour
{
    public Light lit;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        lit.color = Color.Lerp(lit.color, Color.black, 0.01f * Time.deltaTime);
    }
}
