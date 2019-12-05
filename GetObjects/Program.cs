using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetObjects
{
    class SqlDataPrepodCourseRow
    {
        public string Name { get; set; }

        public string Course { get; set; }
    }
    class SqlDataPrepodCourseRowsList
    {
        public SqlDataPrepodCourseRowsList()
        {
            m_courses = new List<Course>();
        }

        public List<SqlDataPrepodCourseRow> PrepodsPerCourses { get; private set; } = new List<SqlDataPrepodCourseRow>();

        public void ReadData(string query, MySqlConnection connection)
        {
            MySqlCommand _getPrepodCourse = new MySqlCommand(query, connection);
            _getPrepodCourse.CommandType = System.Data.CommandType.StoredProcedure;

            using (MySqlDataReader _rdr = _getPrepodCourse.ExecuteReader())
            {
                if (_rdr.HasRows)
                {
                    while (_rdr.Read())
                    {
                        PrepodsPerCourses.Add(new SqlDataPrepodCourseRow() { Course = (string)_rdr.GetValue(1), Name = (string)_rdr.GetValue(0) });
                    }
                }
            }
            CoursesGrpoups = GetGroups();
        }

        public List<Course> GetData()
        {
            foreach (IGrouping<string, SqlDataPrepodCourseRow> group in CoursesGrpoups)
            {
                List<Prepod> _prepods = new List<Prepod>();

                foreach (var el in group)
                {
                    Console.WriteLine($"-{el.Name}");
                    _prepods.Add(new Prepod() { Name = (string)el.Name });
                }

                m_courses.Add(new Course() { Name = group.Key, Prepods = _prepods });
            }

            return m_courses;
        }

        //------------------------------------------------------------------------------------------------------------

        private List<Course> m_courses { get; }

        private IEnumerable<IGrouping<string, SqlDataPrepodCourseRow>> CoursesGrpoups;

        private IEnumerable<IGrouping<string, SqlDataPrepodCourseRow>> GetGroups()
        {
            return from prepod in PrepodsPerCourses
                   group prepod by prepod.Course;
        }
    }

    class SqlDataCourseKafedraRow
    {
        public string Name_Course { get; set; }
        public string Name_Cafedra { get; set; }
    }
    class SqlDataCourseKafedraRowList
    {
        public SqlDataCourseKafedraRowList()
        {
            Courses = new List<Course>();
        }

        public List<Course> Courses { get; set; }

        public List<SqlDataCourseKafedraRow> CoursesPerKafedra { get; private set; } = new List<SqlDataCourseKafedraRow>();

        public void ReadData(string query, MySqlConnection conneciton)
        {
            MySqlCommand _comm = new MySqlCommand(query, conneciton);
            _comm.CommandType = System.Data.CommandType.StoredProcedure;

            using (MySqlDataReader _rdr = _comm.ExecuteReader())
            {
                if (_rdr.HasRows)
                {
                    while (_rdr.Read())
                    {
                        CoursesPerKafedra.Add(new SqlDataCourseKafedraRow() { Name_Cafedra = (string)_rdr.GetValue(1), Name_Course = (string)_rdr.GetValue(0) });
                    }
                }
            }

            KafedrasGrpoups = GetGroups();
        }

        public List<Kafedra> GetData()
        {
            foreach (IGrouping<string, SqlDataCourseKafedraRow> kafedara in KafedrasGrpoups)
            {
                m_kafedras.Add(new Kafedra() { NameKafedra = kafedara.Key });
            }

            foreach (var group in KafedrasGrpoups)
                foreach (var course in Courses)
                    foreach (var el in group)
                        if (course.Name == el.Name_Course)
                        {
                            foreach (var kafedra in m_kafedras)
                            {
                                if (kafedra.NameKafedra == el.Name_Cafedra)
                                {
                                    kafedra.Courses.Add(course);
                                    break;
                                }
                            }
                            break;
                        }

            return m_kafedras;
        }

        //------------------------------------------------------------------------------------------------------------

        private List<Kafedra> m_kafedras { get; } = new List<Kafedra>();

        private IEnumerable<IGrouping<string, SqlDataCourseKafedraRow>> KafedrasGrpoups;

        private IEnumerable<IGrouping<string, SqlDataCourseKafedraRow>> GetGroups()
        {
            return from course in CoursesPerKafedra
                   group course by course.Name_Cafedra;
        }
    }

    class SqlDataKafedraFacultetRow
    {
        public string Name_cafedra { get; set; }
        public string Name_facultet { get; set; }
        public string Abbr_facultet { get; set; }
    }
    class SqlDataKafedraFacultetRowsList
    {
        public SqlDataKafedraFacultetRowsList()
        {
            Kafedras = new List<Kafedra>();
        }

        public List<Kafedra> Kafedras { get; set; }

        public List<SqlDataKafedraFacultetRow> KafedrasPerFacultet { get; private set; } = new List<SqlDataKafedraFacultetRow>();

        public void ReadData(string query, MySqlConnection connection)
        {
            MySqlCommand _comm = new MySqlCommand(query, connection);
            _comm.CommandType = System.Data.CommandType.StoredProcedure;

            using (MySqlDataReader _rdr = _comm.ExecuteReader())
            {
                if (_rdr.HasRows)
                {
                    while (_rdr.Read())
                    {
                        KafedrasPerFacultet.Add(new SqlDataKafedraFacultetRow() { Abbr_facultet = (string)_rdr.GetValue(2), Name_cafedra = (string)_rdr.GetValue(0), Name_facultet = (string)_rdr.GetValue(1) });
                    }
                }
            }

            FacultetsGrpoups = GetGroups();
        }

        public List<Facultet> GetData()
        {
            foreach (IGrouping<string, SqlDataKafedraFacultetRow> group in FacultetsGrpoups)
            {
                string abbr = "";

                foreach (var el in group)
                {
                    abbr = el.Abbr_facultet;
                }
                m_facultets.Add(new Facultet() { NameFacultet = group.Key, AbbrFacultet = abbr });
            }

            foreach (var group in FacultetsGrpoups)
            {
                foreach (var kafedra in Kafedras)
                    foreach (var el in group)
                        if (kafedra.NameKafedra == el.Name_cafedra)
                            foreach (var facultet in m_facultets) {
                                if (facultet.NameFacultet == el.Name_facultet) {
                                    facultet.Kafedras.Add(kafedra);
                                    break;
                                }
                            }
                            break;
                        }
            return m_facultets;
        }

        //------------------------------------------------------------------------------------------------------------

        private List<Facultet> m_facultets { get; } = new List<Facultet>();

        private IEnumerable<IGrouping<string, SqlDataKafedraFacultetRow>> FacultetsGrpoups;

        private IEnumerable<IGrouping<string, SqlDataKafedraFacultetRow>> GetGroups()
        {
            return from kafedra in KafedrasPerFacultet
                   group kafedra by kafedra.Name_facultet;
        }
    }

    class Program
    {
        static MySqlConnection _sqlConn;

        static void Main(string[] args)
        {

            #region --Получение курсов--

            SqlDataPrepodCourseRowsList _PrepodsPerCourse = null;

            using (_sqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString))
            {
                try
                {
                    _sqlConn.Open();

                    _PrepodsPerCourse = new SqlDataPrepodCourseRowsList();

                    _PrepodsPerCourse.ReadData(query, _sqlConn);
                }
                catch (MySqlException ex)
                {
                    Console.WriteLine(ex.ErrorCode);
                }
            }

            courses = _PrepodsPerCourse.GetData();

            #endregion

            #region --Получение кафедр--

            SqlDataCourseKafedraRowList CoursesPerKafedra = null;

            using (_sqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString))
            {
                _sqlConn.Open();

                CoursesPerKafedra = new SqlDataCourseKafedraRowList();

                CoursesPerKafedra.ReadData(query2, _sqlConn);
            }

            CoursesPerKafedra.Courses = courses;

            kafedras = CoursesPerKafedra.GetData();

            #endregion

            #region --Получение факультетов--

            SqlDataKafedraFacultetRowsList KafedrasPerFacultet = new SqlDataKafedraFacultetRowsList();

            using (_sqlConn = new MySqlConnection(ConfigurationManager.ConnectionStrings["Connection"].ConnectionString))
            {
                _sqlConn.Open();

                KafedrasPerFacultet.ReadData(query3, _sqlConn);
            }
            KafedrasPerFacultet.Kafedras = kafedras;
            facultets = KafedrasPerFacultet.GetData();

            #endregion

            Console.Read();
        }

        static List<Kafedra> kafedras = new List<Kafedra>();
        static List<Course> courses = new List<Course>();
        static List<Facultet> facultets = new List<Facultet>();

        static string query = @"sp_GetPrepodsCoursesRows";

        static string query2 = @"sp_GetCoursesKafedrasRows";

        static string query3 = @"sp_GetKafedrasFacultetsRows";
    }
}
