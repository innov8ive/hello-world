using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using iTextSharp.text.pdf;
using iTextSharp.text.pdf.parser;
using WebApplication1.Setu;
using WebApplication1BL;
using WebApplication1OM;

namespace WebApplication1
{
    public partial class WebForm1 : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            //string a = EncryptMobile("9967603698");
            //string b = DecryptMobile(a);
        }

        protected void btnCreateLink_Click(object sender, EventArgs e)
        {
            SetuHelper setuHelper = new SetuHelper();
            txtPlatformBillID.Text = setuHelper.GenerateDeepLink(Convert.ToInt32(txtAmount.Text), "TEST LTD", txtNote.Text);
        }

        protected void btnCheckStatus_Click(object sender, EventArgs e)
        {
            SetuHelper setuHelper = new SetuHelper();
            lblStatus.Text = setuHelper.CheckLinkStatus(txtPlatformBillID.Text);
        }

        protected void btnEncrypt_Click(object sender, EventArgs e)
        {
            txtDecrypt.Text = StringCipher.Encrypt(txtEncrypt.Text, "SALT");
        }

        protected void btnDecrypt_Click(object sender, EventArgs e)
        {
            lblDecryptedText.Text = StringCipher.Decrypt(txtDecrypt.Text, "SALT");
        }

        private string EncryptMobile(string mobileNo)
        {
            if (!string.IsNullOrEmpty(mobileNo) && mobileNo.Length==10)
            {
                string leftPart = mobileNo.Substring(0, 6);
                string rightPart = mobileNo.Substring(6, 4);

                StringBuilder encyptedLeftPart = new StringBuilder();
                char[] charArray = "ZAYBXCWDVEUFTGSHRIQJPKOLNM".ToCharArray();
                int counter = 0;
                foreach(char c in leftPart.ToCharArray())
                {
                    if (counter < 4)
                    {
                        encyptedLeftPart.Append(charArray[Convert.ToInt16(rightPart[counter].ToString()) + Convert.ToInt16(c.ToString())]);
                    }
                    else
                    {
                        encyptedLeftPart.Append(charArray[Convert.ToInt16(leftPart[counter - 4].ToString()) + Convert.ToInt16(c.ToString())]);
                    }
                    counter++;
                }
                return encyptedLeftPart.ToString() + rightPart;
            }
            return string.Empty;
        }

        private string DecryptMobile(string mobileNo)
        {
            if (!string.IsNullOrEmpty(mobileNo) && mobileNo.Length == 10)
            {
                string leftPart = mobileNo.Substring(0, 6);
                string rightPart = mobileNo.Substring(6, 4);

                StringBuilder encyptedLeftPart = new StringBuilder();
                List<char> charArray = "ZAYBXCWDVEUFTGSHRIQJPKOLNM".ToCharArray().ToList();
                int counter = 0;
                foreach (char c in leftPart.ToCharArray())
                {
                    if (counter < 4)
                    {
                        encyptedLeftPart.Append(charArray.IndexOf(c) - Convert.ToInt16(rightPart[counter].ToString()));
                    }
                    else
                    {
                        encyptedLeftPart.Append(charArray.IndexOf(c) - Convert.ToInt16(encyptedLeftPart[counter - 4].ToString()));
                    }
                    counter++;
                }
                return encyptedLeftPart.ToString() + rightPart;
            }
            return string.Empty;
        }

        protected void btnEncryptMobile_Click(object sender, EventArgs e)
        {
            txtMobileEncrypted.Text = EncryptMobile(txtMobile.Text);
        }

        protected void btnDecryptMobile_Click(object sender, EventArgs e)
        {
            lblDecryptedMobileText.Text = DecryptMobile(txtMobileEncrypted.Text);
        }

        protected void btnEmailEn_Click(object sender, EventArgs e)
        {
            string[] emailParts = txtEmail.Text.Split('@');
            txtEmailEncrypted.Text = StringChipherEmail.EncryptString(txtEmail.Text, emailParts[1]) + "@" + emailParts[1];
        }

        protected void btnEmailDec_Click(object sender, EventArgs e)
        {
            string[] emailParts = txtEmailEncrypted.Text.Split('@');
            lblDecryptedEmail.Text = StringChipherEmail.DecryptString(emailParts[0], emailParts[1]);
        }
    }
}