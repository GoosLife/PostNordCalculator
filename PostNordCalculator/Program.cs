double length = -1;
double width = -1;
double height = -1;

byte typeChoice;
bool isParcel; // If this is true, the user is sending a parcel, otherwise a letter.
string specificType = "";

double weight;
byte weightClass = 0;

string input;

int[] prices = new int[6];

bool isChoosingSize = true;

string destination = "Danmark";

while (isChoosingSize == true)
{
    #region Type

    Console.WriteLine("Send post som: \n\n" +
        "Danmark: \n" +
        "    1. Brev\n" +
        "    2. Quickbrev\n" +
        "    3. Online Postpakke Collect\n" +
        "    4. Postpakke Home\n" +
        "Skandinavien:\n" +
        "    5. Brev Udland\n" +
        "    6. Postpakke Udland");

    do
    {
        input = Console.ReadLine();
    } while (!byte.TryParse(input, out typeChoice) && typeChoice > 6); // typeChoice must be parsed as a byte, and its value cannot exceed 6.

    specificType = GetType(); // Get the type of post - parcel or letter + specific type

    #endregion

    #region Dimensions

    // Get dimensions from user input
    Console.WriteLine("Indtast bredde på post i cm: ");

    do
    {
        input = Console.ReadLine();
    } while (!double.TryParse(input, out width));

    Console.WriteLine("Indtast højde på post i cm: ");

    do
    {
        input = Console.ReadLine();
    } while (!double.TryParse(input, out height));

    Console.WriteLine("Indtast længde på post i cm: ");

    do
    {
        input = Console.ReadLine();
    } while (!double.TryParse(input, out length));

    // Determine whether size is acceptable
    if (IsAcceptableSize() == false)
    {
        Console.Clear();
        Console.WriteLine("Din forsendelse er for stor.");
    }
    else
    {
        isChoosingSize = false;
    }

    #endregion
}

#region Weight

Console.WriteLine("Indtast vægt på post i gram: ");

do
{
    input = Console.ReadLine();
} while (!double.TryParse(input, out weight));

// Get weight class
if (weight <= 50)
{
    weightClass = 0;
}
else if (weight > 50 && weight <= 100)
{
    weightClass = 1;
}
else if (weight > 100 && weight <= 250)
{
    weightClass = 2;
}
else if (weight > 250 && weight <= 500)
{
    weightClass = 3;
}
else if (weight > 500 && weight <= 1000)
{
    weightClass = 4;
}
else if (weight > 1000 && weight <= 2000)
{
    weightClass = 5;
}
else // Letter/parcel weighed too much
{
    Console.WriteLine("Dette program egner sig kun til pakker mellem 50g og 2kg.");
}

if (specificType == "brev udland" || specificType == "postpakke udland") // Get postage prices for scandinavian mail
{
    destination = "Skandinavien";
    prices = GetInternationalPrice();
}

#endregion

#region Pricing

Console.Clear();
Console.WriteLine($"For post af typen '{specificType}' til levering i {destination} med målene {width}x{height}x{length} og en vægt på {weight} gram skal du bruge:\n" +
    $"  {prices[weightClass]} DKK i porto.");

#endregion

string GetType()
{
    switch (typeChoice)
    {
        case 1:
            prices = new int[6] { 12, 24, 48, 60, 60, 60 };
            isParcel = false;
            return "brev";
        case 2:
            prices = new int[6] { 29, 29, 58, 87, 87, 87 };
            isParcel = false;
            return "quickbrev";
        case 3:
            prices = new int[6] { 50, 50, 50, 50, 50, 50 };
            isParcel = true;
            return "online postpakke collect";
        case 4:
            prices = new int[6] { 65, 65, 65, 65, 65, 65 };
            isParcel = true;
            return "postpakke home";
        case 5:
            isParcel = false;
            return "brev udland";
        case 6:
            isParcel = true;
            return "postpakke udland";
        default:
            throw new Exception("Post type does not exist");
    }
}


int[] GetInternationalPrice()
{
    if (specificType == "brev udland")
    {
        return new int[6] { 36, 36, 72, 96, 96, 96 };
    }
    else if (specificType == "postpakke udland")
    {
        return new int[6] { 190, 190, 190, 190, 190, 275 };
    }
    else
    {
        throw new Exception("specificType doesn't denote international post; cannot get international prices");
    }
}

bool IsAcceptableSize()
{
    if (width >= 14 && height >= 9) // Minimum size for all post is 14x9cm
    {
        // If it's a parcel
        if (isParcel)
        {
            if (length <= 150 && // Length mustn't exceed 150cm
                (length + (2 * width + 2 * height + 2 * length) <= 300)) // Length + surface area mustn't exceed 300cm
            {
                return true; // Parcel is acceptable size
            }
            Console.WriteLine("Din post er for stor.\nMaksimal længde: 150cm.\nMaksimal længde + overflade: 300cm.");
            return false; // Parcel is too large.
        }
        // If it's a letter
        else 
        { 
            if (length < 60 // Length mustn't exceed 60cm
                && length + height + width <= 90) // Length + height + width mustn't exceed 90cm
            {
                return true; // Letter is acceptable size
            }
            Console.WriteLine("Din post er for stor.\nMaksimal længde: 60cm.\nMaksimal længde + højde + bredde: 90cm.");
            return false; // Letter is too large.
        }
    }
    Console.WriteLine("Din post er for lille.\nMinimum bredde: 14cm.\nMinimum højde: 9cm.");
    return false; // Post is too small.
}