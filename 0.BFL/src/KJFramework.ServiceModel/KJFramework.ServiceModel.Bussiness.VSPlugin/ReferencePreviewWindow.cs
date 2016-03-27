using System.Collections.Generic;
using System.Windows.Forms;
using KJFramework.ServiceModel.Bussiness.VSPlugin.Contracts;
using KJFramework.ServiceModel.Bussiness.VSPlugin.Generators;

namespace KJFramework.ServiceModel.Bussiness.VSPlugin
{
    public partial class ReferencePreviewWindow : Form
    {
        #region Members

        private IRemotingService _remotingService;

        #endregion

        #region Constructor

        public ReferencePreviewWindow()
        {
            InitializeComponent();
        }

        #endregion

        #region Events

        private void btn_Go_Click(object sender, System.EventArgs e)
        {
            lst_Operations.Items.Clear();
            lst_Service.Items.Clear();
            string path = cmb_Address.Text.Trim();
            if (string.IsNullOrEmpty(path))
            {
                MessageBox.Show("Oops, you *MUST* set the remoting metadata address first!");
                return;
            }
            try
            {
                _remotingService = new RemotingService(path, chb_Async.Checked);
                lst_Service.Items.Add(_remotingService.Infomation.FullName);
                IList<IMethod> previewMethods = _remotingService.GetPreviewMethods();
                if (previewMethods == null) return;
                foreach (IMethod method in previewMethods) lst_Operations.Items.Add(method);
            }
            catch (System.Exception ex) { MessageBox.Show(ex.Message); }
        }

        private void btn_OK_Click(object sender, System.EventArgs e)
        {
            if(_remotingService == null)
            {
                MessageBox.Show("Oops, you can't auto generate code from *NULL* contract.");
                return;
            }
            IContractGenerator generator = new ContractGenerator("D:\\");
            generator.Generate(_remotingService);
            Close();
        }

        private void btn_Cancel_Click(object sender, System.EventArgs e)
        {
            Close();
        }

        #endregion
    }
}
