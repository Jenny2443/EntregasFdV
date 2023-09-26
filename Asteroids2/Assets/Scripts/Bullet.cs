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
    
    public GameObject miniMeteoPrefab;
    private float angle; //Para almacenar la direccion en la que desviaran los minimeteorritos
    private float bulletAngle;

    //Como publica la podemos modificar en unity
    public Vector3 targetVector;

    //Variables necesarias para acceder a la lista de balas y meteoritos desactivados
    private GameObject ship;
    private Player scriptJugador;
    private GameObject camara;
    private EnemySpawner scriptSpawner;

    private float PI = Mathf.PI; //Los radianes no se trabajan solos 

    public AudioSource explosion;

    void Start()
    {
        ship = GameObject.Find("Ship");
        scriptJugador = ship.GetComponent<Player>();
        camara = GameObject.Find("Main Camera");
        scriptSpawner = camara.GetComponent<EnemySpawner>();
    }

    void Update()
    {
        //Bala tiene q star alineada con la nave
        transform.Translate(speed * targetVector * Time.deltaTime);
    }

    //Función para detectar colisiones
    private void OnCollisionEnter(Collision other)
    {
        GameObject minimeteorito1;
        GameObject minimeteorito2;
        if (other.gameObject.tag == "Enemy")//Se ejecuta si el meteorito es grande
        {
            //Incrementamos puntuacion
            IncreaseScore();

            //Desactivamos al enemigo y desactivamos la bala y los encolamos para poder reutilizarlos
            other.gameObject.SetActive(false);
            scriptSpawner.meteoDesactivados.Enqueue(other.gameObject);
            gameObject.SetActive(false);
            scriptJugador.balasDesactivadas.Enqueue(gameObject);

            angle = Random.Range(0, PI); //Cogemos un angulo entre 0 y 180 grados para formar la bisectriz
            bulletAngle = getBulletAngle();
            if (scriptSpawner.SmallmeteoDesactivados.Count >= 2)//Si hay 2 minimeteoritos en la pool los sacamos
            {
                minimeteorito1 = scriptSpawner.SmallmeteoDesactivados.Dequeue();
                minimeteorito2 = scriptSpawner.SmallmeteoDesactivados.Dequeue();
                minimeteorito1.transform.position = transform.position;
                minimeteorito2.transform.position = transform.position;
                minimeteorito1.transform.rotation = new Quaternion(0, 0, Mathf.Sin((bulletAngle + angle) / 2), Mathf.Cos((bulletAngle + angle) / 2));
                minimeteorito2.transform.rotation = new Quaternion(0, 0, Mathf.Sin((bulletAngle - angle) / 2), Mathf.Cos((bulletAngle - angle) / 2));
                minimeteorito1.SetActive(true);
                minimeteorito2.SetActive(true);
            }
            else if (scriptSpawner.SmallmeteoDesactivados.Count == 1)//Si hay 1 lo sacamos e instaciamos otro 
            {
                minimeteorito1 = scriptSpawner.SmallmeteoDesactivados.Dequeue();
                minimeteorito1.transform.position = transform.position;
                minimeteorito1.transform.rotation = new Quaternion(0, 0, Mathf.Sin((bulletAngle + angle) / 2), Mathf.Cos((bulletAngle + angle) / 2));
                minimeteorito1.SetActive(true);
                Instantiate(miniMeteoPrefab, transform.position, new Quaternion(0, 0, Mathf.Sin((bulletAngle - angle) / 2), Mathf.Cos((bulletAngle - angle) / 2)));
            }
            else //Y si no hay ninguno nos toca instanciar 2
            {
                Instantiate(miniMeteoPrefab, transform.position, new Quaternion(0, 0, Mathf.Sin((bulletAngle + angle) / 2), Mathf.Cos((bulletAngle + angle) / 2)));
                Instantiate(miniMeteoPrefab, transform.position, new Quaternion(0, 0, Mathf.Sin((bulletAngle - angle) / 2), Mathf.Cos((bulletAngle - angle) / 2)));
            }
        }
        else if (other.gameObject.tag == "SmallEnemy") //Se ejecuta si el meteorito es pequeño
        {
            //Incrementamos puntuacion
            IncreaseScore();

            //Desactivamos al enemigo y desactivamos la bala y los encolamos para poder reutilizarlos
            other.gameObject.SetActive(false);
            scriptSpawner.SmallmeteoDesactivados.Enqueue(other.gameObject);
            gameObject.SetActive(false);
            scriptJugador.balasDesactivadas.Enqueue(gameObject);
        }

        explosion.Play();
    }

    private void OnTriggerEnter(Collider other)
    {
        //Si la bala alcanza el limite de la pantalla sin tocar nada, entonces la desactivamos
        if (other.gameObject.tag == "Limit")
        {
            gameObject.SetActive(false);
            scriptJugador.balasDesactivadas.Enqueue(gameObject);
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

    private float getBulletAngle()
    {
        float result;
        float x = targetVector.x;
        float y = targetVector.y;
        result = Mathf.Atan(Mathf.Abs(y) / Mathf.Abs(x));
        if (x < 0 && y < 0)
        {
            Debug.Log("2er sector");
            result += PI / 2;
        }
        else if (x >= 0 && y < 0)
        {
            Debug.Log("3er sector");
            result -= PI;
        }
        else if (x >= 0 && y >= 0)
        {
            Debug.Log("4er sector");
            result += 3 * PI / 2;
        }

        return result;
    }
}
