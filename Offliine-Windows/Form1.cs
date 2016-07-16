using System;
using System.Windows.Forms;
using Offliine.Injection;

namespace Offliine_Windows
{
    public partial class Form1 : Form
    {
        private readonly InjectionServer _injection = new InjectionServer();

        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _injection.Start();
        }

        protected override void DestroyHandle()
        {
            base.DestroyHandle();

            if (_injection.Running)
                _injection.Stop();
        }
    }
}
