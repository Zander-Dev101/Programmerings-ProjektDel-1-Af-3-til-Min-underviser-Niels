using System.Globalization;

namespace Programmerings_projekt_del_3_af_3;

class Program
{
    static void Main()
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<MedarbejderDto> medarbejdere = TestdataFabrik.OpretMedarbejdere();
        int[,,] lønMatrix = OpretLønMatrix();
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
                    UdskrivMedarbejdereTætPåPension(medarbejdere, lønMatrix);
                    break;

                case "5":
                    RegistrerNyMedarbejder(medarbejdere);
                    break;

                case "6":
                    VisLønMatrix(lønMatrix);
                    break;

                case "7":
                    OpdaterLønMatrix(lønMatrix);
                    break;

                case "8":
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

    static void VisMenu()
    {
        Console.WriteLine("Programmerings projekt del 3 af 3");
        Console.WriteLine("---------------------------------");
        Console.WriteLine("1. Vis alle medarbejdere");
        Console.WriteLine("2. Vis kvindelige medarbejdere");
        Console.WriteLine("3. Vis mandlige medarbejdere");
        Console.WriteLine("4. Vis medarbejdere med 5 eller mindre år til pension");
        Console.WriteLine("5. Registrer ny medarbejder");
        Console.WriteLine("6. Vis løn matrix");
        Console.WriteLine("7. Opdater løn matrix");
        Console.WriteLine("8. Afslut");
        Console.WriteLine();
    }

    static int[,,] OpretLønMatrix()
    {
        int antalAfdelinger = Enum.GetValues<Afdeling>().Length;
        int antalKøn = Enum.GetValues<Køn>().Length;
        int antalLønTyper = Enum.GetValues<LønType>().Length;

        int[,,] lønMatrix = new int[antalAfdelinger, antalKøn, antalLønTyper];

        SætLønOgBonus(lønMatrix, Afdeling.SoftwareDevelopment, Køn.Kvinde, 42000);
        SætLønOgBonus(lønMatrix, Afdeling.SoftwareDevelopment, Køn.Mand, 46000);

        SætLønOgBonus(lønMatrix, Afdeling.Administration, Køn.Kvinde, 35000);
        SætLønOgBonus(lønMatrix, Afdeling.Administration, Køn.Mand, 38000);

        SætLønOgBonus(lønMatrix, Afdeling.ServiceAndSupport, Køn.Kvinde, 33000);
        SætLønOgBonus(lønMatrix, Afdeling.ServiceAndSupport, Køn.Mand, 36000);

        return lønMatrix;
    }

    static void SætLønOgBonus(int[,,] lønMatrix, Afdeling afdeling, Køn køn, int gennemsnitsLøn)
    {
        int afdelingsIndeks = (int)afdeling;
        int kønsIndeks = (int)køn;

        lønMatrix[afdelingsIndeks, kønsIndeks, (int)LønType.GennemsnitsLøn] = gennemsnitsLøn;
        lønMatrix[afdelingsIndeks, kønsIndeks, (int)LønType.PensionFratrædelseBonus] = BeregnBonus(gennemsnitsLøn, køn);
    }

    static int BeregnBonus(int gennemsnitsLøn, Køn køn)
    {
        if (køn == Køn.Mand)
        {
            return gennemsnitsLøn * 2;
        }

        return gennemsnitsLøn + (gennemsnitsLøn * 25 / 100);
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

    static void UdskrivMedarbejdereTætPåPension(List<MedarbejderDto> medarbejdere, int[,,] lønMatrix)
    {
        Console.WriteLine("Medarbejdere med 5 eller mindre år til pension");
        Console.WriteLine("----------------------------------------------");

        List<MedarbejderDto> filtreredeMedarbejdere = medarbejdere
            .Where(medarbejder => medarbejder.ÅrTilPension <= 5)
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
            int bonus = HentBonusFraMatrix(lønMatrix, medarbejder.Afdeling, medarbejder.Køn);

            Console.WriteLine(
                $"{medarbejder.Fornavn} {medarbejder.Efternavn} | Alder: {medarbejder.Alder} | " +
                $"År til pension: {medarbejder.ÅrTilPension} | " +
                $"Køn: {TekstFormattering.FormatérEnumNavn(medarbejder.Køn)} | " +
                $"Afdeling: {TekstFormattering.FormatérEnumNavn(medarbejder.Afdeling)} | " +
                $"Pension fratrædelse bonus: {bonus:N0} kr.");
        }
    }

    static int HentBonusFraMatrix(int[,,] lønMatrix, Afdeling afdeling, Køn køn)
    {
        return lønMatrix[(int)afdeling, (int)køn, (int)LønType.PensionFratrædelseBonus];
    }

    static void RegistrerNyMedarbejder(List<MedarbejderDto> medarbejdere)
    {
        Console.WriteLine("Registrer ny medarbejder");
        Console.WriteLine("------------------------");

        Console.Write("Fornavn: ");
        string fornavn = LæsPåkrævetTekst();

        Console.Write("Efternavn: ");
        string efternavn = LæsPåkrævetTekst();

        Console.Write("Fødselsdato (dd-MM-yyyy eller dd MM yyyy): ");
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

    static void VisLønMatrix(int[,,] lønMatrix)
    {
        Console.WriteLine("Løn matrix");
        Console.WriteLine("----------");

        foreach (Afdeling afdeling in Enum.GetValues<Afdeling>())
        {
            Console.WriteLine($"Afdeling: {TekstFormattering.FormatérEnumNavn(afdeling)}");

            foreach (Køn køn in Enum.GetValues<Køn>())
            {
                int gennemsnitsLøn = lønMatrix[(int)afdeling, (int)køn, (int)LønType.GennemsnitsLøn];
                int bonus = lønMatrix[(int)afdeling, (int)køn, (int)LønType.PensionFratrædelseBonus];

                Console.WriteLine(
                    $"  {TekstFormattering.FormatérEnumNavn(køn)} | Gennemsnitsløn: {gennemsnitsLøn:N0} kr. | Bonus: {bonus:N0} kr.");
            }

            Console.WriteLine();
        }
    }

    static void OpdaterLønMatrix(int[,,] lønMatrix)
    {
        Console.WriteLine("Opdater løn matrix");
        Console.WriteLine("-----------------");

        Afdeling afdeling = LæsEnumValg<Afdeling>("Vælg afdeling");
        Køn køn = LæsEnumValg<Køn>("Vælg køn");

        Console.Write("Indtast ny gennemsnitsløn: ");
        int nyLøn = LæsPositivtHeltal();

        SætLønOgBonus(lønMatrix, afdeling, køn, nyLøn);

        Console.WriteLine();
        Console.WriteLine("Lønmatrix er opdateret.");
        Console.WriteLine(
            $"{TekstFormattering.FormatérEnumNavn(afdeling)} - {TekstFormattering.FormatérEnumNavn(køn)}");
        Console.WriteLine($"Ny gennemsnitsløn: {nyLøn:N0} kr.");
        Console.WriteLine($"Ny bonus: {HentBonusFraMatrix(lønMatrix, afdeling, køn):N0} kr.");
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

    static int LæsPositivtHeltal()
    {
        while (true)
        {
            string? input = Console.ReadLine()?.Trim();

            if (!string.IsNullOrWhiteSpace(input))
            {
                input = input.Replace(".", "").Replace(",", "");

                if (int.TryParse(input, out int tal) && tal > 0)
                {
                    return tal;
                }
            }

            Console.Write("Ugyldigt tal. Prøv igen: ");
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

        foreach (MedarbejderDto medarbejder in medarbejdere
                     .OrderBy(medarbejder => medarbejder.Efternavn, StringComparer.Create(new CultureInfo("da-DK"), true))
                     .ThenBy(medarbejder => medarbejder.Fornavn, StringComparer.Create(new CultureInfo("da-DK"), true)))
        {
            Console.WriteLine(
                $"{medarbejder.Fornavn} {medarbejder.Efternavn} | Født: {medarbejder.Fødselsdato:dd-MM-yyyy} | " +
                $"Alder: {medarbejder.Alder} | År til pension: {medarbejder.ÅrTilPension} | " +
                $"Køn: {TekstFormattering.FormatérEnumNavn(medarbejder.Køn)} | " +
                $"Afdeling: {TekstFormattering.FormatérEnumNavn(medarbejder.Afdeling)}");
        }
    }
}
