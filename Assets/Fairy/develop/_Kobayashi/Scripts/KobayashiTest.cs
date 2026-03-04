using UnityEngine;

public class KobayashiTest : MonoBehaviour
{
    [SerializeField] private GameObject _obj;
    // Update is called once per frame
    void Update()
    {
        if (Input.anyKey)
        {
            _obj.SetActive(true);
        }
    }
}
