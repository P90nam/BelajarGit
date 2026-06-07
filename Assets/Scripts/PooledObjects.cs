using System.Collections.Generic;
using UnityEngine;

public class PooledObjects : MonoBehaviour
{
    public static PooledObjects Instance; 

    [Header("Pengaturan Pool")]
    public GameObject objectToPool;
    // Menentukan jumlah peluru cadangan yang otomatis dibuat di awal game
    public int amountToPool = 10; 
    
    private List<GameObject> pooledObjects = new List<GameObject>();

    void Awake() 
    {
        if (Instance == null) {
            Instance = this;
        }
    }

    void Start()
    {
        // Membuat 10 peluru pasif di awal game agar siap digunakan kapan saja
        for (int i = 0; i < amountToPool; i++)
        {
            GameObject obj = Instantiate(objectToPool);
            obj.SetActive(false); 
            pooledObjects.Add(obj); 
        }
    }

    public GameObject GetPooledObject()
{
    // Cari peluru yang sedang abu-abu (tidak aktif)
    for (int i = 0; i < pooledObjects.Count; i++)
    {
        if (pooledObjects[i] != null && !pooledObjects[i].activeInHierarchy)
        {
            return pooledObjects[i]; 
        }
    }

    // DIKUNCI MUTLAK: Jangan biarkan Instantiate peluru baru lagi!
    // Jika 10 peluru awal sedang terbang semua, player tidak bisa memproduksi klon baru
    return null; 
}
}