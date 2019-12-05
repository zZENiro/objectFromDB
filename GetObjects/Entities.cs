using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetObjects
{
    public interface IEntity
    {

    }

    public class Facultet : IEntity
    {
        public List<Kafedra> Kafedras { get; set; } = new List<Kafedra>();

        public string AbbrFacultet { get; set; }

        public string NameFacultet { get; set; }
    }

    public class Course : IEntity
    {
        public int Number { get; }

        public string Name { get; set; } = "";

        // Что, если один предмет ведут несколько преподавателей?
        public List<Prepod> Prepods { get; set; } = new List<Prepod>();
    }

    public class Prepod : IEntity
    {
        public int Number { get; }

        public string Name { get; set; }

        public Prepod()
        {
        }
    }

    public class Kafedra : IEntity
    {
        public string NameKafedra { get; set; } = "";

        // Лист на объекты класса AbstractEmployee
        public List<Prepod> Prepods = new List<Prepod>();

        // Лист с предметами, которые относятся к кафедре
        public List<Course> Courses = new List<Course>();
    }
}
