using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
   [SerializeField] 
    private TMP_Text playerHP;
    public TMP_Text end;
    public GameObject endImage;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void EndGame(){
        end.gameObject.SetActive(true);
        endImage.SetActive(true);
        PauseGame();
    }

    public void SetPlayerHp(int hp){
        this.playerHP.SetText(""+hp); 
    }

    public void PauseGame ()
    {
        Time.timeScale = 0;
    }
    public void ResumeGame ()
    {
        Time.timeScale = 1;
    }
}
