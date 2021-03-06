using System;
using EnvDTE;
using Extensibility;
using EnvDTE80;
using Microsoft.VisualStudio.CommandBars;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin
{
	/// <summary>The object for implementing an Add-in.</summary>
	/// <seealso class='IDTExtensibility2' />
	public class Connect : IDTExtensibility2
    {
        #region Methods

        /// <summary>Implements the OnConnection method of the IDTExtensibility2 interface. Receives notification that the Add-in is being loaded.</summary>
        /// <param term='application'>Root object of the host application.</param>
        /// <param term='connectMode'>Describes how the Add-in is being loaded.</param>
        /// <param term='addInInst'>Object representing this Add-in.</param>
        /// <seealso class='IDTExtensibility2' />
        public void OnConnection(object application, ext_ConnectMode connectMode, object addInInst, ref Array custom)
        {
            _applicationObject = (DTE2)application;

            #region 创建添加引用的上下文菜单项

            CommandBarControl control = (CommandBarControl)((CommandBars)_applicationObject.CommandBars)["Context Menus"].Controls["Project and Solution Context Menus"].Control;
            CommandBarPopup commandBarPopup = (CommandBarPopup)control.Control;
            CommandBarControl commandBarControl = commandBarPopup.Controls["Reference Root"];
            CommandBarPopup cp = (CommandBarPopup)commandBarControl.Control;
            //"References"
            CommandBarControl newC = cp.Controls.Add(MsoControlType.msoControlButton, Type.Missing, Type.Missing, Type.Missing, Type.Missing);
            newC.Enabled = true;
            newC.Caption = "Add &CONNECT. Service Reference";
            newC.Visible = true;
            CommandBarButton cmb = (CommandBarButton)newC.Control;
            cmb.Click += AddReferenceButtonClick;

            #endregion
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

        #endregion

        #region Members

        private DTE2 _applicationObject;
	    public static EnvDTE.Project CurrentProject;

        #endregion

        #region Events

        //添加引用按钮单击事件
        void AddReferenceButtonClick(CommandBarButton Ctrl, ref bool CancelDefault)
        {
            CurrentProject = (Project) (((Object[])_applicationObject.ActiveSolutionProjects)[0]);
            ReferencePreviewWindow window = new ReferencePreviewWindow();
            window.ShowDialog();
        }

        #endregion
	}
}