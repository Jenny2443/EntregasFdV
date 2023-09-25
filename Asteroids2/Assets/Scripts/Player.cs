using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public float thrustForce = 10f;
    public float rotationSpeed = 120f;
    private Rigidbody _rigid;
    private Vector3 thrustDirection;
    
    //Spawner
    public GameObject gun;
    public GameObject bulletPrefab;
    
    //Puntuacion
    public static int SCORE = 0;
    
    //Limites
    private static float xBorderLimit;
    private static float yBorderLimit;
    
    //Pantalla Pausa
    public GameObject pauseMenu;
    Boolean enPausa = false;
    
    //Pantalla final
    public GameObject gameOverMenu;
    public Text scoreTextGameOver;
    private Boolean enGameOver = false;
    
    //Start is called before the first frame update
    void Start()
    {
        _rigid = GetComponent<Rigidbody>();
        yBorderLimit = (Camera.main.orthographicSize + 1);
        xBorderLimit = (Camera.main.orthographicSize + 1) * Screen.width / Screen.height;
    }

    private void FixedUpdate()
    {
        float thrust = Input.GetAxis("Vertical") * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * Time.deltaTime;
        thrustDirection = transform.right;
        
        _rigid.AddForce(thrustDirection * thrust * thrustForce);
        
        //-rotation para q gire hacia el mismo lado que estamos dando
        transform.Rotate(Vector3.forward, -rotation * rotationSpeed);

    }

    // Update is called once per frame
    void Update()
    {
        
        /*Espacio infinito*/
        var newPos = transform.position;
        //Si sale por el borde derecho
        if (newPos.x > xBorderLimit)
        {
            newPos.x = -xBorderLimit + 1;
            
        }else if (newPos.x < -xBorderLimit)
        {
            //Si sale por el borde izquierdo
            newPos.x = xBorderLimit - 1;
        }else if (newPos.y > yBorderLimit)
        {
            //Si sale por el borde superior
            newPos.y = -yBorderLimit + 1;
        }else if (newPos.y < -yBorderLimit)
        {
            //Si sale por el borde inferior
            newPos.y = yBorderLimit - 1;
        }
        transform.position = newPos;
        
        //Comprobamos si esta dando al espacio
        if (Input.GetKeyDown(KeyCode.Space))
        {
            GameObject bullet = Instantiate(bulletPrefab,gun.transform.position,Quaternion.identity);

            //Para que las balas vayan en la direccion de la nave
            Bullet balaScript = bullet.GetComponent<Bullet>();
            balaScript.targetVector = transform.right;
        }

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (enPausa)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }

        //Si estamos en pausa y le damos a R -> Reiniciamos y volvemos a la escena inicial
        if (Input.GetKeyDown(KeyCode.R) && enPausa)
        {
            Time.timeScale = 1;
            enPausa = false;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        //Si estamos en pausa y le damos a S -> Salimos del juego
        if (Input.GetKeyDown(KeyCode.S) && enPausa)
        {
            Application.Quit();
        }
        
    }
    
    
    //Función para pausar el juego
    private void PauseGame()
    {
        enPausa = true;
        Time.timeScale = 0f; //Pausar el tiempo de juego
        pauseMenu.SetActive(true); //Mostrar menu de pausa
        
    }
    //Función para reanudar el juego
    private void ResumeGame()
    {
        enPausa = false;
        Time.timeScale = 1f; //Reanudar el tiempo de juego
        pauseMenu.SetActive(false); //Ocultar menu de pausa
    }
    
    //Funcion para detectar colisiones
    private void OnCollisionEnter(Collision other)
    {
        //Si colisionamos con un enemigo
        if (other.gameObject.tag == "Enemy" ||
            other.gameObject.tag == "SmallEnemy")
        {
            //Destruimos al enemigo
            Destroy(other.gameObject);
            //Destruimos al jugador
            Destroy(gameObject);
            
            //Mostramos el menu de game over
            scoreTextGameOver.text = "SCORE: " + Player.SCORE;
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;

            //Volvemos a empezar la puntuacion
            SCORE = 0;
        }
    }
}
