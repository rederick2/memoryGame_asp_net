using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace WebApplicationCartas
{
    public partial class MemoryGame : System.Web.UI.Page
    {
        private static readonly Random Random = new Random();
        private static readonly List<string> Symbols = new List<string> { "A", "A", "B", "B", "C", "C", "D", "D", "E", "E", "F", "F", "G", "G", "H", "H" };
        private List<string> shuffledSymbols;
        private int? firstCardIndex;
        private int? secondCardIndex;
        private int flippedCards;
        private int foundPairs;

        protected void Page_Init(object sender, EventArgs e)
        {
            // Generate buttons dynamically
            GenerateButtons();
        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                shuffledSymbols = ShuffleSymbols();
                ViewState["ShuffledSymbols"] = shuffledSymbols;
                ViewState["FirstCardIndex"] = null;
                ViewState["SecondCardIndex"] = null;
                flippedCards = 0;
                foundPairs = 0;
                UpdateLabels();

            }
            else
            {
                shuffledSymbols = (List<string>)ViewState["ShuffledSymbols"];
                firstCardIndex = (int?)ViewState["FirstCardIndex"];
                secondCardIndex = (int?)ViewState["SecondCardIndex"];
                flippedCards = ViewState["FlippedCards"] != null ? (int)ViewState["FlippedCards"] : 0;
                foundPairs = ViewState["FoundPairs"] != null ? (int)ViewState["FoundPairs"] : 0;
                CalculateRemaining();
                UpdateButtonStates();
            }
        }

        private void UpdateButtonStates()
        {
            for (int i = 0; i < 16; i++)
            {
                var button = (Button)gameGrid.FindControl("Card" + i);
                if (firstCardIndex.HasValue && firstCardIndex.Value == i)
                {
                    button.Text = shuffledSymbols[i];
                }
                else if (secondCardIndex.HasValue && secondCardIndex.Value == i)
                {
                    button.Text = shuffledSymbols[i];
                }
                else if (!button.Enabled)
                {
                    button.Text = shuffledSymbols[i];
                }
                else
                {
                    button.Text = "?";
                }
            }
        }

        protected void ResetButton_Click(object sender, EventArgs e)
        {
            shuffledSymbols = ShuffleSymbols();
            ViewState["ShuffledSymbols"] = shuffledSymbols;
            firstCardIndex = null;
            secondCardIndex = null;
            flippedCards = 0;
            foundPairs = 0;
            ViewState["FirstCardIndex"] = firstCardIndex;
            ViewState["SecondCardIndex"] = secondCardIndex;
            ViewState["FlippedCards"] = flippedCards;
            ViewState["FoundPairs"] = foundPairs;

            foreach (Control control in gameGrid.Controls)
            {
                if (control is Button button)
                {
                    button.Enabled = true;
                    button.Text = "?";
                }
            }

            UpdateLabels();
            CalculateRemaining();
        }

        private void GenerateButtons()
        {
            for (int i = 0; i < 16; i++)
            {
                Button button = new Button();
                button.ID = "Card" + i;
                button.CssClass = "card";
                button.Text = "?";
                button.Click += new EventHandler(Card_Click);
                gameGrid.Controls.Add(button);
            }
        }

        protected void Card_Click(object sender, EventArgs e)
        {
            var button = (Button)sender;
            string buttonId = button.ID;

            // Ensure the button ID is in the correct format
            if (buttonId.StartsWith("Card") && int.TryParse(buttonId.Substring(4), out int cardIndex))
            {
                if (!firstCardIndex.HasValue)
                {
                    firstCardIndex = cardIndex;
                    ViewState["FirstCardIndex"] = firstCardIndex;
                    button.Text = shuffledSymbols[cardIndex];
                }
                else if (!secondCardIndex.HasValue && cardIndex != firstCardIndex)
                {
                    secondCardIndex = cardIndex;
                    ViewState["SecondCardIndex"] = secondCardIndex;
                    button.Text = shuffledSymbols[cardIndex];
                    CheckMatch();
                }

                flippedCards++;
                ViewState["FlippedCards"] = flippedCards;
                UpdateLabels();
                CalculateRemaining();
            }
            else
            {
                // Log error or handle unexpected ID format
                throw new InvalidOperationException("Button ID format is incorrect.");
            }
        }

        private void CheckMatch()
        {
            var firstButton = (Button)FindControl($"Card{firstCardIndex.Value}");
            var secondButton = (Button)FindControl($"Card{secondCardIndex.Value}");


            if (shuffledSymbols[firstCardIndex.Value] == shuffledSymbols[secondCardIndex.Value])
            {
                firstButton.Enabled = false;
                secondButton.Enabled = false;
                foundPairs++;
                ViewState["FoundPairs"] = foundPairs;
            }
            else
            {
                ClientScript.RegisterStartupScript(this.GetType(), "resetCards", $"resetCards('Card{firstCardIndex.Value}', 'Card{secondCardIndex.Value}');", true);
            }

            firstCardIndex = null;
            secondCardIndex = null;
            ViewState["FirstCardIndex"] = firstCardIndex;
            ViewState["SecondCardIndex"] = secondCardIndex;

            CalculateRemaining();
            UpdateLabels();
        }

        private List<string> ShuffleSymbols()
        {
            var shuffled = new List<string>(Symbols);
            for (int i = 0; i < shuffled.Count; i++)
            {
                int j = Random.Next(i, shuffled.Count);
                string temp = shuffled[i];
                shuffled[i] = shuffled[j];
                shuffled[j] = temp;
            }
            return shuffled;
        }

        private void UpdateLabels()
        {
            lblFlippedCards.Text = flippedCards.ToString();
            lblFoundPairs.Text = foundPairs.ToString();
            lblRemainingCards.Text = (16 - (flippedCards / 2 * 2)).ToString();
            lblRemainingPairs.Text = (8 - foundPairs).ToString();
        }

        private void CalculateRemaining()
        {
            int flippedPairs = foundPairs * 2;
            lblRemainingCards.Text = (16 - (foundPairs*2)).ToString();
            lblRemainingPairs.Text = (8 - foundPairs).ToString();
        }
    }
}