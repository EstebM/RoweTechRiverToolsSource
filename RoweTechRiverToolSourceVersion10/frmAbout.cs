//Copyright (C) 2002 Microsoft Corporation
//All rights reserved.
//THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF ANY KIND, EITHER 
//EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE IMPLIED WARRANTIES OF 
//MERCHANTIBILITY AND/OR FITNESS FOR A PARTICULAR PURPOSE.
//Requires the Trial or Release version of Visual Studio .NET Professional (or greater).

using System;
using System.Windows.Forms;

public class frmAbout: System.Windows.Forms.Form {

#region " Windows Form Designer generated code "

	public frmAbout () {

		

		//This call is required by the Windows Form Designer.

		InitializeComponent();

		//Add any initialization after the InitializeComponent() call

	}

	//Form overrides dispose to clean up the component list.

	protected override void Dispose(bool disposing) {

		if (disposing) {

			if (components != null) {

				components.Dispose();

			}

		}

		base.Dispose(disposing);

	}

	//Required by the Windows Form Designer

	private System.ComponentModel.IContainer components = null;

	//NOTE: The following procedure is required by the Windows Form Designer

	//It can be modified using the Windows Form Designer.  

	//Do not modify it using the code editor.

	private System.Windows.Forms.PictureBox pbIcon;

	private System.Windows.Forms.Label lblTitle;

	private System.Windows.Forms.Label lblVersion;

	private System.Windows.Forms.Label lblDescription;

	private System.Windows.Forms.Button cmdOK;

	private System.Windows.Forms.Label lblCopyright;

	private System.Windows.Forms.Label lblCodebase;

	private void InitializeComponent() {
        System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmAbout));
        this.pbIcon = new System.Windows.Forms.PictureBox();
        this.lblTitle = new System.Windows.Forms.Label();
        this.lblVersion = new System.Windows.Forms.Label();
        this.lblDescription = new System.Windows.Forms.Label();
        this.cmdOK = new System.Windows.Forms.Button();
        this.lblCopyright = new System.Windows.Forms.Label();
        this.lblCodebase = new System.Windows.Forms.Label();
        ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).BeginInit();
        this.SuspendLayout();
        // 
        // pbIcon
        // 
        this.pbIcon.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
        resources.ApplyResources(this.pbIcon, "pbIcon");
        this.pbIcon.Name = "pbIcon";
        this.pbIcon.TabStop = false;
        // 
        // lblTitle
        // 
        resources.ApplyResources(this.lblTitle, "lblTitle");
        this.lblTitle.Name = "lblTitle";
        // 
        // lblVersion
        // 
        resources.ApplyResources(this.lblVersion, "lblVersion");
        this.lblVersion.Name = "lblVersion";
        // 
        // lblDescription
        // 
        resources.ApplyResources(this.lblDescription, "lblDescription");
        this.lblDescription.Name = "lblDescription";
        // 
        // cmdOK
        // 
        this.cmdOK.DialogResult = System.Windows.Forms.DialogResult.OK;
        resources.ApplyResources(this.cmdOK, "cmdOK");
        this.cmdOK.Name = "cmdOK";
        // 
        // lblCopyright
        // 
        resources.ApplyResources(this.lblCopyright, "lblCopyright");
        this.lblCopyright.Name = "lblCopyright";
        // 
        // lblCodebase
        // 
        resources.ApplyResources(this.lblCodebase, "lblCodebase");
        this.lblCodebase.Name = "lblCodebase";
        // 
        // frmAbout
        // 
        this.AcceptButton = this.cmdOK;
        resources.ApplyResources(this, "$this");
        this.CancelButton = this.cmdOK;
        this.Controls.Add(this.lblCodebase);
        this.Controls.Add(this.lblCopyright);
        this.Controls.Add(this.cmdOK);
        this.Controls.Add(this.lblDescription);
        this.Controls.Add(this.lblVersion);
        this.Controls.Add(this.lblTitle);
        this.Controls.Add(this.pbIcon);
        this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
        this.MaximizeBox = false;
        this.MinimizeBox = false;
        this.Name = "frmAbout";
        this.ShowInTaskbar = false;
        this.Load += new System.EventHandler(this.frmAbout_Load);
        ((System.ComponentModel.ISupportInitialize)(this.pbIcon)).EndInit();
        this.ResumeLayout(false);

    }

#endregion

	// Note: Because this form is opened by frmMain using the ShowDialog command, we simply set the
	// DialogResult property of cmdOK to OK which causes the form to close when clicked.

	private void frmAbout_Load(object sender, System.EventArgs e) {

		try {

			// Set this Form's Text + Icon properties by using values from the parent form

			this.Text = "About " + this.Owner.Text;

			this.Icon = this.Owner.Icon;

			// Set this Form's Picture Box's image using the parent's icon 

			// However, we need to convert it to a Bitmap since the Picture Box Control

			// will not accept a raw Icon.

			this.pbIcon.Image = this.Owner.Icon.ToBitmap();

			// Set the labels identitying the Title, Version, and Description by

			// reading Assembly meta-data originally entered in the AssemblyInfo.cs file

			// using the AssemblyInfo class defined in the same file

			AssemblyInfo ainfo = new AssemblyInfo();

			this.lblTitle.Text = ainfo.Title;

			this.lblVersion.Text = string.Format("Version {0}", ainfo.Version);

			this.lblCopyright.Text = ainfo.Copyright;

			this.lblDescription.Text = ainfo.Description;

			this.lblCodebase.Text = ainfo.CodeBase;

		} catch(System.Exception exp) {

			// This catch will trap any unexpected error.

			MessageBox.Show(exp.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Stop);

		}

	}

}

