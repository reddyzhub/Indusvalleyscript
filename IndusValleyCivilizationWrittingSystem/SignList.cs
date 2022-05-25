using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IndusValleyCivilizationWrittingSystem
{
    public class SignList : List<Sign>
    {
        public SignList()
        {

            for (byte j = 0; j < 114; j++)
            {
                for (byte i = 0; i < 16; i++)
                {

                    Add(new Sign()
                    {
                        code = (char)int.Parse($"E{j.ToString("x2")}{i.ToString("x1")}", System.Globalization.NumberStyles.HexNumber),
                        name = $"E{j.ToString("X2")}{i.ToString("X1")}"
                    });
                }
            }
        }
    }
    public class Sign
    {
        public char code { get; set; }
        public string name { get; set; }
    }
}
