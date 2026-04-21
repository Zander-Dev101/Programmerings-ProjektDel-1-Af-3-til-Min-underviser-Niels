using System.Text;

namespace Programmerings_projekt_del_2_af_3;

public static class TekstFormattering
{
    public static string FormatérEnumNavn<TEnum>(TEnum værdi) where TEnum : struct, Enum
    {
        string tekst = værdi.ToString();
        StringBuilder resultat = new();

        for (int indeks = 0; indeks < tekst.Length; indeks++)
        {
            char tegn = tekst[indeks];

            if (indeks > 0 && char.IsUpper(tegn) && !char.IsUpper(tekst[indeks - 1]))
            {
                resultat.Append(' ');
            }

            resultat.Append(tegn);
        }

        return resultat.ToString();
    }
}
