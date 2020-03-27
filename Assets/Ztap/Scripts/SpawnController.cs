using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnController : MonoBehaviour
{

    [Header("SimpleContiniousSpawn")]
    public EnemyTypeSpawner[] enemyTypeSpawner;

    [System.Serializable]
    public class EnemyTypeSpawner
    {
        public bool isActive;
        public GameObject prefab;
        public float secsBetween;
        public int amount;
        public List<GameObject> onScene;

        public enum SpawnerType
        {
            SimpleContiniuos,
            OneTimeBurst,
        }
    }

    private void OnEnable()
    {
        StopAllCoroutines();
        StartSpawners();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }

    // Start is called before the first frame update
    void Start()
    {
        //StartSpawners();
    }

    // Update is called once per frame
    void Update()
    {

    }

    void StartSpawners()
    {
        for (int i = 0; i < enemyTypeSpawner.Length; i++)
        {
            if (enemyTypeSpawner[i].isActive)
            {
                StartCoroutine(SimpleContiniousSpawn(enemyTypeSpawner[i]));
            }
        }
    }

    IEnumerator SimpleContiniousSpawn(EnemyTypeSpawner spawner)
    {
        spawner.onScene = new List<GameObject>();
        while (true)
        {
            yield return new WaitForSeconds(spawner.secsBetween);
            for (int i = 0; i < spawner.amount; i++)
            {
                GameObject temp = TakeFromPool(spawner);
                temp.transform.position = PosOutside();
                temp.SetActive(true);
            }
        }
    }

    private Vector3 PosOutside()
    {
        bool a = Random.value > 0.5f;
        float x;
        float y;
        if (a)
        {
            x = Random.value > 0.5f ? Random.Range(1.2f, 1.5f) : Random.Range(-0.2f, -0.4f);
            y = Random.Range(0, 1.0f);
        }
        else
        {
            x = Random.Range(0, 1.0f);
            y = Random.value > 0.5f ? Random.Range(1.2f, 1.5f) : Random.Range(-0.2f, -0.4f);
        }

        Vector3 v3Pos = Camera.main.ViewportToWorldPoint(new Vector3(x, y, -2 * Camera.main.transform.position.z));
        v3Pos.y = 0;
        return v3Pos;
    }

    private GameObject TakeFromPool(EnemyTypeSpawner spawner)
    {
        for (int i = 0; i < spawner.onScene.Count; i++)
        {
            if (!spawner.onScene[i].activeInHierarchy)
            {
                return spawner.onScene[i];
            }
        }
        GameObject newOne = Instantiate(spawner.prefab, gameObject.transform);
        spawner.onScene.Add(newOne);
        return newOne;
    }
}