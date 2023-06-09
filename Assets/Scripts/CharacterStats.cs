using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CharacterStats : MonoBehaviour
{
    [SerializeField] float maxHealth = 100;
    public  float power = 10;
    [SerializeField]  int killScore = 200;

    public float currentHealth{get; private set ;}
    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
    }
    public void ChangeHealth(float value)
    {
        currentHealth += value;
        Debug.Log("current health " + currentHealth + "/" + maxHealth);

        //if (transform.CompareTag("Enemy"))
            transform.Find("Canvas").GetChild(1).GetComponent<Image>().fillAmount = currentHealth / maxHealth;

        if (currentHealth <= 0) {
            Die();
        }
    }

    void Die()
    {
        if (transform.CompareTag("Player"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else if (transform.CompareTag("Enemy"))
        {
            LevelManager.instance.score += killScore;
            Destroy(gameObject);
            Instantiate(LevelManager.instance.particles[0], transform.position, transform.rotation);
            //Destroy Enemy
        }
    }


}
