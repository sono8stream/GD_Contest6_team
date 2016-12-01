using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class WarpCube : MonoBehaviour {

    [SerializeField]
    GameObject[] enemiesOrigin;
    List<GameObject> enemies;
    GameObject canvas;
    [SerializeField]
    GameObject unityChan;
    int score;

    [SerializeField]
    AudioClip breakSE;

    // Use this for initialization
    void Start()
    {
        canvas = GameObject.Find("Canvas");
        unityChan = GameObject.Find("SD_unitychan_humanoid");
        enemies = new List<GameObject>();
        /*for (int i = 0; i < 3; i++)
        {*/
            AddEnemy();
        //}
        score = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }

    void AddEnemy()
    {
        enemies.Add(Instantiate(enemiesOrigin[Random.Range(0, enemiesOrigin.Length)]));
        float posX;
        do { posX = Random.Range(-4, 5); }
        while (Mathf.Abs(posX - unityChan.transform.position.x) < 1);
        enemies[enemies.Count-1].transform.position 
            = new Vector3(posX, Random.Range(0, 5) + 0.3f, -1);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemy")
        {
            Transform particle = transform.FindChild("Particle System");
            particle.position
                = new Vector3(particle.position.x, other.transform.position.y, particle.position.z);
            particle.GetComponent<ParticleSystem>().Play();
            enemies.Remove(other.gameObject);
            Destroy(other.gameObject);
            score++;
            canvas.transform.FindChild("Score").GetComponent<Text>().text = score.ToString();
            if ((score - 5) % 10 == 0)
            {
                AddEnemy();
                canvas.transform.FindChild("Manual").gameObject.SetActive(false);
            }
            /*GameObject.Find("SD_unitychan_humanoid").*/GetComponent<AudioSource>().PlayOneShot(breakSE);
            AddEnemy();
        }
        else
        {
            other.transform.position
                = new Vector3(-other.transform.position.x, other.transform.position.y, -1);
        }
    }
}
