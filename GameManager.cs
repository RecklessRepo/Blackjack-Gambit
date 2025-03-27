using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    // Buttons
    public Button DealB;
    public Button HitB;
    public Button StandB;
    public Button BetB;
    void Start()
    {
        // On click listeners
        DealB.onClick.AddListener(() => DealClicked());
        HitB.onClick.AddListener(() => HitClicked());
        StandB.onClick.AddListener(() => StandClicked());
    }

    private void DealClicked()
    {
        throw new NotImplementedException();
    }
    private void HitClicked()
    {
        throw new NotImplementedException();
    }
    private void StandClicked()
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
