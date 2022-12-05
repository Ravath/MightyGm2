using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public static class ExtensionMethods
{
    public static void RemoveChildren(this Node node)
    {
        foreach (Node item in node.GetChildren())
        {
            item.QueueFree();
        }
    }

    public static StringBuilder WordWrap(this string text, int maxLineLength)
    {
        var list = new StringBuilder();

        int currentIndex;
        var lastWrap = 0;
        var whitespace = new[] { ' ', '\r', '\n', '\t' };
        do
        {
            // Get Current index
            currentIndex = text.Length;
                
            if (lastWrap + maxLineLength < text.Length)
            {
                int startIndex = Math.Min(text.Length - 1, lastWrap + maxLineLength);
                // Find new lines in priority
                currentIndex = text.LastIndexOfAny(new[] { '\n', '\r' }, startIndex, maxLineLength) + 1;
                // if no new line found in text, use normal separation
                if (currentIndex <= lastWrap)
                    currentIndex = text.LastIndexOfAny(new[] { ' ', ',', '.', '?', '!', ':', ';', '-', '\t' }, startIndex) + 1;
            }

            // if no separation found, cut arbitrarily at maxLineLength
            if (currentIndex <= lastWrap)
                currentIndex = Math.Min(lastWrap + maxLineLength, text.Length);

            // Add new line
            list.AppendLine(text.Substring(lastWrap, currentIndex - lastWrap).Trim(whitespace));
            // Prepare next iteration
            lastWrap = currentIndex;
        } while (currentIndex < text.Length);

        return list;
    }
}
