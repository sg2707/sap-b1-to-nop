using SAPData.Models;
using SAPData.Services;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Utilities;

namespace SyncToNopCommerce.Views
{
    /// <summary>
    /// Interaction logic for SettingsV
    /// </summary>
    public partial class SettingsV : UserControl
    {
        private readonly ISettingService settingService;
        public SettingsV(ISettingService _settings)
        {
            settingService = _settings;
            InitializeComponent();
        }

        public void dataGrid_AutoGeneratingColumn(object sender, DataGridAutoGeneratingColumnEventArgs e)
        {
            //set Dispaly name to header
            var displayName = Helper.GetPropertyDisplayName(e.PropertyDescriptor);
            if (!string.IsNullOrEmpty(displayName))
            {
                e.Column.Header = displayName;
            }

            //set readonly
            e.Column.IsReadOnly = Helper.GetPropertyReadOnly(e.PropertyDescriptor);

            //Setting String format
            string format = Helper.GetPropertyDisplayFormat(e.PropertyDescriptor);
            DataGridTextColumn col = e.Column as DataGridTextColumn;
            if (!string.IsNullOrWhiteSpace(format))
            {
                col.Binding = new Binding(e.PropertyName) { StringFormat = format };
            }

            //Format datetime to date
            //if (e.PropertyType == typeof(System.DateTime))
            //{
            //    // Create a new template column.              
            //    DataGridTemplateColumn templateColumn = new DataGridTemplateColumn();
            //    templateColumn.Header = e.Column.Header;
            //    templateColumn.CellTemplate = (DataTemplate)Resources["DateCellTemplate"];
            //    templateColumn.CellEditingTemplate = (DataTemplate)Resources["DateCellEditingTemplate"];
            //    //templateColumn.SortMemberPath = "DocDate";

            //    // Replace the auto-generated column with the templateColumn.
            //    e.Column = templateColumn;
            //}
        }


        private void dgUsers_CellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            if (e.EditAction == DataGridEditAction.Commit)
            {
                var column = e.Column as DataGridBoundColumn;
                if (column != null)
                {
                    var bindingPath = (column.Binding as Binding).Path.Path;
                    var el = e.EditingElement as TextBox;

                    int rowIndex = e.Row.GetIndex();

                    //update using EF

                    // rowIndex has the row index
                    // bindingPath has the column's binding
                    // el.Text has the new, user-entered value
                }
            }
        }

    }
}
