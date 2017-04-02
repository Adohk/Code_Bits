//Check if form is open and execute their local function.
void check() {
    if (Application.OpenForms["formName"] != null) {
        (Application.OpenForms["formName"] as formName).func();
    }
}

//XML config file read.
Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
Class.Var = config.AppSettings.Settings["option"].Value.ToString());

//XML config file changes.
Configuration config = ConfigurationManager.OpenExeConfiguration(Application.ExecutablePath);
config.AppSettings.Settings["option"].Value = newValue;
config.Save(ConfigurationSaveMode.Modified);

//Made all childs MDI Controls inherit parameters.
private void Form_Load(object sender, EventArgs e) {
    MdiClient ctlMDI;
    foreach (Control ctl in this.Controls) {
        try {
            ctlMDI = (MdiClient)ctl;    
            ctlMDI.BackColor = this.Parameter;
        } catch (InvalidCastException exc) {
            Console.WriteLine(exc.Message);
        }
    }
}

//Make a child MDI.
OtherForm newform = new OtherForm();
newform.MdiParent = this;
newform.Show();

//Check if MDI form is minimized then restore instead of opening a new form window.
foreach (Form form in this.MdiChildren) {
    if (form.GetType() == typeof(OtherForm)) {
        if (form.WindowState == FormWindowState.Minimized) {
			form.WindowState = FormWindowState.Normal;
        }
        form.Activate();
        return;
    }
}

//Close all childs of an MDI.
this.MdiChildren.OfType<Form>().ToList().ForEach(x => x.Close());

//Fill DataGridView.
private void Class_Load(object sender, EventArgs e) {  
	this.TableAdapter.Fill(this.dataSet.Table);
}

//Make the user unable to select the header cell.
private void dataGridView_CellClick(object sender, DataGridViewCellEventArgs e) {
    if (e.RowIndex < 0) {
        return;
    }
}

//Get selected Cell content.
Control1.Text = dataGridView.Rows[e.RowIndex].Cells[0].Value.ToString();
Control2.Text = dataGridView.Rows[e.RowIndex].Cells["ColumnName"].Value.ToString();

//Params to ReportViewer.
public ReportViewer(string rptsrc) {
	InitializeComponent();    
	reportViewer.LocalReport.ReportEmbeddedResource = rptsrc.ToString();
	ReportParameter[] paramst = new ReportParameter[5];
	paramst[0] = new ReportParameter("param0",Class.Var);
	paramst[1] = new ReportParameter("param1",Class.Var);
	reportViewer.LocalReport.SetParameters(paramst);
	reportViewer.ZoomMode = ZoomMode.PageWidth;
	reportViewer.LocalReport.Refresh();
}

//Change ReportViewer DataSet.
  //Event
private void button_Click(object sender, EventArgs e) {
    string rptsrc = "MyApp.Reports.ReportName.rdlc";
    if (dataGridView.DataSource == DefaultBindingSource) {
        BindingSource bindsrc = (BindingSource)dataGridView.DataSource;
        DataSet ds = (DataSet)bindsrc.DataSource;
        DataTable datattab = ds.Tables[0];                
        ReportViewerForm rpv = new ReportViewerForm(datattab, rptsrc);
        rpv.Show();
    } else {
        DataSet ds = (DataSet)dataGridView.DataSource;
        DataTable datattab = ds.Tables[0];
        ReportViewerForm rpv = new ReportViewerForm(datattab, rptsrc);
        rpv.Show();
    }
}
  //ReportViewerForm
public ReportViewerForm(DataTable dattab, string rptsrc) {
    InitializeComponent();
    reportViewer.LocalReport.DataSources.Clear();
    reportViewer.LocalReport.DataSources.Add(new ReportDataSource("DataSetName", dattab));
    reportViewer.LocalReport.ReportEmbeddedResource = rptsrc.ToString();
    reportViewer.ZoomMode = ZoomMode.PageWidth;
    reportViewer.AutoSizeMode = AutoSizeMode.GrowAndShrink;
    reportViewer.SetDisplayMode(DisplayMode.PrintLayout);                
    reportViewer.LocalReport.Refresh();
}

//Error provider.
errorProvider1.Clear();
var TextBox = new[] { TextBox1, TextBox2 };
var ComboBox = new[] { ComboBox1 };
foreach (var control in TextBox.Where(ex => String.IsNullOrWhiteSpace(ex.Text))) {
    errorProvider1.SetError(control, "Error");
}
if (string.IsNullOrWhiteSpace(ComboBox.Value)) {
    errorProvider1.SetError(ComboBox, "Error");
}


//SQL database connection template
public SqlConnection sqlcon;
public SqlDataReader sqldr;
public void connect() {
    sqlcon = new SqlConnection();
    try {
        sqlcon.ConnectionString = "Initial Catalog=" + database + "; Data Source=" + server + "; Integrated Security=SSPI";
        sqlcon.Open();
        sqlcon.Close();
    } catch (SqlException ex) {
        sqlcon.Close();
    }
}

//SQL with parameters.
void func() {
    DBCLass instance = new DBCLass();
    try {
        instance.connect();
        using (instance.sqlcon) {
            using (SqlCommand cmd = new SqlCommand("SQLCMD", instance.sqlcon)) {
                cmd.CommandType = CommandType.Text;
                cmd.Parameters.Add("@param", SqlDbType.Type).Value = Value;
                instance.sqlcon.Open();
                cmd.ExecuteNonQuery();
                instance.sqlcon.Close();
                instance.sqlcon.Dispose();
            }
        }
    } catch (SqlException ex) {
        instance.sqlcon.Close();
        instance.sqlcon.Dispose();
    }
}

//Read SQL.
void func() {
    DBCLass instance = new DBCLass();
    try {
        instance.connect();
        using (instance.sqlcon) {
            using (SqlCommand cmd = new SqlCommand("SQLCMD", instance.sqlcon)) {
                cmd.CommandType = CommandType.Text;
                instance.sqlcon.Open();
                instance.sqldr = cmd.ExecuteReader();
                while (instance.sqldr.Read()) {                        
                    if (instance.sqldr.GetValue(0).ToString() != string.Empty) {                        
                        Class.Var = instance.sqldr.GetValue(0).ToString();
                        Class.Var2 = instance.sqldr["ColumnName"].ToString();
                    }                    
                }
                instance.sqlcon.Close();
                instance.sqlcon.Dispose();
            }
        }
    } catch (SqlException ex) {
        instance.sqlcon.Close();
        instance.sqlcon.Dispose();
    }
}

//Change Dataset of DataGridView.
private void textBox_TextChanged(object sender, EventArgs e) {
    DBClass instance = new DBClass();
    string new_cmd = @" SELECT 
                            Table.Y
                            OtherTable.Y
                        FROM 
                            Table INNER JOIN
                            OtherTable ON Table.Y = OtherTable.Y
                        WHERE
                            Y like 'X%'";
    try {
        instance.connect();
        instance.sqlcon.Open();
        using (SqlCommand cmd = new SqlCommand(new_cmd , instance.sqlcon)) {
            DataSet ds = new DataSet();
            instance.sqlda.SelectCommand = cmd;
            instance.sqlda.Fill(ds, "TableName");
            dataGridView.DataSource = ds;
            dataGridView.DataMember = "TableName";
            dataGridView.Update();
            dataGridView.Refresh();
            instance.sqlcon.Close();
            instance.sqlcon.Dispose();
        }
    } catch (SqlException ex) {
        instance.sqlcon.Close();
        instance.sqlcon.Dispose();
    }
}

//Cryptography.
public class HashClass {
    //return hash as HEX
    public static string CreateHash(string data) {
        SHA256CryptoServiceProvider Sha256Obj = new SHA256CryptoServiceProvider();
        Byte[] ToHash = Encoding.UTF8.GetBytes(data);
        ToHash = Sha256Obj.ComputeHash(ToHash);
        string result = String.Empty;
        foreach (Byte b in ToHash) {
            result += b.ToString("x2");
        }
        return result.Trim();
    }

    public static bool CompareHash(string Current, string ToCompare) {
        if (Current.Trim() == ToCompare.Trim()) {
            return true;
        } else {
            return false;
        }
    }

    public static string CreateSalt() {
        RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();
        byte[] buff = new byte[32];
        rng.GetBytes(buff);
        return Convert.ToBase64String(buff);
    }
}


//Text File logger template
public class Logger {
    string file = "log.txt";
    string dir2 = "C:/route/";
    
    public bool dircheck() {            
        if (!Directory.Exists(dir)) {
            try {
                Directory.CreateDirectory(dir);
                return true;
            } catch (DirectoryNotFoundException e) {
                return false;
            }
        }
        return true;
    }

    public bool logger(string Tstring) {
        bool chk = dircheck();
        if (chk == true) {
                string text = Environment.NewLine + Environment.MachineName + " | " + DateTime.Now + " |: " + Tstring + "";                                        
                File.AppendAllText(dir2 + file, text);
                return true;
        } else {
            return false;
        }
    }
}
