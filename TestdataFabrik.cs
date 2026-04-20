namespace ProgrammeringsProjektDel1Af3;

public static class TestdataFabrik
{
    public static List<MedarbejderDto> OpretTiMedarbejdere()
    {
        return
        [
            new MedarbejderDto { Fornavn = "Anna", Efternavn = "Jensen", Fødselsdato = new DateOnly(1961, 4, 12), Køn = Køn.F },
            new MedarbejderDto { Fornavn = "Mette", Efternavn = "Larsen", Fødselsdato = new DateOnly(1962, 9, 3), Køn = Køn.F },
            new MedarbejderDto { Fornavn = "Sofie", Efternavn = "Nielsen", Fødselsdato = new DateOnly(1988, 2, 28), Køn = Køn.F },
            new MedarbejderDto { Fornavn = "Laura", Efternavn = "Hansen", Fødselsdato = new DateOnly(1992, 6, 11), Køn = Køn.F },
            new MedarbejderDto { Fornavn = "Freja", Efternavn = "Kristensen", Fødselsdato = new DateOnly(1985, 12, 20), Køn = Køn.F },
            new MedarbejderDto { Fornavn = "Ida", Efternavn = "Madsen", Fødselsdato = new DateOnly(1995, 7, 15), Køn = Køn.F },
            new MedarbejderDto { Fornavn = "Peter", Efternavn = "Andersen", Fødselsdato = new DateOnly(1979, 8, 9), Køn = Køn.M },
            new MedarbejderDto { Fornavn = "Kasper", Efternavn = "Mortensen", Fødselsdato = new DateOnly(1983, 1, 17), Køn = Køn.M },
            new MedarbejderDto { Fornavn = "Jonas", Efternavn = "Olsen", Fødselsdato = new DateOnly(1990, 5, 6), Køn = Køn.M },
            new MedarbejderDto { Fornavn = "Mads", Efternavn = "Pedersen", Fødselsdato = new DateOnly(1987, 11, 25), Køn = Køn.M }
        ];
    }
}