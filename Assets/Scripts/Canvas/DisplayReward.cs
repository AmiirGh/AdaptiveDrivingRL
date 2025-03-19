using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
public class TextUpdater : MonoBehaviour
{
    public GameObject mainCar; // Assign this in the Inspector
    
    public TMP_Text textElement; // Reference to the Text component
    [SerializeField]
    //public CarHandler2 CarHandler2;
    public int numberToDisplay = 0; // Example number
    public int mainCarPositionCurrent = 0;
    public int mainCarPositionPrev = 0;
    public int RLTotalReward = 0;
    private int operatorTotalReward = 0;
    public int distanceToBlueCar = 0;
    
    private int obstacleHitReward = -2;
    private int blueCarHitReward = -15; // -10
    //private int speedLimitExceedReward = -5;
    private int speedSignLimitExceedReward = -5;
    private int distanceTraveledReward = +1;

    public int obstacleHitCountCurrent = 0;
    public int blueCarHitCountCurrent = 0;
    //private int speedLimitExceedCountCurrent = 0;
    public int speedSignLimitExceedCountCurrent = 0;

    private int obstacleHitCountPrev = 0;
    private int blueCarHitCountPrev = 0;
    private int speedLimitExceedCountPrev = 0;
    private int speedSignLimitExceedCountPrev = 0;

    private Color normalColor = Color.white; // Default text color
    private Color warningColor = new Color(1, 0.5f, 0.5f); // Color when speed limit is exceeded
    private float obstacleHitRedColorDuration = 75f; // 0.75
    private float speedLimitExceedRedColorDuration = 20f; // 1.5
    void Update()
    {

        mainCarPositionCurrent = (int)mainCar.transform.position.z / 10;
        obstacleHitCountCurrent = CarHandler2.hitCount;
        //speedLimitExceedCountCurrent = CarHandler2.speedLimitExceedCount;
        blueCarHitCountCurrent  = BlueCarHandler.hitCount;
        speedSignLimitExceedCountCurrent = SpawnSpeedSign.speedSignLimitExceedCount;



        operatorTotalReward += blueCarHitReward * (blueCarHitCountCurrent - blueCarHitCountPrev);
        operatorTotalReward += obstacleHitReward * (obstacleHitCountCurrent - obstacleHitCountPrev);
        // operatorTotalReward += distanceTraveledReward * (mainCarPositionCurrent - mainCarPositionPrev);
        // operatorTotalReward += speedLimitExceedReward * (speedLimitExceedCountCurrent - speedLimitExceedCountPrev);
        operatorTotalReward += speedSignLimitExceedReward * (speedSignLimitExceedCountCurrent - speedSignLimitExceedCountPrev);

        
        if (speedSignLimitExceedCountCurrent - speedSignLimitExceedCountPrev >= 1)
        {
            StartCoroutine(ChangeTextColorForDuration(speedLimitExceedRedColorDuration));
        }
        if (obstacleHitCountCurrent - obstacleHitCountPrev >= 1)
        {
            StartCoroutine(ChangeTextColorForDuration(obstacleHitRedColorDuration));
        }




        RLTotalReward += blueCarHitReward * (blueCarHitCountCurrent - blueCarHitCountPrev);

        textElement.text = "Total reward: " + operatorTotalReward.ToString();

        obstacleHitCountPrev = obstacleHitCountCurrent;
        blueCarHitCountPrev = blueCarHitCountCurrent;
        // speedLimitExceedCountPrev = speedLimitExceedCountCurrent;
        mainCarPositionPrev = mainCarPositionCurrent;
        speedSignLimitExceedCountPrev = speedSignLimitExceedCountCurrent;
    }
    private IEnumerator ChangeTextColorForDuration(float duration)
    {
        // Change the text color to the new color
        textElement.color = warningColor;

        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Revert the text color to the normal color
        textElement.color = normalColor;
    }
}