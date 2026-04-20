using System.Globalization;

namespace ProgrammeringsProjektDel1Af3;

public sealed record MedarbejderRecord(
    string Fornavn,
    string Efternavn,
    DateOnly Fødselsdato,
    Køn Køn,
    int Alder,
    int ÅrTilPension) : IComparable<MedarbejderRecord>
{
    public int CompareTo(MedarbejderRecord? other)
    {
        if (other is null)
        {
            return 1;
        }

        var efternavn = string.Compare(Efternavn, other.Efternavn, true, CultureInfo.GetCultureInfo("da-DK"));
        if (efternavn != 0)
        {
            return efternavn;
        }

        var fornavn = string.Compare(Fornavn, other.Fornavn, true, CultureInfo.GetCultureInfo("da-DK"));
        if (fornavn != 0)
        {
            return fornavn;
        }

        return Fødselsdato.CompareTo(other.Fødselsdato);
    }
}