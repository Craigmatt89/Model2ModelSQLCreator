using Microsoft.VisualBasic.Devices;
using static System.ComponentModel.Design.ObjectSelectorEditor;
using System.Drawing;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;
using System.Diagnostics;
using Microsoft.VisualBasic.Logging;

namespace Model2Model
{
    public partial class FormModel2Model : Form
    {
        public FormModel2Model()
        {
            InitializeComponent();
        }

        private void BtnCopySql_Click(object sender, EventArgs e)
        {
            Clipboard.Clear();
            Clipboard.SetText(@"
--MDLPRGE.SCP
UPDATE Assistants SET ModelID=0;
UPDATE Branches SET ModelID=0;
UPDATE Currencies SET ModelID=0;
UPDATE Departments SET ModelID=0;
UPDATE PMRAllergies SET ModelID=0;
UPDATE PMRCautions SET ModelID=0;
UPDATE PMRConditions SET ModelID=0;
UPDATE PMRDrugFamilies SET ModelID=0;
UPDATE ProductGroups SET ModelID=0;
UPDATE ProductMessages SET ModelID=0;
UPDATE Products SET ModelID=0;
UPDATE Promotions SET ModelID=0;
UPDATE Reports SET ModelID=0;
UPDATE Restrictions SET ModelID=0;
UPDATE Schemes SET ModelID=0;
UPDATE StockDumps SET ModelID=0;
UPDATE Suppliers SET ModelID=0;
UPDATE UserClasses SET ModelID=0;
UPDATE VATExemptions SET ModelID=0;
UPDATE VATRates SET ModelID=0;
DELETE FROM SystemParameters WHERE Keyword LIKE '%DFU%';
DELETE FROM SystemParameters WHERE Keyword LIKE 'Model%';

--IMSOFF.SCP
DELETE FROM SystemParameters WHERE Keyword = 'IMS\Enabled';
DELETE FROM SystemParameters WHERE Keyword = 'PMR\DocPromptMode';

--Remove Internet Manager Address
 UPDATE SystemParameters SET Value = '' WHERE Keyword = 'INetMgr\CliAddress'

--Updates Licensee Details
Update LicenseeDetails set Name = '" + TxtBoxBranchName.Text + @"'
Update LicenseeDetails set Address1 = '"+TxtBoxAddress1.Text + @"'
Update LicenseeDetails set Address2 = '"+txtBoxAddress2.Text + @"'
Update LicenseeDetails set Address3 = '"+txtBoxAddress3.Text + @"'
Update LicenseeDetails set Address4 = '"+txtBoxAddress4.Text + @"'
Update LicenseeDetails set Postcode = '"+txtBoxPostcode.Text + @"'
Update LicenseeDetails set VATNUMBER = ''
Update LicenseeDetails set Contact = ''
Update LicenseeDetails set Telephone = '"+txtBoxPhoneNum.Text + @"'
Update LicenseeDetails set Email = ''
Update LicenseeDetails set Reference = '"+txtboxWellAccount.Text + @"'
Update LicenseeDetails set Fax = ''
Update LicenseeDetails set Website = ''

--Setting for NACSCode
IF EXISTS(SELECT * FROM systemparameters WHERE Keyword = 'NACSCode')
BEGIN
    UPDATE systemparameters
    SET Value = '" + txtBoxODSCode.Text + @"'
    WHERE Keyword = 'NACSCode'
END
            ELSE
BEGIN
    INSERT INTO systemparameters(Keyword, Value)
    VALUES('NACSCode', '"+ txtBoxODSCode.Text + @"')
END

--Setting for SiteCode
IF EXISTS(SELECT * FROM systemparameters WHERE Keyword = 'SiteCode')
BEGIN
    UPDATE systemparameters
    SET Value = '" + txtboxWellAccount.Text + @"'
    WHERE Keyword = 'SiteCode'
END
            ELSE
BEGIN
    INSERT INTO systemparameters(Keyword, Value)
    VALUES('SiteCode', '"+ txtboxWellAccount.Text + @"')
END

--Setting for Endorser Line 1
IF EXISTS(SELECT * FROM systemparameters WHERE Keyword = 'Endorse\StampText1')
BEGIN
    UPDATE systemparameters
    SET Value = '" + txtboxEndorse1.Text + @"'
    WHERE Keyword = 'Endorse\StampText1'
END
            ELSE
BEGIN
    INSERT INTO systemparameters(Keyword, Value)
    VALUES('Endorse\StampText1', '"+ txtboxEndorse1.Text + @"')
END

--Setting for Endorser Line 2
IF EXISTS(SELECT * FROM systemparameters WHERE Keyword = 'Endorse\StampText2')
BEGIN
    UPDATE systemparameters
    SET Value = '" + txtboxEndorse2.Text + @"'
    WHERE Keyword = 'Endorse\StampText2'
END
ELSE
BEGIN
    INSERT INTO systemparameters(Keyword, Value)
    VALUES('Endorse\StampText2', '"+ txtboxEndorse2.Text + @"')
END

--Setting for Endorser Line 3
IF EXISTS(SELECT * FROM systemparameters WHERE Keyword = 'Endorse\StampText3')
BEGIN
    UPDATE systemparameters
    SET Value = '" + txtboxEndorse3.Text + @"'
    WHERE Keyword = 'Endorse\StampText3'
END
            ELSE
BEGIN
    INSERT INTO systemparameters(Keyword, Value)
    VALUES('Endorse\StampText3', '"+ txtboxEndorse3.Text + @"')
END

--Setting for PSL Data Exchange Number
IF EXISTS(SELECT * FROM systemparameters WHERE Keyword = 'PSL\AccountCode')
BEGIN
    UPDATE systemparameters
    SET Value = '" + txtboxPslDataNum.Text + @"'
    WHERE Keyword = 'PSL\AccountCode'
END
            ELSE
BEGIN
    INSERT INTO systemparameters(Keyword, Value)
    VALUES('PSL\AccountCode', '"+ txtboxPslDataNum.Text + @"')
END

--Setting for Head Office Branch Number
IF EXISTS(SELECT * FROM systemparameters WHERE Keyword = 'HQS\AccountCode')
BEGIN
    UPDATE systemparameters
    SET Value = '" + txtBoxHoBranchNum.Text + @"'
    WHERE Keyword = 'HQS\AccountCode'
END
            ELSE
BEGIN
    INSERT INTO systemparameters(Keyword, Value)
    VALUES('HQS\AccountCode', '" + txtBoxHoBranchNum.Text + @"')
END
");
            MessageBox.Show("SQL code has been copied to the clipboard.");

        }

        private void txtboxPslDataNum_TextChanged(object sender, EventArgs e)
        {
            // No More than 4 Digits
            if (txtboxPslDataNum.Text.Length > 4)
                txtboxPslDataNum.Text = txtboxPslDataNum.Text.Substring(0, 4);
        }

        private void txtBoxHoBranchNum_TextChanged(object sender, EventArgs e)
        {
            // No More than 4 Digits
            if (txtBoxHoBranchNum.Text.Length > 4)
                txtBoxHoBranchNum.Text = txtBoxHoBranchNum.Text.Substring(0, 4);
        }

        private void txtboxPSLAccount_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtboxPSLAccount.SelectionStart;
            int selectionLength = txtboxPSLAccount.SelectionLength;

            // Limit to 6 characters and UpperCase
            txtboxPSLAccount.Text = txtboxPSLAccount.Text.ToUpper().Substring(0, Math.Min(txtboxPSLAccount.Text.Length, 6));

            // Restore the cursor position and selection length - added as Text was in reverse
            txtboxPSLAccount.SelectionStart = Math.Min(selectionStart, txtboxPSLAccount.Text.Length);
            txtboxPSLAccount.SelectionLength = selectionLength;
        }

        private void txtboxWellAccount_TextChanged(object sender, EventArgs e)
        {
            // No More than 6 Digits
            if (txtboxWellAccount.Text.Length > 6)
                txtboxWellAccount.Text = txtboxWellAccount.Text.Substring(0, 6);
        }

        private void txtBoxODSCode_TextChanged(object sender, EventArgs e)
        {
            // To uppercase and limit to 5 characters
            string newText = txtBoxODSCode.Text.ToUpper().Substring(0, Math.Min(txtBoxODSCode.Text.Length, 5));
            txtBoxODSCode.Text = newText;

            // Set the cursor position to the end of the text
            txtBoxODSCode.SelectionStart = newText.Length;
        }

        private void btnRestoreDatabase_Click(object sender, EventArgs e)
        {
            try
            {

                //Closes all Analyst Programs by running Ash Kill *
                Process p1 = new Process();
                p1.StartInfo.FileName = "cmd.exe";
                p1.StartInfo.Arguments = "/c cd \\analyst && ash kill *";
                p1.Start();
                p1.WaitForExit();

                // Start Dbadmin.exe and restore the specified STX file
                Process p2 = new Process();
                p2.StartInfo.FileName = @"\Analyst\Dbadmin.exe";
                p2.StartInfo.Arguments = @"/Restore /STX:C:\Analyst\Data\Ntx\Database.STX";
                p2.Start();
                p2.WaitForExit();

            }
            catch (Exception ex)
            {
                // Handle any exceptions that occur
                Console.WriteLine("An error occurred: " + ex.Message);
            }
        }

        private void btnClearText_Click(object sender, EventArgs e)
        {
            TxtBoxBranchName.Clear();
            TxtBoxAddress1.Clear();
            txtBoxAddress2.Clear();
            txtBoxAddress3.Clear();
            txtBoxAddress4.Clear();
            txtboxEndorse1.Clear();
            txtboxEndorse2.Clear();
            txtboxEndorse3.Clear();
            txtBoxHoBranchNum.Clear();
            txtBoxODSCode.Clear();
            txtBoxPhoneNum.Clear();
            txtBoxPostcode.Clear();
            txtboxPSLAccount.Clear();
            txtboxWellAccount.Clear();
            txtboxPslDataNum.Clear();
        }

        private void TxtBoxBranchName_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = TxtBoxBranchName.SelectionStart;
            int selectionLength = TxtBoxBranchName.SelectionLength; 
            
            if (TxtBoxBranchName.Text.Length > 30)
                TxtBoxBranchName.Text = TxtBoxBranchName.Text.Substring(0, 30);

            // Restore the cursor position and selection length - added as Text was in reverse
            TxtBoxBranchName.SelectionStart = Math.Min(selectionStart, TxtBoxBranchName.Text.Length);
            TxtBoxBranchName.SelectionLength = selectionLength;

        }

        private void TxtBoxAddress1_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = TxtBoxAddress1.SelectionStart;
            int selectionLength = TxtBoxAddress1.SelectionLength;

            if (TxtBoxAddress1.Text.Length > 30)
                TxtBoxAddress1.Text = TxtBoxAddress1.Text.Substring(0, 30);

            // Restore the cursor position and selection length - added as Text was in reverse
            TxtBoxAddress1.SelectionStart = Math.Min(selectionStart, TxtBoxAddress1.Text.Length);
            TxtBoxAddress1.SelectionLength = selectionLength;
        }

        private void txtBoxAddress2_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtBoxAddress2.SelectionStart;
            int selectionLength = txtBoxAddress2.SelectionLength;

            if (txtBoxAddress2.Text.Length > 30)
                txtBoxAddress2.Text = txtBoxAddress2.Text.Substring(0, 30);

            // Restore the cursor position and selection length - added as Text was in reverse
            txtBoxAddress2.SelectionStart = Math.Min(selectionStart, txtBoxAddress2.Text.Length);
            txtBoxAddress2.SelectionLength = selectionLength;
        }

        private void txtBoxAddress3_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtBoxAddress3.SelectionStart;
            int selectionLength = txtBoxAddress3.SelectionLength;

            if (txtBoxAddress3.Text.Length > 30)
                txtBoxAddress3.Text = txtBoxAddress3.Text.Substring(0, 30);

            // Restore the cursor position and selection length - added as Text was in reverse
            txtBoxAddress3.SelectionStart = Math.Min(selectionStart, txtBoxAddress3.Text.Length);
            txtBoxAddress3.SelectionLength = selectionLength;
        }

        private void txtBoxAddress4_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtBoxAddress4.SelectionStart;
            int selectionLength = txtBoxAddress4.SelectionLength;

            if (txtBoxAddress4.Text.Length > 30)
                txtBoxAddress4.Text = txtBoxAddress4.Text.Substring(0, 30);

            // Restore the cursor position and selection length - added as Text was in reverse
            txtBoxAddress4.SelectionStart = Math.Min(selectionStart, txtBoxAddress4.Text.Length);
            txtBoxAddress4.SelectionLength = selectionLength;
        }

        private void txtBoxPostcode_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtBoxPostcode.SelectionStart;
            int selectionLength = txtBoxPostcode.SelectionLength;

            if (txtBoxPostcode.Text.Length > 10)
                txtBoxPostcode.Text = txtBoxPostcode.Text.Substring(0, 10);

            // Restore the cursor position and selection length - added as Text was in reverse
            txtBoxPostcode.SelectionStart = Math.Min(selectionStart, txtBoxPostcode.Text.Length);
            txtBoxPostcode.SelectionLength = selectionLength;
        }

        private void txtBoxPhoneNum_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtBoxPhoneNum.SelectionStart;
            int selectionLength = txtBoxPhoneNum.SelectionLength;

            if (txtBoxPhoneNum.Text.Length > 20)
                txtBoxPhoneNum.Text = txtBoxPhoneNum.Text.Substring(0, 20);

            // Restore the cursor position and selection length - added as Text was in reverse
            txtBoxPhoneNum.SelectionStart = Math.Min(selectionStart, txtBoxPhoneNum.Text.Length);
            txtBoxPhoneNum.SelectionLength = selectionLength;
        }

        private void txtboxEndorse1_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtboxEndorse1.SelectionStart;
            int selectionLength = txtboxEndorse1.SelectionLength;

            if (txtboxEndorse1.Text.Length > 30)
                txtboxEndorse1.Text = txtboxEndorse1.Text.Substring(0, 30);

            // Restore the cursor position and selection length - added as Text was in reverse
            txtboxEndorse1.SelectionStart = Math.Min(selectionStart, txtboxEndorse1.Text.Length);
            txtboxEndorse1.SelectionLength = selectionLength;
        }

        private void txtboxEndorse2_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtboxEndorse2.SelectionStart;
            int selectionLength = txtboxEndorse2.SelectionLength;

            if (txtboxEndorse2.Text.Length > 30)
                txtboxEndorse2.Text = txtboxEndorse2.Text.Substring(0, 30);

            // Restore the cursor position and selection length - added as Text was in reverse
            txtboxEndorse2.SelectionStart = Math.Min(selectionStart, txtboxEndorse2.Text.Length);
            txtboxEndorse2.SelectionLength = selectionLength;
        }

        private void txtboxEndorse3_TextChanged(object sender, EventArgs e)
        {
            // Store the current cursor position and selection length - added as Text was in reverse
            int selectionStart = txtboxEndorse3.SelectionStart;
            int selectionLength = txtboxEndorse3.SelectionLength;

            if (txtboxEndorse3.Text.Length > 30)
                txtboxEndorse3.Text = txtboxEndorse3.Text.Substring(0, 30);

            // Restore the cursor position and selection length - added as Text was in reverse
            txtboxEndorse3.SelectionStart = Math.Min(selectionStart, txtboxEndorse3.Text.Length);
            txtboxEndorse3.SelectionLength = selectionLength;
        }

        private void FormModel2Model_Load(object sender, EventArgs e)
        {

        }
    }
}