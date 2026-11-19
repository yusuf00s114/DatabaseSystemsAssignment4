using Murmur;
using System.Text;

if (!File.Exists(GetPathOfFile("overflow_buckets.txt")))
{
	CreateFileInCurrentDirectory("overflow_buckets.txt");
}


int n = int.Parse(args[0]);
int s = int.Parse(args[1]);

var murmur32 = MurmurHash.Create32(0);

while (true)
{
	Console.Write("Please enter the string: ");
	if (Console.ReadLine() is string next)
	{
		Add(next);
	}
	else
	{
		break;
	}
}


void Add(string str)
{
	byte[] input = Encoding.UTF8.GetBytes(str);
	byte[] hash32 = murmur32.ComputeHash(input);
	uint hashInt = BitConverter.ToUInt32(hash32, 0);

	int bucketNumber = (int)(hashInt % n);
	if (bucketNumber == 0)
	{
		bucketNumber = n;
	}
	string bucketFilename = bucketNumber + ".txt";

	if (!File.Exists(GetPathOfFile(bucketFilename)))
	{
		CreateFileInCurrentDirectory(bucketFilename);
	}

	string lastOverflowBucketPath = GetLastOverflowBucketPath(bucketNumber);
	int numberOfCharacters = File.ReadAllText(GetPathOfFile(bucketFilename)).Length;
	string bucketToWriteTo = "";
	if (lastOverflowBucketPath != "" || numberOfCharacters + str.Length > s)
	{
		// initial bucket is full, need to add data to overflow bucket

		if (File.Exists(lastOverflowBucketPath))
		{
			if (File.ReadAllText(lastOverflowBucketPath).Length + str.Length > s)
			{
				// last overflow bucket is full, create new one, and set that as the file to
				// add data to
				UpdateLastOverflowBucket(bucketNumber);
				bucketToWriteTo = GetLastOverflowBucketPath(bucketNumber);
			}
			else
			{
				// last overflow bucket isn't full, set that as the file to add data to 
				bucketToWriteTo = lastOverflowBucketPath;
			}
		}
		else
		{
			UpdateLastOverflowBucket(bucketNumber);
			bucketToWriteTo = GetLastOverflowBucketPath(bucketNumber);
		}


	}
	else
	{
		bucketToWriteTo = GetPathOfFile(bucketFilename);
	}


	WriteToFile(bucketToWriteTo, str);

	Console.WriteLine(str + " added to " + Path.GetFileName(bucketToWriteTo));
}

// overflow buckets are stored in overflow_buckets.txt in the following format:
// bucketNumber: lastOverflowBucketNumber
// Overflow buckets are name as follows: bucketNumber_overflow_overflowBucketNumber.txt
// Example, for bucket number 2 which has 5 overflow buckets:
// entry in overflow_buckets.txt: 2:5
// overflow bucket names: 2_overflow_1.txt, 2_overflow_2.txt, 2_overflow_3.txt, 2_overflow_4.txt, 2_overflow_5.txt
// if a bucket has no overflow buckets, it will not have an entry in overflow_buckets.txt
string GetLastOverflowBucketPath(int bucketNumber)
{
	string path = "";

	foreach (string line in File.ReadAllLines(GetPathOfFile("overflow_buckets.txt")))
	{
		string[] parts = line.Split(':');
		if (parts.Length == 2 && int.Parse(parts[0].Trim()) == bucketNumber)
		{
			int lastOverflowBucketNumber = int.Parse(parts[1].Trim());
			path = GetPathOfFile($"{bucketNumber}_overflow_{lastOverflowBucketNumber}.txt");
			break;
		}
	}

	return path;
}

// creates a new overflow bucket and updates the reference to it on overflow_buckets.txt
void UpdateLastOverflowBucket(int bucketNumber)
{
	string overflowBucketsPath = GetPathOfFile("overflow_buckets.txt");
	List<string> lines = new(File.ReadAllLines(overflowBucketsPath));

	int newLastOverflowBucketNumber = 1;
	bool found = false;

	for (int i = 0; i < lines.Count; i++)
	{
		string[] parts = lines[i].Split(':');
		if (parts.Length == 2 && int.Parse(parts[0].Trim()) == bucketNumber)
		{
			newLastOverflowBucketNumber = int.Parse(parts[1].Trim()) + 1;
			lines[i] = $"{bucketNumber}:{newLastOverflowBucketNumber}";
			found = true;
			break;
		}
	}

	if (!found)
	{
		lines.Add($"{bucketNumber}:1");
	}

	File.WriteAllLines(overflowBucketsPath, lines);
	CreateFileInCurrentDirectory($"{bucketNumber}_overflow_{newLastOverflowBucketNumber}.txt");
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