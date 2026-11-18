
int n = int.Parse(args[0]);

int s = int.Parse(args[1]);

while (true)
{
	Console.WriteLine("Please enter the string: ");
	if(Console.ReadLine() is string next)
	{
		Add(next);
	}
}


void Add(string str)
{
	Console.WriteLine(str + " added");
}