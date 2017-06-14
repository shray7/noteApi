using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebApplication1.Models
{
    public class NoteList
    {
        public int Count { get; set; }
        public Note[] Notes { get; set; }
    }
    public class Note
    {
        public int Id { get; set; }
        public string Body { get; set; }
    }
}
