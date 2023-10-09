using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSlot : MonoBehaviour
{
    public List<BuffSlotData> slots = new List<BuffSlotData>();
    private int maxSlot = 5;
    public GameObject slotPrefab;

    private void Start()
    {
        GameObject slotPanel = GameObject.Find("Panel");

        for (int i = 0; i < maxSlot; i++)
        {
            GameObject go = Instantiate(slotPrefab, slotPanel.transform, false);
            go.name = "Slot_" + i;
            BuffSlotData slot = new BuffSlotData();
            slot.isEmpty = true;
            slot.slotObj = go;
            slots.Add(slot);
        }
    }
}
