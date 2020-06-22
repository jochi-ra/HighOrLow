﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UI;

public class HighOrLow : MonoBehaviour
{
    Deck deck;
    List<Card> hand;
    private int cardsToDraw;
    public Text deckText;
    public Text handText;

    // Start is called before the first frame update
    void Start()
    {
        cardsToDraw = 2;
        deck = new Deck();
    }

    public void JudgeHand()
    {

    }

    public void DrawCard()
    {        
        hand = deck.Draw(cardsToDraw);
        handText.text = "";

        for (int i = 0; i < hand.Count; i++)
        {
            if (hand[i] != null)
                handText.text += hand[i].GetName() + " of " + hand[i].GetSuit();

            if (i != hand.Count - 1)
                handText.text += ", ";
        }

        //Debug.Log("Current card count: " + deck.GetCurrentDeckSize());
        deckText.text = "Deck: " + deck.GetCurrentDeckSize();
    }
}

public class Deck
{
    private List<Card> deck;
    private int deckSize;

    public Deck()
    {
        // standard deck constructor
        deckSize = 52;
        deck = new List<Card>();
        InitializeDeck();
    }

    public Deck(int numCards)
    {
        // constructor for deck variants
        deckSize = numCards;
        deck = new List<Card>();
        InitializeDeck(numCards);
    }

    public int GetOriginalDeckSize()
    {
        return deckSize;
    }

    public int GetCurrentDeckSize()
    {
        return deck.Count;
    }

    public void PrintDeck()
    {
        int b = 0;
        while (b < deck.Count)
        {
            Debug.Log(deck[b].GetValue() + " : " + deck[b].GetSuit());
            b++;
        }

        //Debug.Log(deck.Count);
    }

    private int CountSuit(char suit)
    {
        int suitCount = 0;        

        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].GetSuit().Equals(suit))
            {
                suitCount++;
            }
        }

        return suitCount;
    }

    private int SumDeck()
    {
        int b = 0, sum = 0;
        while (b < deck.Count)
        {
            sum += deck[b].GetValue();
            b++;
        }

        //Debug.Log("SUM = " + deck.SumDeck());

        return sum;
    }

    private void InitializeDeck()
    {
        int maxCardVal = 13;     

        for (int i = 1; i <= maxCardVal; i++)
        {
            Card hearts = new Card(i, 'h');            
            Card clubs = new Card(i, 'c');
            Card diamonds = new Card(i, 'd');
            Card spades = new Card(i, 's');

            deck.Add(hearts);
            deck.Add(clubs);
            deck.Add(diamonds);
            deck.Add(spades);
        }
    }

    private void InitializeDeck(int maxVal)
    {
        // variant deck build        
    }
    
    public void Shuffle()
    {
        int r;
        for (int i = 0; i < deck.Count; i++)
        {
            r = UnityEngine.Random.Range(1, deck.Count);
            Card temp = deck[i];
            deck[i] = deck[r];
            deck[r] = temp;
        }                   
    }

    private Card FindCard(int value, char suit)
    {
        for (int i = 0; i < deck.Count; i++)
        {
            if (deck[i].GetValue() == value && deck[i].GetSuit() == suit)
                return deck[i];            
        }

        return null;
    }

    public List<Card> Draw(int numToDraw)
    {
        // aces = 1, jacks = 11, queens = 12, kings = 13
        // hearts 2x chance, ace of spades 3x chance

        List<Card> hand = new List<Card>();

        for (int i = 0; i < numToDraw; i++)
        {
            // generate a random number between 0 and 1
            int result = UnityEngine.Random.Range(1, 7);

            Card drawnCard;

            if (result == 1)
            {
                // draw a random non-special card
                int randomSuit = UnityEngine.Random.Range(1, 4);

                if (randomSuit == 1)
                {
                    drawnCard = FindCard(UnityEngine.Random.Range(1, 14), 'd');
                }
                else if (randomSuit == 2)
                {
                    drawnCard = FindCard(UnityEngine.Random.Range(1, 14), 'c');
                }
                else
                {
                    drawnCard = FindCard(UnityEngine.Random.Range(2, 14), 's');
                }

            }
            else if (result == 2 || result == 3)
            {
                // draw a random heart
                drawnCard = FindCard(UnityEngine.Random.Range(1, 14), 'h');
            }
            else
            {
                // draw the ace of spades if it wasn't drawn already
                Debug.Log("drew ace of spades");
                drawnCard = FindCard(1, 's');
            }

            if (drawnCard != null)
            {
                hand.Add(drawnCard);
                deck.Remove(drawnCard);
                //Debug.Log(deck.Remove(drawnCard) ? "card removal success" : "card removal failed);
            }            
        }

        return hand;
    }
}

public class Card
{
    private int value;
    private char suit;
    private Color color;
    private string name;
    
    public Card(int cardValue, char cardSuit)
    {
        value = cardValue;
        suit = cardSuit;
        SetName();
        SetColor();
    }

    public string GetName()
    {
        return name;
    }

    private void SetName()
    {
        switch (value)
        {
            case 1:
                name = "Ace";
                break;
            case 11:
                name = "Jack";
                break;
            case 12:
                name = "Queen";
                break;
            case 13:
                name = "King";
                break;
            default:
                name = value.ToString();
                break;
        }
    }

    private void SetColor()
    {
        color = suit.Equals('h') || suit.Equals('d') ? Color.red : Color.black;
    }

    public Color GetColor()
    {
        return color;
    }

    public int GetValue()
    {
        return value;
    }

    public char GetSuit()
    {
        return suit;
    }
}