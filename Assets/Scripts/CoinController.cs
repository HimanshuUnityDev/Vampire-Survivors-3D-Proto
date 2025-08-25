using UnityEngine;

public class CoinController : MonoBehaviour
{
    public static CoinController instance;
    [SerializeField] int totalCoins;
    private void Start()
    {
        instance = this;
    }
    public void AddCoins(int coinsToAdd) 
    {
        totalCoins += coinsToAdd;
    }
   
}
