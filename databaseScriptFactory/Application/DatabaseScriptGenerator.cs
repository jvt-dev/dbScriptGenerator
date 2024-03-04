using ExcelDataReader;

namespace databaseScriptFactory
{
    public partial class DatabaseScriptGenerator : Form
    {
        public DatabaseScriptGenerator()
        {
            InitializeComponent();
        }

        private void FormLoad(object sender, EventArgs e) { }

        private static string JoinValues(IExcelDataReader reader)
        {
            var values = new List<object?>();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                values.Add(ObjectParser(reader.GetValue(i)));
            }

            return string.Join(",", values);
        }

        private static bool IsString(object? value)
        {
            if (value != null)
            {
                return value is string && !value.ToString().Contains("TO_DATE");
            }

            return false;
        }

        private static object ObjectParser(object value)
        {
            if (IsString(value))
            {
                return $"'{value}'";
            }

            return value;
        }

        private void ChooseFileClick(object sender, EventArgs e)
        {
            using var dialog = new OpenFileDialog()
            {
            };

            if (dialog.ShowDialog() == DialogResult.OK)
            {
                var script = new List<string>();
                var filestream = File.Open(dialog.FileName, FileMode.Open, FileAccess.Read);
                var reader = ExcelReaderFactory.CreateReader(filestream);
                do
                {
                    while (reader.Read())
                    {
                        script.Add($"INSERT INTO {reader.Name} VALUES({JoinValues(reader)})");
                    }
                } while (reader.NextResult());

                dataGridView1.DataSource = script;
            }
        }

        private void DataGridView(object sender, DataGridViewCellEventArgs e)
        {
        }
    }
}