using System.Globalization;

namespace ProgrammeringsProjektDel1Af3;

public sealed class MedarbejderDto : IComparable<MedarbejderDto>, IEquatable<MedarbejderDto>
{
    public required string Fornavn { get; set; }
    public required string Efternavn { get; set; }
    public required DateOnly Fødselsdato { get; set; }
    public required Køn Køn { get; set; }

    public int CompareTo(MedarbejderDto? other)
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

        var fødselsdato = Fødselsdato.CompareTo(other.Fødselsdato);
        if (fødselsdato != 0)
        {
            return fødselsdato;
        }

        return Køn.CompareTo(other.Køn);
    }

    public bool Equals(MedarbejderDto? other)
    {
        if (other is null)
        {
            return false;
        }

        return string.Equals(Fornavn, other.Fornavn, StringComparison.OrdinalIgnoreCase)
               && string.Equals(Efternavn, other.Efternavn, StringComparison.OrdinalIgnoreCase)
               && Fødselsdato.Equals(other.Fødselsdato)
               && Køn == other.Køn;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as MedarbejderDto);
    }

    public override int GetHashCode()
    {
        return HashCode.Combine(Fornavn.ToUpperInvariant(), Efternavn.ToUpperInvariant(), Fødselsdato, Køn);
    }
}