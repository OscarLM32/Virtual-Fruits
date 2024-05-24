using Enemies;
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

    private void UpdateHealth(EnemyType? enemyType)
    {
        Transform child = transform.GetChild(_currentChild);
        child.GetComponent<Image>().sprite = EmptyHeartImg;
        _currentChild--;
    }

    private void OnEnable()
    {
        GameActions.OnPlayerDeath += UpdateHealth;
    }

    private void OnDisable()
    {
        GameActions.OnPlayerDeath -= UpdateHealth;
    }
}
