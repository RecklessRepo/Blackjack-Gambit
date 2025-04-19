using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardScript : MonoBehaviour
{
    // Value of card, e.g., 2 of clubs = 2, etc.
    public int value = 0;

    public int GetValueOfCard()
    {
        return value;
    }

    public void SetValue(int newValue)
    {
        value = newValue;
    }

    public string GetSpriteName()
    {
        return GetComponent<SpriteRenderer>().sprite.name;
    }

    public void SetSprite(Sprite newSprite)
    {
        gameObject.GetComponent<SpriteRenderer>().sprite = newSprite;
    }

    public void ResetCard()
    {
        // Assumes there is a DeckController object in the scene 
        // with a DeckScript component that has a GetCardBack() method
        Sprite back = GameObject.Find("Deck").GetComponent<DeckScript>().GetCardBack();

        // Set this card to use the back sprite
        gameObject.GetComponent<SpriteRenderer>().sprite = back;

        // Reset the card’s value
        value = 0;
    }
}
