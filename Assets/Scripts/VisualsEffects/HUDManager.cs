using UnityEngine;
using UnityEngine.UI;

public class HUDManager : MonoBehaviour
{
    public Sprite EmptyHeartImg;

    private int _currentChild;
    void Start()
    {
        _currentChild = transform.childCount - 1;
    }

    private void UpdateHealth()
    {
        Transform child = transform.GetChild(_currentChild);
        child.GetComponent<Image>().sprite = EmptyHeartImg;
        _currentChild--;
    }

    private void OnEnable()
    {
        GameActions.PlayerDeath += UpdateHealth;
    }

    private void OnDisable()
    {
        GameActions.PlayerDeath -= UpdateHealth;
    }
}
