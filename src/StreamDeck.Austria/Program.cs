using OpenMacroBoard.SDK;

namespace StreamDeck.Austria;

internal class Program
{
    static void Main(string[] args)
    {
        //This example is designed for the 5x3 (original) Stream Deck.
        if (args.Length == 0)
        {
            // optionally do something with startup arguments.
        }

        //Create some color we use later to draw the flag of austria
        var red = KeyBitmap.Create.FromRgb(237, 41, 57);
        var white = KeyBitmap.Create.FromRgb(255, 255, 255);
        var rowColors = new KeyBitmap[] { red, white, red };

        //Open the Stream Deck device
        using (var deck = StreamDeckSharp.StreamDeck.OpenDevice())
        {
            deck.SetBrightness(100);

            //Send the bitmap informaton to the device
            for (int i = 0; i < deck.Keys.Count; i++)
                deck.SetKeyBitmap(i, rowColors[i / 5]);

            Console.ReadKey();
        }
    }
}
