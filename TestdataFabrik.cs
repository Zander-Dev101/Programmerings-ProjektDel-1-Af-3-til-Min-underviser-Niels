namespace Programmerings_projekt_del_2_af_3;

public static class TestdataFabrik
{
    public static List<MedarbejderDto> OpretMedarbejdere()
    {
        return
        [
            new MedarbejderDto { Fornavn = "Anna", Efternavn = "Jensen", Fødselsdato = new DateOnly(1961, 4, 12), Køn = Køn.Kvinde, Afdeling = Afdeling.SoftwareDevelopment },
            new MedarbejderDto { Fornavn = "Mette", Efternavn = "Larsen", Fødselsdato = new DateOnly(1962, 9, 3), Køn = Køn.Kvinde, Afdeling = Afdeling.Administration },
            new MedarbejderDto { Fornavn = "Sofie", Efternavn = "Nielsen", Fødselsdato = new DateOnly(1988, 2, 28), Køn = Køn.Kvinde, Afdeling = Afdeling.ServiceAndSupport },
            new MedarbejderDto { Fornavn = "Laura", Efternavn = "Hansen", Fødselsdato = new DateOnly(1992, 6, 11), Køn = Køn.Kvinde, Afdeling = Afdeling.SoftwareDevelopment },
            new MedarbejderDto { Fornavn = "Freja", Efternavn = "Kristensen", Fødselsdato = new DateOnly(1985, 12, 20), Køn = Køn.Kvinde, Afdeling = Afdeling.Administration },
            new MedarbejderDto { Fornavn = "Ida", Efternavn = "Madsen", Fødselsdato = new DateOnly(1995, 7, 15), Køn = Køn.Kvinde, Afdeling = Afdeling.ServiceAndSupport },
            new MedarbejderDto { Fornavn = "Peter", Efternavn = "Andersen", Fødselsdato = new DateOnly(1960, 8, 9), Køn = Køn.Mand, Afdeling = Afdeling.SoftwareDevelopment },
            new MedarbejderDto { Fornavn = "Kasper", Efternavn = "Mortensen", Fødselsdato = new DateOnly(1983, 1, 17), Køn = Køn.Mand, Afdeling = Afdeling.Administration },
            new MedarbejderDto { Fornavn = "Jonas", Efternavn = "Olsen", Fødselsdato = new DateOnly(1990, 5, 6), Køn = Køn.Mand, Afdeling = Afdeling.ServiceAndSupport },
            new MedarbejderDto { Fornavn = "Mads", Efternavn = "Pedersen", Fødselsdato = new DateOnly(1961, 11, 25), Køn = Køn.Mand, Afdeling = Afdeling.Administration }
        ];
    }
}
