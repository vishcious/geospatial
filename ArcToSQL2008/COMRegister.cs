using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration.Install;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Runtime.InteropServices;


namespace ArcToSQL2008
{
    [RunInstaller( true )]
    public partial class COMRegister : Installer
    {
        public COMRegister()
        {
            InitializeComponent();
        }

        public override void Install( System.Collections.IDictionary stateSaver )
        {
            base.Install( stateSaver );
            RegistrationServices regSrv = new RegistrationServices();
            regSrv.RegisterAssembly( base.GetType().Assembly,
              AssemblyRegistrationFlags.SetCodeBase );
        }

        public override void Uninstall( System.Collections.IDictionary savedState )
        {
            base.Uninstall( savedState );
            RegistrationServices regSrv = new RegistrationServices();
            regSrv.UnregisterAssembly( base.GetType().Assembly );
        }
    }
}
