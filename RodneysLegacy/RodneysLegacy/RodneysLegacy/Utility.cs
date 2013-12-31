﻿using System.Text;
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
    }
}
