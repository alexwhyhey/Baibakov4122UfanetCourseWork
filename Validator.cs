using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Baibakov4122UfanetCourseWork
{
    public class Validator
    {
        public bool Email(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
            {
                return false;
            }

            if (email.Length > 254)
            {
                return false;
            }

            if (!Regex.IsMatch(email, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$"))
            {
                return false;
            }

            return true;
        }

        public bool Phone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone))
            {
                return false;
            }

            if (!Regex.IsMatch(phone.Trim(), @"^\+7\(\d{3}\)\d{3}-\d{2}-\d{2}$"))
            {
                return false;
            }

            return true;
        }

        public bool Passport(string passport)
        {
            if (string.IsNullOrWhiteSpace(passport))
            {
                return false;
            }

            if (!Regex.IsMatch(passport, @"^\d{4} \d{6}$"))
            {
                return false;
            }

            return true;
        }

        public bool LastName(string lastName)
        {
            if (string.IsNullOrWhiteSpace(lastName))
            {
                return false;
            }

            if (!Regex.IsMatch(lastName, @"^[А-ЯЁ][а-яё]{1,49}$"))
            {
                return false;
            }

            return true;
        }

        public bool FirstName(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
            {
                return false;
            }

            if (!Regex.IsMatch(firstName, @"^[А-ЯЁ][а-яё]{1,49}$"))
            {
                return false;
            }

            return true;
        }

        public bool MiddleName(string middleName)
        {

            if (string.IsNullOrWhiteSpace(middleName))
            {
                return true;
            }

            if (!Regex.IsMatch(middleName, @"^[А-ЯЁ][а-яё]{1,49}$"))
            {
                return false;
            }

            return true;
        }

        public bool Address(string address)
        {
            if (string.IsNullOrWhiteSpace(address))
            {
                return false;
            }

            if (!Regex.IsMatch(address, @"^г\.[ ]*[А-Яа-я\- ]+,[ ]*ул\.[ ]*[А-Яа-я\-\d ]+,[ ]*д\.[ ]*\d+[А-Яа-я\-\d]*(,[ ]*кв\.[ ]*\d+)?$"))
            {
                return false;
            }

            return true;
        }

        public bool IsAdult(DateTime birth, DateTime signUp)
        {
            int age = signUp.Year - birth.Year;

            if (signUp.Month < birth.Month || (signUp.Month == birth.Month && signUp.Day < birth.Day))
            {
                age--;
            }

            return age >= 18;
        }
    }
}
