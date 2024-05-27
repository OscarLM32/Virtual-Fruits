using UnityEngine;
using UnityEngine.UI;

namespace Level
{
    public class HUDManager : MonoBehaviour
    {
        [SerializeField] private Image _healthBar;
        private float _currentFill = 1;

        public void UpdateHP(float factor)
        {
            _currentFill += factor;
            _healthBar.fillAmount = _currentFill;
        }
    }
}