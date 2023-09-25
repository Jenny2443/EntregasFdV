using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Update()
    {
        //Verificar si el jugador ha presionado la tecla Return para reiniciar
        if (Input.GetKeyDown(KeyCode.Return))
        {
            //Cargar la escena inicial
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }else if(Input.GetKeyDown(KeyCode.S))
        {
            // Salir del juego
            Application.Quit();
        }
    }
}

