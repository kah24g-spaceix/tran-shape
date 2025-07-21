using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LootBag : MonoBehaviour
{
    public GameObject droppedItemPrefab;
    public List<Loot> lootList = new List<Loot>();


    List<Loot> GetDroppedItems()
    {
        List<Loot> droppedItems = new List<Loot>();
        foreach (Loot item in lootList)
        {
            int randomNumber = Random.Range(1, 101); // 1-100
            if (randomNumber <= item.dropChance)
            {
                droppedItems.Add(item);
            }
        }
        return droppedItems;
    }

    public void InstantiateLoot(Vector3 spawnPosition)
    {
        List<Loot> droppedItems = GetDroppedItems();

        if (droppedItems.Count > 0)
        {
            foreach (Loot droppedItem in droppedItems)
            {
                if (droppedItem != null)
                {
                    GameObject lootGameObject = Instantiate(droppedItemPrefab, spawnPosition, Quaternion.identity);
                    lootGameObject.GetComponent<SpriteRenderer>().sprite = droppedItem.lootSprite;
                    lootGameObject.GetComponent<Animator>().runtimeAnimatorController = droppedItem.lootAnimatorController;
                    lootGameObject.tag = droppedItem.lootName;
                }
                else
                {
                    Debug.Log("No loot dropped");
                }
            }
        }
        else
        {
            Debug.Log("All No loot dropped");
        }
    }

}
