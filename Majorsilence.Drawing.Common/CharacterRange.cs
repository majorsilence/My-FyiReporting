using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Majorsilence.Drawing
{
    public class CharacterRange
    {
        public int First { get; set; }
        public int Length { get; set; }
        public CharacterRange(int first, int length)
        {
            First = first;
            Length = length;
        }
    }
}
