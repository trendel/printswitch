using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Printing;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace PrintSwitch
{
    public partial class MainForm : Form
    {
        // API!
        [DllImport("winspool.drv", CharSet=CharSet.Auto, SetLastError=true)]
        public static extern bool SetDefaultPrinter(string name);

        public MainForm()
        {
            InitializeComponent();

            // I should probably find a better icon...
            //this.notifyIcon1.Icon = SystemIcons.WinLogo;
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            // Hide the window
            this.WindowState = FormWindowState.Minimized;

            // Get a list of printers and sort it
            ArrayList printers = new ArrayList(PrinterSettings.InstalledPrinters);
            printers.Sort();

            // Add the printers to the menu
            PrintDocument pd = new PrintDocument();
            foreach (string printerName in printers)
            {
                ToolStripMenuItem item = new ToolStripMenuItem(printerName);
                printersToolStripMenuItem.DropDownItems.Add(item);

                // If it's the default printer, check it
                if (printerName == pd.PrinterSettings.PrinterName)
                {
                    item.Checked = true;
                }

                // Add an event handler for the item
                item.Click += somePrinterToolStripMenuItem_Click;
            }
        }

        private void MainForm_Resize(object sender, EventArgs e)
        {
            // Hide the window when minimized
            if (this.WindowState == FormWindowState.Minimized)
            {
                this.Hide();
            }
        }

        private void notifyIcon1_DoubleClick(object sender, EventArgs e)
        {
            // Do nothing

            //this.Show();
            //this.WindowState = FormWindowState.Normal;
        }

        private void somePrinterToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ToolStripMenuItem item = (ToolStripMenuItem)sender;

            // If they set the existing default, do nothing
            if (!item.Checked)
            {
                // First, go through and uncheck everything
                foreach (ToolStripMenuItem current in printersToolStripMenuItem.DropDownItems)
                {
                    current.Checked = false;
                }

                // Set the selected item to the default printer
                SetDefaultPrinter(item.Text);

                // Check the item
                item.Checked = true;
            }
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Bye!
            this.Close();
        }
    }
}
