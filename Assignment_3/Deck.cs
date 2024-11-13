using Assignment_3;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Assignment_3
{
    public class Deck
    {
        private readonly List<Card> cards;
        private readonly ImageList imageList;

        public Deck(ImageList imageList)
        {
            this.imageList = imageList;
            this.cards = new List<Card>();
            Shuffle();
        }

        public int Count => cards.Count;

        public Card GetCard(int index)
        {
            if (index >= 0 && index < cards.Count)
            {
                return cards[index];
            }

            return Card.NoCard;
        }

        public void Shuffle()
        {
            cards.Clear();

            for (int i = 0; i < imageList.Images.Count; i++)
            {
                cards.Add(new Card(i, imageList.Images[i]));
            }

            Random rng = new Random();
            int n = cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                (cards[k], cards[n]) = (cards[n], cards[k]);

            }

        }
        public Card DealCard()
        {
            if (cards.Count > 0)
            {
                Card card = cards[0];
                cards.RemoveAt(0);
                return card;
            }
            return Card.NoCard;  
        }

        public bool SaveHand(string filename, Card[] hand)
        {
            try
            {
              using (StreamWriter writer = new StreamWriter(filename))
                {
                  foreach (Card card in hand)
                    {
                        writer.WriteLine(card?.Id ?? -1);
                    }
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving hand: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }
        public bool LoadHand(string filename, Card[] hand)
            {
              try
                {
                   using (StreamReader reader = new StreamReader(filename))
                    {
                        for (int i = 0; i < hand.Length; i++)
                        {
                            string line = reader.ReadLine();
                            if (line != null)
                            {
                                int cardId = int.Parse(line);  

                                if (cardId >= 0)
                                { 
                                    hand[i] = new Card(cardId, imageList.Images[cardId]);
                                }
                                else
                                {
                                    hand[i] = null;  
                                }
                            }
                            else
                            {
                                hand[i] = null;  
                            }
                        }
                    }
                    return true;  
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Error loading hand: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;  
                }
         }
       
    }
}



