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

            try
            {
                if (html.Length == 3)
                {
                    return new Color(
                        Convert.ToInt32(new string(html[0], 2), 16),
                        Convert.ToInt32(new string(html[1], 2), 16),
                        Convert.ToInt32(new string(html[2], 2), 16)
                    );
                }
                else if (html.Length == 4)
                {
                    return new Color(
                        Convert.ToInt32(new string(html[0], 2), 16),
                        Convert.ToInt32(new string(html[1], 2), 16),
                        Convert.ToInt32(new string(html[2], 2), 16),
                        Convert.ToInt32(new string(html[3], 2), 16)
                    );
                }
                else if (html.Length == 5)
                {
                    return new Color(
                        Convert.ToInt32(html.Substring(0, 2), 16),
                        Convert.ToInt32(html.Substring(1, 2), 16),
                        Convert.ToInt32(html.Substring(3, 2), 16),
                        Convert.ToInt32(html.Substring(4, 2), 16)
                    );
                }
                else if (html.Length == 6)
                {
                    return new Color(
                        Convert.ToInt32(html.Substring(0, 2), 16),
                        Convert.ToInt32(html.Substring(2, 2), 16),
                        Convert.ToInt32(html.Substring(4, 2), 16)
                    );
                }
                else if (html.Length == 8)
                {
                    return new Color(
                        Convert.ToInt32(html.Substring(0, 2), 16),
                        Convert.ToInt32(html.Substring(2, 2), 16),
                        Convert.ToInt32(html.Substring(4, 2), 16),
                        Convert.ToInt32(html.Substring(6, 2), 16)
                    );
                }
            }
            catch (FormatException)
            {
            }

            return Color.FromName(html);
        }
    }
}