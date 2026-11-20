# Build Instructions

Requires .NET 10.0 SDK or later.

Run these commands from the project directory. 
The executable is created in the current directory.

For Windows:
```bash
dotnet publish -c Release -r win-x64 -o .
```
This produces `hash_app.exe`. Use `win-arm64` for ARM-based Windows machines.

For Mac:
```bash
# Apple Silicon (M1 and later)
dotnet publish -c Release -r osx-arm64 -o .

# Intel
dotnet publish -c Release -r osx-x64 -o .
```
This produces `hash_app`.

For Linux:
```bash
dotnet publish -c Release -r linux-x64 -o .
```
This produces `hash_app`. Use `linux-arm64` for ARM machines.

# Running Instructions

For Windows:
```bash
# PowerShell
.\hash_app n s

# Command Prompt
hash_app.exe n s
```
For Mac and Linux:
```bash
./hash_app n s
```

# Hashing Algorithm/Library
The hashing algorithm I used is MurmurHash3. 
The library I used is [murmurhash](https://www.nuget.org/packages/murmurhash/). 

# Overflow Bucket Reference Method
Overflow buckets are stored in `overflow_buckets.txt` in the following format: <br>
`bucketNumber: lastOverflowBucketNumber` <br>
Overflow bucket files are named as follows: `bucketNumber_overflow_overflowBucketNumber.txt`.
For example, `3_overflow_2.txt` .<br>
Example, for bucket number 2 which has 5 overflow buckets: <br>
Entry in overflow_buckets.txt: `2:5` <br>
Overflow bucket names: `2_overflow_1.txt`, `2_overflow_2.txt`, `2_overflow_3.txt`,
`2_overflow_4.txt`, `2_overflow_5.txt` <br>
If a bucket has no overflow buckets, it will not have an entry in `overflow_buckets.txt`