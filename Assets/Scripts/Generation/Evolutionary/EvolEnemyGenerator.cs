using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class EvolEnemyGenerator : MonoBehaviour
{
    public int mu;
    public int lambda;
    public int generations;
    public GameObject prefab;
    public Vector2 prefabBounds;
    public EvolEnemy[] enemyList;
    protected int gen;
    private int total;
    private int numAlive;
    private List<EvolEnemyController> population;

    private void Start()
    {
        Init();
    }

    private void Init()
    {
        gen = 0;
        total = mu + lambda;
        population = new List<EvolEnemyController>();
        int i, x;
        bool isRandom = total != enemyList.Length;

        //create initial generation or wave of enemies
        for (i = 0; i < enemyList.Length; i++)
        {
            EvolEnemyController enemy;
            enemy = Instantiate(prefab).GetComponent<EvolEnemyController>();
            enemy.transform.parent = transform;
            EvolEnemy template;
            x = i;
            if (isRandom)
            {
                x = Random.Range(0, enemyList.Length - 1);
            }

            template = enemyList[x];
            enemy.Init(template, prefabBounds);
            population.Add(enemy);
        }

        //initialize the number of enemies alive relative to the size of the population
        numAlive = population.Count;
    }

    public void CreateGeneration()
    {
        //check if the number of generations created is greater than allowed
        if (gen > generations)
        {
            return;
        }
        //sort the individuals in ascending order, and create a list of surviving individuals
        population.Sort();
        List<EvolEnemy> templateList = new List<EvolEnemy>();
        int i, x;
        for (i = mu; i < population.Count; i++)
        {
            EvolEnemy template = population[i].template;
            templateList.Add(template);
            population[i].Revive();
        }

        //create new individuals from the surviving types
        bool isRandom = templateList.Count != mu;
        for (i = 0; i < mu; i++)
        {
            x = i;
            if (isRandom)
            {
                x = Random.Range(0, templateList.Count - 1);
            }

            population[i].template = templateList[x];
            population[i].Revive();
        }
        
        //increase the number of generations, and reset the number of individuals alive
        gen++;
        numAlive = population.Count;
    }

    public void KillEnemy(EvolEnemyController enemy)
    {
        enemy.gameObject.SetActive(false);
        numAlive--;
        if (numAlive > 0)
        {
            return;
        }
        Invoke(nameof(CreateGeneration), 3f);
    }
}