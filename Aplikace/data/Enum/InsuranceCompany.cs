using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplikace.data.Enum
{
    public enum InsuranceCompany
    {
        [EnumInfo("VZP", 1)]
        VšeobecnáZdravotníPojišťovnaČeskéRepubliky,

        [EnumInfo("RBP", 2)]
        RevírníBratrskáPokladna,

        [EnumInfo("ZPMV", 3)]
        ZdravotníPojišťovnaMinisterstvaVnitra,

        [EnumInfo("BPST", 4)]
        ZdravotníPojišťovnaZaměstnancůBankPojišťovenAStavebnictví,

        [EnumInfo("MA", 5)]
        ZdravotníPojišťovnaMETALALIANCE,

        [EnumInfo("OZP", 6)]
        ZdravotníPojišťovnaOZP
    }

    [AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
    sealed class EnumInfoAttribute : Attribute
    {
        public string Abbreviation { get; }
        public int Id { get; }

        public EnumInfoAttribute(string abbreviation, int id)
        {
            Abbreviation = abbreviation;
            Id = id;
        }
    }

    public static class InsuranceCompanyExtensions
    {
        public static string GetAbbreviation(this InsuranceCompany insuranceCompany)
        {
            var enumType = insuranceCompany.GetType();
            var fieldInfo = enumType.GetField(insuranceCompany.ToString());
            var attribute = (EnumInfoAttribute)fieldInfo.GetCustomAttributes(typeof(EnumInfoAttribute), false)[0];
            return attribute.Abbreviation;
        }

        public static int GetId(this InsuranceCompany insuranceCompany)
        {
            var enumType = insuranceCompany.GetType();
            var fieldInfo = enumType.GetField(insuranceCompany.ToString());
            var attribute = (EnumInfoAttribute)fieldInfo.GetCustomAttributes(typeof(EnumInfoAttribute), false)[0];
            return attribute.Id;
        }
    }
}
