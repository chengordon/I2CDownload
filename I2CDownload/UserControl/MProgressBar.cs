using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace I2CDownload
{
    public partial class MProgressBar : UserControl
    {

        public MProgressBar()
        {
            InitializeComponent();
        }

        public int Value
        {
            set
            {
                this.progressBar_seting.Value = value;
                Application.DoEvents();
            }
            get
            {
                return this.progressBar_seting.Value;
            }
        }

        public int Maximum
        {
            set
            {
                this.progressBar_seting.Maximum = value;
            }
            get
            {
                return this.progressBar_seting.Maximum;
            }
        }

        public int Minimum
        {
            set
            {
                this.progressBar_seting.Minimum = value;
            }
            get
            {
                return this.progressBar_seting.Minimum;
            }
        }
    }
}
