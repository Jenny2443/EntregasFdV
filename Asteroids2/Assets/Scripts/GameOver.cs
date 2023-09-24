using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        // Verificar si el jugador ha presionado la tecla "L" para reiniciar
        if (Input.GetKeyDown(KeyCode.Return))
        {
            // Cargar la escena inicial o la escena que desees reiniciar
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            Time.timeScale = 1;
        }else if(Input.GetKeyDown(KeyCode.S))
        {
            // Salir del juego
            Application.Quit();
        }
    }
}

