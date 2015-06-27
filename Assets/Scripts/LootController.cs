using UnityEngine;
using System.Collections;
using Soomla.Store;

public class LootController : MonoBehaviour {

    public float minX;
    public float maxX;
    public float minY;
    public float maxY;
         
    void Start()
    {
        // Determine the spawn position
        float xPos = Random.Range(0f, 1f);
        float yPos = Random.Range(0f, 1f);

        transform.position = new Vector3(
            minX + (xPos * (maxX - minX)),
            minY + (yPos * (maxY - minY)),
            0
        );
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            StoreInventory.GiveItem(FranticFuguAssets.CURRENCY_SPONGE_ID, 1);
            SpawnController.instance.ResetLootTimer();
            Destroy(gameObject);
        }
    }
}
