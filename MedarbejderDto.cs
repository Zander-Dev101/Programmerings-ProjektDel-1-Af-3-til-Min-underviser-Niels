namespace Programmerings_projekt_del_2_af_3;

public sealed class MedarbejderDto
{
    public required string Fornavn { get; set; }
    public required string Efternavn { get; set; }
    public required DateOnly Fødselsdato { get; set; }
    public required Køn Køn { get; set; }
    public required Afdeling Afdeling { get; set; }
}
