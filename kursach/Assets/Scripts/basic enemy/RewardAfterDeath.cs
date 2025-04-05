using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RewardAfterDeath : MonoBehaviour
{
    public int level = 0;
    public GameObject moneyPrefab;
    private void Spawn(bool isFarm, float coef_, int minReward, int maxReward, float radius, int leftBound, int rightBound, Vector2 center)
    {
        System.Random random = new System.Random();
        int countOfObjects = UnityEngine.Random.Range(leftBound, rightBound);
        float coef = isFarm ? coef_ : 1;
        int reward = (int)(UnityEngine.Random.Range(minReward, maxReward) * coef);
        if (level == 1)
        {
            int taken = transform.GetComponent<GreedMelee>().taken;
            if (taken != 0)
            {
                GameObject money = Instantiate(moneyPrefab, transform.position, Quaternion.identity);
                money.GetComponent<Recklessness>().count = taken;
            }
        }
        if (level > 100)
        {
            TeleportToNewLevel teleport = FindAnyObjectByType<TeleportToNewLevel>(FindObjectsInactive.Include);
            teleport.gameObject.SetActive(true);
        }
        int[] values = new int[countOfObjects];
        int remain = reward;
        for (int i = 0; i < countOfObjects - 1; i++)
        {
            values[i] = UnityEngine.Random.Range(0, remain - countOfObjects + i);
            remain -= values[i];
        }
        values[countOfObjects - 1] = remain;
        for (int i = 0; i < countOfObjects; i++)
        {
            float dx = (float)random.NextDouble() - (float)random.NextDouble();
            float dy = (float)random.NextDouble() - (float)random.NextDouble();
            dx *= radius;
            dy *= radius;
            Vector2 newPos = new Vector2(dx, dy);
            newPos += center;
            GameObject money = Instantiate(moneyPrefab, newPos, Quaternion.identity);
            SpriteRenderer spriteRenderer = money.GetComponent<SpriteRenderer>();
            Color color = spriteRenderer.color;
            color.r = Mathf.Clamp01((float)reward / (float)maxReward);
            Recklessness recklessness = money.GetComponent<Recklessness>();
            recklessness.count = values[i];
        }
    }
    public void GetReward(Vector2 center)
    {
        bool isFarm = SceneManager.GetActiveScene().name == "LevelFarm";
        if (level == 1)
        {
            Spawn(isFarm, 2.8f, 3, 5, 1, 1, 1, center);
        }
        else if (level == 2)
        {
            Spawn(isFarm, 2.7f, 10, 15, 1, 2, 5, center);
        }
        else if (level == 3)
        {
            Spawn(isFarm, 2.2f, 40, 45, 1, 5, 10, center);
        }
        else if (level == 4)
        {
            Spawn(isFarm, 1.5f, 125, 135, 1, 5, 10, center);
        }
        else if (level == 5)
        {
            Spawn(isFarm, 1.5f, 390, 405, 1, 5, 10, center);
        }
        else if (level == 101)
        {
            // boss 1
            Vector2 cringe = new Vector2(0, 0);
            Spawn(false, 1, 330, 350, 3, 10, 20, cringe);
        }
        else if (level == 102)
        {
            // boss 2
            Vector2 cringe = new Vector2(0, 0);
            Spawn(false, 1, 3000, 3150, 3, 20, 30, cringe);
        }
        else if (level == 103)
        {
            // boss 3
            Vector2 cringe = new Vector2(0, 0);
            Spawn(false, 1, 40000, 50000, 3, 100, 200, cringe);
        }
    }
}
