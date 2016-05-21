using System;
using EncryptionProvider.String;
using NUnit.Framework;

namespace ReportTests
{
    [TestFixture()]
    public class StringEncryptionTests
    {
        [Test()]
        public void DecryptTest()
        {
            // Arrange
            var pass = "Your+Secret+Static+Encryption+Key+Goes+Here=";
            var pass2 = "This is a test key";
            var subject = new StringEncryption(pass);
            var subject2 = new StringEncryption(pass2);
            var originalString = "Testing123!£$";

            // Act
            var encryptedString1 = subject.Encrypt(originalString);
            var encryptedString2 = subject.Encrypt(originalString);
            var decryptedString1 = subject.Decrypt(encryptedString1);
            var decryptedString2 = subject.Decrypt(encryptedString2);

            var encryptedString3 = subject2.Encrypt(originalString);
            var encryptedString4 = subject2.Encrypt(originalString);
            var decryptedString3 = subject2.Decrypt(encryptedString3);
            var decryptedString4 = subject2.Decrypt(encryptedString4);

            // Assert
            Assert.AreEqual(originalString, decryptedString1, "Decrypted string should match original string");
            Assert.AreEqual(originalString, decryptedString2, "Decrypted string should match original string");
            Assert.AreEqual(originalString, decryptedString3, "Decrypted string should match original string");
            Assert.AreEqual(originalString, decryptedString4, "Decrypted string should match original string");


        }

        [Test()]
        public void EncryptTest()
        {
            var pass = "Your+Secret+Static+Encryption+Key+Goes+Here=";
            var pass2 = "This is a test key";
            var subject = new StringEncryption(pass);
            var subject2 = new StringEncryption(pass2);
            var originalString = "Testing123!£$";

            // Act
            var encryptedString1 = subject.Encrypt(originalString);
            var encryptedString2 = subject.Encrypt(originalString);
            var encryptedString3 = subject.Encrypt(originalString);
            var encryptedString4 = subject.Encrypt(originalString);

            Assert.AreNotEqual(originalString, encryptedString1, "Encrypted string should not match original string");
            Assert.AreNotEqual(encryptedString1, encryptedString2, "String should never be encrypted the same twice");
            Assert.AreNotEqual(originalString, encryptedString3, "Encrypted string should not match original string");
            Assert.AreNotEqual(encryptedString3, encryptedString4, "String should never be encrypted the same twice");
        }
    }
}

