using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniAsteroids : MonoBehaviour
{
    public float speed = 5f;

    private Vector3 direction;
    // Start is called before the first frame update
    void Start()
    {
        // Mover el mini-asteroide en la dirección establecida
        transform.Translate(speed * direction);
    }
    
    // Método para establecer la dirección del mini-asteroide
    public void SetDirection(Vector3 dir)
    {
        direction = dir;
    }
    
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Bullet")
        {
            // Si colisiona con una bala, destruir el mini-asteroide
            Destroy(gameObject);
            Destroy(other.gameObject);
        }
    }
}
