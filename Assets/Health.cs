using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    private Transform spawnPoint;
    private Transform spawnPoint2;
    [Header("Health")]
    [SerializeField] private float health;
    [SerializeField] private float maxhealth;
    [SerializeField] private bool terrenoCounter = true;

    // Start is called before the first frame update
    void Start()
    {
        health = maxhealth;
        spawnPoint = GameObject.Find("Spawn").transform;
        spawnPoint2 = GameObject.Find("Spawn2").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (terrenoCounter == true && Input.GetButtonDown("Fire1"))
        {
            // Teleport to Terrain 2
            terrenoCounter = false;

        }
        else if (terrenoCounter == false && Input.GetButtonDown("Fire1"))
        {
            // Teleport to Terrain 1
            terrenoCounter = true;

        }
        if (health > maxhealth)
        {
            health = maxhealth;
        }
        if (health < 0.1 && terrenoCounter == true)
        {
            // Actualiza la posición del personaje al punto de spawn
            transform.position = spawnPoint.position;


            // Restablece la salud a su valor máximo
            health = maxhealth;
        }
        else if (health < 0.1 && terrenoCounter == false)
        {
            // Actualiza la posición del personaje al punto de spawn
            transform.position = spawnPoint2.position;


            // Restablece la salud a su valor máximo
            health = maxhealth;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Damage"))
        {
            health = health - 1;
        }
    }
}
