using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Bullet : MonoBehaviour
{
    public float speed = 10f;

    public float maxLife = 3f;

    public GameObject miniMeteoPrefab;
    private float angle; //Para almacenar la direccion en la que desviaran los minimeteorritos

    //Como publica la podemos modificar en unity
    public Vector3 targetVector;
    
    //public GameObject miniAsteroidPrefab;
    
    void Start()
    {
        //En el primer frame, desps de 3s se autodestruye
        Destroy(gameObject,maxLife);    
    }

    void Update()
    {
        //Bala tiene q star alineada con la nave
        transform.Translate(speed * targetVector * Time.deltaTime);
    }

    //Función para detectar colisiones
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy" ||
            other.gameObject.tag == "SmallEnemy")
        {
            //Incrementamos puntuacion
            IncreaseScore();
            
            //Destruimos al enemigo y a la bala
            Destroy(gameObject);
            Destroy(other.gameObject);
            
            //Si hemos destruido un meteorito grande entonces instanciamos 2 meteoritos pequeños.
            if (other.gameObject.tag == "Enemy")
            {
                angle = Random.Range(0, 180);
                Instantiate(miniMeteoPrefab, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z + angle + 90));
                Instantiate(miniMeteoPrefab, transform.position, Quaternion.Euler(0, 0, transform.eulerAngles.z - angle + 90));
            }
        }
    }
  
//Funcion para aumentar los puntos
    private void IncreaseScore()
    {
        Player.SCORE++;
        UpdateScoreText();
    }
    
    //Funcion para actualizar el canvas
    private void UpdateScoreText()
    {
        GameObject go = GameObject.FindGameObjectWithTag("UI");
        go.GetComponent<Text>().text = "Score: " + Player.SCORE;
    }
}
