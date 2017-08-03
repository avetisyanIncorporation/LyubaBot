using System;
using System.Text.RegularExpressions;

namespace Lyuba02_with_luis_ai
{
    [Serializable]
    public class Helper
    {
        
        public bool IsPhoneNumber(string text)
        {
            text = DeleteAll(text);

            char[] mas = text.ToCharArray();

            if (mas[0] == '+')
                return false;

            if (mas[0] == '8')
                return false;

            if (mas[0] == '7')
                return false;

            if (mas[0] != '9')
                return false;

            if (mas.Length != 10)
                return false;

            for (int i = 0; i < mas.Length; i++)
            {
                if (mas[i] < '0' || mas[i] > '9')
                    return false;
            }
            
            return true;
        }

        
        public string DeleteAll(string text)
        { 
            text = text.Replace(" ", "");
            text = Regex.Replace(text, "[-.;?!)(,:]", "");
            text = text.ToLower();

            return text;
        }


        public bool IsMail(string text)
        {
            if (Regex.IsMatch(text, @"^([0-9a-zA-Z]([-.\w]*[0-9a-zA-Z])*@([0-9a-zA-Z][-\w]*[0-9a-zA-Z]\.)+[a-zA-Z]{2,9})$"))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

    }
}