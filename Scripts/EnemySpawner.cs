using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject asteroidPrefab;

    public float spawnRatePerMinute = 30;

    public float spawnRateIncrement = 1f;

    public float xLimit,yLimit;
    public float maxLifeTime = 4f;
    
    private float spawnNext = 0;
    
    // Update is called once per frame
    void Update()
    {
        //Si ha pasao el tiempo de spawn -> instanciamos
        if (Time.time > spawnNext)
        {
            spawnNext = Time.time + 60 / spawnRatePerMinute;
            spawnRatePerMinute += spawnRateIncrement;
            /*SpawnPosition -> podemos ponerle un objeto en especifico o darle un lugar (miramos en unity a ver en q coords sta 
                el meteorito fuera de escena
                Vector2 spawnPosition = new Vector2(0, 8); Esto nos spawnearia el meteorito siempre en el 0,8
                Para hacerlo random*/
            
            float rand = Random.Range(-xLimit, xLimit);
            Vector2 spawnPosition = new Vector2(rand, yLimit);
            //Creamos el meteorito         
            GameObject meteor = Instantiate(asteroidPrefab, spawnPosition, Quaternion.identity);
            
            //Destruimos el meteorito desps de un tiempo para no llenar la escena
            Destroy(meteor, maxLifeTime);
        }
    }
}
