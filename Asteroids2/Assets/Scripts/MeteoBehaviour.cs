using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeteoBehaviour : MonoBehaviour
{
    public float speed = 3f;
    public Vector3 targetVector;

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject, 20f);

    }

    // Update is called once per frame
    void Update()
    {   
        //El meteorito se desplazara con ladireccion que se le indique en el editor
        transform.Translate(targetVector * speed * Time.deltaTime);
    }
}
