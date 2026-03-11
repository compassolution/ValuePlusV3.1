using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;

public partial class crypt : System.Web.UI.Page
{

    
    protected void Page_Load(object sender, EventArgs e)
    {

    }
    protected void Button1_Click(object sender, EventArgs e)
    {
        TextBox2.Text = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Encrypt3des(new byte[] {0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38
                , 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66
                , 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2}, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, TextBox1.Text, System.Text.Encoding.UTF8); 
    }
    protected void Button2_Click(object sender, EventArgs e)
    {
        TextBox2.Text =  Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Decrypt3des(new byte[] {0x11, 0x22, 0x5F, 0x68, (byte)0x88, 0x70, 0x40, 0x38
                , 0x28, 0x25, 0x79, 0x51, (byte)0xCB, (byte)0xDD, 0x55, 0x66
                , 0x77, 0x29, 0x74, (byte)0x98, 0x30, 0x40, 0x36, (byte)0xE2}, new byte[] { 0x12, 0x34, 0x56, 0x78, 0x90, 0xAB, 0xCD, 0xEF }, TextBox1.Text, System.Text.Encoding.UTF8); 
    }
    protected void Button3_Click(object sender, EventArgs e)
    {
        //11225F688870403828257951CBDD556677297498304036E2,1234567890ABCDEF
        TextBox2.Text = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Encrypt3des(TextBox3.Text, TextBox4.Text, TextBox1.Text, System.Text.Encoding.GetEncoding(encodingdrop.SelectedValue)); 
    }
    protected void Button4_Click(object sender, EventArgs e)
    {
        TextBox2.Text = Com.ValuePlus.Utils.Cryptography.CryptographyHelper.Decrypt3des(TextBox3.Text, TextBox4.Text, TextBox1.Text, System.Text.Encoding.GetEncoding(encodingdrop.SelectedValue)); 
    }
}
