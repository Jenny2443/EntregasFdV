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

    //Como publica la podemos modificar en unity
    public Vector3 targetVector;
    
    //public GameObject miniAsteroidPrefab;
    
    // Start is called before the first frame update
    void Start()
    {
        //En el primer frame, desps de 3s se autodestruye
        Destroy(gameObject,maxLife);    
    }

    // Update is called once per frame
    void Update()
    {
        //Bala tiene q star alineada con la nave
        transform.Translate(speed * targetVector * Time.deltaTime);
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            //Incrementamos puntuacion
            IncreaseScore();
            
            //Destruimos al enemigo y a la bala
            Destroy(gameObject);
            Destroy(other.gameObject);
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
