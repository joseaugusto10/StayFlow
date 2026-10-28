using Konscious.Security.Cryptography;
using System.Security.Cryptography;
using System.Text;
using StayFlow.Domain.Security.PasswordHashing;

namespace StayFlow.Infrastructure.Security.PasswordHashing;

internal sealed class Argon2PasswordHasher : IPasswordHasher
{
	private const int DEGREE_OF_PARALLELISM = 1;
	private const int ITERATIONS = 2;
	private const int MEMORY_SIZE_KB = 19 * 1024;
	private const int SALT_SIZE = 16;
	private const int HASH_SIZE = 32;

	public string HashPassword(string password)
	{
		var salt = RandomNumberGenerator.GetBytes(SALT_SIZE);

		var hash = HashPassword(password, salt);

		var combinedBytes = new byte[SALT_SIZE + HASH_SIZE];
		salt.CopyTo(combinedBytes, 0);
		hash.CopyTo(combinedBytes, SALT_SIZE);

		return Convert.ToBase64String(combinedBytes);
	}

	public bool VerifyPassword(string password, string passwordHash)
	{
		byte[] combinedBytes;
		try
		{
			combinedBytes = Convert.FromBase64String(passwordHash);
		}
		catch (FormatException)
		{
			return false;
		}

		if (combinedBytes.Length != SALT_SIZE + HASH_SIZE)
			return false;

		var salt = combinedBytes[..SALT_SIZE];
		var hash = combinedBytes[SALT_SIZE..];

		var newHash = HashPassword(password, salt);

		return CryptographicOperations.FixedTimeEquals(newHash, hash);
	}

	private static byte[] HashPassword(string password, byte[] salt)
	{
		using var hashAlgorithm = new Argon2id(Encoding.UTF8.GetBytes(password))
		{
			DegreeOfParallelism = DEGREE_OF_PARALLELISM,
			Iterations = ITERATIONS,
			MemorySize = MEMORY_SIZE_KB,
			Salt = salt
		};

		return hashAlgorithm.GetBytes(HASH_SIZE);
	}
}
