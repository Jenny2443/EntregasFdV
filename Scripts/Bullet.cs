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
    
    public GameObject miniAsteroidPrefab;
    
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
            
            
            SplitAsteroids();
        }
    }
    
    //Funcion para dividir la bala en dos mini-asteroides
    private void SplitAsteroids()
    {
        // Calcular el ángulo de separación (en grados) para los mini-asteroides
        float splitAngle = 30f; // Ángulo fijo para dividir en dos
        
        // Calcular la mitad del ángulo de separación
        float halfSplitAngle = splitAngle / 2f;
        
        // Calcular las direcciones de los mini-asteroides aplicando la rotación
        Vector3 dir1 = Quaternion.Euler(0, 0, -halfSplitAngle) * targetVector; // Dirección a la izquierda
        Vector3 dir2 = Quaternion.Euler(0, 0, halfSplitAngle) * targetVector;  // Dirección a la derecha
        
        // Crear dos mini-asteroides a partir de los prefabs
        GameObject miniAsteroid1 = Instantiate(miniAsteroidPrefab, transform.position, Quaternion.identity);
        GameObject miniAsteroid2 = Instantiate(miniAsteroidPrefab, transform.position, Quaternion.identity);

        // Asignar las direcciones a los mini-asteroides
        miniAsteroid1.GetComponent<MiniAsteroids>().SetDirection(dir1);
        miniAsteroid2.GetComponent<MiniAsteroids>().SetDirection(dir2);
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
