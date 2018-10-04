﻿using System;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace MemoryProjectFull
{
    class GamePanel : Grid
    {

        bool firstCardClicked;

        OnClickDoneArgs doneArgs;

        Card[,] cards;
        public GamePanel()
        {
            Run(4, 4);
        }

        /// <summary>
        /// GamePanel Constructor FOR DEBUGGING PURPOSE
        /// </summary>
        /// <param name="widht"></param>
        /// <param name="height"></param>
        public GamePanel(int widht, int height)
        {
            Run(widht, height);
        }

        /// <summary>
        /// Constructor for the gamepanel that that creates the card and grid with set parameters
        /// </summary>
        /// <param name="widht">Amount of cards in the x axis</param>
        /// <param name="height">Amount of cards in the y axis</param>
        /// <param name="carSizeX">Size of the cards in the x axis</param>
        /// <param name="carSizeY">Size of the cards in the y axis</param>
        public GamePanel(int widht, int height, int carSizeX, int carSizeY, string theme)
        {
            Card.callback = HandleCallback;
            Run(widht, height, carSizeX, carSizeY, theme);
        }

        ~GamePanel()
        {
            Card.callback = null;
        }

        private void HandleCallback(Card c)
        {
            c.Flip();
            CardClicked(c);
        }

        public void Build(Card[,] cards, int xAmount, int yAmount)
        {
            //Cards.Count() needs to be more or equale too (x * y)
            if (cards.Length < xAmount * yAmount)
            {
                Console.WriteLine("Cards.Count() needs to be more or equale too (x * y)");
                return;
            }

            Width = 200 * xAmount;
            Height = 250 * yAmount;

            VerticalAlignment = VerticalAlignment.Center;
            for (int i = 0; i < xAmount; i++)
            {
                this.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < yAmount; j++)
                {
                    this.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                    Card c = cards[i, j];
                    c.Margin = new Thickness(10);
                    Border border = new Border();
                    border.Child = c;
                    Grid.SetColumn(((Card)(border.Child)), i);
                    Grid.SetRow(((Card)(border.Child)), j);
                    Children.Add(((Card)(border.Child)));
                }
            }
            doneArgs = new OnClickDoneArgs();
            firstCardClicked = false;
        }

        public void Build(Card[,] cards, int xAmount, int yAmount, int carSizeX, int carSizeY)
        {
            //Cards.Count() needs to be more or equale too (x * y)
            if (cards.Length < xAmount * yAmount)
            {
                Console.WriteLine("Cards.Count() needs to be more or equale too (x * y)");
                return;
            }

            Width = (carSizeX + 10) * xAmount;
            Height = (carSizeY + 10) * yAmount;

            for (int i = 0; i < xAmount; i++)
            {
                this.ColumnDefinitions.Add(new ColumnDefinition() { Width = GridLength.Auto });
                for (int j = 0; j < yAmount; j++)
                {
                    this.RowDefinitions.Add(new RowDefinition());
                    Card c = cards[i, j];
                    Grid.SetColumn(c, i);
                    Grid.SetRow(c, j);
                    Children.Add(c);
                }
            }
            doneArgs = new OnClickDoneArgs();
            firstCardClicked = false;
        }

        public void Activate()
        {

        }

        public void Deactivate()
        {

        }

        private void CardClicked(Card c)
        {
            if (!firstCardClicked)
            {
                doneArgs.firstCard = c;
                firstCardClicked = true;
            }
            else
            {
                doneArgs.secondCard = c;
                doneArgs.Correct = (doneArgs.firstCard.ID == c.ID);
                if (doneArgs.Correct)
                {
                    Children.Remove(doneArgs.firstCard);
                    Children.Remove(doneArgs.secondCard);
                }
                else
                {
                    doneArgs.firstCard.Flip();
                    doneArgs.secondCard.Flip();
                }
                OnClickDone(doneArgs);
                doneArgs = new OnClickDoneArgs();
                firstCardClicked = false;
            }
        }

        protected virtual void OnClickDone(OnClickDoneArgs e)
        {
            EventHandler<OnClickDoneArgs> eventHandler = onClickDone;
            if (eventHandler != null)
            {
                eventHandler(this, e);
            }
        }

        public class OnClickDoneArgs : EventArgs
        {
            public Card firstCard;
            public Card secondCard;
            public bool Correct;
        }

        public event EventHandler<OnClickDoneArgs> onClickDone;

        public void Run(int x, int y)
        {
            cards = new Card[x, y];
            List<BitmapImage> bitmapImages = ImageGetter.GetImagesByTheme("Dank memes", x * y, 200);

            int image = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    Card card = new Card(image, new Size(200, 300), new Point(0, 0), bitmapImages[image]);
                    cards[i, j] = card;
                }
            }
            Build(cards, x, y);
        }
        public void Run(int x, int y, int cardSizeX, int cardSizeY, string theme)
        {
            cards = new Card[x, y];
            List<BitmapImage> bitmapImages = ImageGetter.GetImagesByTheme(theme, x * y, cardSizeX);
            int image = 1;
            List<Card> cardEntries = new List<Card>();
            for (int i = 0; i < (x * y) / 2; i++)
            {
                cardEntries.Add(new Card(image, new Size(cardSizeX, cardSizeY), new Point(0, 0), bitmapImages[image]));
                cardEntries.Add(new Card(image, new Size(cardSizeX, cardSizeY), new Point(0, 0), bitmapImages[image]));
                image++;
            }
            Card.BackImage = bitmapImages[0];
            List<int> cardOrder = new List<int>();

            for (int i = 0; i < cardEntries.Count; i++)
            {
                cardOrder.Add(i);
            }
            Console.WriteLine(cardOrder.Count);

            ExtensionMethods.Shuffle(cardOrder);

            int index = 0;
            for (int i = 0; i < x; i++)
            {
                for (int j = 0; j < y; j++)
                {
                    cards[i, j] = cardEntries[cardOrder[index]];
                    index++;
                }
            }

            Build(cards, x, y, cardSizeX, cardSizeY);
        }

    }

    public static class ExtensionMethods
    {
        public static void Shuffle<T>(this IList<T> list)
        {
            Random r = new Random();
            for (int i = 0; i < list.Count; i++)
            {
                T t = list[i];
                int rand = r.Next(0, list.Count);
                T oldValue = list[rand];
                list[i] = oldValue;
                list[rand] = t;
            }
        }
    }
}
