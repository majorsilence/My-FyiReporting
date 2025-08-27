using System;
using EncryptionProvider.String;
using NUnit.Framework;

namespace ReportTests.Windows
{
    [TestFixture()]
    public class StringEncryptionTests
    {
        [Test()]
        public void DecryptTest()
        {
            // Arrange
            // var pass = "Your+Secret+Static+Encryption+Key+Goes+Here=";
            var pass = "AAECAwQFBgcICQoLDA0ODw==";

            // var pass2 = "This is a test key";
            var pass2 = "CCECAwQFBgcICQoLDA0ODw==";
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
            Assert.That(originalString, Is.EqualTo(decryptedString1), "Decrypted string should match original string");
            Assert.That(originalString, Is.EqualTo(decryptedString2), "Decrypted string should match original string");
            Assert.That(originalString, Is.EqualTo(decryptedString3), "Decrypted string should match original string");
            Assert.That(originalString, Is.EqualTo(decryptedString4), "Decrypted string should match original string");


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

            Assert.That(originalString, Is.Not.EqualTo(encryptedString1), "Encrypted string should not match original string");
            Assert.That(encryptedString1, Is.Not.EqualTo(encryptedString2), "String should never be encrypted the same twice");
            Assert.That(originalString, Is.Not.EqualTo(encryptedString3), "Encrypted string should not match original string");
            Assert.That(encryptedString3, Is.Not.EqualTo(encryptedString4), "String should never be encrypted the same twice");
        }
    }
}

