namespace Ployd.Core.Services;

public class ResourceNameGenerator {
    private static readonly Random _random = new Random();

    private static readonly string[] _adjectives = new string[]
    {
        "Adaptable", "Adventurous", "Aggressive", "Ambitious", "Amiable",
        "Compassionate", "Considerate", "Courageous", "Diligent", "Dynamic",
        "Generous", "Loyal", "Persistent", "Responsible", "Sensible",
        "Sincere", "Sympathetic", "Tactful", "Trustworthy", "Warmhearted",
        "Witty", "Eager", "Easygoing", "Efficient", "Empathetic"
    };

    private static readonly string[] _nouns = new string[]
    {
        "Accountant", "Actor", "Air Traffic Controller", "Architect", "Artist",
        "Astronomer", "Attorney", "Baker", "Barber", "Bartender",
        "Biologist", "Bookkeeper", "Builder", "Bus Driver", "Butcher",
        "Carpenter", "Cashier", "Chef", "Chemist", "Civil Engineer",
        "Cleaner", "Coach", "Computer Programmer", "Consultant", "Counselor"
    };

    private static readonly string[] _names = new string[]
    {
        "Aiden", "Alexander", "Andrew", "Anthony", "Asher",
        "Benjamin", "Caleb", "Carter", "Christopher", "Daniel",
        "David", "Elijah", "Ethan", "Gabriel", "Henry",
        "Isaac", "Jack", "Jackson", "Jacob", "James",
        "John", "Joseph", "Joshua", "Liam", "Logan",
        "Lucas", "Mason", "Matthew", "Michael", "Nathan",
        "Noah", "Oliver", "Owen", "Samuel", "Sebastian",
        "Thomas", "William", "Wyatt", "Zachary"
    };


    public static string GenerateResourceName() {
        var adjective = _adjectives[_random.Next(_adjectives.Length)];
        var noun = _nouns[_random.Next(_nouns.Length)];
        var name = _names[_random.Next(_names.Length)];
        return $"{adjective}-{noun}-{name}";
    }
}