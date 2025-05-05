using System;
using System.Data;
using System.Drawing;
using System.Windows.Forms;
using Microsoft.Data.SqlClient;

namespace BVS_VideoRental
{
    public static class DatabaseConfig
    {
        public static string ConnectionString { get; } =
            "Data Source=(LocalDB)\\MSSQLLocalDB;AttachDbFilename=C:\\Users\\SKYE\\Documents\\BoggyVideoStore.mdf;Integrated Security=True;";
    }
}
