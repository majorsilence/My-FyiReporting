using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsilence.Drawing
{
    public class ColorTranslator
    {
        public ColorTranslator() { }

        public static Color FromHtml(string html)
        {
            if (string.IsNullOrEmpty(html))
                throw new ArgumentException("Invalid HTML color code", nameof(html));

            if (html[0] == '#')
            {
                html = html.Substring(1);
            }

            if (html.Length == 6)
            {
                return new Color(
                    Convert.ToInt32(html.Substring(0, 2), 16),
                    Convert.ToInt32(html.Substring(2, 4), 16),
                    Convert.ToInt32(html.Substring(4, 6), 16)
                );
            }
            else if (html.Length == 8)
            {
                return new Color(
                    Convert.ToInt32(html.Substring(0, 2), 16),
                    Convert.ToInt32(html.Substring(2, 4), 16),
                    Convert.ToInt32(html.Substring(4, 6), 16),
                    Convert.ToInt32(html.Substring(6, 8), 16)
                );
            }
            else
            {
                return Color.FromName(html);
            }
        }
    }
}
