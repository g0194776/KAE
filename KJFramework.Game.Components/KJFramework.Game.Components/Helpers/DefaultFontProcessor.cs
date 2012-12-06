using System.IO;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;

namespace KJFramework.Game.Components.Helpers
{
    [ContentProcessor]
    public class DefaultFontProcessor : FontDescriptionProcessor
    {
        public override SpriteFontContent Process(FontDescription input, ContentProcessorContext context)
        {
            //�����ļ�
            string fullPath = Path.GetFullPath("message.txt");
            context.AddDependency(fullPath);
            string letters = File.ReadAllText(fullPath, System.Text.Encoding.UTF8);

            //�����ַ�
            foreach (char c in letters)
            {
                input.Characters.Add(c);
            }
            return base.Process(input, context);
        }
    }
}