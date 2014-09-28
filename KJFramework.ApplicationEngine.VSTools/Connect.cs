using System;
using System.Text;
using System.Windows.Forms;
using Extensibility;
using EnvDTE;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;
using Microsoft.VisualStudio.Shell;

using IServiceProvider = Microsoft.VisualStudio.OLE.Interop.IServiceProvider;

namespace KJFramework.ApplicationEngine.VSTools
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2
	{
		/// <summary>Implements the constructor for the Add-in object. Place your initialization code within this method.</summary>
		public Connect()
		{
            
		}

		/// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
		/// <param term='application'>Root object of the host application.</param>
		/// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
		/// <param term='addInInst'>Object representing this Add-in.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
		{
		    MessageBox.Show("Debugging a Simple Add-in");
			_applicationObject = (DTE2)application;
			_addInInstance = (AddIn)addInInst;
            //solution loaded event.
            var serviceProvider = new ServiceProvider((IServiceProvider)_applicationObject);
            SolutionEventsListener solutionEventsListener = new SolutionEventsListener(serviceProvider);
            solutionEventsListener.AfterSolutionLoaded += SolutionEvents_Opened;
            #region 创建添加引用的上下文菜单项

            CommandBarControl control = (CommandBarControl)((CommandBars)_applicationObject.CommandBars)["Context Menus"].Controls["Project and Solution Context Menus"].Control;
            CommandBarPopup commandBarPopup = (CommandBarPopup)control.Control;
            CommandBarControl commandBarControl = commandBarPopup.Controls["Project"];
            CommandBarPopup cp = (CommandBarPopup)commandBarControl.Control;
            //"References"
            CommandBarControl newC = cp.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            newC.Enabled = true;
            newC.Caption = "P&ackage as your KPP";
            newC.Visible = true;
            CommandBarButton cmb = (CommandBarButton)newC.Control;
            cmb.Click += cmb_Click;
            //cmb.Click += AddReferenceButtonClick;

            #endregion
		}

        void SolutionEvents_Opened()
        {
            StringBuilder builder = new StringBuilder();
            foreach (Project project in _applicationObject.Solution.Projects)
            {
                //virtual folder.
                if (project.Kind != "{66A26720-8FB5-11D2-AA7E-00C04F688DDE}")
                    builder.AppendLine(project.Name);
            }
            MessageBox.Show(builder.ToString());
        }

        void cmb_Click(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            MessageBox.Show("sdfsfsdf");
            MessageBox.Show(((Project)((object[])_applicationObject.DTE.ActiveSolutionProjects)[0]).FullName);
            _applicationObject.Solution.SolutionBuild.Build(false);
        }

		/// <summary>Implements the OnDisconnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being unloaded.</summary>
		/// <param term='disconnectMode'>Describes how the Add-in is being unloaded.</param>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnDisconnection(ext_DisconnectMode disconnectMode, ref Array custom)
		{
		}

		/// <summary>Implements the OnAddInsUpdate method of the IDTExtensibility2 interface. Receives notification when the collection of Add-ins has changed.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />		
		public void OnAddInsUpdate(ref Array custom)
		{
		}

		/// <summary>Implements the OnStartupComplete method of the IDTExtensibility2 interface. Receives notification that the host application has completed loading.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnStartupComplete(ref Array custom)
		{
		}

		/// <summary>Implements the OnBeginShutdown method of the IDTExtensibility2 interface. Receives notification that the host application is being unloaded.</summary>
		/// <param term='custom'>Array of parameters that are host application specific.</param>
		/// <seealso class='IDTExtensibility2' />
		public void OnBeginShutdown(ref Array custom)
		{
		}
		
		private DTE2 _applicationObject;
		private AddIn _addInInstance;
	}
}