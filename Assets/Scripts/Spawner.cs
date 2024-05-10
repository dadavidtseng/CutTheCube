using System.Collections;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject objectToGenerate;
    public float      flyingSpeed = 10f;
    public float      generationInterval;
    public float      maxRandomOffset = 0.1f;
    public float      objectLifetime  = 2f;

    private float lastGenerationTime;

    private void Start()
    {
        lastGenerationTime = Time.time;
        generationInterval = Random.Range(1f, 3f);
    }

    private void Update()
    {
        if (!(Time.time - lastGenerationTime >= generationInterval))
            return;

        GenerateAndFlyObject();

        lastGenerationTime = Time.time;
    }

    private void GenerateAndFlyObject()
    {
        var newObject = Instantiate(objectToGenerate, transform.position, Quaternion.identity);
        var rb        = newObject.GetComponent<Rigidbody>();

        if (rb != null)
        {
            var playerPosition       = Camera.main.transform.position; // 玩家的位置
            var originalFlyDirection = (playerPosition - transform.position).normalized;

            // 在每個軸上應用隨機偏移
            var offsetX = Random.Range(-maxRandomOffset, maxRandomOffset);
            var offsetY = Random.Range(-maxRandomOffset, maxRandomOffset);
            var offsetZ = Random.Range(-maxRandomOffset, maxRandomOffset);

            var randomOffset    = new Vector3(offsetX, offsetY, offsetZ);
            var newFlyDirection = originalFlyDirection + randomOffset;

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

    private static IEnumerator DestroyObjectAfterDelay(Object obj, float delay)
    {
        yield return new WaitForSeconds(delay);

        Destroy(obj);
    }
}