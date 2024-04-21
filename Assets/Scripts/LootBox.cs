using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Loot
{
    public GameObject item;
    public int weight;
}

public class LootBox : MonoBehaviour
{
    public Loot[] loot;
    // Start is called before the first frame update
    void Start()
    {
        
    }


    public void LootDrop()
    {
        //        Transform t = set.lootLocations[Random.Range(0, set.lootLocations.Length)];
        //        lastEnergyCollectionPosition = t;
        //        lot.vehicle.GetComponentInChildren<Vehicle>().LootAvailable(true);

        //      Loot[] drop;

        int total = 0;
        int[] lootRange = new int[loot.Length];

        for (int i = 0; i < loot.Length; i++)
        {
            total += loot[i].weight;
            lootRange[i] = total;
        }

        int roll = Random.Range(0, total);
        int selectedLoot = -1;

        for (int i = 1; i <= lootRange.Length; i++)
        {
            if (roll > lootRange[i - 1] && roll < lootRange[i])
            {
                selectedLoot = i;
            }
        }
        if (selectedLoot == -1)
        {
            selectedLoot = 0;
        }
        Instantiate(loot[selectedLoot].item, transform.parent);
    }



}
