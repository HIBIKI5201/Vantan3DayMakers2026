using UnityEngine;

public class NameManager : MonoBehaviour
{
    private DataManager _dataManager;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _dataManager = FindAnyObjectByType<DataManager>();
        SetName("");
    }
     public void SetName(string name)
    {
        _dataManager.SetName(name);
    }
}
