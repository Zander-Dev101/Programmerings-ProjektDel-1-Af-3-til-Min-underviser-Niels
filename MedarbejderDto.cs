namespace Programmerings_projekt_del_3_af_3;

public sealed class MedarbejderDto
{
    private DateOnly fødselsdato;

    public required string Fornavn { get; set; }
    public required string Efternavn { get; set; }

    public required DateOnly Fødselsdato
    {
        get => fødselsdato;
        set
        {
            fødselsdato = value;
            Alder = PensionsHjælper.BeregnAlder(value, DateOnly.FromDateTime(DateTime.Today));
            ÅrTilPension = PensionsHjælper.BeregnÅrTilPension(value, DateOnly.FromDateTime(DateTime.Today));
        }
    }

    public required Køn Køn { get; set; }
    public required Afdeling Afdeling { get; set; }

    public int Alder { get; private set; }
    public int ÅrTilPension { get; private set; }
}