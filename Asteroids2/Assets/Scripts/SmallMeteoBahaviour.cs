using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmallMeteoBehaviour : MonoBehaviour
{
    public float speed = 3f;
    public Vector3 targetVector;

    //Variables necesarias para acceder a la lista de balas desactivadas
    private GameObject camara;
    private EnemySpawner script;

    // Start is called before the first frame update
    void Start()
    {
        camara = GameObject.Find("Main Camera");
        script = camara.GetComponent<EnemySpawner>();
    }

    // Update is called once per frame
    void Update()
    {
        //El meteorito se desplazara con ladireccion que se le indique en el editor
        transform.Translate(targetVector * speed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si el meteorito alcanza el limite de la pantalla sin tocar nada, entonces lo desactivamos
        if (other.gameObject.tag == "Limit")
        {
            gameObject.SetActive(false);
            script.SmallmeteoDesactivados.Enqueue(gameObject);
        }
    }
}
