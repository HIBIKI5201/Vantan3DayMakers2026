using UnityEngine;

public class NameManager : MonoBehaviour
{
    public static string UserName { get; private set; } = "名無し";
    public void SetName(string inputName)
    {
        UserName = inputName;
    }
}
