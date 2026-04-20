namespace ProgrammeringsProjektDel1Af3;

public static class PensionsHjælper
{
    public const int Pensionsalder = 67;

    public static int BeregnAlder(DateOnly fødselsdato, DateOnly dagsDato)
    {
        var alder = dagsDato.Year - fødselsdato.Year;

        if (dagsDato < fødselsdato.AddYears(alder))
        {
            alder--;
        }

        return alder;
    }

    public static int BeregnÅrTilPension(DateOnly fødselsdato, DateOnly dagsDato)
    {
        var alder = BeregnAlder(fødselsdato, dagsDato);
        return Math.Max(Pensionsalder - alder, 0);
    }
}