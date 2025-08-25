using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{
    public int coinAmount = 1;
    private bool movingToPlayer;
    [SerializeField] float movespeed = 5f;
    private float timeBetweenChecks = 0.2f;
    private float checkCounter;
    [SerializeField] float Pickup_MinDis;
    public static CoinPickup instance;


    private void Awake()
    {
        instance = this;
        checkCounter = timeBetweenChecks;
    }

    private void Update()
    {
        if (movingToPlayer)
        {
            Vector3 targetPos = new Vector3(Player_Controller.instance.transform.position.x, (transform.position.y+0.002f), Player_Controller.instance.transform.position.z);
            transform.position = Vector3.MoveTowards(transform.position, targetPos, movespeed * Time.deltaTime);

        }
        else
        {
            checkCounter -= Time.deltaTime;
            if (checkCounter <= 0)
            {
                checkCounter = timeBetweenChecks;
                if (Vector3.Distance(transform.position, Player_Controller.instance.transform.position) < Pickup_MinDis)
                {
                 
                    StartCoroutine(EnableMagnetAfterDelay());
                }
            }
        }
    }
    private IEnumerator EnableMagnetAfterDelay()
    {
        yield return new WaitForSeconds(0.7f); // delay of 2 seconds
        movingToPlayer = true;
        movespeed += 5f;

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            CoinController.instance.AddCoins(coinAmount);
            Destroy(gameObject,.4f);
/*            Debug.Log("Coins are destroyed");
*/            ExperienceForThisEnemy(1);
            AudioManager.Instance.Play("Gems");

        }
    }
    public void ExperienceForThisEnemy(int exp)
    {
        Player_Controller.instance.GetExperience(exp);

    }
}
