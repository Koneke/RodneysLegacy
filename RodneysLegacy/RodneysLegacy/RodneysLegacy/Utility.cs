using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace RodneysLegacy
{
    static class Utility
    {
        /* word wrapping fr http://www.xnawiki.com/index.php/Basic_Word_Wrapping
         * because I can't be arsed with writing this again */
        public static string WrapText(
            SpriteFont spriteFont,
            string text,
            float maxLineWidth
        ) {
            string[] words = text.Split(' ');
            StringBuilder sb = new StringBuilder();
            float lineWidth = 0f;
            float spaceWidth = spriteFont.MeasureString(" ").X;
            foreach (string word in words)
            {
                Vector2 size = spriteFont.MeasureString(word);
                if (lineWidth + size.X < maxLineWidth)
                {
                    sb.Append(word + " ");
                    lineWidth += size.X + spaceWidth;
                }
                else
                {
                    sb.Append("\n" + " " + word + " ");
                    lineWidth = size.X + spaceWidth;
                }
            }
            return sb.ToString();
        }

        public static bool KeypadNeighbour(
            int _a,
            int _b
        ) {
            switch (_a)
            {
                case 1: return _b == 4 || _b == 2;
                case 2: return _b == 1 || _b == 3;
                case 3: return _b == 2 || _b == 6;
                case 6: return _b == 3 || _b == 9;
                case 9: return _b == 6 || _b == 8;
                case 8: return _b == 9 || _b == 7;
                case 7: return _b == 8 || _b == 4;
                case 4: return _b == 7 || _b == 1;
            }
            return false;
        }
    }
}
