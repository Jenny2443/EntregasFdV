using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;

    public float spawnRatePerMinute = 30;

    public float spawnRateIncrement = 1f;

    public float xBorderLimit1, yBorderLimit1; //Limites para asteroides que spawnean arriba
    public float xBorderLimit2, yBorderLimit2; //Limites para asteroides que spawnean a la derecha
    public float xBorderLimit3, yBorderLimit3; //Limites para asteroides que spawnean abajo
    public float xBorderLimit4, yBorderLimit4; //Limites para asteroides que spawnean a la izquierda
    public float maxLifeTime = 4f;
    
    private float spawnNext = 0;
    private int _spawnSelect; // Variable para seleccionar aleatoriamente el lado donde spawnea el meteorito
    private float angle; //Variable para guardar el angulo aleatorio del meteorito.
    private float PI = Mathf.PI; //Los radianes no se trabajan solos :)

    //Listas de Meteoritos desactivadas para usar en el pooling
    public Queue<GameObject> meteoDesactivados;
    public Queue<GameObject> SmallmeteoDesactivados;

    private void Start()
    {
        meteoDesactivados = new Queue<GameObject>();
        SmallmeteoDesactivados = new Queue<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
        //Si ha pasao el tiempo de spawn -> instanciamos
        if (Time.time > spawnNext)
        {
            spawnNext = Time.time + 60 / spawnRatePerMinute;
            spawnRatePerMinute += spawnRateIncrement;

            /*Seleccionamos aleatoriamente uno de los 4 lados de la pantalla, siendo la correspondencia exacta:
              1 -> Arriba
              2 -> Derecha
              3 -> Abajo
              4 -> Izquierda*/
            _spawnSelect = Random.Range(1, 5);
            //Creamos el meteorito         
            GameObject meteor = spawn(_spawnSelect);
            
        }
    }

    private GameObject spawn(int i)
    {
        float rand; //Variable para almacenar la psicion aleatoria de nuestro meteorito, dentro de se lado correspondiente
        Vector2 spawnPosition = new Vector2(0, 0); //se inicializa spawnPosition por defecto a (0,0), luego si algo spawnea ahí, mala cosa

        if (i == 1)
        {
            rand = Random.Range(-xBorderLimit1, xBorderLimit1);
            spawnPosition.Set(rand, yBorderLimit1);
            angle = Random.Range(-PI / 4, PI / 4);
        }
        else if (i == 2)
        {
            rand = Random.Range(-yBorderLimit2, yBorderLimit2);
            spawnPosition.Set(xBorderLimit2, rand);
            angle = Random.Range(-3 * PI / 4, -PI / 4);
        }
        else if (i == 3)
        {
            rand = Random.Range(-xBorderLimit3, xBorderLimit3);
            spawnPosition.Set(rand, yBorderLimit3);
            angle = Random.Range(3 * PI / 4, 5 * PI / 4);
        }
        else if (i == 4)
        {
            rand = Random.Range(-yBorderLimit4, yBorderLimit4);
            spawnPosition.Set(xBorderLimit4, rand);
            angle = Random.Range(PI / 4, 3 * PI / 4);
        }

        return Instantiate(asteroidPrefab, spawnPosition, new Quaternion(0, 0, Mathf.Sin(angle / 2), Mathf.Cos(angle / 2)));
    }
}
