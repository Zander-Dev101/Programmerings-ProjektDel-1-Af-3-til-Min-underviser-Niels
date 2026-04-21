using System.Globalization;

namespace Programmerings_projekt_del_2_af_3;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<MedarbejderDto> medarbejdere = TestdataFabrik.OpretMedarbejdere();
        int[,] lønData = OpretLønData();
        bool kører = true;

        while (kører)
        {
            VisMenu();
            Console.Write("Vælg et menupunkt: ");
            string? valg = Console.ReadLine()?.Trim();

            Console.Clear();

            switch (valg)
            {
                case "1":
                    UdskrivAlleMedarbejdere(medarbejdere);
                    break;
                case "2":
                    UdskrivMedarbejdereEfterKøn(medarbejdere, Køn.Kvinde, "Kvindelige medarbejdere");
                    break;
                case "3":
                    UdskrivMedarbejdereEfterKøn(medarbejdere, Køn.Mand, "Mandlige medarbejdere");
                    break;
                case "4":
                    UdskrivMedarbejdereTætPåPension(medarbejdere, lønData);
                    break;
                case "5":
                    RegistrerNyMedarbejder(medarbejdere);
                    break;
                case "6":
                    kører = false;
                    Console.WriteLine("Programmet afsluttes.");
                    break;
                default:
                    Console.WriteLine("Ugyldigt valg. Prøv igen.");
                    break;
            }

            if (kører)
            {
                Console.WriteLine();
                Console.WriteLine("Tryk Enter for at vende tilbage til menuen.");
                Console.ReadLine();
                Console.Clear();
            }
        }
    }

    static int[,] OpretLønData()
    {
        return new int[,]
        {
            { 42000, 46000, 92000, 52500 },
            { 35000, 38000, 76000, 43750 },
            { 33000, 36000, 72000, 41250 }
        };
    }

    static void VisMenu()
    {
        Console.WriteLine("Programmerings projekt del 2 af 3");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1. Vis alle medarbejdere");
        Console.WriteLine("2. Vis kvindelige medarbejdere");
        Console.WriteLine("3. Vis mandlige medarbejdere");
        Console.WriteLine("4. Vis medarbejdere med 5 eller mindre år til pension");
        Console.WriteLine("5. Registrer ny medarbejder");
        Console.WriteLine("6. Afslut");
        Console.WriteLine();
    }

    static void UdskrivAlleMedarbejdere(List<MedarbejderDto> medarbejdere)
    {
        Console.WriteLine("Alle medarbejdere");
        Console.WriteLine("-----------------");
        UdskrivMedarbejderListe(medarbejdere);
    }

    static void UdskrivMedarbejdereEfterKøn(List<MedarbejderDto> medarbejdere, Køn køn, string overskrift)
    {
        Console.WriteLine(overskrift);
        Console.WriteLine(new string('-', overskrift.Length));

        List<MedarbejderDto> filtreredeMedarbejdere = medarbejdere
            .Where(medarbejder => medarbejder.Køn == køn)
            .OrderBy(medarbejder => medarbejder.Efternavn, StringComparer.Create(new CultureInfo("da-DK"), true))
            .ThenBy(medarbejder => medarbejder.Fornavn, StringComparer.Create(new CultureInfo("da-DK"), true))
            .ToList();

        UdskrivMedarbejderListe(filtreredeMedarbejdere);
    }

    static void UdskrivMedarbejdereTætPåPension(List<MedarbejderDto> medarbejdere, int[,] lønData)
    {
        Console.WriteLine("Medarbejdere med 5 eller mindre år til pension");
        Console.WriteLine("----------------------------------------------");

        DateOnly dagsDato = DateOnly.FromDateTime(DateTime.Today);

        List<MedarbejderDto> filtreredeMedarbejdere = medarbejdere
            .Where(medarbejder => PensionsHjælper.BeregnÅrTilPension(medarbejder.Fødselsdato, dagsDato) <= 5)
            .OrderBy(medarbejder => medarbejder.Efternavn, StringComparer.Create(new CultureInfo("da-DK"), true))
            .ThenBy(medarbejder => medarbejder.Fornavn, StringComparer.Create(new CultureInfo("da-DK"), true))
            .ToList();

        if (filtreredeMedarbejdere.Count == 0)
        {
            Console.WriteLine("Ingen medarbejdere fundet.");
            return;
        }

        foreach (MedarbejderDto medarbejder in filtreredeMedarbejdere)
        {
            int alder = PensionsHjælper.BeregnAlder(medarbejder.Fødselsdato, dagsDato);
            int årTilPension = PensionsHjælper.BeregnÅrTilPension(medarbejder.Fødselsdato, dagsDato);
            int bonus = HentPensionsBonus(medarbejder, lønData);

            Console.WriteLine(
                $"{medarbejder.Fornavn} {medarbejder.Efternavn} | Alder: {alder} | År til pension: {årTilPension} | " +
                $"Køn: {TekstFormattering.FormatérEnumNavn(medarbejder.Køn)} | " +
                $"Afdeling: {TekstFormattering.FormatérEnumNavn(medarbejder.Afdeling)} | " +
                $"Pension fratrædelse bonus: {bonus:N0} kr.");
        }
    }

    static int HentPensionsBonus(MedarbejderDto medarbejder, int[,] lønData)
    {
        int række = (int)medarbejder.Afdeling;
        int kolonne = medarbejder.Køn == Køn.Mand ? 2 : 3;
        return lønData[række, kolonne];
    }

    static void RegistrerNyMedarbejder(List<MedarbejderDto> medarbejdere)
    {
        Console.WriteLine("Registrer ny medarbejder");
        Console.WriteLine("------------------------");

        Console.Write("Fornavn: ");
        string fornavn = LæsPåkrævetTekst();

        Console.Write("Efternavn: ");
        string efternavn = LæsPåkrævetTekst();

        Console.Write("Fødselsdato (dd-MM-yyyy): ");
        DateOnly fødselsdato = LæsFødselsdato();

        Køn køn = LæsEnumValg<Køn>("Vælg køn");
        Afdeling afdeling = LæsEnumValg<Afdeling>("Vælg afdeling");

        MedarbejderDto nyMedarbejder = new()
        {
            Fornavn = fornavn,
            Efternavn = efternavn,
            Fødselsdato = fødselsdato,
            Køn = køn,
            Afdeling = afdeling
        };

        medarbejdere.Add(nyMedarbejder);
        Console.WriteLine();
        Console.WriteLine("Medarbejderen er registreret.");
    }

    static TEnum LæsEnumValg<TEnum>(string overskrift) where TEnum : struct, Enum
    {
        TEnum[] værdier = Enum.GetValues<TEnum>();

        while (true)
        {
            Console.WriteLine();
            Console.WriteLine(overskrift + ":");

            for (int indeks = 0; indeks < værdier.Length; indeks++)
            {
                Console.WriteLine($"{indeks + 1}. {TekstFormattering.FormatérEnumNavn(værdier[indeks])}");
            }

            Console.Write("Indtast dit valg: ");
            string? input = Console.ReadLine()?.Trim();

            if (int.TryParse(input, out int valgNummer) && valgNummer >= 1 && valgNummer <= værdier.Length)
            {
                return værdier[valgNummer - 1];
            }

            Console.WriteLine("Ugyldigt valg. Prøv igen.");
        }
    }

    static string LæsPåkrævetTekst()
    {
        while (true)
        {
            string? input = Console.ReadLine()?.Trim();
            if (!string.IsNullOrWhiteSpace(input))
            {
                return input;
            }

            Console.Write("Feltet må ikke være tomt. Prøv igen: ");
        }
    }

    static DateOnly LæsFødselsdato()
    {
        string[] formater = ["dd-MM-yyyy", "d-M-yyyy", "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "dd MM yyyy", "d M yyyy"];

        while (true)
        {
            string? input = Console.ReadLine()?.Trim();

            if (DateOnly.TryParseExact(input, formater, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateOnly dato))
            {
                return dato;
            }

            Console.Write("Ugyldig dato. Prøv igen (f.eks. 27-05-2007 eller 27 05 2007): ");
        }
    }

    static void UdskrivMedarbejderListe(List<MedarbejderDto> medarbejdere)
    {
        if (medarbejdere.Count == 0)
        {
            Console.WriteLine("Ingen medarbejdere fundet.");
            return;
        }

        DateOnly dagsDato = DateOnly.FromDateTime(DateTime.Today);

        foreach (MedarbejderDto medarbejder in medarbejdere
                     .OrderBy(medarbejder => medarbejder.Efternavn, StringComparer.Create(new CultureInfo("da-DK"), true))
                     .ThenBy(medarbejder => medarbejder.Fornavn, StringComparer.Create(new CultureInfo("da-DK"), true)))
        {
            int alder = PensionsHjælper.BeregnAlder(medarbejder.Fødselsdato, dagsDato);
            int årTilPension = PensionsHjælper.BeregnÅrTilPension(medarbejder.Fødselsdato, dagsDato);

            Console.WriteLine(
                $"{medarbejder.Fornavn} {medarbejder.Efternavn} | Født: {medarbejder.Fødselsdato:dd-MM-yyyy} | " +
                $"Alder: {alder} | År til pension: {årTilPension} | " +
                $"Køn: {TekstFormattering.FormatérEnumNavn(medarbejder.Køn)} | " +
                $"Afdeling: {TekstFormattering.FormatérEnumNavn(medarbejder.Afdeling)}");
        }
    }
}
