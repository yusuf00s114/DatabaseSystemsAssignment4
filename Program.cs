
using Murmur;
using System.Text;

int n = int.Parse(args[0]);

int s = int.Parse(args[1]);

while (true)
{
	Console.Write("Please enter the string: ");
	if(Console.ReadLine() is string next)
	{
		Add(next);
	}
}


void Add(string str)
{
	Console.WriteLine(str + " added");

	byte[] input = Encoding.UTF8.GetBytes(str);
	var murmur32 = MurmurHash.Create32(0);
	byte[] hash32 = murmur32.ComputeHash(input);
	uint hashInt = BitConverter.ToUInt32(hash32, 0);

	Console.WriteLine("MurmurHash3 32-bit: " + hashInt);
}


void CreateFileInCurrentDirectory(string fileName)
{
	string path = Path.Combine(Directory.GetCurrentDirectory(), fileName);

	using (File.Create(path)) { }
}

string GetPathOfFile(string fileName)
{
	return Path.Combine(Directory.GetCurrentDirectory(), fileName);
}

void WriteToFile(string path, string data)
{
	File.AppendAllText(path, data);
}