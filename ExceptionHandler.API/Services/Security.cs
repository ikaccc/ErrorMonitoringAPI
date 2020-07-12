using ExceptionHandler.API.Intefaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Threading.Tasks;
using ExceptionHandler.API.Models;

namespace ExceptionHandler.API.Services
{
    public class Security : ISecurity
    {
        private readonly IDataManager _dataManager;

        public RegistredApplications User { get; set; }

        public Security(IDataManager dataManager)
        {
            _dataManager = dataManager;
        }
        public void Encript(string val)
        {
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {
                byte[] encrypted = EncryptStringToBytes(val, myRijndael.Key, myRijndael.IV);

                var encriptedPass = "";
                foreach (var item in myRijndael.Key)//32
                {
                    encriptedPass += item.ToString().Length.ToString() + item.ToString();
                }
                encriptedPass += ",";
                foreach (var item in myRijndael.IV) //16
                {
                    encriptedPass += item.ToString().Length.ToString() + item.ToString();
                }
                foreach (var item in encrypted) //16
                {
                    encriptedPass += item.ToString().Length.ToString() + item.ToString();
                }
                //_db.InsertEncriptedPassword(encriptedPass);
            }
        }

        public static void Decript(string accessToken)
        {
            //izvadi go od baza
            //var key =
        }
        private static byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }

            // Return the encrypted bytes from the memory stream.
            return encrypted;
        }

        public async Task<string> DecriptDbPassword(string dbConfigDbPassword)
        {
            var keyIVPass = dbConfigDbPassword.Split(',');

            var key = GetKeyOrIV(keyIVPass[0]);
            var iv = GetKeyOrIV(keyIVPass[1]);
            var encriptedValye = GetKeyOrIV(keyIVPass[2]);

            string decriptedvalue = await DecryptStringFromBytes(encriptedValye.ToArray(), key.ToArray(), iv.ToArray());
            return decriptedvalue;
        }


        private async Task<string> DecryptStringFromBytes(byte[] cipherText, byte[] Key, byte[] IV)
        {
            if (cipherText == null || cipherText.Length <= 0)
                throw new ArgumentNullException("cipherText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");

            // Declare the string used to hold
            // the decrypted text.
            string plaintext = null;

            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create a decryptor to perform the stream transform.
                ICryptoTransform decryptor = rijAlg.CreateDecryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for decryption.
                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                            // Read the decrypted bytes from the decrypting stream
                            // and place them in a string.
                            plaintext = await srDecrypt.ReadToEndAsync();
                        }
                    }
                }

            }

            return plaintext;
        }


        private static List<byte> GetKeyOrIV(string inputVal)
        {
            var passHash = "";
            var next = Convert.ToInt32(inputVal[0].ToString());
            var key = new List<byte>();
            var count = 0;
            var countAll = 0;
            foreach (var item in inputVal)
            {
                if (next + 1 == count)
                {
                    if (count != 0)
                    {
                        key.Add(Convert.ToByte(Convert.ToInt32(passHash)));
                    }
                    next = Convert.ToInt32(item.ToString());
                    count = 0;
                    passHash = "";
                }
                else
                {
                    if (count != 0)
                        passHash += item;
                    if (countAll == inputVal.Length - 1)
                    {
                        key.Add(Convert.ToByte(Convert.ToInt32(passHash)));
                    }
                }
                count++;
                countAll++;
            }
            return key;
        }

        public async Task<bool> CheckTokenAndUser(string dataAccessToken)
        {
            var user = await _dataManager.GetRegistredApplication(dataAccessToken);

            if (user == null) return false;
            if (user.IsTrial)
            {
                var duration = user.DateRegistred.AddMinutes(user.TrialDuration);
                if (DateTime.Now > duration)
                {
                    return false;
                }
            }
            return true;
        }

        //public async Task<bool> ValidateWebUser(Guid userId)
        //{
        //    var user = await _dataManager.GetWebUser(userId);

        //    if (user == null) return false;
        //    if (user.IsTrial)
        //    {
        //        var duration = user.DateRegistred.AddMinutes(user.TrialDuration);
        //        if (DateTime.Now > duration)
        //        {
        //            return false;
        //        }
        //    }
        //    return true;
        //}
    }
}
