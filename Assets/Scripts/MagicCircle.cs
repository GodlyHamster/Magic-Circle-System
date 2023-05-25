using UnityEngine;

public class MagicCircle : MonoBehaviour
{
    [SerializeField] private Ring[] rings;

    private void Start()
    {
        for (int i = 0; i < rings.Length; i++)
        {
            rings[i].CreateCirclesWithAmount(gameObject);
        }
    }
}