using System.Collections.Generic;

[System.Serializable]
public class StoragePageData
{
    public List<SlotData> slots;
    public StoragePageData()
    {
        slots = new List<SlotData>();
    }
}
[System.Serializable]
public class StorageData
{
    public List<StoragePageData> pages;
    public StorageData()
    {
        pages = new List<StoragePageData>();
    }
}
