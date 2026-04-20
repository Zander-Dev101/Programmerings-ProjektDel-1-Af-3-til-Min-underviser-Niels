namespace ProgrammeringsProjektDel1Af3;

public sealed class MedarbejderRecordTjeneste
{
    public List<MedarbejderRecord> OpretRecordsEfterKøn(List<MedarbejderDto> medarbejdere, Køn køn)
    {
        var dagsDato = DateOnly.FromDateTime(DateTime.Today);
        var records = new List<MedarbejderRecord>();

        foreach (var medarbejder in medarbejdere)
        {
            if (medarbejder.Køn != køn)
            {
                continue;
            }

            records.Add(OpretRecord(medarbejder, dagsDato));
        }

        records.Sort();
        return records;
    }

    public List<MedarbejderRecord> OpretRecordsMedFemEllerMindreÅrTilPension(List<MedarbejderDto> medarbejdere)
    {
        var dagsDato = DateOnly.FromDateTime(DateTime.Today);
        var records = new List<MedarbejderRecord>();

        foreach (var medarbejder in medarbejdere)
        {
            var årTilPension = PensionsHjælper.BeregnÅrTilPension(medarbejder.Fødselsdato, dagsDato);
            if (årTilPension > 5)
            {
                continue;
            }

            records.Add(OpretRecord(medarbejder, dagsDato));
        }

        records.Sort();
        return records;
    }

    private static MedarbejderRecord OpretRecord(MedarbejderDto medarbejder, DateOnly dagsDato)
    {
        var alder = PensionsHjælper.BeregnAlder(medarbejder.Fødselsdato, dagsDato);
        var årTilPension = PensionsHjælper.BeregnÅrTilPension(medarbejder.Fødselsdato, dagsDato);

        return new MedarbejderRecord(
            medarbejder.Fornavn,
            medarbejder.Efternavn,
            medarbejder.Fødselsdato,
            medarbejder.Køn,
            alder,
            årTilPension);
    }
}