using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using PlateauMed.Infrastructure.Exceptions;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PlateauMed.Infrastructure.Utilities
{
    public static class Helpers
    {
        public static string HashPassword(string password)
        {
            byte[] salt = new byte[16];
            using (var rng = System.Security.Cryptography.RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            var hashed = KeyDerivation.Pbkdf2(
                password: password,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            );

            byte[] hashBytes = new byte[salt.Length + hashed.Length];
            Buffer.BlockCopy(salt, 0, hashBytes, 0, salt.Length);
            Buffer.BlockCopy(hashed, 0, hashBytes, salt.Length, hashed.Length);

            return Convert.ToBase64String(hashBytes);
        }

        
        public static bool VerifyPassword(string enteredPassword, string storedHash)
        {
            byte[] hashBytes = Convert.FromBase64String(storedHash);

            byte[] salt = new byte[16];
            Buffer.BlockCopy(hashBytes, 0, salt, 0, 16);

            
            byte[] storedPasswordHash = new byte[hashBytes.Length - 16];
            Buffer.BlockCopy(hashBytes, 16, storedPasswordHash, 0, storedPasswordHash.Length);

            var hashed = KeyDerivation.Pbkdf2(
                password: enteredPassword,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 10000,
                numBytesRequested: 32
            );

            return hashed.SequenceEqual(storedPasswordHash);
        }


        public static DateTime ConvertToDate(this string value)
        {
            bool result = DateTime.TryParseExact(value, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime convertedDate);
            if (!result)
            {
                throw new BadRequestException("Date is not in the correct format - dd/MM/yyyy");
            }
            return convertedDate;
        }
    }
}
