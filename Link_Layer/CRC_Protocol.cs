using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Link_Layer
{
    public class CRC_Protocol
    {
        string generator_polinomial;

        public CRC_Protocol()
        {
            generator_polinomial = "10011";
        }

        public string Generate_Verification_Data(string data)
        {
            string aux_data = data.Clone().ToString();
            for (int i = 0; i < generator_polinomial.Length - 1; i++)
            {
                aux_data += "0";
            }
            return Division_Reminder(aux_data);
        }
        public string Division_Reminder(string data)
        {
            int pointer = generator_polinomial.Length;
            string dividend = data.Substring(0,pointer);
            string divisor = generator_polinomial;
            while (pointer < data.Length)
            {
                dividend = Mod2_Substraction(dividend, divisor);
                dividend += data[pointer];
                pointer++;
            }
            return Mod2_Substraction(dividend, divisor);
        }

        private string Mod2_Substraction(string dividend, string divisor)
        {
            if (dividend[0] == '0')
                return dividend.Substring(1);
            else
            {
                string result = "";
                for (int i = 1; i < dividend.Length; i++)
                {
                    int aux = int.Parse(dividend[i].ToString());
                    int aux2 = int.Parse(divisor[i].ToString());
                    if (aux + aux2 > 1)
                        result += "0";
                    else
                        result += aux + aux2;
                }
                return result;
            }
        }
    }
}
