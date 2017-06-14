using System;
using System.IO;
using System.Linq;
using System.Web.Script.Serialization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.PlatformAbstractions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    public class  NotesController : Controller
    {
        // GET api/notes
        [HttpGet]
        public JsonResult Get(string query)
        {
            if (!string.IsNullOrEmpty(query))
               return Json(ReadFile().ToObject<NoteList>().Notes.Where(x => x.Body.Contains(query)));
            return Json(ReadFile());
        }

        private JObject ReadFile()
        {
            JObject j;
            var path = PlatformServices.Default.Application.ApplicationBasePath + "\\note.json";
            using (StreamReader file = System.IO.File.OpenText(path))
            using (JsonTextReader reader = new JsonTextReader(file))
            {
                j = (JObject)JToken.ReadFrom(reader);
            }
            return j;
        }

        private void WriteFile(NoteList list)
        {
            var path = PlatformServices.Default.Application.ApplicationBasePath + "\\note.json";
            using (StreamWriter file = System.IO.File.CreateText(path)) 
            {
                JsonSerializer serializer = new JsonSerializer();
                //serialize object directly into file stream
                serializer.Serialize(file, list);
            }
        }
        // GET api/notes/5
        [HttpGet("{id}")]
        public JsonResult Get(int id)
        {
            var j = ReadFile().ToObject<NoteList>();
            if (id > j.Count || id < 1)
                return Json("Invalid Request");
            return Json(j.Notes[id-1]);
        }

        // POST api/notes
        [HttpPost]
        public JsonResult Post([FromBody]Note note)
        {
            var j = ReadFile().ToObject<NoteList>();
            note.Id = ++j.Count;
            var newNotes = new Note[j.Count];
            Array.Copy(j.Notes, newNotes, j.Count-1);
            newNotes[j.Count - 1] = note;
            j.Notes = newNotes;
            WriteFile(j);
            return Json(note);
        }

        // PUT api/notes/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value){}

        // DELETE api/notes/5
        [HttpDelete("{id}")]
        public void Delete(int id){}
    }
}
