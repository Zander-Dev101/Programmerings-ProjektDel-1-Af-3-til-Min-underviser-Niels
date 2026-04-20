using System.Globalization;
using ProgrammeringsProjektDel1Af3;

var medarbejdere = TestdataFabrik.OpretTiMedarbejdere();
var recordService = new MedarbejderRecordTjeneste();

Console.OutputEncoding = System.Text.Encoding.UTF8;
Console.WriteLine("Medarbejder Pension Oversigt");
Console.WriteLine();

UdskrivMedarbejdere(medarbejdere, Sortering.Efternavn);
UdskrivStatistikMedArrays(medarbejdere);
UdskrivRecordLister(medarbejdere, recordService);

Console.WriteLine();
Console.WriteLine("Vælg sortering til næste visning: 1 = Efternavn, 2 = Fornavn");
var sortering = LæsSortering();

Console.WriteLine();
Console.WriteLine("Opret ny medarbejder:");
var nyMedarbejder = LæsNyMedarbejder();

if (medarbejdere.Contains(nyMedarbejder))
{
    Console.WriteLine();
    Console.WriteLine("Fejl: Medarbejderen findes allerede.");
}
else
{
    medarbejdere.Add(nyMedarbejder);
    Console.WriteLine();
    Console.WriteLine("Ny medarbejder er tilføjet.");
}

Console.WriteLine();
UdskrivMedarbejdere(medarbejdere, sortering);
UdskrivStatistikMedArrays(medarbejdere);
UdskrivRecordLister(medarbejdere, recordService);

return;

static MedarbejderDto LæsNyMedarbejder()
{
    Console.Write("Fornavn: ");
    var forNavn = LæsPåkrævetTekst();

    Console.Write("Efternavn: ");
    var efterNavn = LæsPåkrævetTekst();

    Console.Write("Fødselsdato (dd-MM-yyyy eller dd MM yyyy): ");
    var fødselsdato = LæsFødselsdato();

    Console.Write("Køn (M/F): ");
    var køn = LæsKøn();

    return new MedarbejderDto
    {
        Fornavn = forNavn,
        Efternavn = efterNavn,
        Fødselsdato = fødselsdato,
        Køn = køn
    };
}

static string LæsPåkrævetTekst()
{
    while (true)
    {
        var input = Console.ReadLine()?.Trim();
        if (!string.IsNullOrWhiteSpace(input))
        {
            return input;
        }

        Console.Write("Værdi må ikke være tom. Prøv igen: ");
    }
}

static DateOnly LæsFødselsdato()
{
    string[] formater = ["dd-MM-yyyy", "d-M-yyyy", "dd/MM/yyyy", "d/M/yyyy", "yyyy-MM-dd", "dd MM yyyy", "d M yyyy"];

    while (true)
    {
        var input = Console.ReadLine();
        if (DateOnly.TryParseExact(input, formater, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dato))
        {
            return dato;
        }

        Console.Write("Ugyldig dato. Prøv igen (dd-MM-yyyy eller dd MM yyyy): ");
    }
}

static Køn LæsKøn()
{
    while (true)
    {
        var input = Console.ReadLine()?.Trim().ToUpperInvariant();
        if (input == "M")
        {
            return Køn.M;
        }

        if (input == "F")
        {
            return Køn.F;
        }

        Console.Write("Ugyldigt køn. Skriv M eller F: ");
    }
}

static Sortering LæsSortering()
{
    while (true)
    {
        var input = Console.ReadLine()?.Trim();
        if (input == "1")
        {
            return Sortering.Efternavn;
        }

        if (input == "2")
        {
            return Sortering.Fornavn;
        }

        Console.Write("Ugyldigt valg. Vælg 1 eller 2: ");
    }
}

static void UdskrivMedarbejdere(List<MedarbejderDto> medarbejdere, Sortering sorteringEfter)
{
    List<MedarbejderDto> sorteret;

    if (sorteringEfter == Sortering.Fornavn)
    {
        sorteret = [.. medarbejdere.OrderBy(m => m.Fornavn, StringComparer.Create(new CultureInfo("da-DK"), true))
            .ThenBy(m => m.Efternavn, StringComparer.Create(new CultureInfo("da-DK"), true))
            .ThenBy(m => m.Fødselsdato)];
    }
    else
    {
        sorteret = [.. medarbejdere];
        sorteret.Sort();
    }

    var sorteringTekst = sorteringEfter == Sortering.Fornavn ? "fornavn" : "efternavn";
    Console.WriteLine($"Medarbejderliste sorteret efter {sorteringTekst}:");

    var dagsDato = DateOnly.FromDateTime(DateTime.Today);
    foreach (var medarbejder in sorteret)
    {
        var alder = PensionsHjælper.BeregnAlder(medarbejder.Fødselsdato, dagsDato);
        var årTilPension = PensionsHjælper.BeregnÅrTilPension(medarbejder.Fødselsdato, dagsDato);
        Console.WriteLine($"- {medarbejder.Fornavn} {medarbejder.Efternavn}, Alder: {alder}, År til pension: {årTilPension}, Køn: {(char)medarbejder.Køn}");
    }
}

static void UdskrivStatistikMedArrays(List<MedarbejderDto> medarbejdere)
{
    var dagsDato = DateOnly.FromDateTime(DateTime.Today);

    string[] labels = ["Antal", "Pension <= 5 år"];
    int[,] statistik = new int[2, labels.Length];

    foreach (var medarbejder in medarbejdere)
    {
        var række = medarbejder.Køn == Køn.F ? 0 : 1;
        statistik[række, 0]++;

        var årTilPension = PensionsHjælper.BeregnÅrTilPension(medarbejder.Fødselsdato, dagsDato);
        if (årTilPension <= 5)
        {
            statistik[række, 1]++;
        }
    }

    Console.WriteLine();
    Console.WriteLine("Statistik (arrays):");
    Console.WriteLine($"- Kvinder: {labels[0]} = {statistik[0, 0]}, {labels[1]} = {statistik[0, 1]}");
    Console.WriteLine($"- Mænd: {labels[0]} = {statistik[1, 0]}, {labels[1]} = {statistik[1, 1]}");
}

static void UdskrivRecordLister(List<MedarbejderDto> medarbejdere, MedarbejderRecordTjeneste recordService)
{
    Console.WriteLine();
    Console.WriteLine("Record-liste: Kvindelige medarbejdere");
    UdskrivRecords(recordService.OpretRecordsEfterKøn(medarbejdere, Køn.F));

    Console.WriteLine();
    Console.WriteLine("Record-liste: Mandlige medarbejdere");
    UdskrivRecords(recordService.OpretRecordsEfterKøn(medarbejdere, Køn.M));

    Console.WriteLine();
    Console.WriteLine("Record-liste: 5 år eller mindre til pension");
    UdskrivRecords(recordService.OpretRecordsMedFemEllerMindreÅrTilPension(medarbejdere));
}

static void UdskrivRecords(List<MedarbejderRecord> records)
{
    if (records.Count == 0)
    {
        Console.WriteLine("- Ingen medarbejdere fundet");
        return;
    }

    foreach (var record in records)
    {
        Console.WriteLine($"- {record.Fornavn} {record.Efternavn}, Alder: {record.Alder}, År til pension: {record.ÅrTilPension}, Køn: {(char)record.Køn}");
    }
}
