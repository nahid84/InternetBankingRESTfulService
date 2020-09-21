using Microsoft.AspNetCore.Mvc;
using System;
using System.Text;
using System.Text.RegularExpressions;

namespace InternetBankingRESTfulService.Api
{
    [ApiController]
    [Route("bank")]
    public class InternetBankingRESTfulService : ControllerBase, IInternetBankingApi
    {
        private const int API_MAJOR_VERSION = 1;
        private const int API_MINOR_VERSION = 0;
        private const string REGEX_PATTERN = @"^(((?=.*[a-z])(?=.*[A-Z]))|((?=.*[a-z])(?=.*[0-9]))|((?=.*[A-Z])(?=.*[0-9])))(?=.{6,})";

        [HttpGet("api/version")]
        [HttpGet("api-version")]
        public IActionResult IB_GetApiVersion()
        {
            return Ok(GetApiVersion());
        }

        [HttpGet("api/calc/MD5/{value}")]
        [HttpGet("api/calc/{value}/MD5")]
        public IActionResult IB_CalculateMD5(string value)
        {
            if (string.IsNullOrEmpty(value))
                return BadRequest();

            return Ok(CalculateMD5(value));
        }

        [HttpGet("api/password/strong/{password}")]
        [HttpGet("api/is-password-strong/{password}")]
        public IActionResult IB_IsPasswordStrong(string password)
        {
            if (string.IsNullOrEmpty(password))
                return BadRequest();

            return Ok(IsPasswordStrong(password));
        }

        public bool IsPasswordStrong(string password)
        {
            var regEx = new Regex(REGEX_PATTERN);
            return regEx.IsMatch(password);
        }

        public string GetApiVersion()
        {
            return $"{FormattedDate}.{API_MAJOR_VERSION}.{API_MINOR_VERSION}";
        }

        public string CalculateMD5(string value)
        {
            return ComputeMD5(value);
        }

        private string FormattedDate => DateTime.Now.ToUniversalTime().ToString("yyyy.MM.dd");

        public static string ComputeMD5(string value)
        {
            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] inputBytes = Encoding.ASCII.GetBytes(value);
                byte[] hashBytes = md5.ComputeHash(inputBytes);

                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hashBytes.Length; i++)
                {
                    sb.Append(hashBytes[i].ToString("X2"));
                }
                return sb.ToString();
            }
        }
    }
}
