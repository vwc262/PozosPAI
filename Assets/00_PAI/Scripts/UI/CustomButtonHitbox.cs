using UnityEngine;
using UnityEngine.UI;

public class CustomButtonHitbox : MonoBehaviour
{
    void Start()
    {
        // Only pixels with alpha >= 0.5 will be clickable
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
