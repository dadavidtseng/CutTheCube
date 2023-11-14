using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToGenerate; // 要生成的物件
    public float flyingSpeed = 10f; // 飛行速度
    public float generationInterval; // 物件生成間隔
    public float maxRandomOffset = 0.1f; // 最大隨機偏移量
    public float objectLifetime = 2f; // 物件存活時間

    private float lastGenerationTime;

    void Start()
    {
        lastGenerationTime = Time.time;
        generationInterval = Random.Range(1f, 3f);
    }

    void Update()
    {
        if (Time.time - lastGenerationTime >= generationInterval)
        {
            GenerateAndFlyObject();
            lastGenerationTime = Time.time;
        }
    }

    void GenerateAndFlyObject()
    {
        GameObject newObject = Instantiate(objectToGenerate, transform.position, Quaternion.identity);
        Rigidbody rb = newObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            Vector3 playerPosition = Camera.main.transform.position; // 玩家的位置
            Vector3 originalFlyDirection = (playerPosition - transform.position).normalized;

            // 在每個軸上應用隨機偏移
            float offsetX = Random.Range(-maxRandomOffset, maxRandomOffset);
            float offsetY = Random.Range(-maxRandomOffset, maxRandomOffset);
            float offsetZ = Random.Range(-maxRandomOffset, maxRandomOffset);

            Vector3 randomOffset = new Vector3(offsetX, offsetY, offsetZ);
            Vector3 newFlyDirection = originalFlyDirection + randomOffset;

            // 正規化新方向以保持為單位向量
            newFlyDirection.Normalize();

            rb.velocity = newFlyDirection * flyingSpeed;

            // 啟動協程以在指定時間後刪除物件
            StartCoroutine(DestroyObjectAfterDelay(newObject, objectLifetime));
        }
        else
        {
            Debug.LogWarning("生成的物件沒有 Rigidbody 組件！");
        }
    }

    IEnumerator DestroyObjectAfterDelay(GameObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        Destroy(obj);
    }
}
