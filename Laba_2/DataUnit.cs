using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Laba_2
{
    public class DataUnit
    {
        public int Id { get; set; }
        public string FormatId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; }
        public string Obj { get; set; }
        private string conf;
        public string Conf { get => conf; set { conf = value == "1" ? "Да" : (value == "0" ? "Нет" : ""); } }
        private string integr;
        public string Integr { get => integr; set { integr = value == "1" ? "Да" : (value == "0" ? "Нет" : ""); } }
        private string avail;
        public string Avail { get => avail; set { avail = value == "1" ? "Да" : (value == "0" ? "Нет" : ""); } }
        public DateTime dateOfChange;

        public DataUnit(int id, string name, string description, string source, string obj, string conf, string integr, string avail, DateTime dateOfChange)
        {
            Id = id;
            Name = name;
            Description = description;
            Source = source;
            Obj = obj;
            Conf = conf;
            Integr = integr;
            Avail = avail;
            this.dateOfChange = dateOfChange;
            FormatId = $"УБИ.{Id}";
        }

        public override string ToString()
        {
            return $"Id: {Id}" + "\n\n" +
            $"Name: {Name}" + "\n\n" +
            $"Description: {Description}" + "\n\n" +
            $"Source: {Source}" + "\n\n" +
            $"Object: {Obj}" + "\n\n" +
            $"Confidentiality: {Conf}" + "\n\n" +
            $"Integrity: {Integr}" + "\n\n" +
            $"Availability: {Avail}";
        }
    }
}
