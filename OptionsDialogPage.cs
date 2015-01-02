using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.VisualStudio.Shell;

namespace DelegateAS.SmartResource
{
    internal class OptionsDialogPage : DialogPage
    {
        private Button resetSharePoint;

        public OptionsDialogPage()
        {
            SetSharePointDefaults();
        }

        private void SetSharePointDefaults()
        {
            //Set defaults
            XmlPattern = "$Resources:{0},{1};";
            CsPattern = "ResourceLookup.SPGetLocalizedString(\"$Resources:{0},{1}\")";
            VbPattern = "ResourceLookup.SPGetLocalizedString(\"$Resources:{0},{1}\")";
            AsxxPattern = "<%=ResourceLookup.SPGetLocalizedString(\"$Resources:{0},{1}\")%>";
        }

        /*
        public override void LoadSettingsFromStorage()
        {
            base.LoadSettingsFromStorage();

            //Hook a button in
            this.resetSharePoint = new System.Windows.Forms.Button();

            // 
            // resetSharePoint
            // 
            this.resetSharePoint.Location = new System.Drawing.Point(53, 134);
            this.resetSharePoint.Name = "resetSharePoint";
            this.resetSharePoint.Size = new System.Drawing.Size(162, 23);
            this.resetSharePoint.TabIndex = 10;
            this.resetSharePoint.Text = "Reset to SharePoint defaults";
            this.resetSharePoint.UseVisualStyleBackColor = true;
            this.resetSharePoint.Click += resetSharePoint_Click;

            ((System.Windows.Forms.PropertyGrid)Window).Controls.Add( this.resetSharePoint );

        }

        void resetSharePoint_Click(object sender, System.EventArgs e)
        {
            throw new System.NotImplementedException();
        }*/

        [LocDisplayName("General Replace Pattern")]
        [Description(
            "Pattern generally used to replace in files, i.e. XML, XSD, TXT etc. {0} is the resource file, {1} is the resource key, {2} is the resource file with \".\" replaced with \"_\"."
            )]
        [Category("Replace patterns")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string XmlPattern { get; set; }


        [LocDisplayName("C# Replace Pattern")]
        [Description(
            "Pattern used to replace in CS files. {0} is the resource file, {1} is the resource key, {2} is the resource file with \".\" replaced with \"_\".\r\nNOTE: Default .NET replace pattern would be \"{2}.{1}\""
            )]
        [Category("Replace patterns")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string CsPattern { get; set; }

        [LocDisplayName("VB Replace Pattern")]
        [Description(
            "Pattern used to replace in VB files. {0} is the resource file, {1} is the resource key, {2} is the resource file with \".\" replaced with \"_\".\r\nNOTE: Default .NET replace pattern would be \"{2}.{1}\""
            )]
        [Category("Replace patterns")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string VbPattern { get; set; }


        [LocDisplayName("as?x Replace Pattern")]
        [Description(
            "Pattern used to replace in as?x files. {0} is the resource file, {1} is the resource key, {2} is the resource file with \".\" replaced with \"_\".\r\nNOTE: Default .NET replace pattern would be \"<%={2}.{1}%>\""
            )]
        [Category("Replace patterns")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public string AsxxPattern { get; set; }

    }
}
