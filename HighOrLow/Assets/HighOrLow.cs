﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HighOrLow : MonoBehaviour
{
    private Deck deck;
    private List<Card> hand;

    public GameObject cardBack1;
    public GameObject cardBack2;
    public GameObject cardBack3;
    public int cardsToDraw;
    public Text handText1;
    public Text handText2;
    public Text winnerText;
    public Button drawBtn;

    void Start()
    {
        deck = new Deck();
        hand = new List<Card>();
        deck.Shuffle();
    }

    private void DownsizeDeck()
    {
        if (deck.GetCurrentCardCount() < deck.GetDeckSize() - (deck.GetDeckSize() / 3) && cardBack1.activeSelf)
        {
            cardBack1.SetActive(false);
        }
        
        if (deck.GetCurrentCardCount() < (deck.GetDeckSize() / 3) && cardBack2.activeSelf)
        {
            cardBack2.SetActive(false);
        }

        if (deck.GetCurrentCardCount() == 0 && cardBack3.activeSelf)
        {
            cardBack3.SetActive(false);
        }
    }

    private void DiscardHand()
    {
        for (int i = 0; i < hand.Count; i++)
        {
            hand[i].DestroySprite();
        }        
    }

    private void DisplayWinner()
    {
        Card winner = DetermineHigherCard();
        winnerText.text = winner.GetFullName() + "\nwins!";        
    }

    private Card DetermineHigherCard()
    {
        // assumes two cards to compare
        if (hand[0].GetValue() > hand[1].GetValue())
            return hand[0];
        else if (hand[0].GetValue() < hand[1].GetValue())
            return hand[1];
        else
        {
            if (hand[0].GetSuitValue() > hand[1].GetSuitValue())
                return hand[0];
            else
                return hand[1];
        }
    }

    public void DrawCards()
    {
        // discard previous hand
        DiscardHand();

        handText1.text = handText2.text = winnerText.text = "";

        if (deck.GetCurrentCardCount() > 0)
        {
            // get new hand
            //hand = deck.Draw(cardsToDraw);
            Debug.Log("Card count: " + deck.GetCurrentCardCount() + " | Sum: " + deck.SumCurrentDeck());
            hand = new List<Card>();
            hand.Add(deck.TopDeck());
            hand.Add(deck.TopDeck());
            Debug.Log("Card count: " + deck.GetCurrentCardCount() + " | Sum: " + deck.SumCurrentDeck());

            // animate card draw
            GameObject card1 = Instantiate(cardBack3);
            GameObject card2 = Instantiate(cardBack3);
            StartCoroutine(TranslateCard(card1, card1.transform.position, new Vector3(-3.0f, -1.0f)));
            StartCoroutine(TranslateCard(card2, card2.transform.position, new Vector3(3.0f, -1.0f)));

            // reveal cards and announce winner
            StartCoroutine(RevealCardDraw(1.0f, card1, card2));
            DownsizeDeck();
        }
        else
        {
            // notify refresh
            winnerText.text = "Shuffling deck . . .";

            // refresh deck
            cardBack1.SetActive(true);
            cardBack2.SetActive(true);
            cardBack3.SetActive(true);
            Start();
        }        
    }

    private IEnumerator TranslateCard(GameObject card, Vector3 startPos, Vector3 endPos)
    {
        drawBtn.interactable = false;

        float time = 0f;
        float moveSpeed = 2.0f;

        while (time <= 1.0f)
        {
            if (card != null)
            {
                time += moveSpeed * Time.deltaTime;
                card.transform.position = Vector3.Lerp(startPos, endPos, time);
                yield return new WaitForSeconds(Time.deltaTime / moveSpeed);
            }
        }

        card.transform.position = endPos;
    }

    private IEnumerator RevealCardDraw(float time, GameObject first, GameObject second)
    {
        yield return new WaitForSeconds(time);

        Destroy(first);
        Destroy(second);

        hand[0].CreateSprite();
        hand[0].SetCardPosition(new Vector3(-3.0f, -1.0f));
        hand[1].CreateSprite();
        hand[1].SetCardPosition(new Vector3(3.0f, -1.0f));

        if (hand[0] != null)
            handText1.text = hand[0].GetFullName();

        if (hand[1] != null)
            handText2.text = hand[1].GetFullName();

        yield return new WaitForSeconds(time / 2.0f);

        DisplayWinner();
        drawBtn.interactable = true;
    }
}