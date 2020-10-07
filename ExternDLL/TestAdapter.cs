using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExternDLL
{
    public class TestAdapter
    {
        public static TestAdapter Instance { get; } = new TestAdapter();

        public class Benutzer
        {
            public int kBenutzer { get; set; }
            public string cName { get; set; }
        }

        public class Artikel
        {
            public int kArtikel { get; set; }
            public string cName { get; set; }
        }

        public class Warenlagerplatz
        {
            public int kWarenlagerplatz { get; set; }
            public string cName { get; set; }
        }

        public IList<Benutzer> AlleBenutzer { get; private set; }
        public IList<Artikel> AlleArtikel { get; private set; }
        public IList<Warenlagerplatz> AlleWarenlagerplätze { get; private set; }

        private TestAdapter()
        {

        }

        private static IEnumerable<SqlDataReader> ExecuteReader(IDbConnection connection, string query)
        {
            using (var command = connection.CreateCommand())
            {
                command.CommandText = query;
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        yield return (SqlDataReader)reader;
                    }
                }
            }
        }

        public void Reload(string srv, string dbn, string dbu, string dbp)
        {
            using (var connection = new SqlConnection(new SqlConnectionStringBuilder()
                                                      {
                                                          UserID = dbu,
                                                          Password = dbp,
                                                          InitialCatalog = dbn,
                                                          DataSource = srv
                                                      }.ToString()))
            {
                connection.Open();


                AlleBenutzer = ExecuteReader(connection, "SELECT kBenutzer, cName FROM dbo.tbenutzer").Select(reader =>
                    new Benutzer()
                    {
                        kBenutzer = Convert.ToInt32(reader["kBenutzer"]),
                        cName = Convert.ToString(reader["cName"])
                    }).ToList();

                AlleArtikel = ExecuteReader(connection, "SELECT kArtikel, cArtNr FROM dbo.tArtikel WHERE cLagerAktiv = 'Y' AND cAktiv = 'Y' ORDER BY cArtNr").Select(reader =>
                    new Artikel()
                    {
                        kArtikel = Convert.ToInt32(reader["kArtikel"]),
                        cName = Convert.ToString(reader["cArtNr"])
                    }).ToList();

                AlleWarenlagerplätze = ExecuteReader(connection, "SELECT kWarenlagerplatz, cName FROM dbo.tWarenlagerplatz WHERE kWarenLagerPlatzTyp = 1 ORDER BY cName").Select(reader =>
                    new Warenlagerplatz()
                    {
                        kWarenlagerplatz = Convert.ToInt32(reader["kWarenlagerplatz"]),
                        cName = Convert.ToString(reader["cName"])
                    }).ToList();
            }
        }


    }
}
