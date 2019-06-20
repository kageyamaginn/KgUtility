using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Kg.Db.Oracle
{
    public class Database
    {
        public static string ConnectionStringEssentus { get; set; }
        public static string ConnectionStringWebPDM { get; set; }
        //public static string ConnectionStringLogging { get; set; }

        static Database()
        {
            //Dictionary<string, string> connections = null;
            //ServiceEnvironment.ApplicationName = WebConfigurationManager
            //if (ServiceEnvironment.RegistryOk)
            //{
            //    connections = ServiceEnvironment.DatabaseConnections;
            //}
            //if (connections != null && connections.ContainsKey("Essentus"))
            //    ConnectionStringEssentus = connections["Essentus"];
            //else 
            //    ConnectionStringEssentus = ConfigurationManager.ConnectionStrings["EssentusConnectionString"].ConnectionString;

            //if (connections != null && connections.ContainsKey("WebPDM"))
            //    ConnectionStringWebPDM = connections["WebPDM"];
            //else 
            //    ConnectionStringWebPDM = ConfigurationManager.ConnectionStrings["WebPDMConnectionString"].ConnectionString;

        }

        public static OracleConnection ConnectionEssentus
        {
            get { return new OracleConnection(ConnectionStringEssentus); }
        }

        public static OracleConnection ConnectionWebPDM
        {
            get { return new OracleConnection(ConnectionStringWebPDM); }
        }

        //public static SqlConnection ConnectionLogging
        //{
        //    get { return new SqlConnection(ConnectionStringLogging); }
        //}

        public static Dictionary<string, string> LoadSqls(string sqlFile)
        {
            Dictionary<string, string> sqls = new Dictionary<string, string>();
            string sqlFolder = AppDomain.CurrentDomain.BaseDirectory;
            string sqlPath = Path.Combine(sqlFolder, sqlFile);
            if (!File.Exists(sqlPath))
            {
                sqlFolder = Path.Combine(sqlFolder, "Bin");
                sqlPath = Path.Combine(sqlFolder, sqlFile);
            }
            using (StreamReader reader = new StreamReader(sqlPath))
            {
                string l, sn;
                StringBuilder sb;
                while ((l = reader.ReadLine()) != null)
                {
                    if (l.StartsWith("---"))
                    {
                        sn = l.Substring(3);
                        sb = new StringBuilder();
                        while ((l = reader.ReadLine()) != null)
                        {
                            if (l.StartsWith("--!")) break;
                            if (l.StartsWith("--")) continue;
                            sb.Append(l.Replace('\t', ' ') + " ");
                        }
                        sqls.Add(sn, sb.ToString());
                    }
                }
            }
            return sqls;
        }
        public static string Format(string format, params object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                args[i] = args[i].ToString().Replace("'", "''");
                //args[i] = Utilities.Utility.ReplaceStringForFilter(args[i].ToString());
            }
            return string.Format(format, args);
        }
        public static bool ExecuteNonQuery(List<OracleCommand> oraCommands, ConnString connStr)
        {
            OracleTransaction oraTran = null;
            OracleConnection con = new OracleConnection(GetConnectionString(connStr));
            OracleCommand oraCommand = con.CreateCommand();
            oraCommand.BindByName = true;
            string log = String.Empty;
            try
            {
                con.Open();
                oraTran = con.BeginTransaction();
                oraCommand.Transaction = oraTran;

                foreach (OracleCommand command in oraCommands)
                {
                    oraCommand.Parameters.Clear();
                    oraCommand.CommandText = command.CommandText;
                    log = String.Empty;
                    foreach (OracleParameter op in command.Parameters)
                    {
                        log += op.ParameterName + ": " + (op.Value == null ? String.Empty : op.Value.ToString().Substring(0, op.Value.ToString().Length > 512 ? 512 : op.Value.ToString().Length)) + "\r\n";
                        oraCommand.Parameters.Add(op);
                    }
                    //Logging.WriteLog(oraCommand.CommandText + "\r\n" + log);

                    oraCommand.ExecuteNonQuery();
                }
                oraTran.Commit();

                return true;
            }
            catch (Exception ex)
            {
                oraTran.Rollback();
                throw ex;
            }
            finally
            {
                con.Close();
            }
        }
        public static bool ExecuteNonQuery(string cmdText, ConnString connStr, params OracleParameter[] param)
        {
            bool result = false;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                cmd.BindByName = true;
                foreach (OracleParameter p in param)
                    cmd.Parameters.Add(p);
                result = cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        public static string Convert2Hex(byte[] bytes)
        {
            string str = String.Empty;
            string s = String.Empty;
            for (int jj = bytes.Length; jj > 0; jj--)
            {
                s = System.Convert.ToString(bytes[jj - 1], 16);
                for (int i = 0; i < 2 - s.Length; i++)
                    s = "0" + s;
                str += s;
            }
            return str.ToUpper();
        }
        public static bool ExecuteNonQuery(string cmdText, string connStr, params OracleParameter[] param)
        {
            bool result = false;
            string log = String.Empty;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(connStr);
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                cmd.BindByName = true;
                foreach (OracleParameter p in param)
                {
                    if (p.Value.GetType().Equals(typeof(byte[])) && ((byte[])p.Value).Length == 16)
                    {
                        log += p.ParameterName + ": " + (p.Value == null ? String.Empty : Convert2Hex((byte[])p.Value)) + "\r\n";
                    }
                    else
                    {
                        log += p.ParameterName + ": " + (p.Value == null ? String.Empty : p.Value.ToString()) + "\r\n";
                    }
                    cmd.Parameters.Add(p);
                }
                //Logging.WriteLog(cmd.Connection.ConnectionString + "\r\n" + cmd.CommandText + "\r\n" + log);

                result = cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        public static bool ExecuteNonQuery(string cmdText, ConnString connStr)
        {
            bool result = false;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                result = cmd.ExecuteNonQuery() > 0;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        public static bool Exists(string cmdText, ConnString connStr, params OracleParameter[] param)
        {
            bool result = false;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                cmdText = String.Format("select count(1) from ({0}) t", cmdText);
                OracleCommand cmd = new OracleCommand(cmdText, con);
                cmd.BindByName = true;
                foreach (OracleParameter p in param)
                    cmd.Parameters.Add(p);
                object obj = cmd.ExecuteScalar();
                result = (obj != null && int.Parse(obj.ToString()) > 0);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        public static int Count(string cmdText, ConnString connStr)
        {
            int result = 0;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                cmdText = String.Format("select count(1) from ({0}) t", cmdText);
                OracleCommand cmd = new OracleCommand(cmdText, con);
                object obj = cmd.ExecuteScalar();
                result = (obj.Equals(DBNull.Value)) ? 0 : int.Parse(obj.ToString());
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        public static object[] ExecuteSingleRow(string cmdText, ConnString connStr)
        {
            object[] result = null;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                OracleDataReader read = cmd.ExecuteReader(CommandBehavior.SingleRow);
                while (read.Read())
                {
                    result = new object[read.FieldCount];
                    for (int i = 0; i < result.Length; i++)
                    {
                        result[i] = read[i];
                        if (result[i] != null && result[i].Equals(DBNull.Value))
                            result[i] = null;
                    }
                }
                read.Close();
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        public static object ExecuteScalar(string cmdText, ConnString connStr)
        {
            object result = null;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                result = cmd.ExecuteScalar();
                if (result != null && result.Equals(DBNull.Value))
                    result = null;
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return result;
        }
        public static T ExecuteScalar<T>(string cmdText, string connStr, params OracleParameter[] param)
        {
            Type type = typeof(T);
            T t = default(T);
            object result = null;
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(connStr);
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                cmd.BindByName = true;
                foreach (OracleParameter p in param)
                    cmd.Parameters.Add(p);
                result = cmd.ExecuteScalar();
                if (result == null || result.Equals(DBNull.Value))
                {
                    result = null;
                }
                else
                {
                    result = SetValue(typeof(T), result);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (con != null) con.Close();
            }
            t = (T)result;
            return t;
        }
        private static object SetValue(Type type, object value)
        {
            object val = null;
            if (type.Equals(typeof(Int16)))
                val = Int16.Parse(value.ToString());
            else if (type.Equals(typeof(Int32)))
                val = Int32.Parse(value.ToString());
            else if (type.Equals(typeof(UInt32)))
                val = UInt32.Parse(value.ToString());
            else if (type.Equals(typeof(String)))
                val = value.ToString();
            else if (type.Equals(typeof(byte[])))
                val = (byte[])value;
            else
                val = value;
            return val;
        }
        public static DataSet Fill(string cmdText, ConnString connStr)
        {
            DataSet ds = new DataSet();
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return ds;
        }

        public static DataSet FillDataSet(string cmdText, ConnString connStr, params OracleParameter[] param)
        {
            DataSet ds = new DataSet();
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                cmd.BindByName = true;
                foreach (OracleParameter p in param)
                    cmd.Parameters.Add(p);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return ds;
        }

        public static void FillDataSet(string cmdText, ConnString connStr, DataSet ds, String[] tableMapping, params OracleParameter[] param)
        {
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                cmd.BindByName = true;
                foreach (OracleParameter p in param)
                    cmd.Parameters.Add(p);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                for (int i = 0; i < tableMapping.Length; i++)
                {
                    oda.TableMappings.AddRange(new System.Data.Common.DataTableMapping[] {
                        new System.Data.Common.DataTableMapping("Table" + (i ==0 ? String.Empty: i.ToString()) , tableMapping[i])});
                }

                oda.Fill(ds);
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
        }

        public static DataTable Fill(string cmdText, ConnString connStr, params OracleParameter[] param)
        {
            DataSet ds = new DataSet();
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                cmd.BindByName = true;
                foreach (OracleParameter p in param)
                    cmd.Parameters.Add(p);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(ds);

            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return ds.Tables[0];
        }

        public static T Fill<T>(string cmdText, ConnString connStr, params OracleParameter[] param)
        {
            while (cmdText.Contains("/*") && cmdText.Contains("*/"))
            {
                int start = cmdText.IndexOf("/*");
                int end = cmdText.IndexOf("*/");
                cmdText = cmdText.Substring(0, start) + cmdText.Substring(end + 2);
            }

            Type type = typeof(T);
            T Instance = (T)Activator.CreateInstance(type);
            OracleConnection con = null;
            try
            {
                int start = cmdText.IndexOf("select", StringComparison.OrdinalIgnoreCase) + 6;
                int end = cmdText.IndexOf("from", StringComparison.OrdinalIgnoreCase);
                List<String> names = new List<string>();
                foreach (String field in cmdText.Substring(start, end - start).Split(','))
                {
                    string fieldName;
                    fieldName = field.Trim();
                    if (fieldName.Contains("nvl("))
                        fieldName = "";
                    if (fieldName.Contains('.'))
                        fieldName = fieldName.Substring(fieldName.IndexOf('.') + 1);
                    if (fieldName.Contains(' '))
                        fieldName = fieldName.Substring(fieldName.LastIndexOf(' ') + 1);

                    // skip if empty
                    if (String.IsNullOrEmpty(fieldName))
                        continue;
                    // 是数字的跳过
                    short tryInt;
                    if (Int16.TryParse(fieldName, out tryInt))
                        continue;
                    // 包含括号的跳过
                    if (fieldName.Contains('(') || fieldName.Contains(')'))
                        continue;

                    names.Add(fieldName);
                }
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                OracleCommand cmd = new OracleCommand(cmdText, con);
                cmd.BindByName = true;
                foreach (OracleParameter p in param)
                    cmd.Parameters.Add(p);

                OracleDataReader reader = cmd.ExecuteReader();
                while (reader.Read())
                {
                    if (type.IsGenericType)
                    {
                        if (type.GetGenericTypeDefinition().Equals(typeof(List<>)))
                        {
                            if (type.GetGenericArguments()[0].Equals(typeof(String)))
                            {
                                Instance.GetType().GetMethod("Add").Invoke(Instance, new object[] { reader[names[0]] });
                            }
                            else if (type.GetGenericArguments()[0].Equals(typeof(Guid)))
                            {
                                Instance.GetType().GetMethod("Add").Invoke(Instance, new object[] { new Guid((byte[])reader[names[0]]) });
                            }
                            else
                            {
                                object ItemInstance = Activator.CreateInstance(type.GetGenericArguments()[0]);

                                for (int i = 0; i < reader.FieldCount; i++)
                                {
                                    string field = names[i];
                                    object value = reader[field];
                                    if (!value.Equals(DBNull.Value))
                                    {
                                        if (!SetProperty(ItemInstance, field, value))
                                            SetField(ItemInstance, field, value);
                                    }
                                }
                                Instance.GetType().GetMethod("Add").Invoke(Instance, new object[] { ItemInstance });
                            }
                        }
                    }
                    else
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string field = names[i];
                            object value = reader[field];
                            if (!value.Equals(DBNull.Value))
                            {
                                if (!SetProperty(Instance, field, value))
                                    SetField(Instance, field, value);
                            }
                        }
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return Instance;
        }

        public static Guid GetGuid(OracleDataReader reader, int index)
        {
            byte[] b = (Byte[])reader.GetValue(index);
            return new Guid(b);
        }

        public static string Fill(string cmdText, ConnString connStr, Type type, int pageNumber, int pageSize)
        {
            string json = "{}";
            DataTable dt = new DataTable();
            OracleConnection con = null;
            try
            {
                con = new OracleConnection(GetConnectionString(connStr));
                con.Open();
                cmdText = String.Format("SELECT * FROM (SELECT A.*, ROWNUM RN FROM ({0}) A WHERE ROWNUM <= {1}) WHERE RN >= {2}"
                                            , cmdText, pageNumber * pageSize, (pageNumber - 1) * pageSize);

                OracleCommand cmd = new OracleCommand(cmdText, con);
                OracleDataAdapter oda = new OracleDataAdapter(cmd);
                oda.Fill(dt);
                //json = Utility.CreateJsonParams(dt.DefaultView);
                //retObj = Newtonsoft.Json.JsonConvert.DeserializeObject(json, type);
            }
            catch (Exception)
            {
                throw;
            }
            finally
            {
                if (con != null) con.Close();
            }
            return json;
        }
        public static OracleCommand NewWebPDMCmd()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.BindByName = true;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = new OracleConnection(Database.ConnectionStringWebPDM);
            return cmd;
        }
        public static OracleCommand NewEssentusCmd()
        {
            OracleCommand cmd = new OracleCommand();
            cmd.BindByName = true;
            cmd.CommandType = CommandType.Text;
            cmd.Connection = new OracleConnection(Database.ConnectionStringEssentus);
            return cmd;
        }
        private static string GetConnectionString(ConnString connStr)
        {
            string connstring = String.Empty;
            switch (connStr)
            {
                case ConnString.WebPDM:
                    connstring = ConnectionStringWebPDM;
                    break;
                case ConnString.Essentus:
                    connstring = ConnectionStringEssentus;
                    break;
            }
            return connstring;
        }
        public enum ConnString
        {
            WebPDM = 1,
            Essentus = 2
        }

        public static bool SetProperty(object Instance, string field, object value)
        {
            bool exist = false;
            Type type = Instance.GetType();
            foreach (PropertyInfo p in type.GetProperties())
            {
                if (p.Name.Equals(field, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (p.PropertyType.Equals(typeof(Guid)))
                        value = new Guid((Byte[])value);
                    else if (p.PropertyType.Equals(typeof(Int16)))
                        value = Int16.Parse(value.ToString());
                    else if (p.PropertyType.Equals(typeof(Int32)))
                        value = Int32.Parse(value.ToString());
                    else if (p.PropertyType.Equals(typeof(UInt32)))
                        value = UInt32.Parse(value.ToString());
                    else if (p.PropertyType.Equals(typeof(Boolean)))
                        value = Boolean.Parse(value.ToString());
                    else if (p.PropertyType.IsEnum)
                        value = Int32.Parse(value.ToString());

                    p.SetValue(Instance, value, null);
                    exist = true;
                    break;
                }
            }
            return exist;
        }
        public static bool SetField(object Instance, string field, object value)
        {
            bool exist = false;
            Type type = Instance.GetType();
            foreach (FieldInfo f in type.GetFields())
            {
                if (f.Name.Equals(field, StringComparison.CurrentCultureIgnoreCase))
                {
                    if (f.FieldType.Equals(typeof(Guid)))
                        value = new Guid((Byte[])value);
                    else if (f.FieldType.Equals(typeof(Int16)))
                        value = Int16.Parse(value.ToString());
                    else if (f.FieldType.Equals(typeof(Int32)))
                        value = Int32.Parse(value.ToString());
                    else if (f.FieldType.Equals(typeof(UInt32)))
                        value = UInt32.Parse(value.ToString());
                    else if (f.FieldType.Equals(typeof(Boolean)))
                        value = Boolean.Parse(value.ToString());
                    f.SetValue(Instance, value);
                    exist = true;
                    break;
                }
            }
            return exist;
        }
    }
}
